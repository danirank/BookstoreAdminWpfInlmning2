using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookstoreAdminWpf.Models
{
    public partial class Store
    {
        public  string FullAdress => $"{Country} {Street} {City} {PostalCode}"; 
    }
}
