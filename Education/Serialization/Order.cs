using System;
using System.Runtime.Serialization;

namespace Serialization
{
    [Serializable]
    public class Order : IDeserializationCallback
    {
        public int Count;

        public decimal Price;
        [NonSerialized]
        public decimal TotalPrice;

        public Order(int count, decimal price)
        {
            Count = count;
            Price = price;
            TotalPrice = count * price;
        }

        public void OnDeserialization(object sender)
        {
            TotalPrice = Count * Price;
        }
    }
}
