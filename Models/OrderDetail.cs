﻿using System;
using System.Collections.Generic;

namespace Models;

public partial class OrderDetail
{
    public int OrderDetailId { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool? IsDeleted { get; set; }
    public int? CustomProductId { get; set; } 

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
    public virtual CustomProduct CustomProduct { get; set; } = null!;
}
