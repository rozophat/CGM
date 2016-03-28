using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Root.AuthenticationModels
{
	public class UserModel
	{
		[Required]
		public string UserName { get; set; }
		
		public string IsActive { get; set; }

		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }

		[Required]
		[StringLength(100, MinimumLength = 6)]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }
	}
}
