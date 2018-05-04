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
    public class CommandBalance : IRocketCommand
    {
        #region Properties
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "balance";

        public string Help => "Gets your inventory's total worth.";

        public string Syntax => "/balance";

        public List<string> Aliases => new List<string> { "bal" };

        public List<string> Permissions => new List<string> { "itemcurrency.balance" };
        #endregion

        public void Execute(IRocketPlayer caller, string[] args)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            UnturnedChat.Say(caller, Util.Translate("inventory_value", Util.FindMoney(player.Inventory).Sum(x => x.Value), Util.Config().CurrencySymbol));
        }
    }
}
