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
    public class CommandBuy : IRocketCommand
    {
        #region Properties
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "buy";

        public string Help => "Buys an item from the shop.";

        public string Syntax => "/buy <item> <amt>";

        public List<string> Aliases => new List<string> { "purchase" };

        public List<string> Permissions => new List<string> { "itemcurrency.buy" };
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

            var item = Util.Config().Prices.Where(x => x.BuyPrice > 0).FirstOrDefault(x => x.Id == id);

            if (item == null)
            {
                UnturnedChat.Say(caller, Util.Translate("not_for_buy"), Color.red);
                return;
            }

            UnturnedPlayer player = (UnturnedPlayer)caller;
            ItemAsset asset = (ItemAsset)Assets.find(EAssetType.ITEM, (ushort)id);

            var money = Util.FindMoney(player.Inventory);
            var cost = new List<MoneyValue>();

            decimal price = item.BuyPrice * amt;

            if (price > money.Sum(x => x.Value))
            {
                UnturnedChat.Say(caller, Util.Translate("cannot_afford", amt), Color.red);
                return;
            }

            foreach (MoneyValue i in money)
            {
                cost.Add(i);

                if (cost.Sum(x => x.Value) >= price)
                    break;
            }

            foreach (MoneyValue i in cost)
                Util.RemoveFromInventory(player.Inventory, i.Id);

            decimal change = cost.Sum(x => x.Value) - price;

            foreach (var x in Util.Config().Money.OrderByDescending(x => x.Value))
            {
                int count = (int)(change / x.Value);
                change -= count * x.Value;

                for (int i = 0; i < count; i++)
                    player.Inventory.forceAddItem(new Item(x.Id, true), true);
            }

            for (int i = 0; i < amt; i++)
                player.Inventory.forceAddItem(new Item(item.Id, true), true);

            UnturnedChat.Say(caller, Util.Translate("purchase_success", amt, asset.itemName, Util.Config().CurrencySymbol, price));
        }
    }
}
