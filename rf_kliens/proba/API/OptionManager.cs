using Hotcakes.CommerceDTO.v1;
using Hotcakes.CommerceDTO.v1.Catalog;
using Hotcakes.CommerceDTO.v1.Client;
using proba;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace YourNamespace.Managers
{
    public class OptionManager
    {
        private readonly string _url;
        private readonly string _key;

        public OptionManager(string url, string key)
        {
            _url = url;
            _key = key;
        }

        public List<Options> LoadOptions()
        {
            var options = new List<Options>();
            try
            {
                Api proxy = new Api(_url, _key);
                ApiResponse<List<OptionDTO>> response = proxy.ProductOptionsFindAll();

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
                Api proxy = new Api(_url, _key);
                ApiResponse<List<OptionDTO>> response = proxy.ProductOptionsFindAllByProductId(productId);

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
                Api proxy = new Api(_url, _key);
                ApiResponse<bool> response = proxy.ProductOptionsAssignToProduct(optionId, productId, false);
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
                Api proxy = new Api(_url, _key);
                ApiResponse<bool> response = proxy.ProductOptionsUnassignFromProduct(optionId, productId);
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
                Api proxy = new Api(_url, _key);
                ApiResponse<bool> response = proxy.ProductOptionsDelete(optionId);
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