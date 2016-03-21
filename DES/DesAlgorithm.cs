using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DES
{
    internal class DesAlgorithm
    {
        private readonly List<int> initialPermutationIndexes = new List<int>
        {
            58, 50, 42, 34, 26, 18, 10, 2,
            60, 52, 44, 36, 28, 20, 12, 4,
            62, 54, 46, 38, 30, 22, 14, 6,
            64, 56, 48, 40, 32, 24, 16, 8,
            57, 49, 41, 33, 25, 17,  9, 1,
            59, 51, 43, 35, 27, 19, 11, 3,
            61, 53, 45, 37, 29, 21, 13, 5,
            63, 55, 47, 39, 31, 23, 15, 7
        };

        private readonly List<int> pBoxExpansion = new List<int>
        {
            32,  1,  2,  3,  4,  5,
             4,  5,  6,  7,  8,  9,
             8,  9, 10, 11, 12, 13,
            12, 13, 14, 15, 16, 17,
            16, 17, 18, 19, 20, 21,
            20, 21, 22, 23, 24, 25,
            24, 25, 26, 27, 28, 29,
            28, 29, 30, 31, 32,  1
        };

        private KeyManager keyManager;

        public DesAlgorithm(string key)
        {
            this.keyManager = new KeyManager(key);
        }

        public BitArray InitialPermutation(BitArray bits)
        {
            if (bits.Length != 64)
            {
                throw new Exception("bits length = " + bits.Length);
            }

            BitArray bitArray = new BitArray(64);
            for (int i = 0; i < bits.Length; i++)
            {
                bitArray[i] = bits[initialPermutationIndexes[i] - 1];
            }

            return bitArray;
        }

        public BitArray ApplyPBoxTo32(BitArray bits)
        {
            if (bits.Length != 32)
            {
                throw new Exception("bits length = " + bits.Length);
            }

            //List<bool> pBoxedBits = new List<bool>();
            bool[] pBoxedBits = new bool[48];
            for (int i = 0; i < pBoxExpansion.Count; i++)
            {
                pBoxedBits[i] = bits[pBoxExpansion[i] - 1];
            }

            return new BitArray(pBoxedBits);
        }

        public BitArray GetLeft32Bits(BitArray array64)
        {
            IEnumerable<bool> bit32Collection = array64.Cast<bool>().Take(32);
            return new BitArray(bit32Collection.ToArray());
        }

        public BitArray GetRight32Bits(BitArray array64)
        {
            IEnumerable<bool> bit32Collection = array64.Cast<bool>().Skip(32).Take(32);
            return new BitArray(bit32Collection.ToArray());
        }

        public void RunDes(BitArray bits)
        {
            BitArray permutedBits = InitialPermutation(bits);
            BitArray left32 = GetLeft32Bits(permutedBits);
            BitArray rigth32 = GetRight32Bits(permutedBits);

            DesFunction(rigth32);
        }

        public void DesFunction(BitArray rigth32)
        {
            BitArray pBoxedRight48 = ApplyPBoxTo32(rigth32);

            BitArray roundKey = keyManager.GenerateRoundKey(0);

            //BitHelper.PrintBitArray(pBoxedRight32);


            pBoxedRight48.Xor(roundKey);

            //BitHelper.PrintBitArray(pBoxedRight32);
            //BitHelper.PrintBitArray(roundKey);
            //BitHelper.PrintBitArray(pBoxedRight32);
            //Console.WriteLine(pBoxedRight32.Count);

            SBoxService sBoxService = new SBoxService();
            sBoxService.ReplaceWithSBoxes(pBoxedRight48);
        }
    }
}