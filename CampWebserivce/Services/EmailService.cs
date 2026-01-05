using System.Net;
using System.Net.Mail;

namespace CampWebservice.Services;
f
public interface IEmailService
{
    Task SendIssueCreatedAsync(string toEmail, int issueId, string title);
    Task SendIssueClosedAsync(string toEmail, int issueId, string title);
}

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    private SmtpClient CreateClient()
    {
        var host = _config["Brevo:SmtpHost"] ?? "smtp-relay.brevo.com";
        var port = int.Parse(_config["Brevo:SmtpPort"] ?? "587");
        var user = _config["Brevo:SmtpUser"]!;
        var pass = _config["Brevo:SmtpPass"]!;

        return new SmtpClient(host, port)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(user, pass)
        };
    }

    private MailMessage CreateMessage(string toEmail, string subject, string body)
    {
        var fromEmail = _config["Brevo:FromEmail"]!;
        var fromName = _config["Brevo:FromName"] ?? "Campus Support";

        return new MailMessage
        {
            From = new MailAddress(fromEmail, fromName),
            Subject = subject,
            Body = body,
            IsBodyHtml = false,
            To = { toEmail }
        };
    }

    public async Task SendIssueCreatedAsync(string toEmail, int issueId, string title)
    {
        using var client = CreateClient();
        using var msg = CreateMessage(
            toEmail,
            $"Vi har modtaget din ticket (ID: {issueId})",
            $"Hej,\n\nVi har modtaget din ticket:\nID: {issueId}\nTitel: {title}\n\nDu får besked, når sagen er behandlet.\n\nMvh\nCampus Support"
        );

        await client.SendMailAsync(msg);
    }

    public async Task SendIssueClosedAsync(string toEmail, int issueId, string title)
    {
        using var client = CreateClient();
        using var msg = CreateMessage(
            toEmail,
            $"Din ticket er lukket (ID: {issueId})",
            $"Hej,\n\nDin ticket er nu lukket:\nID: {issueId}\nTitel: {title}\n\nTak for din henvendelse.\n\nMvh\nCampus Support"
        );

        await client.SendMailAsync(msg);
    }
}
