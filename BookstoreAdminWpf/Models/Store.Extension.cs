using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookstoreAdminWpf.Models
{
    public partial class Store
    {
        public  string FullAdress => $"{Street} {PostalCode} {City} "; 

        public decimal GetTotalInventoryValue()
        {
            if (Inventories == null) return 0;

            return Inventories.Sum(i => i.Quantity * i.Isbn13Navigation.Price);       
        }
    }
}
