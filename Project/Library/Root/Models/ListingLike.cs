using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Root.Models
{
    public class ListingLike
    {
        public int Id { get; set; }
        public int ListingId { get; set; }
        public int ByAccountId { get; set; }
    }
}
