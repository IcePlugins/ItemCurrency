using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ExtraConcentratedJuice.ItemCurrency.Entities
{
    public class ItemPrice
    {
        [XmlAttribute]
        public ushort Id { get; set; }
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public decimal BuyPrice { get; set; }
        [XmlAttribute]
        public decimal SellPrice { get; set; }

        public ItemPrice() { }
        public ItemPrice(ushort id, string name, decimal buyPrice, decimal sellPrice)
        {
            Id = id;
            Name = name;
            BuyPrice = buyPrice;
            SellPrice = sellPrice;
        }
    }
}
