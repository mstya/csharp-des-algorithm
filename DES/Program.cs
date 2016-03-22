using System;
using System.Collections;
using System.Text;

namespace DES
{
    class Program
    {
        static void Main(string[] args)
        {
            //KeyService manager = new KeyService("1234567");
            //BitArray roundKey = manager.GenerateRoundKey(0);

            //Console.WriteLine(roundKey.Count);

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

            BitArray bits = new BitArray(bitsArray);
            //byte[] bytes2 = BitArrayToByteArray(bits);
            //Console.WriteLine(Encoding.Default.GetString(bytes2));
            //BitHelper.PrintBitArray(bits);

            DesAlgorithm algorithm = new DesAlgorithm("1234567");
            //BitArray key = algorithm.InitialPermutation(bits);
            BitArray result = algorithm.RunDes(bits);

            byte[] bytes = BitArrayToByteArray(result);
            Console.WriteLine(Encoding.Default.GetString(bytes));
            BitHelper.PrintBitArray(result);

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