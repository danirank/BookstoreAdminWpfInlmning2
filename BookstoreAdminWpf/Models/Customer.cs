using System;
using System.Collections.Generic;

namespace BookstoreAdminWpf.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateOnly? Birthday { get; set; }

    public string? Email { get; set; }

    public string? Phonenumber { get; set; }

    public string? Country { get; set; }

    public string? PostalCode { get; set; }

    public string? City { get; set; }

    public string? StreetName { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
