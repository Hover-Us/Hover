using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;


namespace Hover.Models
{
    public class Hash
    {
        public static string GetHash(string value, string salt, int IterationCount)
        {
            // Thanks to github.com/TahirNaushad/Fiver.Security.Hashing
            var valueBytes = KeyDerivation.Pbkdf2(
                                password: value,
                                salt: System.Text.Encoding.UTF8.GetBytes(salt),
                                prf: KeyDerivationPrf.HMACSHA512,
                                iterationCount: IterationCount,
                                numBytesRequested: 256 / 8); // 32 bytes

            // Replace the 2 characters that can cause URL problems
            return Convert.ToBase64String(valueBytes).Replace('+', '-').Replace('/', '_');
        }

        public static bool Validate(string value, string salt, int IterationCount, string hash)
            => GetHash(value, salt, IterationCount) == hash;
    }
}
