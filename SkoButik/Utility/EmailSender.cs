using Microsoft.AspNetCore.Identity.UI.Services;

namespace SkoButik.Utility
{
    public class EmailSender:IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // här kan vi lägga emailslogik
            return Task.CompletedTask;
        }
    }
}
