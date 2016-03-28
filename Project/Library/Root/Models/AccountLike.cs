using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Root.Models
{
    public class AccountLike
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int ByAccountId { get; set; }
    }
}
