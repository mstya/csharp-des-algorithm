using System.Collections.Generic;

namespace DES.Interfaces
{
    internal interface IPBoxService
    {
        List<bool> ApplyPBoxTo32(IList<bool> bits);

        List<bool> ApplyStraightPBox(IList<bool> bits);
    }
}