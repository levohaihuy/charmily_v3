using Models;

namespace Xanh_Dau.DTO;

public class CustomizeProductViewModel
{
    public Product BaseProduct { get; set; }
    public int CharmWidth { get; set; }
    public int CharmHeight { get; set; }
    public double AspectRatio { get; set; }
}