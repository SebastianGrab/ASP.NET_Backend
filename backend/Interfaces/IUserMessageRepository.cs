using Models;

namespace Interfaces
{
    public interface IUserMessageRepository
    {
        ICollection<UserMessage> GetMessages();
        UserMessage GetMessage(long id);
        bool MessageExists(long id);
        ICollection<UserMessage> GetMessagesByUser(long userId);
        bool CreateUserMessage(UserMessage userMessage);
        bool UpdateUserMessage(UserMessage userMessage);
        bool DeleteUserMessage(UserMessage userMessage);
        bool DeleteUserMessages(List<UserMessage> userMessages);
        bool Save();
    }
}

