using System;
using System.Collections;

namespace DES.Extensions
{
    public static class BitArrayExtensions
    {
        public static byte[] ToByteArray(this BitArray bits)
        {
            byte[] ret = new byte[(bits.Length - 1) / 8 + 1];
            bits.CopyTo(ret, 0);
            return ret;
        }

        public static void PrintBitArray(this BitArray array)
        {
            foreach (bool item in array)
            {
                Console.Write(item ? "1" : "0");
            }

            Console.WriteLine();
        }
    }
}