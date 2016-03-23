using System.Collections.Generic;

namespace DES.Interfaces
{
    internal interface IKeyService
    {
        List<bool> GenerateRoundKey(int roundIndex);
    }
}