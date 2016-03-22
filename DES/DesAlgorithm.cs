using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DES.Services;
using DES.Util;

namespace DES
{
    internal class DesAlgorithm
    {
        private const int ROUND_AMOUNT = 16;

        private readonly KeyService keyService;

        private readonly PermutationService permutationService;

        private readonly PBoxService pBoxService;

        public DesAlgorithm(string key)
        {
            this.keyService = new KeyService(key);
            this.permutationService = new PermutationService();
            this.pBoxService = new PBoxService();
        }

        public BitArray GetLeft32Bits(IList<bool> list64)
        {
            return new BitArray(list64.Take(Constants.SEMIBLOCK_LENGTH).ToArray());
        }

        public BitArray GetRight32Bits(IList<bool> list64)
        {
            return new BitArray(list64.Skip(Constants.SEMIBLOCK_LENGTH).Take(Constants.SEMIBLOCK_LENGTH).ToArray());
        }

        public BitArray RunDes(BitArray bits)
        {
            List<bool> bitList = bits.Cast<bool>().ToList();

            this.permutationService.InitialPermutation(ref bitList);

            BitArray left32 = this.GetLeft32Bits(bitList);
            BitArray rigth32 = this.GetRight32Bits(bitList);

            for (int i = 0; i < ROUND_AMOUNT; i++)
            {
                rigth32 = this.DesFunction(rigth32, i);
                BitArray left = left32.Xor(rigth32);
                left32 = rigth32;
                rigth32 = left;
            }

            List<bool> fullBits = new List<bool>();

            fullBits.AddRange(left32.Cast<bool>());
            fullBits.AddRange(rigth32.Cast<bool>());

            this.permutationService.FinialPermutation(ref fullBits);
            return new BitArray(fullBits.ToArray());
        }

        public BitArray DesFunction(BitArray rigth32, int roundIndex)
        {
            BitArray pBoxedRight48 = this.pBoxService.ApplyPBoxTo32(rigth32);

            BitArray roundKey = keyService.GenerateRoundKey(roundIndex);

            pBoxedRight48.Xor(roundKey);

            SBoxService sBoxService = new SBoxService();
            BitArray sBoxedArray = sBoxService.ReplaceWithSBoxes(pBoxedRight48);

            sBoxedArray = this.pBoxService.ApplyStraightPBox(sBoxedArray);

            return sBoxedArray;
        }
    }
}