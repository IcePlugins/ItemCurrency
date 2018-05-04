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
    public class CommandValue : IRocketCommand
    {
        #region Properties
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "value";

        public string Help => "Tells you the monetary value of a certain item.";

        public string Syntax => "/value <item>";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string> { "itemcurrency.value" };
        #endregion

        public void Execute(IRocketPlayer caller, string[] args)
        {
            if (args.Length < 1)
            {
                UnturnedChat.Say(caller, Syntax, Color.red);
                return;
            }

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

            var moneyItem = Util.Config().Money.FirstOrDefault(x => x.Id == id);
            ItemAsset asset = (ItemAsset)Assets.find(EAssetType.ITEM, id);

            if (moneyItem == null)
            {
                UnturnedChat.Say(caller, Util.Translate("no_value", asset.itemName));
                return;
            }

            UnturnedChat.Say(caller, Util.Translate("value", asset.itemName, Util.Config().CurrencySymbol, moneyItem.Value));
        }
    }
}
