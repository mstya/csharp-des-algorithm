using System.Collections.Generic;

namespace DES.Interfaces
{
    internal interface ISBoxService
    {
        List<bool> ReplaceWithSBoxes(IList<bool> bits48);
    }
}