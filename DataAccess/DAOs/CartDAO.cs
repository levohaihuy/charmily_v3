using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccess.DAOs;

public class CartDAO : SingletonBase<CartDAO>
{
    public async Task<Cart> GetCartItemsByCustomerIdAsync(int customerId)
    {
        try
        {
            var cart = await _context.Carts
                .Include(c => c.CartDetails.Where(cd => !cd.IsDeleted.Value))
                .ThenInclude(cd => cd.Product)
                .ThenInclude(p => p.ProductImages)
                .Include(c => c.CartDetails)
                .ThenInclude(cd => cd.CustomProduct)
                .ThenInclude(cp => cp.CustomProductImages)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId && !c.IsDeleted.Value);
            return cart;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting cart items: {ex.Message}");
            return null;
        }
    }


    public async Task<Cart> AddCartAsync(Cart cart)
    {
        try
        {
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();
            return cart;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding cart: {ex.Message}");
            return null;
        }
    }

    
    // Thêm phương thức cho custom product
    public async Task<CartDetail> AddCustomProductToCartAsync(int cartId, int customProductId)
    {
        try
        {
            // Kiểm tra custom product
            var customProduct = await _context.CustomProducts
                .Include(cp => cp.BaseProduct)
                .FirstOrDefaultAsync(cp => cp.CustomProductId == customProductId && !cp.IsDeleted.Value);

            if (customProduct == null)
            {
                Console.WriteLine("Custom product không tồn tại.");
                return null;
            }

            var cartDetail = new CartDetail
            {
                CartId = cartId,
                ProductId = customProduct.BaseProductId,
                CustomProductId = customProductId,
                Quantity = 1, // Custom product luôn có số lượng là 1
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsDeleted = false
            };

            await _context.CartDetails.AddAsync(cartDetail);
            await _context.SaveChangesAsync();
            return cartDetail;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding custom product to cart: {ex.Message}");
            return null;
        }
    }
    // Sửa lại phương thức GetCartDetailByCartAndProductAsync để hỗ trợ custom product
    public async Task<CartDetail> GetCartDetailByCartAndProductAsync(int cartId, int productId, int? customProductId = null)
    {
        try
        {
            return await _context.CartDetails
                .FirstOrDefaultAsync(cd => cd.CartId == cartId &&
                                           cd.ProductId == productId &&
                                           cd.CustomProductId == customProductId &&
                                           !cd.IsDeleted.Value);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting cart detail: {ex.Message}");
            return null;
        }
    }


    public async Task<bool> AddCartDetailAsync(CartDetail cartDetail)
    {
        try
        {
            // Kiểm tra số lượng tồn kho
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == cartDetail.ProductId && !p.IsDeleted.Value);

            if (product == null)
            {
                Console.WriteLine("Sản phẩm không tồn tại.");
                return false;
            }

            if (product.StockQuantity < cartDetail.Quantity)
            {
                Console.WriteLine(
                    $"Số lượng sản phẩm trong kho không đủ. Tồn kho: {product.StockQuantity}, yêu cầu: {cartDetail.Quantity}");
                return false;
            }

            // Nếu kiểm tra thành công, thêm vào giỏ hàng
            await _context.CartDetails.AddAsync(cartDetail);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding cart detail: {ex.Message}");
            return false;
        }
    }

    public async Task<CartDetail> GetCartDetailByIdAsync(int cartDetailId)
    {
        try
        {
            return await _context.CartDetails
                .Include(cd => cd.Cart) // Đảm bảo lấy thông tin về giỏ hàng
                .FirstOrDefaultAsync(cd => cd.CartDetailId == cartDetailId && !cd.IsDeleted.Value);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting cart detail by ID: {ex.Message}");
            return null;
        }
    }

    // Sửa lại UpdateCartDetailAsync để xử lý custom product
    public async Task<bool> UpdateCartDetailAsync(CartDetail cartDetail)
    {
        try
        {
            // Nếu là custom product, giữ số lượng là 1
            if (cartDetail.CustomProductId.HasValue)
            {
                cartDetail.Quantity = 1;
                _context.CartDetails.Update(cartDetail);
                await _context.SaveChangesAsync();
                return true;
            }

            // Logic xử lý sản phẩm thường
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == cartDetail.ProductId && !p.IsDeleted.Value);

            if (product == null)
            {
                Console.WriteLine("Sản phẩm không tồn tại.");
                return false;
            }

            if (product.StockQuantity < cartDetail.Quantity)
            {
                Console.WriteLine(
                    $"Số lượng sản phẩm trong kho không đủ. Tồn kho: {product.StockQuantity}, yêu cầu: {cartDetail.Quantity}");
                return false;
            }

            _context.CartDetails.Update(cartDetail);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating cart detail: {ex.Message}");
            return false;
        }
    }

    public async Task RemoveFromCartAsync(int cartDetailId)
    {
        try
        {
            var cartDetail = await _context.CartDetails.FindAsync(cartDetailId);
            if (cartDetail == null) return;

            // Lấy thông tin sản phẩm từ CartDetail
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == cartDetail.ProductId && !p.IsDeleted.Value);

            if (product != null)
            {
                // Cập nhật lại số lượng tồn kho (nếu cần)
                product.StockQuantity += cartDetail.Quantity;
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
            }

            // Đánh dấu sản phẩm trong giỏ hàng là đã xóa
            cartDetail.IsDeleted = true;
            cartDetail.DeletedAt = DateTime.Now;
            cartDetail.UpdatedAt = DateTime.Now;

            _context.CartDetails.Update(cartDetail);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error removing cart detail: {ex.Message}");
        }
    }

    public async Task<bool> UpdateSelectionAsync(int cartDetailId, int customerId, bool isSelected)
    {
        try
        {
            var cartDetail = await _context.CartDetails
                .Include(cd => cd.Cart)
                .FirstOrDefaultAsync(cd =>
                    cd.CartDetailId == cartDetailId &&
                    cd.Cart.CustomerId == customerId &&
                    !cd.IsDeleted.Value);

            if (cartDetail == null) return false;
            cartDetail.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating selection: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> RemoveSelectedItemsAsync(int cartId, IEnumerable<int> selectedDetailIds)
    {
        try
        {
            var cartDetails = await _context.CartDetails
                .Where(cd => cd.CartId == cartId &&
                             selectedDetailIds.Contains(cd.CartDetailId))
                .ToListAsync();

            foreach (var detail in cartDetails)
            {
                detail.IsDeleted = true;
                detail.DeletedAt = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error removing selected items: {ex.Message}");
            return false;
        }
    }
    
    
}