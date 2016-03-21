using System.Collections;

namespace DES
{
    class Program
    {
        static void Main(string[] args)
        {
            //KeyManager manager = new KeyManager("1234567");
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

            //BitHelper.PrintBitArray(bits);

            DesAlgorithm algorithm = new DesAlgorithm("1234567");
            //BitArray key = algorithm.InitialPermutation(bits);
            algorithm.RunDes(bits);

            //BitHelper.PrintBitArray(key);

            //algorithm.ReplaceKeyBits(key);
        }
    }
}