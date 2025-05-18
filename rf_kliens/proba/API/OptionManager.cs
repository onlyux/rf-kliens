using Hotcakes.CommerceDTO.v1.Catalog;
using Hotcakes.CommerceDTO.v1;
using Kliens.Interfaces;
using Kliens.Wrappers;
using proba;
using System.Collections.Generic;
using System.Windows.Forms;
using System;

namespace Kliens.Managers
{
    public class OptionManager : IOptionManager
    {
        private readonly IApiProxy _apiProxy;

        public OptionManager(IApiProxy apiProxy)
        {
            _apiProxy = apiProxy;
        }

        public OptionManager(string url, string key) : this(new ApiProxy(url, key))
        {
        }

        public List<Options> LoadOptions()
        {
            var options = new List<Options>();
            try
            {
                ApiResponse<List<OptionDTO>> response = _apiProxy.ProductOptionsFindAll();

                if (response.Content != null)
                {
                    foreach (var item in response.Content)
                    {
                        options.Add(new Options
                        {
                            Name = item.Name,
                            Bvin = item.Bvin
                        });
                    }
                }
                else
                {
                    MessageBox.Show("No options found.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading options: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
            return options;
        }

        public List<Termekchoices> GetAssignedOptions(string productId)
        {
            var termekchoices = new List<Termekchoices>();
            try
            {
                ApiResponse<List<OptionDTO>> response = _apiProxy.ProductOptionsFindAllByProductId(productId);

                if (response.Content != null)
                {
                    foreach (var item in response.Content)
                    {
                        termekchoices.Add(new Termekchoices
                        {
                            Name = item.Name,
                            Bvin = item.Bvin,
                            StoreId = (int)item.StoreId
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching assigned options: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
            return termekchoices;
        }

        public bool AssignOptionToProduct(string optionId, string productId)
        {
            try
            {
                ApiResponse<bool> response = _apiProxy.ProductOptionsAssignToProduct(optionId, productId, false);
                var option = _apiProxy.ProductOptionsFind(optionId).Content;
                ApiResponse<OptionDTO> response1 = _apiProxy.ProductOptionsUpdate(option);
                return response.Content;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error assigning option: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool UnassignOptionFromProduct(string optionId, string productId)
        {
            try
            {
                ApiResponse<bool> response = _apiProxy.ProductOptionsUnassignFromProduct(optionId, productId);
                var option = _apiProxy.ProductOptionsFind(optionId).Content;
                ApiResponse<OptionDTO> response1 = _apiProxy.ProductOptionsUpdate(option);
                return response.Content;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error unassigning option: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool DeleteOption(string optionId)
        {
            try
            {
                ApiResponse<bool> response = _apiProxy.ProductOptionsDelete(optionId);
                return response.Content;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting option: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
