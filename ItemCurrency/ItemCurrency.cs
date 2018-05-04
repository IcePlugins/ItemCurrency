using Rocket.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rocket.API.Collections;

namespace ExtraConcentratedJuice.ItemCurrency
{
    public class ItemCurrency : RocketPlugin<ItemCurrencyConfiguration>
    {
        public static ItemCurrency Instance { get; private set; }

        protected override void Load()
        {
            Instance = this;
        }

        public override TranslationList DefaultTranslations =>
            new TranslationList
            {
                { "inventory_value", "The total worth of your inventory is {1}{0}." },
                { "not_for_buy", "That item is not listed for purchase." },
                { "not_for_sell", "That item is not registered for sales." },
                { "cannot_afford", "You cannot afford to buy {0}x of that item." },
                { "purchase_success", "You have successfully purchased {0}x of {1} for {2}{3}." },
                { "sell_success", "You have successfully sold {0}x of {1}. for {2}{3}." },
                { "not_enough_items", "You do not have enough of that item to sell." },
                { "value", "The value of {0} is {1}{2}." },
                { "no_value", "{0} has no monetary value." },
                { "item_not_found", "That item was not found." }
            };
    }
}
