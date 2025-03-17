namespace Xanh_Dau.DTO;

public class CustomImageRequest
{
    public IFormFile ImageFile { get; set; }
    public decimal PositionX { get; set; }
    public decimal PositionY { get; set; }
    public decimal Scale { get; set; }
    public decimal Rotation { get; set; }
    public int OrderIndex { get; set; }
}