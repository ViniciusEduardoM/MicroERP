using System.Security.Cryptography;
using System.Text;

namespace MicroERP.API.Domain.Common.Utils
{
    public static class Auth
    {
        public static string CreateHash(string initialString)
        {
            var stringBytes = Encoding.UTF8.GetBytes(initialString);
            using (var alg = SHA512.Create())
            {
                string hex = "";

                var hashValue = alg.ComputeHash(stringBytes);
                foreach (byte x in hashValue)
                {
                    hex += String.Format("{0:x2}", x);
                }
                return hex;
            }
        }

        public static bool CompareHashes(string hash1, string hash2)
            => CreateHash(hash1) == hash2;
        
    }
}
