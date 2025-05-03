using Hotcakes.CommerceDTO.v1;
using Hotcakes.CommerceDTO.v1.Catalog;
using Hotcakes.CommerceDTO.v1.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace proba
{
    public partial class Form1 : Form
    {
        private readonly string url;
        private readonly string key;
        List<Termekek> termekek = new List<Termekek>();
        List<Options> options = new List<Options>();
        List<Termekchoices> termekchoices = new List<Termekchoices>();

        public Form1()
        {
            InitializeComponent();
            url = "http://rendfejl10002.northeurope.cloudapp.azure.com:8080/";
            key = "1-c4de6d11-f89c-40e7-82d8-cf7a1365cdd2";
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            termek_betolt();
            opcio_betolt();
            try
            {
                listazas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listazas();
        }

        private void termek_betolt()
        {
            // Termékek behívása API-on keresztül
            try
            {
                Api proxy = new Api(url, key);

                ApiResponse<List<ProductDTO>> api_termek = proxy.ProductsFindAll();


                if (api_termek.Content != null)
                {
                    foreach (var item in api_termek.Content)
                    {
                        Termekek current = new Termekek
                        {
                            Bvin = item.Bvin,
                            Sku = item.Sku,
                            ProductName = item.ProductName,
                            SitePrice = (int)item.SitePrice
                        };
                        termekek.Add(current);
                    }
                    //MessageBox.Show(termekek.Count().ToString());
                    //MessageBox.Show("Found " + response.Content.Count + " products");
                    listBox1.DataSource = termekek;
                    listBox1.DisplayMember = "ProductName";
                    //dataGridView1.DataSource = termekek;


                }
                else
                {
                    //ha üres a content vagy az api elérhetetlensége miatt, pl. nem megy a szerver, akkor fut ide
                    MessageBox.Show(api_termek.ToString());

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void opcio_betolt()
        {
            // Opciók behívása API-on keresztül
            try
            {
                Api proxy = new Api(url, key);

                ApiResponse<List<OptionDTO>> response = proxy.ProductOptionsFindAll();


                if (response.Content != null)
                {
                    foreach (var item in response.Content)
                    {
                        Options current = new Options
                        {
                            Name = item.Name

                        };
                        options.Add(current);
                    }
                    //MessageBox.Show("Found " + response.Content.Count + " options");
                    listBox2.DataSource = options;
                    listBox2.DisplayMember = "Name";


                }
                else
                {
                    //ha üres a content vagy az api elérhetetlensége miatt, pl. nem megy a szerver, akkor fut ide
                    MessageBox.Show(response.ToString());

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void listazas()
        {
            try
            {
                if (listBox1.SelectedItem is null) return;

                Termekek kivalasztott = (Termekek)listBox1.SelectedItem;
                string productId = kivalasztott.Bvin;

                Api proxy = new Api(url, key);
                ApiResponse<List<OptionDTO>> response = proxy.ProductOptionsFindAllByProductId(productId);

                if (response.Content == null || response.Content.Count == 0)
                {
                    //MessageBox.Show("No options found or API unavailable");
                    dataGridView1.DataSource = null;
                    return;
                }

                termekchoices.Clear();

                foreach (var item in response.Content)
                {
                    termekchoices.Add(new Termekchoices
                    {
                        Name = item.Name,
                        Bvin = item.Bvin,
                        StoreId = (int)item.StoreId
                    });
                }

                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = termekchoices;
                dataGridView1.Refresh();

                //MessageBox.Show($"Found {response.Content.Count} options");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}",
                              "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}