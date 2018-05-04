using ExtraConcentratedJuice.ItemCurrency.Entities;
using Rocket.API;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtraConcentratedJuice.ItemCurrency
{
    public class ItemCurrencyConfiguration : IRocketPluginConfiguration
    {
        public string CurrencySymbol;
        public List<MoneyValue> Money;
        public List<ItemPrice> Prices;

        public void LoadDefaults()
        {
            CurrencySymbol = "$";
            Money = new List<MoneyValue>
            {
                new MoneyValue(1051, 5),
                new MoneyValue(1052, 10),
                new MoneyValue(1053, 20),
                new MoneyValue(1054, 50),
                new MoneyValue(1055, 100),
                new MoneyValue(1056, 1),
                new MoneyValue(1057, 2)
            };

            Prices = new List<ItemPrice>();

            for (ushort i = 0; i < ushort.MaxValue; i++)
            {
                ItemAsset x = Assets.find(EAssetType.ITEM, i) as ItemAsset;

                if (x?.itemName != null)
                    Prices.Add(new ItemPrice(x.id, x.itemName, 0, 0));
            }
        }
    }
}
