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

        public Form1()
        {
            InitializeComponent();
            url = "http://rendfejl10002.northeurope.cloudapp.azure.com:8080/";
            key = "1-c4de6d11-f89c-40e7-82d8-cf7a1365cdd2";
        }


        private void Form1_Load(object sender, EventArgs e)
        {

            
            
            
        }


    }
}