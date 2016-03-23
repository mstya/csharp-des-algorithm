using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DES.Extensions
{
    internal static class ListExtensions
    {
        public static void PrintBitArray(this IList<bool> bits)
        {
            foreach (bool item in bits)
            {
                Console.Write(item ? "1" : "0");
            }

            Console.WriteLine();
        }

        public static void PrintBitArray(this IList<int> bits)
        {
            foreach (int item in bits)
            {
                Console.Write(item);
            }

            Console.WriteLine();
        }

        public static List<bool> ShiftLeft(this IList<bool> sourceList, int shiftOn)
        {
            List<bool> shifted = new List<bool>(new bool[28]);

            for (int i = 0; i < sourceList.Count - shiftOn; i++)
            {
                shifted[i] = sourceList[i + shiftOn];
            }

            return shifted;
        }

        public static List<bool> Xor(this IList<bool> left, IList<bool> rigth)
        {
            return left.Select((leftItem, i) => leftItem ^ rigth[i]).ToList();
        }
    }
}