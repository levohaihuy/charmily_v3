using System;
using System.Collections.Generic;

namespace Models;

public partial class CustomProduct
{
    public int CustomProductId { get; set; }

    public int BaseProductId { get; set; }

    public int CustomerId { get; set; }

    public string? Status { get; set; }

    public string? AdminComments { get; set; }

    public decimal TotalPrice { get; set; }

    public string? PreviewImage { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? ApprovedBy { get; set; }

    public DateTime? ApprovedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Admin? ApprovedByNavigation { get; set; }

    public virtual Product BaseProduct { get; set; } = null!;

    public virtual ICollection<CustomProductImage> CustomProductImages { get; set; } = new List<CustomProductImage>();

    public virtual Customer Customer { get; set; } = null!;
}
