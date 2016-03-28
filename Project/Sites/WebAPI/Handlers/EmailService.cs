using System;
using System.Configuration;
using System.Net.Mail;
using Microsoft.AspNet.Identity;

namespace WebAPI.Handlers
{
	public static class Mail
	{
		public static void SendEmail(IdentityMessage imessage)
		{
			var mailAUser = ConfigurationSettings.AppSettings["MailA_User"];
			var mailAPass = ConfigurationSettings.AppSettings["MailA_Pass"];
			var mailFrom = ConfigurationSettings.AppSettings["MailFrom"];
			var mailServer = ConfigurationSettings.AppSettings["MailServer"];
			try
			{
				var message = new MailMessage { BodyEncoding = System.Text.Encoding.UTF8, IsBodyHtml = true };
				message.From = new MailAddress(mailFrom, "BESP Admin");

				message.To.Add(new MailAddress(imessage.Destination));

				//Lay tieu de
				message.Subject = imessage.Subject;

				//Lay noi dung
				message.Body = imessage.Body;

				//send mail
				if (mailServer.CompareTo("smtp.gmail.com") == 0)
				{
					//Gmail use SSL on port 587 or 465
					var emailClient = new SmtpClient(mailServer, 587);
					var SMTPUserInfo =
						new System.Net.NetworkCredential(mailAUser, mailAPass);
					emailClient.EnableSsl = true;
					emailClient.UseDefaultCredentials = false;
					emailClient.Credentials = SMTPUserInfo;
					emailClient.Send(message);
				}
				else if (mailServer.CompareTo("") == 0
					|| mailServer.CompareTo("localhost") == 0
					|| mailServer.CompareTo("127.0.0.1") == 0)
				{
					var emailClient = new SmtpClient();
					emailClient.DeliveryMethod = SmtpDeliveryMethod.PickupDirectoryFromIis;
					var SMTPUserInfo =
						new System.Net.NetworkCredential(mailAUser, mailAPass);
					emailClient.UseDefaultCredentials = false;
					emailClient.Credentials = SMTPUserInfo;
					emailClient.Send(message);
				}
				else
				{
					var emailClient = new SmtpClient(mailServer);
					var SMTPUserInfo =
						new System.Net.NetworkCredential(mailAUser, mailAPass);
					emailClient.UseDefaultCredentials = false;
					emailClient.Credentials = SMTPUserInfo;
					emailClient.Send(message);
				}
			}
			catch (Exception ex)
			{
			}
		}
	}
}