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

            Console.WriteLine(1 << 7);

            //                                                       0        0        0        0        0        0       156      64
            // assuming file of 5000 characters == 40,000 bits == 00000000 00000000 00000000 00000000 00000000 00000000 10011100 01000000
            //                                                                                                             ^------------^ = 7232

            //                                                      204      204      204      204      204      204      204      204
            //  14,576,347,871,954,379,929  bits  ==              11001100 11001100 11001100 11001100 11001100 11001100 11001100 11001100
            //                                                                                                             ^------------^ = 3276
            UInt64 bits_to_append = 40000;
            Console.WriteLine(12313532 << 7);

            Console.WriteLine(Convert.ToInt32(bits_to_append));


            //Console.WriteLine((bits_to_append << 50) >> 50); // 7232

            byte muff = Convert.ToByte((bits_to_append << 56) >> 56);


            Console.WriteLine(bits_to_append <<56 >> 56);
            Console.WriteLine(muff);

            Console.WriteLine("-----------------------");


            
            UInt64 almost_max = 18432332554901966031;

            byte[] arr = new byte[8];

            /*
            //loop - set values
            for (int i = 1; i <= arr.Length; i++)
            {
                int left_offset = 64 - (i * 8);
                int right_offset = 56;
                //if (i == 8) offset = 


                arr[i - 1] = Convert.ToByte((almost_max << left_offset) >> right_offset);
                if (i == 8) arr[i - 1] = Convert.ToByte(almost_max >> right_offset);
            }
            */



            //loop - set values - REVERSE
            for (int i = 0; i < arr.Length; i++)
            {
                int left_offset = 64 - (8 * (8 - i));
                int right_offset = 56;
                //if (i == 8) offset = 


                arr[i] = Convert.ToByte((almost_max << left_offset) >> right_offset);
                if (i == 8) arr[i] = Convert.ToByte(almost_max >> right_offset);
            }

            // loop - print values
            for (int i = 0; i < arr.Length; i++)
            {
                Console.WriteLine($"{i}: {Convert.ToString(arr[i], 2)}");
            }
            

        }
    }
}
