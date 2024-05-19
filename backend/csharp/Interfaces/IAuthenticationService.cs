using Helper;

namespace Interfaces
{
    public interface IAuthenticationService
    {
        string Login(LoginObject loginObject);
    }
}