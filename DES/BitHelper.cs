using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DES
{
    static class BitHelper
    {
        public static void PrintBitArray(BitArray array)
        {
            foreach (bool item in array)
            {
                Console.Write(item ? "1" : "0");
            }

            Console.WriteLine();
        }

        public static void PrintBitArray(List<bool> bits)
        {
            foreach (var item in bits)
            {
                Console.Write(item ? "1" : "0");
            }

            Console.WriteLine();
        }

        public static BitArray ShiftLeft(BitArray aSource, int shiftOn)
        {
            bool[] new_arr = new bool[aSource.Count];
            for (int i = 0; i < aSource.Count - shiftOn; i++)
            {
                new_arr[i] = aSource[i + shiftOn];
            }

            return new BitArray(new_arr);
        }

        public static List<bool> ShiftLeft(List<bool> aSource, int shiftOn)
        {
            bool[] new_arr = new bool[aSource.Count];
            for (int i = 0; i < aSource.Count - shiftOn; i++)
            {
                new_arr[i] = aSource[i + shiftOn];
            }

            return new_arr.ToList();
        }
    }
}
