using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DES.Extensions;
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

        private readonly List<List<bool>> roundKeys = new List<List<bool>>(); 

        public DesAlgorithm(string key)
        {
            this.keyService = new KeyService(key);
            this.permutationService = new PermutationService();
            this.pBoxService = new PBoxService();
            this.GenerateKeys();
        }

        private List<bool> GetLeft32Bits(IList<bool> list64)
        {
            return list64.Take(Constants.SEMIBLOCK_LENGTH).ToList();
        }

        private List<bool> GetRight32Bits(IList<bool> list64)
        {
            return list64.Skip(Constants.SEMIBLOCK_LENGTH).Take(Constants.SEMIBLOCK_LENGTH).ToList();
        }

        private void GenerateKeys()
        {
            for (var i = 0; i < ROUND_AMOUNT; i++)
            {
                this.roundKeys.Add(this.keyService.GenerateRoundKey(i));
            }
        }

        public List<bool> RunDes(IList<bool> bits)
        {
            List<bool> bitList = bits.ToList();

            this.permutationService.InitialPermutation(ref bitList);

            List<bool> left32 = this.GetLeft32Bits(bitList);
            List<bool> rigth32 = this.GetRight32Bits(bitList);

            for (int i = 0; i < ROUND_AMOUNT; i++)
            {
                List<bool> temp = left32.ToList();
                left32 = rigth32;
                rigth32 = temp.Xor(this.DesFunction(rigth32, this.roundKeys[i]));
            }

            List<bool> fullBits = new List<bool>();

            fullBits.AddRange(rigth32);
            fullBits.AddRange(left32);

            this.permutationService.FinialPermutation(ref fullBits);

            return fullBits;
        }

        public List<bool> RunUnDes(IList<bool> bits)
        {
            List<bool> bitList = bits.ToList();

            this.permutationService.InitialPermutation(ref bitList);

            List<bool> left32 = this.GetLeft32Bits(bitList);
            List<bool> rigth32 = this.GetRight32Bits(bitList);

            for (int i = 0; i < ROUND_AMOUNT; i++)
            {
                List<bool> temp = left32.ToList();
                left32 = rigth32;
                rigth32 = temp.Xor(this.DesFunction(rigth32, this.roundKeys[15 - i]));
            }

            List<bool> fullBits = new List<bool>();

            fullBits.AddRange(rigth32);
            fullBits.AddRange(left32);

            this.permutationService.FinialPermutation(ref fullBits);

            return fullBits;
        }

        private List<bool> DesFunction(IList<bool> rigth32, IList<bool> roundKey)
        {
            List<bool> pBoxedRight48 = this.pBoxService.ApplyPBoxTo32(rigth32);

            pBoxedRight48 = pBoxedRight48.Xor(roundKey);

            SBoxService sBoxService = new SBoxService();
            List<bool> sBoxedArray = sBoxService.ReplaceWithSBoxes(pBoxedRight48);

            return this.pBoxService.ApplyStraightPBox(sBoxedArray);
        }
    }
}