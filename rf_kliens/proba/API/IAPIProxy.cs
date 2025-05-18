using Hotcakes.CommerceDTO.v1.Catalog;
using Hotcakes.CommerceDTO.v1;
using System.Collections.Generic;

namespace Kliens.Interfaces
{
    public interface IApiProxy
    {
        ApiResponse<List<OptionDTO>> ProductOptionsFindAll();
        ApiResponse<List<OptionDTO>> ProductOptionsFindAllByProductId(string productId);
        ApiResponse<bool> ProductOptionsAssignToProduct(string optionId, string productId, bool shared);
        ApiResponse<bool> ProductOptionsUnassignFromProduct(string optionId, string productId);
        ApiResponse<bool> ProductOptionsDelete(string optionId);
        ApiResponse<OptionDTO> ProductOptionsCreate(OptionDTO option);
        ApiResponse<OptionDTO> ProductOptionsUpdate(OptionDTO option);
        ApiResponse<List<ProductDTO>> ProductsFindAll();
        ApiResponse<OptionDTO> ProductOptionsFind(string optionId);
    }
}