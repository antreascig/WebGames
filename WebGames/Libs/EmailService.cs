using SendGrid;
using System.Net;
using System.Configuration;
using System.Diagnostics;
using SendGrid.Helpers.Mail;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System;

namespace WebGames.Libs
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            await configSendGridasync(message);
        }

        // Use NuGet to install SendGrid (Basic C# client lib) 
        private async Task configSendGridasync(IdentityMessage message)
        {
            try
            {
                var myMessage = new SendGridMessage();
                myMessage.AddTo(new EmailAddress(message.Destination));
                myMessage.From = new EmailAddress("info@webgames.com", "Auto-Message");
                myMessage.Subject = message.Subject;
                myMessage.PlainTextContent = message.Body;
                myMessage.HtmlContent = message.Body;

                //var credentials = new NetworkCredential(
                //           ConfigurationManager.AppSettings["mailAccount"],
                //           ConfigurationManager.AppSettings["mailPassword"]
                //           );
                var apiKey = ConfigurationManager.AppSettings["SENDGRID_KEY"];

                var client = new SendGridClient(apiKey);

                // Send the email.
                if (client != null)
                {
                    var response = await client.SendEmailAsync(myMessage);
                }
                else
                {
                    Trace.TraceError("Failed to create Web transport.");
                    await Task.FromResult(0);
                }
            }
            catch(Exception exc)
            {
                Trace.TraceError(exc.Message);
                await Task.FromResult(0);
            }

        }
    }
}