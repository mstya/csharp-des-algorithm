using System;
using System.Collections;
using System.Collections.Generic;
using DES.Util;

namespace DES.Services
{
    public class PBoxService
    {
        private const int EXPANDED_SEMIBLOCK = 48;

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

        private readonly List<int> straightPBoxPositions = new List<int>
        {
            16, 7,  20, 21, 29, 12, 28, 17,
            1,  15, 23, 26, 5,  18, 31, 10,
            2,  8,  24, 14, 32, 27, 3,  9,
            19, 13, 30, 6,  22, 11, 4,  25
        };

        public BitArray ApplyPBoxTo32(BitArray bits)
        {
            if (bits.Length != Constants.SEMIBLOCK_LENGTH)
            {
                throw new Exception("bits length = " + bits.Length);
            }

            bool[] pBoxedBits = new bool[EXPANDED_SEMIBLOCK];
            for (int i = 0; i < pBoxExpansion.Count; i++)
            {
                pBoxedBits[i] = bits[pBoxExpansion[i] - 1];
            }

            return new BitArray(pBoxedBits);
        }

        public BitArray ApplyStraightPBox(BitArray bits)
        {
            bool[] pBoxedBits = new bool[Constants.SEMIBLOCK_LENGTH];
            for (int i = 0; i < straightPBoxPositions.Count; i++)
            {
                pBoxedBits[i] = bits[straightPBoxPositions[i] - 1];
            }

            return new BitArray(pBoxedBits);
        }
    }
}
