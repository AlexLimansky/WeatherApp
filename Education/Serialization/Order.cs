using System;
using System.Runtime.Serialization;

namespace Serialization
{
    [Serializable]
    public class Order : ISerializable
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

        public Order(SerializationInfo info, StreamingContext context)
        {
            Count = (int)info.GetValue("Count", typeof(int));
            Price = (decimal)info.GetValue("Price", typeof(decimal));
            TotalPrice = Count * Price;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Count", Count, typeof(int));
            info.AddValue("Price", Price, typeof(decimal));
        }
    }
}
