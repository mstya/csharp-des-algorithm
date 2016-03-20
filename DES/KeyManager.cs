﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DES
{
    class KeyManager
    {
        private BitArray keyBits;

        public List<bool> Key64Bits { get; private set; }

        static private List<int> replaceKeyPositions = new List<int>()
        {
            57, 49, 41, 33, 25, 17,  9,  1, 58, 50, 42, 34, 26, 18,
            10,  2, 59, 51, 43, 35, 27, 19, 11,  3, 60, 52, 44, 36,
            63, 55, 47, 39, 31, 23, 15,  7, 62, 54, 46, 38, 30, 22,
            14,  6, 61, 53, 45, 37, 29, 21, 13,  5, 28, 20, 12,  4
        };

        static private Dictionary<int, int> keyShifts = new Dictionary<int, int>()
        {
            { 0,  1 },
            { 1,  1 },
            { 2,  2 },
            { 3,  2 },
            { 4,  2 },
            { 5,  2 },
            { 6,  2 },
            { 7,  2 },
            { 8,  1 },
            { 9, 2 },
            { 10, 2 },
            { 11, 2 },
            { 12, 2 },
            { 13, 2 },
            { 14, 2 },
            { 15, 1 }
        };

        static private List<int> keyCompression = new List<int>()
        {
            14, 17, 11, 24,  1,  5,  3, 28,
            15,  6, 21, 10, 23, 19, 12,  4,
            26,  8, 16,  7, 27, 20, 13,  2,
            41, 52, 31, 37, 47, 55, 30, 40,
            51, 45, 33, 48, 44, 49, 39, 56,
            34, 53, 46, 42, 50, 36, 29, 32
        };

        private List<bool> replacedKeyList;

        public KeyManager(string key)
        {
            byte[] keyBytes = Encoding.ASCII.GetBytes(key);
            this.keyBits = new BitArray(keyBytes);

            if (this.keyBits.Count != 56)
            {
                throw new Exception("key bits length = " + this.keyBits.Count);
            }
        }

        public void Generate64BitKey()
        {
            this.Key64Bits = this.keyBits.Cast<bool>().ToList();

            for (int i = 0; i < Key64Bits.Count; i += 7)
            {
                int countOf1InByte = Key64Bits.Skip(i).Take(7).Count(x => x);
                int insertPosition = i + 7;

                if (countOf1InByte % 2 == 0)
                {
                    Key64Bits.Insert(insertPosition, true);
                }
                else
                {
                    Key64Bits.Insert(insertPosition, false);
                }

                i += 1;
            }
        }

        public void ReplaceAndRemoveKeyBits()
        {
            //BitArray bitArray = new BitArray(56);
            List<bool> bitList = new List<bool>();

            for (int i = 0; i < replaceKeyPositions.Count; i++)
            {
                bool bit = this.Key64Bits[replaceKeyPositions[i] - 1];
               // bitArray[i] = bit;
                bitList.Add(bit);
            }

            //this.ReplacedKey = bitArray;
            this.replacedKeyList = bitList;
        }

        public BitArray GenerateRoundKey(int roundIndex)
        {
            if (roundIndex < 0 || roundIndex > 16)
            {
                throw new ArgumentException("Ivalid round index");
            }

            List<bool> c0 = this.replacedKeyList.Take(28).ToList();
            List<bool> d0 = this.replacedKeyList.Skip(28).Take(28).ToList();

            int shiftValue;
            keyShifts.TryGetValue(roundIndex, out shiftValue);

            List<bool> c0Shifted = BitHelper.ShiftLeft(c0, shiftValue);
            List<bool> d0Shifted = BitHelper.ShiftLeft(d0, shiftValue);

            c0Shifted.AddRange(d0Shifted);

            return this.CompressRoundKey(c0Shifted);
        }

        private BitArray CompressRoundKey(List<bool> array)
        {
            BitArray bitArray = new BitArray(48);
            for (int i = 0; i < keyCompression.Count; i++)
            {
                bitArray[i] = array[keyCompression[i] - 1];
            }

            return bitArray;
        }
    }
}