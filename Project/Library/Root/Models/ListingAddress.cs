using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Root.Models
{
    public class ListingAddress
    {
        public int Id { get; set; }
        public int ListingId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string ProvinceState { get; set; }
        public string Country { get; set; }
        public string PostalZip { get; set; }
		public string GPSLatitude { get; set; }
		public string GPSLongitude { get; set; }
    }
}
