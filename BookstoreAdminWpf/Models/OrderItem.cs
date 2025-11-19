using System;
using System.Collections.Generic;

namespace BookstoreAdminWpf.Models;

public partial class OrderItem
{
    public int OrderItemId { get; set; }

    public int OrderId { get; set; }

    public string BookIsbn13 { get; set; } = null!;

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    public virtual Book BookIsbn13Navigation { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
