using Hotcakes.CommerceDTO.v1;
using Hotcakes.CommerceDTO.v1.Catalog;
using System.Collections.Generic;

namespace YourNamespace.ApiWrappers
{
    public interface IProductApiWrapper
    {
        ApiResponse<List<ProductDTO>> ProductsFindAll();
    }

    public interface IOptionApiWrapper
    {
        ApiResponse<List<OptionDTO>> ProductOptionsFindAll();
        ApiResponse<List<OptionDTO>> ProductOptionsFindAllByProductId(string productId);
        ApiResponse<bool> ProductOptionsAssignToProduct(string optionId, string productId, bool isShared);
        ApiResponse<bool> ProductOptionsUnassignFromProduct(string optionId, string productId);
        ApiResponse<bool> ProductOptionsDelete(string optionId);
        ApiResponse<OptionDTO> ProductOptionsCreate(OptionDTO option);
    }

    public class HotcakesApiWrapper : IProductApiWrapper, IOptionApiWrapper
    {
        private readonly Api _proxy;

        public HotcakesApiWrapper(string url, string key)
        {
            _proxy = new Api(url, key);
        }

        public ApiResponse<List<ProductDTO>> ProductsFindAll()
        {
            return _proxy.ProductsFindAll();
        }

        public ApiResponse<List<OptionDTO>> ProductOptionsFindAll()
        {
            return _proxy.ProductOptionsFindAll();
        }

        public ApiResponse<List<OptionDTO>> ProductOptionsFindAllByProductId(string productId)
        {
            return _proxy.ProductOptionsFindAllByProductId(productId);
        }

        public ApiResponse<bool> ProductOptionsAssignToProduct(string optionId, string productId, bool isShared)
        {
            return _proxy.ProductOptionsAssignToProduct(optionId, productId, isShared);
        }

        public ApiResponse<bool> ProductOptionsUnassignFromProduct(string optionId, string productId)
        {
            return _proxy.ProductOptionsUnassignFromProduct(optionId, productId);
        }

        public ApiResponse<bool> ProductOptionsDelete(string optionId)
        {
            return _proxy.ProductOptionsDelete(optionId);
        }

        public ApiResponse<OptionDTO> ProductOptionsCreate(OptionDTO option)
        {
            return _proxy.ProductOptionsCreate(option);
        }
    }
}