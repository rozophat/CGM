using System.Collections.Generic;

namespace Website.ViewModels.User
{
	public class ResetPasswordViewModel
	{
		public string Email { get; set; }

		public string Password { get; set; }

		public string ConfirmPassword { get; set; }

		public string Code { get; set; }
	}
}
