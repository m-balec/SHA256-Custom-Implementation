using System;
using System.Text;

namespace SHA256_Custom
{
    class Sha256Custom
    {
        // Custom implementation of SHA256 hashing algorithm (one-way 256 bit encryption)
        // Intended as a project to help me build familiarity with C#, as well as working with bitwise operations
        //
        // NOT INTENDED FOR USE IN PRODUCTION ENVIRONMENT


        // Function to return the fully padded, multiple-of-512 chunk(s)
        private static byte[] prepareChunks(string phrase)
        {
            char[] letters;
            byte[] messageBytes, ubytes;
            int arraySize, numOfChunks;
            UInt64 phraseLength = (uint)phrase.Length * 8;


            // Converting phrase to byte array
            letters = phrase.ToCharArray();
            messageBytes = Encoding.ASCII.GetBytes(letters);


            // Getting num of bits and chunks
            numOfChunks = (int)Math.Ceiling((decimal)(((float)phraseLength + 64) / 512));


            // Determining size of array based on num of chunks
            arraySize = 64 * numOfChunks;


            // Setting the size of this array which will hold the entire 512 bit-multiple message
            ubytes = new byte[arraySize];


            // Filling entire message schedule
            for (int i = 0; i < arraySize; i++)
            {
                if (i <= messageBytes.Length - 1)
                {
                    ubytes[i] = messageBytes[i];
                }
                else if (i == messageBytes.Length)
                {
                    ubytes[i] = 0x80; // 128 or 0b10000000 or (1 << 7)
                }
                else if (i < arraySize - 8)
                {
                    ubytes[i] = 0x0; // NOTHING
                }
                else
                {
                    int left_offset = 64 - (8 * (64 - i)),
                        right_offset = 56;

                    if (i == 8) left_offset = 0;

                    ubytes[i] = Convert.ToByte((phraseLength << left_offset) >> right_offset);
                }
            }

            return ubytes;
        }


        // Function to process chunks
        public static string processChunks(string phrase)
        {
            byte[] byteArray = prepareChunks(phrase);
            int chunks = (byteArray.Length * 8) / 512;

            // Hash constants, defined as the first 32 bits of the fractional parts of the square roots of the first 8 prime numbers
            UInt32[] hashConstants = new UInt32[] {
                0x6a09e667, 0xbb67ae85, 0x3c6ef372, 0xa54ff53a, 0x510e527f, 0x9b05688c, 0x1f83d9ab, 0x5be0cd19
            };

            // Round constants, defined as the first 32 bits of the fractional parts of the cube roots of the first 64 prime numbers
            UInt32[] roundConstants = new UInt32[] {
                0x428a2f98, 0x71374491, 0xb5c0fbcf, 0xe9b5dba5, 0x3956c25b, 0x59f111f1, 0x923f82a4, 0xab1c5ed5,
                0xd807aa98, 0x12835b01, 0x243185be, 0x550c7dc3, 0x72be5d74, 0x80deb1fe, 0x9bdc06a7, 0xc19bf174,
                0xe49b69c1, 0xefbe4786, 0x0fc19dc6, 0x240ca1cc, 0x2de92c6f, 0x4a7484aa, 0x5cb0a9dc, 0x76f988da,
                0x983e5152, 0xa831c66d, 0xb00327c8, 0xbf597fc7, 0xc6e00bf3, 0xd5a79147, 0x06ca6351, 0x14292967,
                0x27b70a85, 0x2e1b2138, 0x4d2c6dfc, 0x53380d13, 0x650a7354, 0x766a0abb, 0x81c2c92e, 0x92722c85,
                0xa2bfe8a1, 0xa81a664b, 0xc24b8b70, 0xc76c51a3, 0xd192e819, 0xd6990624, 0xf40e3585, 0x106aa070,
                0x19a4c116, 0x1e376c08, 0x2748774c, 0x34b0bcb5, 0x391c0cb3, 0x4ed8aa4a, 0x5b9cca4f, 0x682e6ff3,
                0x748f82ee, 0x78a5636f, 0x84c87814, 0x8cc70208, 0x90befffa, 0xa4506ceb, 0xbef9a3f7, 0xc67178f2
            };


            // Chunk Loop
            for (int chunk = 0; chunk < chunks; chunk++) // CHUNK LOOP
            {
                byte[] bitsProcessing = new byte[64];       // Byte array to hold the chunk currently being processed
                UInt32[] messageSchedule = new UInt32[64];  // Unsigned 32bit int array to store message schedule
                StringBuilder temp;                         // Temp string to hold 4 8-bit binary strings at a time that make up each 32-bit word in the message schedule

                // Fill new byte array of 512 bits with the correct chunk
                for (int i = 0; i < 64; i++)
                {
                    bitsProcessing[i] = byteArray[(chunk * 64) + i];
                }


                // go through each word in messageSchedule and append the binary values into 32 bit strings -> then back to uint32
                for (int i = 0; i < 16; i++)
                {
                    temp = new StringBuilder();

                    for (int j = 0; j < 4; j++)
                    {
                        temp.Append(Convert.ToString(bitsProcessing[i * 4 + j], 2).PadLeft(8, '0'));
                    }

                    messageSchedule[i] = Convert.ToUInt32(temp.ToString(), 2);
                }


                // Initialize values from 16-63
                for (int i = 16; i < 64; i++)
                {
                    messageSchedule[i] = messageSchedule[i - 16] + s0(messageSchedule[i - 15]) + messageSchedule[i - 7] + s1(messageSchedule[i - 2]);
                }


                UInt32 a = hashConstants[0],
                       b = hashConstants[1],
                       c = hashConstants[2],
                       d = hashConstants[3],
                       e = hashConstants[4],
                       f = hashConstants[5],
                       g = hashConstants[6],
                       h = hashConstants[7];


                // Compression
                for (int i = 0; i < 64; i++)
                {
                    UInt32 temp1 = h + S1(e) + choice(e, f, g) + roundConstants[i] + messageSchedule[i],
                           temp2 = S0(a) + major(a, b, c);

                    h = g;
                    g = f;
                    f = e;
                    e = d + temp1;
                    d = c;
                    c = b;
                    b = a;
                    a = temp1 + temp2;
                }

                hashConstants[0] = hashConstants[0] + a;
                hashConstants[1] = hashConstants[1] + b;
                hashConstants[2] = hashConstants[2] + c;
                hashConstants[3] = hashConstants[3] + d;
                hashConstants[4] = hashConstants[4] + e;
                hashConstants[5] = hashConstants[5] + f;
                hashConstants[6] = hashConstants[6] + g;
                hashConstants[7] = hashConstants[7] + h;

            }


            // Putting together final concatenated hash
            StringBuilder finalHash = new StringBuilder();
            foreach (uint hash in hashConstants)
            {
                finalHash.Append(Convert.ToString(hash, 16).PadLeft(8, '0'));
            }

            return finalHash.ToString();
        }


        // ---- Bitwise operations ----
        private static UInt32 s1(UInt32 x)
        {
            UInt32 RightRotate17, RightRotate19, RightShift10;

            RightRotate17 = (x >> 17) | (x << 15);
            RightRotate19 = (x >> 19) | (x << 13);
            RightShift10 = x >> 10;

            return RightRotate17 ^ RightRotate19 ^ RightShift10;
        }

        private static UInt32 s0(UInt32 x)
        {
            UInt32 RightRotate7, RightRotate18, RightShift3;

            RightRotate7 = (x >> 7) | (x << 25);
            RightRotate18 = (x >> 18) | (x << 14);
            RightShift3 = x >> 3;

            return RightRotate7 ^ RightRotate18 ^ RightShift3;
        }

        private static UInt32 choice(UInt32 x, UInt32 y, UInt32 z)
        {
            return (UInt32)((x & y) ^ ((~x) & z));
        }

        private static UInt32 S1(UInt32 x)
        {
            UInt32 RightRotate6, RightRotate11, RightRotate25;

            RightRotate6 = (x >> 6) | (x << 26);
            RightRotate11 = (x >> 11) | (x << 21);
            RightRotate25 = (x >> 25) | (x << 7);

            return RightRotate6 ^ RightRotate11 ^ RightRotate25;
        }

        private static UInt32 S0(UInt32 x)
        {
            UInt32 RightRotate2, RightRotate13, RightRotate22;

            RightRotate2 = (x >> 2) | (x << 30);
            RightRotate13 = (x >> 13) | (x << 19);
            RightRotate22 = (x >> 22) | (x << 10);

            return RightRotate2 ^ RightRotate13 ^ RightRotate22;
        }

        private static UInt32 major(UInt32 x, UInt32 y, UInt32 z)
        {
            return (x & y) ^ (x & z) ^ (y & z);
        }
    }
}
