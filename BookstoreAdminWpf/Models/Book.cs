using System;
using System.Collections.Generic;

namespace BookstoreAdminWpf.Models;

public partial class Book
{
    public string Isbn13 { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Language { get; set; } = null!;

    public decimal Price { get; set; }

    public DateOnly ReleaseDate { get; set; }

    public int GenreId { get; set; }

    public int WriterId { get; set; }

    public int PublisherId { get; set; }

    public virtual Genre Genre { get; set; } = null!;

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual Publisher Publisher { get; set; } = null!;

    public virtual Writer Writer { get; set; } = null!;
}
