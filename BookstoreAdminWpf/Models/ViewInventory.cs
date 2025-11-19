using System;
using System.Collections.Generic;

namespace BookstoreAdminWpf.Models;

public partial class ViewInventory
{
    public string BookId { get; set; } = null!;

    public string BookTitel { get; set; } = null!;

    public int StoreId { get; set; }

    public string? Bookstore { get; set; }

    public int Quantity { get; set; }
}
