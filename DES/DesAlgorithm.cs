using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DES
{
    class DesAlgorithm
    {
        private BitArray keyBits;

        private List<int> replacePositions = new List<int>()
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

        private List<int> expansionPositions = new List<int>()
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

        public DesAlgorithm(string key)
        {
            byte[] keyBytes = Encoding.ASCII.GetBytes(key);
            this.keyBits = new BitArray(keyBytes);

            if (this.keyBits.Count != 56)
            {
                throw new Exception("key bits length = " + this.keyBits.Count);
            }
        }

        public BitArray InitialPermutation(BitArray bits)
        {
            if (bits.Length != 64)
            {
                throw new Exception("bits length = " + bits.Length);
            }

            BitArray bitArray = new BitArray(64);
            string replacedBits = string.Empty;
            for (int i = 0; i < bits.Length; i++)
            {
                bitArray[i] = bits[replacePositions[i] - 1];
            }

            return bitArray;
        }

        public string BitExpansion(BitArray bits)
        {
            if (bits.Length != 32)
            {
                throw new Exception("bits length = " + bits.Length);
            }

            string expansion = string.Empty;

            for (int i = 0; i < expansionPositions.Count; i++)
            {
                expansion += bits[expansionPositions[i] - 1];
            }

            return expansion;
        }

        public static BitArray GetLeft32Bits(BitArray array64)
        {
            IEnumerable<bool> bit32Collection = array64.Cast<bool>().Take(32);
            return new BitArray(bit32Collection.ToArray());
        }

        public static BitArray GetRight32Bits(BitArray array64)
        {
            IEnumerable<bool> bit32Collection = array64.Cast<bool>().Skip(32).Take(32);
            return new BitArray(bit32Collection.ToArray());
        }
    }
}