using ExtraConcentratedJuice.ItemCurrency.Entities;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ExtraConcentratedJuice.ItemCurrency.Commands
{
    public class CommandSell : IRocketCommand
    {
        #region Properties
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "sell";

        public string Help => "Allows you to sell some items.";

        public string Syntax => "/sell <item> <amt>";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string> { "itemcurrency.sell" };
        #endregion

        public void Execute(IRocketPlayer caller, string[] args)
        {
            if (args.Length < 1)
            {
                UnturnedChat.Say(caller, Syntax, Color.red);
                return;
            }

            int amt = 1;

            if (!ushort.TryParse(args[0], out ushort id))
            {
                var items = new List<ItemAsset>(Assets.find(EAssetType.ITEM).Cast<ItemAsset>());
                var a = items.Where(x => x.itemName != null)
                    .OrderBy(x => x.itemName.Length)
                    .FirstOrDefault(x => x.itemName.IndexOf(args[0], StringComparison.OrdinalIgnoreCase) >= 0);

                if (a == null)
                {
                    UnturnedChat.Say(caller, Util.Translate("item_not_found"), Color.red);
                    return;
                }

                id = a.id;
            }

            if (args.Length > 1 && !int.TryParse(args[1], out amt))
            {
                UnturnedChat.Say(caller, Syntax, Color.red);
                return;
            }

            var item = Util.Config().Prices.Where(x => x.SellPrice > 0).FirstOrDefault(x => x.Id == id);

            if (item == null)
            {
                UnturnedChat.Say(caller, Util.Translate("not_for_sell"), Color.red);
                return;
            }

            UnturnedPlayer player = (UnturnedPlayer)caller;
            ItemAsset asset = (ItemAsset)Assets.find(EAssetType.ITEM, (ushort)id);

            int count = Util.CountItems(player.Inventory, item.Id);

            if (count < amt)
            {
                UnturnedChat.Say(caller, Util.Translate("not_enough_items"), Color.red);
                return;
            }

            for (int i = 0; i < amt; i++)
                Util.RemoveFromInventory(player.Inventory, item.Id);

            decimal price = item.SellPrice * amt;

            foreach (var x in Util.Config().Money.OrderByDescending(x => x.Value))
            {
                int c = (int)(price / x.Value);
                price -= c * x.Value;

                for (int i = 0; i < c; i++)
                    player.Inventory.forceAddItem(new Item(x.Id, true), true);
            }

            UnturnedChat.Say(caller, Util.Translate("sell_success", amt, asset.itemName, Util.Config().CurrencySymbol, price));
        }
    }
}
