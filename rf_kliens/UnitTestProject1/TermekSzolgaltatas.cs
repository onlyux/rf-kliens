using Hotcakes.CommerceDTO.v1;
using Hotcakes.CommerceDTO.v1.Catalog;
using Hotcakes.CommerceDTO.v1.Client;
using System.Collections.Generic;

namespace proba
{
    public interface ITermekKliens
    {
        ApiResponse<OptionDTO> LetrehozValasztek(OptionDTO valasztek);
    }

    public class TermekKliens : ITermekKliens
    {
        private readonly Api _api;

        public TermekKliens(string cim, string kulcs)
        {
            _api = new Api(cim, kulcs);
        }

        public ApiResponse<OptionDTO> LetrehozValasztek(OptionDTO valasztek)
        {
            return _api.ProductOptionsCreate(valasztek);
        }
    }

    public class TermekSzolgaltatas
    {
        private readonly ITermekKliens _kliens;

        public TermekSzolgaltatas(ITermekKliens kliens)
        {
            _kliens = kliens;
        }

        public bool LetrehozValasztek(string nev, List<string> opciok, string beallitasKulcs, string beallitasErtek)
        {
            var valasztek = new OptionDTO
            {
                Name = nev,
                OptionType = OptionTypesDTO.RadioButtonList
            };

            foreach (var opcio in opciok)
            {
                if (!string.IsNullOrWhiteSpace(opcio))
                    valasztek.Items.Add(new OptionItemDTO { Name = opcio });
            }

            valasztek.Settings.Add(new OptionSettingDTO
            {
                Key = beallitasKulcs,
                Value = beallitasErtek
            });

            var valasz = _kliens.LetrehozValasztek(valasztek);
            return valasz?.Content != null;
        }
    }
}
