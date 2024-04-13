using Data;
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
            return _context.UserMessages.Where(u => u.Users.Id == id).FirstOrDefault();
        }

        public ICollection<UserMessage> GetMessages()
        {
            return _context.UserMessages.OrderByDescending(um => um.Id).ToList();
        }

        public ICollection<UserMessage> GetMessagesByUser(long userId)
        {
            return _context.Users.Where(u => u.Id == userId).Select(um => um.UserMessages).FirstOrDefault();
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