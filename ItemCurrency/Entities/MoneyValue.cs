using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ExtraConcentratedJuice.ItemCurrency.Entities
{
    public class MoneyValue
    {
        [XmlAttribute]
        public ushort Id { get; set; }
        [XmlAttribute]
        public decimal Value { get; set; }

        public MoneyValue() {}

        public MoneyValue(ushort id, decimal value)
        {
            Id = id;
            Value = value;
        }
    }
}
