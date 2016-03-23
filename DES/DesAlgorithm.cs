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

        private readonly List<BitArray> roundKeys = new List<BitArray>(); 

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

        private void GenerateKeys()
        {
            for (var i = 0; i < ROUND_AMOUNT; i++)
            {
                this.roundKeys.Add(this.keyService.GenerateRoundKey(i));
            }
        }

        public BitArray RunDes(BitArray bits)
        {
            this.GenerateKeys();

            List<bool> bitList = bits.Cast<bool>().ToList();

            this.permutationService.InitialPermutation(ref bitList);

            BitArray left32 = this.GetLeft32Bits(bitList);
            BitArray rigth32 = this.GetRight32Bits(bitList);

            for (int i = 0; i < ROUND_AMOUNT; i++)
            {
                BitArray temp = (BitArray)left32.Clone();
                left32 = rigth32;
                rigth32 = temp.Xor(this.DesFunction(rigth32, this.roundKeys[i]));
            }

            List<bool> fullBits = new List<bool>();

            fullBits.AddRange(rigth32.Cast<bool>());
            fullBits.AddRange(left32.Cast<bool>());

            this.permutationService.FinialPermutation(ref fullBits);

            return new BitArray(fullBits.ToArray());
        }

        public BitArray RunUnDes(BitArray bits)
        {
            List<bool> bitList = bits.Cast<bool>().ToList();

            this.permutationService.InitialPermutation(ref bitList);

            BitArray left32 = this.GetLeft32Bits(bitList);
            BitArray rigth32 = this.GetRight32Bits(bitList);

            for (int i = 0; i < ROUND_AMOUNT; i++)
            {
                BitArray temp = (BitArray)left32.Clone();
                left32 = rigth32;
                rigth32 = temp.Xor(this.DesFunction(rigth32, this.roundKeys[15 - i]));
            }

            List<bool> fullBits = new List<bool>();

            fullBits.AddRange(rigth32.Cast<bool>());
            fullBits.AddRange(left32.Cast<bool>());

            this.permutationService.FinialPermutation(ref fullBits);

            return new BitArray(fullBits.ToArray());
        }

        public BitArray DesFunction(BitArray rigth32, BitArray roundKey)
        {
            BitArray pBoxedRight48 = this.pBoxService.ApplyPBoxTo32(rigth32);

            pBoxedRight48.Xor(roundKey);

            SBoxService sBoxService = new SBoxService();
            BitArray sBoxedArray = sBoxService.ReplaceWithSBoxes(pBoxedRight48);

            sBoxedArray = this.pBoxService.ApplyStraightPBox(sBoxedArray);

            return sBoxedArray;
        }
    }
}