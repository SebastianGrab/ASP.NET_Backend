public interface IEmailService
{
    bool SendRegistrationEmail(string name, string to, string password);

    bool SendEmailFromProtocol(string to, string subject, string content);
}