using System;
using System.Collections.Generic;

namespace Models;

public partial class CustomProductImage
{
    public int CustomImageId { get; set; }

    public int CustomProductId { get; set; }

    public string ImageUrl { get; set; } = null!;

    public int ImageWidth { get; set; }

    public int ImageHeight { get; set; }

    public decimal PositionX { get; set; }

    public decimal PositionY { get; set; }

    public decimal Scale { get; set; }

    public decimal? Rotation { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? OrderIndex { get; set; }

    public virtual CustomProduct CustomProduct { get; set; } = null!;
}
