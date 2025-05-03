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
            
            
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

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
    }
}