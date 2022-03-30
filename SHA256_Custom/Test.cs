using System.Text;
using System.Security.Cryptography;

namespace SHA256_Custom
{
    class Test
    {
        /*
        // TEST SINGLE PHRASE
        public static bool EvaluateHash(string phrase)
        {
            SHA256 hash = SHA256.Create();
            byte[] bytes = hash.ComputeHash(Encoding.UTF8.GetBytes(phrase));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }

            string libraryHash = builder.ToString();
            string customHash = Sha256Custom.processChunks(phrase);

            if (!libraryHash.Equals(customHash))
            {
                return false;
            }

            return true;
        }
        */


        // TEST SINGLE PHRASE MULTIPLE TIMES
        public static bool EvaluateHash(string phrase, int iterations = 1)
        {
            string libraryHash = phrase;
            string customHash = phrase;


            for (int j = 0; j < iterations; j++)
            {
                SHA256 hash = SHA256.Create();
                byte[] bytes = hash.ComputeHash(Encoding.UTF8.GetBytes(libraryHash));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                libraryHash = builder.ToString();
                customHash = Sha256Custom.processChunks(customHash);
            }

            if (!libraryHash.Equals(customHash))
            {
                return false;
            }

            return true;
        }


        // TEST ARRAY OF PHRASES
        public static bool EvaluateHash(string[] phrases)
        {

            foreach (string phrase in phrases)
            {
                SHA256 hash = SHA256.Create();
                byte[] bytes = hash.ComputeHash(Encoding.UTF8.GetBytes(phrase));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                //string libraryHash = builder.ToString();
                string customHash = Sha256Custom.processChunks(phrase);

                if (!builder.Equals(customHash))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
