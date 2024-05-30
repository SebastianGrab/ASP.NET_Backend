using Models;

public interface IEmailService
{
    bool SendRegistrationEmail(string name, string to, string password);

    bool SendEmailFromProtocol(List<User> to, string subject, string content, long protocolId);
}