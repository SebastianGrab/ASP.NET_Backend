using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace csharp.Interfaces
{
    public interface IPasswordGenerator
    {
        string GetRandomAlphanumericString(int length);
        string GetRandomString(int length, IEnumerable<char> characterSet);
    }
}