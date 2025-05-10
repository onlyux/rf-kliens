using proba;
using System.Collections.Generic;

namespace Kliens.Interfaces
{
    public interface IOptionManager
    {
        List<Options> LoadOptions();
        List<Termekchoices> GetAssignedOptions(string productId);
        bool AssignOptionToProduct(string optionId, string productId);
        bool UnassignOptionFromProduct(string optionId, string productId);
        bool DeleteOption(string optionId);
    }
}