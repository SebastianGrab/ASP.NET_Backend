using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Security.Cryptography;
using csharp.Interfaces;

namespace csharp.Services
{
    public class PasswordGenerator : IPasswordGenerator
    {

        public string GetRandomAlphanumericString(int length)
        {
            const string alphanumericCharacters =
                "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
                "abcdefghijklmnopqrstuvwxyz" +
                "0123456789";
            return GetRandomString(length, alphanumericCharacters);
        }

        private static readonly Random _random = new Random();

        public string GetRandomString(int length, IEnumerable<char> characterSet)
        {
            var characterArray = characterSet.Distinct().ToArray();

            var result = "";
            for (int i = 0; i < length; i++)
            {
                result += characterArray[_random.Next(characterArray.Length)];
            }

            return result;
        }
    }
}