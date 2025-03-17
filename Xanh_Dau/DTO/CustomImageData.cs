namespace Xanh_Dau.DTO;

public class CustomImageData
{
    public string Base64Image { get; set; }  // Thay vì ImageUrl, ta sẽ dùng Base64 string của ảnh
    public decimal PositionX { get; set; }
    public decimal PositionY { get; set; }
    public decimal Scale { get; set; }
    public decimal Rotation { get; set; }
}