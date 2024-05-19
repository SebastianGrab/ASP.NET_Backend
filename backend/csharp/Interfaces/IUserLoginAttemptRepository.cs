using Models;

namespace Interfaces
{
    public interface IUserLoginAttemptRepository
    {
        UserLoginAttempt GetUserLoginAttempt(long userId);
        bool UserLoginAttemptExists(long userId);
        bool UpdateUserLoginAttempt(UserLoginAttempt userLoginAttempt);
        bool DeleteUserLoginAttempt(UserLoginAttempt userLoginAttempt);
        bool Save();
    }
}

