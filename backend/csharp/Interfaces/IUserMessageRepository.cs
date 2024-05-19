using Helper;
using Helper.SearchObjects;
using Models;

namespace Interfaces
{
    public interface IUserMessageRepository
    {
        ICollection<UserMessage> GetMessages(QueryObject dateQuery, UserMessageSearchObject userMessageSearch);
        UserMessage GetMessage(long id);
        bool MessageExists(long id);
        ICollection<UserMessage> GetMessagesByUser(long userId, QueryObject dateQuery, UserMessageSearchObject userMessageSearch);
        bool CreateUserMessage(UserMessage userMessage);
        bool UpdateUserMessage(UserMessage userMessage);
        bool DeleteUserMessage(UserMessage userMessage);
        bool DeleteUserMessages(List<UserMessage> userMessages);
        int GetNumberOfUnreadMessagesPerUser(long userId);
        bool Save();
    }
}

