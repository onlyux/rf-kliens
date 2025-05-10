using Hotcakes.CommerceDTO.v1;
using Hotcakes.CommerceDTO.v1.Catalog;
using Hotcakes.CommerceDTO.v1.Client;
using proba;
using System;
using System.Windows.Forms;

namespace YourNamespace.Managers
{
    public class OptionCreator
    {
        private readonly string _url;
        private readonly string _key;

        public OptionCreator(string url, string key)
        {
            _url = url;
            _key = key;
        }

        public bool CreateOption(string name, string item1, string item2, string item3, string settingKey, string settingValue)
        {
            try
            {
                Api proxy = new Api(_url, _key);
                var option = new OptionDTO
                {
                    Name = name,
                    OptionType = OptionTypesDTO.RadioButtonList,
                    Settings = { new OptionSettingDTO { Key = settingKey, Value = settingValue } }
                };

                option.Items.Add(new OptionItemDTO { Name = item1 });
                if (!string.IsNullOrWhiteSpace(item2)) option.Items.Add(new OptionItemDTO { Name = item2 });
                if (!string.IsNullOrWhiteSpace(item3)) option.Items.Add(new OptionItemDTO { Name = item3 });

                ApiResponse<OptionDTO> response = proxy.ProductOptionsCreate(option);
                return response.Content != null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating option: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}