using Data;
using Interfaces;
using Models;

namespace Repository
{
    public class UserLoginAttemptRepository : IUserLoginAttemptRepository
    {
        private readonly ProtocolContext _context;
        
        public UserLoginAttemptRepository(ProtocolContext context)
        {
            _context = context;
        }

        public bool DeleteUserLoginAttempt(UserLoginAttempt userLoginAttempt)
        {
            _context.Remove(userLoginAttempt);
            return Save();
        }

        public UserLoginAttempt GetUserLoginAttempt(long userId)
        {
            return _context.UserLoginAttempts.Where(u => u.userId == userId).FirstOrDefault();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateUserLoginAttempt(UserLoginAttempt userLoginAttempt)
        {
            _context.Update(userLoginAttempt);
            return Save();
        }

        public bool UserLoginAttemptExists(long userId)
        {
            return _context.UserLoginAttempts.Any(u => u.userId == userId);
        }
    }
}