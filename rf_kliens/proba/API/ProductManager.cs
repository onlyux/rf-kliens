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
    public class ProductManager : IProductManager
    {
        private readonly IApiProxy _apiProxy;

        public ProductManager(IApiProxy apiProxy)
        {
            _apiProxy = apiProxy;
        }

        public ProductManager(string url, string key) : this(new ApiProxy(url, key))
        {
        }

        public List<Termekek> LoadProducts()
        {
            var termekek = new List<Termekek>();
            try
            {
                ApiResponse<List<ProductDTO>> api_termek = _apiProxy.ProductsFindAll();

                if (api_termek.Content != null)
                {
                    foreach (var item in api_termek.Content)
                    {
                        termekek.Add(new Termekek
                        {
                            Bvin = item.Bvin,
                            Sku = item.Sku,
                            ProductName = item.ProductName,
                            SitePrice = (int)item.SitePrice
                        });
                    }
                }
                else
                {
                    MessageBox.Show("No products found.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading products: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
            return termekek;
        }
    }
}