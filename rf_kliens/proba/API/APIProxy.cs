using Hotcakes.CommerceDTO.v1.Catalog;
using Hotcakes.CommerceDTO.v1.Client;
using Hotcakes.CommerceDTO.v1;
using Kliens.Interfaces;
using System.Collections.Generic;

namespace Kliens.Wrappers
{
    public class ApiProxy : IApiProxy
    {
        private readonly Api _api;

        public ApiProxy(string url, string key)
        {
            _api = new Api(url, key);
        }

        public ApiResponse<List<OptionDTO>> ProductOptionsFindAll()
        {
            return _api.ProductOptionsFindAll();
        }

        public ApiResponse<List<OptionDTO>> ProductOptionsFindAllByProductId(string productId)
        {
            return _api.ProductOptionsFindAllByProductId(productId);
        }

        public ApiResponse<bool> ProductOptionsAssignToProduct(string optionId, string productId, bool shared)
        {
            return _api.ProductOptionsAssignToProduct(optionId, productId, shared);
        }

        public ApiResponse<bool> ProductOptionsUnassignFromProduct(string optionId, string productId)
        {
            return _api.ProductOptionsUnassignFromProduct(optionId, productId);
        }

        public ApiResponse<bool> ProductOptionsDelete(string optionId)
        {
            return _api.ProductOptionsDelete(optionId);
        }

        public ApiResponse<OptionDTO> ProductOptionsCreate(OptionDTO option)
        {
            return _api.ProductOptionsCreate(option);
        }

        public ApiResponse<OptionDTO> ProductOptionsUpdate(OptionDTO option)
        {
            return _api.ProductOptionsUpdate(option);
        }

        public ApiResponse<List<ProductDTO>> ProductsFindAll()
        {
            return _api.ProductsFindAll();
        }

        public ApiResponse<OptionDTO> ProductOptionsFind(string optionId)
        {
            return _api.ProductOptionsFind(optionId);
        }
    }
}