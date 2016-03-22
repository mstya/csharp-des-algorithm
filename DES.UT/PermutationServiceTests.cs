using System.Collections.Generic;
using DES.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace DES.UT
{
    [TestFixture]
    public class PermutationServiceTests
    {
        private IPermutationService permutationService;
        private bool[] bitsArray;
        private bool[] expectedArray;

        [SetUp]
        public void SetUp()
        {
            this.permutationService = Substitute.For<IPermutationService>();
            this.bitsArray = new[]
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

            this.expectedArray = new[]
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
        }

        [Test]
        public void InitialPermutation_PassValid64BitSequence_GetPermutedSequence()
        {
            List<bool> bits = new List<bool>(this.bitsArray);
            this.permutationService.InitialPermutation(ref bits);
        }
    }
}