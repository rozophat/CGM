using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Root.Models
{
    public class Keyword
    {
        public int Id { get; set; }
        public string KeywordName { get; set; }
		public int? KeywordType { get; set; }
    }
}
