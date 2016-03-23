using System;
using System.Collections.Generic;
using DES.Interfaces;
using DES.Util;

namespace DES.Services
{
    public class PBoxService : IPBoxService
    {
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

        public List<bool> ApplyPBoxTo32(IList<bool> bits)
        {
            if (bits.Count != Constants.SEMIBLOCK_LENGTH)
            {
                throw new Exception("bits length = " + bits.Count);
            }

            List<bool> pBoxedBits = new List<bool>();
            for (int i = 0; i < pBoxExpansion.Count; i++)
            {
                pBoxedBits.Add(bits[pBoxExpansion[i] - 1]);
            }

            return pBoxedBits;
        }

        public List<bool> ApplyStraightPBox(IList<bool> bits)
        {
            List<bool> pBoxedBits = new List<bool>();
            for (int i = 0; i < straightPBoxPositions.Count; i++)
            {
                pBoxedBits.Add(bits[straightPBoxPositions[i] - 1]);
            }

            return pBoxedBits;
        }
    }
}