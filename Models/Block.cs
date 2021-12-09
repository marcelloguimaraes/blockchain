using System;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace BlockChain.Models
{
    public class Block
    {
        public DateTime Timestamp { get; init; }
        public object[] Data { get; init; }
        public string Hash { get; private set; }
        public string PreviousHash { get; init; }
        public long Nonce { get; private set; }

        public Block(DateTime timestamp, string previousHash = "", object[] data = null)
        {
            Timestamp = timestamp;
            Data = data;
            Hash = CreateHash();
            PreviousHash = previousHash;
            Nonce = 0;
        }

        public string CreateHash()
        {
            using (var sha256 = SHA256.Create())
            {
                var builder = new StringBuilder();
                builder.Append(PreviousHash)
                    .Append(Timestamp.ToString("yyyyMdhhmmssms"))
                    .Append(JsonSerializer.Serialize(Data))
                    .Append(Nonce);

                var seed = builder.ToString();
                builder.Clear();
                var bytes = Encoding.UTF8.GetBytes(seed);
                var computedHash = sha256.ComputeHash(bytes);

                foreach (var @byte in computedHash)
                {
                    builder.Append(@byte.ToString("x2"));
                }

                return builder.ToString();
            }
        }

        public void Mine(int difficulty)
        {
            if (difficulty <= 0)
            {
                difficulty = 1;
            }
            var array = new string[difficulty + 1];
            var startDifficulty = string.Join("0", array);
            bool isHashNotFound;

            do
            {
                Nonce++;
                Hash = CreateHash();
                isHashNotFound = !Hash.StartsWith(startDifficulty);
            } while (isHashNotFound);
        }
    }
}