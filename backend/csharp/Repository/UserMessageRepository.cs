using Data;
using Helper;
using Helper.SearchObjects;
using Interfaces;
using Models;

namespace Repository
{
    public class UserMessageRepository : IUserMessageRepository
    {
        private readonly ProtocolContext _context;
        
        public UserMessageRepository(ProtocolContext context)
        {
            _context = context;
        }

        public bool CreateUserMessage(UserMessage userMessage)
        {
            _context.Add(userMessage);
            return Save();
        }

        public bool DeleteUserMessage(UserMessage userMessage)
        {
            _context.Remove(userMessage);
            return Save();
        }

        public bool DeleteUserMessages(List<UserMessage> userMessages)
        {
            _context.RemoveRange(userMessages);
            return Save();
        }

        public UserMessage GetMessage(long id)
        {
            return _context.UserMessages.Where(u => u.User.Id == id).FirstOrDefault();
        }

        public ICollection<UserMessage> GetMessages(QueryObject dateQuery, UserMessageSearchObject userMessageSearch)
        {
            var userMessages = _context.UserMessages.OrderByDescending(um => um.Id).AsQueryable();

            if(!string.IsNullOrWhiteSpace(dateQuery.minCreatedDateTime.ToString()))
            {
                userMessages = userMessages.Where(o => o.CreatedDate >= dateQuery.minCreatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(dateQuery.maxCreatedDateTime.ToString()))
            {
                userMessages = userMessages.Where(o => o.CreatedDate <= dateQuery.maxCreatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(dateQuery.minUpdatedDateTime.ToString()))
            {
                userMessages = userMessages.Where(o => o.UpdatedDate >= dateQuery.minUpdatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(dateQuery.maxUpdatedDateTime.ToString()))
            {
                userMessages = userMessages.Where(o => o.UpdatedDate <= dateQuery.maxUpdatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(userMessageSearch.Subject))
            {
                userMessages = userMessages.Where(o => o.Subject.Contains(userMessageSearch.Subject));
            }

            if(!string.IsNullOrWhiteSpace(userMessageSearch.SentFrom))
            {
                userMessages = userMessages.Where(o => o.SentFrom.Contains(userMessageSearch.SentFrom));
            }

            if(!string.IsNullOrWhiteSpace(userMessageSearch.MessageContent))
            {
                userMessages = userMessages.Where(o => o.MessageContent.Contains(userMessageSearch.MessageContent));
            }

            if(!string.IsNullOrWhiteSpace(userMessageSearch.IsArchived.ToString()))
            {
                userMessages = userMessages.Where(o => o.IsArchived == userMessageSearch.IsArchived);
            }

            if(!string.IsNullOrWhiteSpace(userMessageSearch.IsRead.ToString()))
            {
                userMessages = userMessages.Where(o => o.IsRead == userMessageSearch.IsRead);
            }

            return userMessages.OrderByDescending(p => p.Id).ToList();
        }

        public ICollection<UserMessage> GetMessagesByUser(long userId, QueryObject dateQuery, UserMessageSearchObject userMessageSearch)
        {
            var userMessages = _context.UserMessages.Where(u => u.User.Id == userId).OrderByDescending(um => um.Id).AsQueryable();

            if(!string.IsNullOrWhiteSpace(dateQuery.minCreatedDateTime.ToString()))
            {
                userMessages = userMessages.Where(o => o.CreatedDate >= dateQuery.minCreatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(dateQuery.maxCreatedDateTime.ToString()))
            {
                userMessages = userMessages.Where(o => o.CreatedDate <= dateQuery.maxCreatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(dateQuery.minUpdatedDateTime.ToString()))
            {
                userMessages = userMessages.Where(o => o.UpdatedDate >= dateQuery.minUpdatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(dateQuery.maxUpdatedDateTime.ToString()))
            {
                userMessages = userMessages.Where(o => o.UpdatedDate <= dateQuery.maxUpdatedDateTime);
            }

            if(!string.IsNullOrWhiteSpace(userMessageSearch.Subject))
            {
                userMessages = userMessages.Where(o => o.Subject.Contains(userMessageSearch.Subject));
            }

            if(!string.IsNullOrWhiteSpace(userMessageSearch.SentFrom))
            {
                userMessages = userMessages.Where(o => o.SentFrom.Contains(userMessageSearch.SentFrom));
            }

            if(!string.IsNullOrWhiteSpace(userMessageSearch.MessageContent))
            {
                userMessages = userMessages.Where(o => o.MessageContent.Contains(userMessageSearch.MessageContent));
            }

            if(!string.IsNullOrWhiteSpace(userMessageSearch.IsArchived.ToString()))
            {
                userMessages = userMessages.Where(o => o.IsArchived == userMessageSearch.IsArchived);
            }

            if(!string.IsNullOrWhiteSpace(userMessageSearch.IsRead.ToString()))
            {
                userMessages = userMessages.Where(o => o.IsRead == userMessageSearch.IsRead);
            }

            return userMessages.OrderByDescending(p => p.Id).ToList();
        }

        public int GetNumberOfUnreadMessagesPerUser(long userId)
        {
            return _context.UserMessages.Where(um => um.User.Id == userId).Count();
        }

        public bool MessageExists(long id)
        {
            return _context.UserMessages.Any(um => um.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateUserMessage(UserMessage userMessage)
        {
            _context.Update(userMessage);
            return Save();
        }
    }
}