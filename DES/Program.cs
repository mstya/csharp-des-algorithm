using System;
using System.Collections;
using System.Text;
using DES.Services;

namespace DES
{
    class Program
    {
        static void Main(string[] args)
        {
            //byte[] asciiKey = Encoding.ASCII.GetBytes("1234567");
            //KeyService manager = new KeyService(Encoding.ASCII.GetString(asciiKey));
            //BitArray array = manager.GenerateRoundKey(0);
            //BitHelper.PrintBitArray(array);
            //BitHelper.PrintBitArray(manager.Key64Bits);
            //BitArray roundKey = manager.GenerateRoundKey(0);

            //Console.WriteLine(roundKey.Count);

            //return;
            //string testText = "AABB09182736CCDD";
            //Console.WriteLine(new BitArray(Encoding.ASCII.GetBytes(testText)).Count);
            //return;
            //   return;



            bool[] bitsArray =
            {
                false, false, false, false, false, false, false, false,
                false, false, false, false, false, false, true, false,
                false, false, false, false, false, false, false, false,
                false, false, false, false, false, false, false, false,
                false, false, false, false, false, false, false, false,
                false, false, false, false, false, false, false, false,
                false, false, false, false, false, false, false, false,
                false, false, false, false, false, false, false, true
            };

         //   bool[] bitsArray = bitsArray;// new BitArray(Encoding.ASCII.GetBytes(testText).Ca);

            BitArray bits = new BitArray(bitsArray);
            BitHelper.PrintBitArray(bits);
            DesAlgorithm algorithm = new DesAlgorithm("1234567");
            BitArray result = algorithm.RunDes(bits);

            BitHelper.PrintBitArray(result);

            BitArray initial = algorithm.RunUnDes(result);
            BitHelper.PrintBitArray(initial);

            //   BitArray initial = algorithm.RunDes(result);

            //    byte[] bytes = BitArrayToByteArray(result);
            //    Console.WriteLine(Encoding.Default.GetString(bytes));
            //BitHelper.PrintBitArray(result);

            //algorithm.ReplaceKeyBits(key);
        }

        public static byte[] BitArrayToByteArray(BitArray bits)
        {
            byte[] ret = new byte[(bits.Length - 1) / 8 + 1];
            bits.CopyTo(ret, 0);
            return ret;
        }
    }
}