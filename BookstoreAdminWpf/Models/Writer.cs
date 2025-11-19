using System;
using System.Collections.Generic;

namespace BookstoreAdminWpf.Models;

public partial class Writer
{
    public int WriterId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateOnly? Birthdate { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}

