using Hotcakes.CommerceDTO.v1.Catalog;
using Hotcakes.CommerceDTO.v1;
using Kliens.Interfaces;
using Kliens.Wrappers;
using System.Windows.Forms;
using System;

namespace Kliens.Managers
{
    public class OptionCreator : IOptionCreator
    {
        private readonly IApiProxy _apiProxy;

        public OptionCreator(IApiProxy apiProxy)
        {
            _apiProxy = apiProxy;
        }

        public OptionCreator(string url, string key) : this(new ApiProxy(url, key))
        {
        }

        public bool CreateOption(string name, string item1, string item2, string item3, string settingKey, string settingValue)
        {
            try
            {
                var option = new OptionDTO
                {
                    Name = name,
                    OptionType = OptionTypesDTO.RadioButtonList,
                    Settings = { new OptionSettingDTO { Key = settingKey, Value = settingValue } }
                };

                option.Items.Add(new OptionItemDTO { Name = item1 });
                if (!string.IsNullOrWhiteSpace(item2)) option.Items.Add(new OptionItemDTO { Name = item2 });
                if (!string.IsNullOrWhiteSpace(item3)) option.Items.Add(new OptionItemDTO { Name = item3 });

                ApiResponse<OptionDTO> response = _apiProxy.ProductOptionsCreate(option);
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