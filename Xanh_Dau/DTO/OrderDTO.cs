namespace Xanh_Dau.DTO;

public class OrderDTO
{
    public string SelectedItemIds { get; set; }
    public bool UseDefaultAddress { get; set; }
    public string? ShippingAddress { get; set; }
    public string? Receiver { get; set; }
    public string? ShipPhone { get; set; }
    public string? VoucherCode { get; set; }
    public int? VoucherId { get; set; }
}