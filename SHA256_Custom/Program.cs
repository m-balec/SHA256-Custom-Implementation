using System;

namespace SHA256_Custom
{
    class Program
    {
        static void Main(string[] args)
        {
            string phrase1 = "a"; // 1 Chunk
            string phrase2 = "The greatest glory in living lies not in never falling, but in rising every time we fall."; // 2 Chunks
            string phrase3 = "Your time is limited, so don't waste it living someone else's life. Don't be trapped by dogma - which is living with the results of other people's thinking"; // 3 Chunks

            string[] phrases = new string[]
            {
                phrase1,
                phrase2,
                phrase3,
            };

            // Example usage:
            Console.WriteLine(Sha256Custom.processChunks(phrase1));
            // Returns: ca978112ca1bbdcafac231b39a23dc4da786eff8147c4e72b9807785afee48bb


            // TESTING: all phrases
            Console.WriteLine(Test.EvaluateHash(phrase3));
            // Returns: TRUE (All phrases tested match the returned hash values of a reputable source, System.Security.Cryptography library)
        }
    }
}
