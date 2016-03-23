using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DES.Extensions;
using DES.Interfaces;

namespace DES.Services
{
    internal class KeyService : IKeyService
    {
        private const int KEY_LENGTH = 56;

        private readonly BitArray keyBits;

        public List<bool> Key64Bits { get; private set; }

        private static readonly List<int> replaceKeyPositions = new List<int>
        {
            57, 49, 41, 33, 25, 17,  9,  1, 58, 50, 42, 34, 26, 18,
            10,  2, 59, 51, 43, 35, 27, 19, 11,  3, 60, 52, 44, 36,
            63, 55, 47, 39, 31, 23, 15,  7, 62, 54, 46, 38, 30, 22,
            14,  6, 61, 53, 45, 37, 29, 21, 13,  5, 28, 20, 12,  4
        };

        private static readonly Dictionary<int, int> keyShifts = new Dictionary<int, int>
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

        private static readonly List<int> keyCompression = new List<int>
        {
            14, 17, 11, 24,  1,  5,  3, 28,
            15,  6, 21, 10, 23, 19, 12,  4,
            26,  8, 16,  7, 27, 20, 13,  2,
            41, 52, 31, 37, 47, 55, 30, 40,
            51, 45, 33, 48, 44, 49, 39, 56,
            34, 53, 46, 42, 50, 36, 29, 32
        };

        private List<bool> replacedKeyList;

        public KeyService(string key)
        {
            byte[] keyBytes = Encoding.ASCII.GetBytes(key);
            this.keyBits = new BitArray(keyBytes);

            if (this.keyBits.Count != KEY_LENGTH)
            {
                throw new Exception("key bits length = " + this.keyBits.Count);
            }
        }

        private void Generate64BitKey()
        {
            int vectorLength = 7;
            this.Key64Bits = this.keyBits.Cast<bool>().ToList();
            int count = Key64Bits.Count;

            for (int i = 0; i < count; i += vectorLength)
            {
                int countOf1InByte = Key64Bits.Skip(i).Take(vectorLength).Count(x => x);
                int insertPosition = i + vectorLength;

                Key64Bits.Insert(insertPosition, countOf1InByte%2 == 0);

                i += 1;
            }
        }

        private void ReplaceAndRemoveKeyBits()
        {
            List<bool> bitList = new List<bool>();

            for (int i = 0; i < replaceKeyPositions.Count; i++)
            {
                bool bit = this.Key64Bits[replaceKeyPositions[i] - 1];
                bitList.Add(bit);
            }

            this.replacedKeyList = bitList;
        }

        public List<bool> GenerateRoundKey(int roundIndex)
        {
            if (roundIndex < 0 || roundIndex > 16)
            {
                throw new ArgumentException("Ivalid round index");
            }

            int keyPartLength = 28;

            this.Generate64BitKey();
            this.ReplaceAndRemoveKeyBits();

            List<bool> c0 = this.replacedKeyList.Take(keyPartLength).ToList();
            List<bool> d0 = this.replacedKeyList.Skip(keyPartLength).Take(keyPartLength).ToList();

            int shiftValue;
            keyShifts.TryGetValue(roundIndex, out shiftValue);

            List<bool> c0Shifted = c0.ShiftLeft(shiftValue);
            List<bool> d0Shifted = d0.ShiftLeft(shiftValue);

            c0Shifted.AddRange(d0Shifted);

            return this.CompressRoundKey(c0Shifted);
        }

        private List<bool> CompressRoundKey(IList<bool> array)
        {
            List<bool> bitArray = new List<bool>();
            for (int i = 0; i < keyCompression.Count; i++)
            {
                bitArray.Add(array[keyCompression[i] - 1]);
            }

            return bitArray;
        }
    }
}