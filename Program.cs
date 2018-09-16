using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace chopeen
{
    class Program
    {
        private static IConfiguration _config;

        private static void Main()
        {
            _config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.PROD.json", optional: false, reloadOnChange: true)
                .Build();

            Execute().Wait();
        }

        private static async Task Execute()
        {
            var from = MailHelper.StringToEmailAddress(_config["FromAddress"]);
            var to = MailHelper.StringToEmailAddress(_config["ToAddress"]);
            // subject and body are set in the template (configured via the SendGrid portal)
            var subject = "";  
            var plainTextContent = "";
            var htmlContent = "";

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            msg.TemplateId = _config["SendGridTemplateId"];

            var client = new SendGridClient(_config["SendGridKey"]);
            var response = await client.SendEmailAsync(msg);

            Console.WriteLine(response.StatusCode);
        }
    }
}
