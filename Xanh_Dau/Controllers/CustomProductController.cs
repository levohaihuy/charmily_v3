using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Xanh_Dau.Services;
using Models;
using Repository;
using Repository.Interface;
using Xanh_Dau.DTO;
using Xanh_Dau.Services;
namespace Xanh_Dau.Controllers;

public class CustomProductController : Controller
{
    private readonly ICustomProductRepository _customProductRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICartRepository _cartRepository;
    private readonly FileService _fileService;
    private readonly TokenService _tokenService;

    public CustomProductController(
        ICustomProductRepository customProductRepository,
        IProductRepository productRepository,
        ICartRepository cartRepository,
        FileService fileService,
        TokenService tokenService)
    {
        _customProductRepository = customProductRepository;
        _productRepository = productRepository;
        _cartRepository = cartRepository;
        _fileService = fileService;
        _tokenService = tokenService;
    }

    public async Task<IActionResult> Customize(int productId)
    {
        var product = await _productRepository.GetProductByIdAsync(productId);
        if (product == null || !product.AllowsCustomization)
            return NotFound();

        var viewModel = new CustomizeProductViewModel
        {
            BaseProduct = product,
            CharmWidth = 10,  // 10mm
            CharmHeight = 8   // 8mm
        };

        return View(viewModel);
    }

    [HttpPost]
public async Task<IActionResult> SaveCustomProduct([FromBody] SaveCustomProductRequest request)
{
    try
    {
        var customerId = GetUserId();
        var baseProduct = await _productRepository.GetProductByIdAsync(request.BaseProductId);
        
        // Xử lý CustomBasePrice nullable
        var customProduct = new CustomProduct
        {
            BaseProductId = request.BaseProductId,
            CustomerId = customerId,
            TotalPrice = baseProduct.CustomBasePrice ?? baseProduct.Price,
            PreviewImage = request.PreviewImage,
            Status = "pending"
        };

        await _customProductRepository.CreateCustomProductAsync(customProduct);

        // Convert base64 to IFormFile và upload
        foreach (var image in request.Images)
        {
            // Convert base64 to IFormFile
            byte[] imageBytes = Convert.FromBase64String(image.Base64Image.Split(',')[1]);
            var stream = new MemoryStream(imageBytes);
            IFormFile file = new FormFile(stream, 0, imageBytes.Length, "image", "image.png");

            // Upload to Azure Blob Storage
            var imageUrl = await _fileService.UploadImageAsync(file);

            await _customProductRepository.AddCustomProductImageAsync(new CustomProductImage
            {
                CustomProductId = customProduct.CustomProductId,
                ImageUrl = imageUrl,
                PositionX = image.PositionX,
                PositionY = image.PositionY,
                Scale = image.Scale,
                Rotation = image.Rotation,
                OrderIndex = 0
            });
        }

        var cart = await _cartRepository.GetCartItemsByCustomerIdAsync(customerId);
        if (cart == null)
        {
            cart = await _cartRepository.AddCartAsync(new Cart
            {
                CustomerId = customerId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsDeleted = false
            });
        }

        await _cartRepository.AddCustomProductToCartAsync(cart.CartId, customProduct.CustomProductId);

        return Json(new { success = true });
    }
    catch (Exception ex)
    {
        return Json(new { success = false, message = ex.Message });
    }
}

    private int GetUserId()
    {
        var token = Request.Cookies["auth_token"];
        return Convert.ToInt32(_tokenService.GetUserIdFromToken(token));
    }
}