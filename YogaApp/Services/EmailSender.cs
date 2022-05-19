using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace YogaApp.Services;

public class EmailSender : IEmailSender
{
    public AuthMessageSenderOptions Options { get; } //Set with Secret Manager.


    private string apiKey = "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";
    private string fromAddress = "ANAS.SHAHEEN@stu.mmu.ac.uk";
    private string fromName = "YogaApp Emailer";


    private readonly ILogger _logger;

    public EmailSender(ILogger<EmailSender> logger)
    {
        Options = new AuthMessageSenderOptions(apiKey, fromAddress, fromName);

        _logger = logger;
    }


    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        await Execute( subject, message, toEmail);
    }





    public async Task Execute( string subject, string message, string toEmail)
    {

        var client = new SendGridClient(Options.SendGridKey);
        var msg = new SendGridMessage()
        {
            From = new EmailAddress(Options.FromAddress, Options.FromName),
            Subject = subject,
            PlainTextContent = message,
            HtmlContent = message
        };
        msg.AddTo(new EmailAddress(toEmail));

        // Disable click tracking.
        // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
        msg.SetClickTracking(false, false);
        var response = await client.SendEmailAsync(msg);



    }
}