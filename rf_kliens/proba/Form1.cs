using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Kliens.Managers;
using System.Runtime.Remoting.Contexts;
using System.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Kliens.Interfaces;
using Kliens.Wrappers;
using Hotcakes.CommerceDTO.v1.Catalog;
using Hotcakes.CommerceDTO.v1.Client;
using Hotcakes.CommerceDTO.v1;


namespace proba
{
    public partial class Form1 : Form
    {
        private readonly string url;
        private readonly string key;
        List<Termekek> termekek = new List<Termekek>();
        List<Options> options = new List<Options>();
        List<Termekchoices> termekchoices = new List<Termekchoices>();
        List<Kateg> kateg = new List<Kateg>();

        private readonly IProductManager _productManager;
        private readonly IOptionManager _optionManager;
        private readonly IOptionCreator _optionCreator;

        public Form1()
        {
            InitializeComponent();
            url = "http://rendfejl10002.northeurope.cloudapp.azure.com:8080/";
            key = "1-c4de6d11-f89c-40e7-82d8-cf7a1365cdd2";

            var apiProxy = new ApiProxy(url, key);
            _productManager = new ProductManager(apiProxy);
            _optionManager = new OptionManager(apiProxy);
            _optionCreator = new OptionCreator(apiProxy);
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            termekek = _productManager.LoadProducts();
            listBox1.DataSource = termekek;
            listBox1.DisplayMember = "ProductName";

            options = _optionManager.LoadOptions();
            listBox2.DataSource = options;
            listBox2.DisplayMember = "Name";

            listazas();
            kategoria();

        }
        private void listazas()
        {
            if (listBox1.SelectedItem == null) return;

            string productId = ((Termekek)listBox1.SelectedItem).Bvin;
            termekchoices = _optionManager.GetAssignedOptions(productId);

            // Töröljük a meglévő oszlopokat (ha vannak)
            dataGridView1.Columns.Clear();

            // Kényszerítjük az oszlopok automatikus generálását
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = null; // Először töröljük az adatforrást
            dataGridView1.DataSource = termekchoices; // Újra kötjük

            // Formázás
            if (dataGridView1.Columns.Count > 0)
            {
                dataGridView1.Columns["Name"].HeaderText = "Opció neve";
                dataGridView1.Columns["Bvin"].HeaderText = "Azonosító";
                dataGridView1.Columns["StoreId"].HeaderText = "Áruház ID";
            }

            // Frissítjük a megjelenítést
            dataGridView1.Refresh();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                label1.Text = "Nincs kiválasztott termék";
                return;
            }

            listazas();

            try
            {
                var selectedProduct = (Termekek)listBox1.SelectedItem;
                var jelen = termekek.Where(x => x.ProductName == selectedProduct.ProductName);

                if (jelen.Any())
                {
                    label1.Text = jelen.First().ProductName + " termék opciói";
                }
                else
                {
                    label1.Text = "Nincs ilyen termék a listában";
                }
            }
            catch (Exception ex)
            {
                label1.Text = "Hiba történt";
                MessageBox.Show($"Hiba a termék betöltésekor: {ex.Message}");
            }
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
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Name is required.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool success = _optionCreator.CreateOption(
                textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text);

            if (success)
            {
                options = _optionManager.LoadOptions(); // Refresh list
                listBox2.DataSource = options;
            }
        }

        private void button_hozzaad_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null || listBox2.SelectedItem == null)
            {
                MessageBox.Show("Select both a product and an option.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string productId = ((Termekek)listBox1.SelectedItem).Bvin;
            string optionId = ((Options)listBox2.SelectedItem).Bvin;

            bool success = _optionManager.AssignOptionToProduct(optionId, productId);
            if (success) listazas();
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem == null)
            {
                MessageBox.Show("Select an option to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string optionId = ((Options)listBox2.SelectedItem).Bvin;
            bool success = _optionManager.DeleteOption(optionId);
            if (success)
            {
                options = _optionManager.LoadOptions(); // Lista frissítése
                listBox2.DataSource = options;
            }
        }

        private void button_unassign_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null || listBox2.SelectedItem == null)
            {
                MessageBox.Show("Select both a product and an option.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string productId = ((Termekek)listBox1.SelectedItem).Bvin;
            string optionId = ((Options)listBox2.SelectedItem).Bvin;

            bool success = _optionManager.UnassignOptionFromProduct(optionId, productId);
            if (success) listazas();
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox7.Text))
            {
                listBox1.DataSource = termekek;
            }
            else
            {
                var filteredList = termekek
                .Where(x => x.ProductName.IndexOf(textBox7.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();
                listBox1.DataSource = filteredList;
            }
            listBox1.DisplayMember = "ProductName";
        }
        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox8.Text))
            {
                listBox2.DataSource = options;

            }
            else
            {
                var filteredList = options
                .Where(x => x.Name.IndexOf(textBox8.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();
                listBox2.DataSource = filteredList;

            }

        }
        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            szurtkat();
        }

        private void kategoria()
        {
            Api proxy = new Api(url, key);
            try
            {
                ApiResponse<List<CategorySnapshotDTO>> response = proxy.CategoriesFindAll();

                if (response.Content != null)
                {
                    foreach (var item in response.Content)
                    {
                        kateg.Add(new Kateg
                        {
                            Bvin = item.Bvin,
                            Name = item.Name
                        });
                    }
                    
                }
                else
                {
                    MessageBox.Show("No categories found.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                listBox3.DataSource = kateg;
                listBox3.DisplayMember = "Name";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading categories: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
            
        }
        private void szurtkat()
        {
            if (listBox3.SelectedItem == null)
            {
                MessageBox.Show("Select a category from the list!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedCategory = (Kateg)listBox3.SelectedItem;
            var categoryId = selectedCategory.Bvin;

            try
            {
                Api proxy = new Api(url, key);

                var page = 1;
                var pageSize = int.MaxValue; 

                ApiResponse<PageOfProducts> response = proxy.ProductsFindForCategory(categoryId, page, pageSize);

                if (response.Content != null && response.Content.Products.Count > 0)
                {
                    termekek.Clear();
                    foreach (var product in response.Content.Products)
                    {
                        termekek.Add(new Termekek
                        {
                            Bvin = product.Bvin,
                            ProductName = product.ProductName,
                        });
                    }

                    listBox1.DataSource = null;
                    listBox1.DataSource = termekek;
                    listBox1.DisplayMember = "ProductName";

                }
                else
                {
                    MessageBox.Show("No products found.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while loading products: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

       
    }

}

