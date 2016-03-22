using System.Collections.Generic;

namespace DES.Interfaces
{
    public interface IPermutationService
    {
        void InitialPermutation(ref List<bool> bits);

        void FinialPermutation(ref List<bool> bits);
    }
}