using System;
using System.Collections.Generic;

namespace Models;

public partial class CartDetail
{
    public int CartDetailId { get; set; }

    public int CartId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool? IsDeleted { get; set; }   
    public int? CustomProductId { get; set; } 
    

    public virtual Cart Cart { get; set; } = null!;
    public virtual CustomProduct CustomProduct { get; set; } 

    public virtual Product Product { get; set; } = null!;
}
