using ExtraConcentratedJuice.ItemCurrency.Entities;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtraConcentratedJuice.ItemCurrency
{
    public static class Util
    {
        public static ItemCurrencyConfiguration Config() => ItemCurrency.Instance.Configuration.Instance;

        public static string Translate(string key, params object[] placeholders) =>
            ItemCurrency.Instance.Translations.Instance.Translate(key, placeholders);

        public static List<MoneyValue> FindMoney(PlayerInventory inv)
        {
            var money = new List<MoneyValue>();

            try
            {
                for (byte page = 0; page < PlayerInventory.PAGES; page++)
                {
                    byte itemCount = inv.getItemCount(page);
                    for (byte index = 0; index < itemCount; index++)
                    {
                        Item item = inv.getItem(page, index).item;

                        foreach (MoneyValue m in Config().Money)
                        {
                            if (item.id == m.Id)
                                money.Add(m);
                        }
                    }
                }
            }
            catch { /* xd */ }

            return money;
        }

        public static int CountItems(PlayerInventory inv, ushort id)
        {
            int count = 0;

            try
            {
                for (byte page = 0; page < PlayerInventory.PAGES; page++)
                {
                    byte itemCount = inv.getItemCount(page);
                    for (byte index = 0; index < itemCount; index++)
                    {
                        Item item = inv.getItem(page, index).item;

                        if (item.id == id)
                            count++;
                    }
                }
            }
            catch { /* xd */ }

            return count;
        }

        public static bool RemoveFromInventory(PlayerInventory inv, ushort id)
        {
            for (byte i = 0; i < PlayerInventory.PAGES; i++)
            {
                byte itemCount = inv.getItemCount(i);

                for (byte j = 0; j < itemCount; j++)
                    if (inv.getItem(i, j).item.id == id)
                    {
                        inv.removeItem(i, j);
                        return true;
                    }
            }

            return false;
        }
    }
}
