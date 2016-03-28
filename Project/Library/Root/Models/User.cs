using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Root.Models
{
    public class User : IdentityUser
    {
		[Column(TypeName = "CHAR")]
		[StringLength(1)]
		public string IsActive { get; set; }
    }
}
