using Hotcakes.CommerceDTO.v1;
using Hotcakes.CommerceDTO.v1.Catalog;
using Hotcakes.CommerceDTO.v1.Client;
using System.Collections.Generic;

namespace Service.Services
{
    public class HotcakesService
    {
        private readonly Api _proxy;

        public HotcakesService(string url, string key)
        {
            _proxy = new Api(url, key);
        }

        public List<OptionDTO> GetAllOptions()
        {
            return _proxy.ProductOptionsFindAll().Content;
        }

        public bool AssignOption(string optionId, string productId)
        {
            return _proxy.ProductOptionsAssignToProduct(optionId, productId, false).Content;
        }

        public bool UnassignOption(string optionId, string productId)
        {
            return _proxy.ProductOptionsUnassignFromProduct(optionId, productId).Content;
        }

        public bool DeleteOption(string optionId)
        {
            return _proxy.ProductOptionsDelete(optionId).Content;
        }
    }
}