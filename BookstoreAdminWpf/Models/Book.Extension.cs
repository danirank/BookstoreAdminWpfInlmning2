using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookstoreAdminWpf.Models
{
    public partial class Book
    {
        [NotMapped]
        public DateTime ReleaseDateAsDateTime
        {
            get => ReleaseDate.ToDateTime(TimeOnly.MinValue);
            set => ReleaseDate = DateOnly.FromDateTime(value);
        }
    }
}
