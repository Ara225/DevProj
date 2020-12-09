using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess
{
    public class SESEmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string receiverAddress, string subject, string htmlBody)
        {
            string senderAddress = "no-reply@aitchisonsoft.co.uk";

            using (var client = new AmazonSimpleEmailServiceClient(RegionEndpoint.EUWest2))
            {
                var sendRequest = new SendEmailRequest
                {
                    Source = senderAddress,
                    Destination = new Destination
                    {
                        ToAddresses =
                        new List<string> { receiverAddress }
                    },
                    Message = new Message
                    {
                        Subject = new Content(subject),
                        Body = new Body
                        {
                            Html = new Content
                            {
                                Charset = "UTF-8",
                                Data = htmlBody
                            },
                            Text = new Content
                            {
                                Charset = "UTF-8",
                                Data = htmlBody
                            }
                        }
                    }
                };
                var response = await client.SendEmailAsync(sendRequest);

            }
        }
    }
}