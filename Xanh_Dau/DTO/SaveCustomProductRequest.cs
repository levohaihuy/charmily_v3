namespace Xanh_Dau.DTO;

public class SaveCustomProductRequest
{
    public int BaseProductId { get; set; }
    public string PreviewImage { get; set; }
    public List<CustomImageData> Images { get; set; }
}
