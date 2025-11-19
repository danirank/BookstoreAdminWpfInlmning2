using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookstoreAdminWpf.Models
{
    public partial class Writer
    {
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
    }
}
