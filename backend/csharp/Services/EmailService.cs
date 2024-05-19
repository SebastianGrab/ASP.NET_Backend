using MailKit;
using MailKit.Net.Smtp;
using MimeKit;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<MailService> _logger;

    public EmailService(IConfiguration configuration, ILogger<MailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public bool SendEmailFromProtocol(string to, string subject, string content)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress("DRK", _configuration["EmailSettings:Username"]));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;
        
        var builder = new BodyBuilder { TextBody = content };
        email.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();
        smtp.Connect(_configuration["EmailSettings:Host"], 
                                int.Parse(_configuration["EmailSettings:Port"]), 
                                bool.Parse(_configuration["EmailSettings:UseSsl"]));
        smtp.Authenticate(_configuration["EmailSettings:Username"],
                                    _configuration["EmailSettings:Password"]);

        try
        {
            smtp.Send(email);
            _logger.LogInformation("E-Mail sent successfully to user " + email.To.ToString() + ".");
            smtp.Disconnect(true);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            smtp.Disconnect(true);
            return false;
        }
    }

    public bool SendRegistrationEmail(string to, string password)
    {
        var htmlBody = 
        @"<p>Hallo,</p>
        <p>f&uuml;r Sie wurde ein Account in der Protokollerfassungsanwendung des Deutschen Roten Kreuzes angelegt.</p>
        <p>Die folgenden Anmeldedaten erm&ouml;glichen Ihnen den Zugang zu Ihrem Konto.</p>
        <table style='width: 535px; height: 52px;'>
        <tbody>
        <tr>
        <td style='width: 250.083px;'>E-Mail</td>
        <td style='width: 270.117px;'>Passwort</td>
        </tr>
        <tr>
        <td style='width: 250.083px;'>" + to + @"</td>
        <td style='width: 270.117px;'>" + password + @"</td>
        </tr>
        </tbody>
        </table>
        <p>Bitte &auml;ndern Sie Ihr Passwort jetzt, um sicherzustellen, dass niemand Zugriff auf Ihre Daten hat.</p>
        <p>Hier gelangen Sie zur Anmeldung:</p>
        <p><a href='" + _configuration["EmailSettings:FrontendUrl"] + @"'>" + _configuration["EmailSettings:FrontendUrl"] + @"</a></p>
        <p>Mit freundlichen Gr&uuml;&szlig;en,</p>
        <p>Team der Protokollerfassung</p>";

        var email = new MimeMessage();
        email.From.Add(new MailboxAddress("DRK", _configuration["EmailSettings:Username"]));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = "subject";
        
        var builder = new BodyBuilder { HtmlBody = htmlBody };
        email.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();
        smtp.Connect(_configuration["EmailSettings:Host"], 
                                int.Parse(_configuration["EmailSettings:Port"]), 
                                bool.Parse(_configuration["EmailSettings:UseSsl"]));
        smtp.Authenticate(_configuration["EmailSettings:Username"],
                                    _configuration["EmailSettings:Password"]);

        try
        {
            smtp.Send(email);
            _logger.LogInformation("E-Mail sent successfully to user " + email.To.ToString() + ".");
            smtp.Disconnect(true);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            smtp.Disconnect(true);
            return false;
        }
    }
}
