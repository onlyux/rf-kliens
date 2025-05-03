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
using System.Text.RegularExpressions;

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

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            string lastValidInput = "";
            string currentText = textBox6.Text;
            if (!string.IsNullOrWhiteSpace(textBox6.Text))
            {
                if (Regex.IsMatch(currentText, @"^(0|[1-9][0-9]*)$"))
                {
                    lastValidInput = currentText;
                }
                else
                {

                    textBox6.Text = lastValidInput;
                    textBox6.SelectionStart = textBox6.Text.Length;
                }
            }
        }


        private void button_create_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox5.Text) ||
                string.IsNullOrWhiteSpace(textBox6.Text))
            {
                if (string.IsNullOrWhiteSpace(textBox1.Text))
                    MessageBox.Show("Hiba: Üres az 1. mező! Adj meg legalább egy karaktert.");
                if (string.IsNullOrWhiteSpace(textBox2.Text))
                    MessageBox.Show("Hiba: Üres a 2. mező! Adj meg legalább egy karaktert.");
                if (string.IsNullOrWhiteSpace(textBox5.Text))
                    MessageBox.Show("Hiba: Üres az 5. mező! Adj meg legalább egy karaktert.");
                if (string.IsNullOrWhiteSpace(textBox6.Text))
                    MessageBox.Show("Hiba: Üres a 6. mező! Adj meg legalább egy karaktert.");
            }
            else
            {
                create();
            }
        }

        private void button_hozzaad_Click(object sender, EventArgs e)
        {
            assign();
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
                            Name = item.Name,
                            Bvin = item.Bvin
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

        private void create()
        {
            // Termékek behívása API-on keresztül
            try
            {
                Api proxy = new Api(url, key);

                var option = new OptionDTO();

                option.Name = textBox1.Text;
                option.OptionType = OptionTypesDTO.RadioButtonList;
                option.Items.Add(new OptionItemDTO
                {
                    Name = textBox2.Text
                });
                if (!string.IsNullOrWhiteSpace(textBox3.Text))
                {
                    option.Items.Add(new OptionItemDTO
                    {
                        Name = textBox3.Text
                    });
                }
                if (!string.IsNullOrWhiteSpace(textBox4.Text))
                {
                    option.Items.Add(new OptionItemDTO
                    {
                        Name = textBox4.Text
                    });
                }
                option.Settings.Add(new OptionSettingDTO
                {
                    Key = textBox5.Text,
                    Value = textBox6.Text
                });

                ApiResponse<OptionDTO> optionResponse = proxy.ProductOptionsCreate(option);
                if (optionResponse.Content is null)
                {
                    MessageBox.Show("Gebasz van!");
                }
                else
                {
                    MessageBox.Show("Sikeres hozzáadás!");
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void assign()
        {
            try
            {
                Api proxy = new Api(url, key);

                if (listBox1.SelectedItem is null) return;

                Termekek kivalasztott = (Termekek)listBox1.SelectedItem;
                string productId = kivalasztott.Bvin;

                if (listBox2.SelectedItem is null) return;

                Options valasztott = (Options)listBox2.SelectedItem;
                string optionId = valasztott.Bvin;


                ApiResponse<bool> response = proxy.ProductOptionsAssignToProduct(optionId, productId, false);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }
    }

}
