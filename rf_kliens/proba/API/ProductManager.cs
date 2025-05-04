using Hotcakes.CommerceDTO.v1;
using Hotcakes.CommerceDTO.v1.Catalog;
using Hotcakes.CommerceDTO.v1.Client;
using proba;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace YourNamespace.Managers
{
    public class ProductManager
    {
        private readonly string _url;
        private readonly string _key;

        public ProductManager(string url, string key)
        {
            _url = url;
            _key = key;
        }

        public List<Termekek> LoadProducts()
        {
            var termekek = new List<Termekek>();
            try
            {
                Api proxy = new Api(_url, _key);
                ApiResponse<List<ProductDTO>> api_termek = proxy.ProductsFindAll();

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