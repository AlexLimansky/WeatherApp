using static System.Console;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Serialization
{
    class Program
    {
        static void Main(string[] args)
        {
            WriteLine("Enter the count:");
            int count;
            decimal price;
            string pathToFile = "test.dat";
            bool isCountCorrect = int.TryParse(ReadLine(), out count);
            WriteLine("Enter the price:");
            bool isPriceCorrect = decimal.TryParse(ReadLine(), out price);
            if (isCountCorrect & isPriceCorrect)
            {
                Order baseOrder = new Order(count, price);
                DisplayOrder(baseOrder);
                BinarySerialize(baseOrder, pathToFile);
                Order deserializedOrder = BinaryDeserialize(pathToFile);
                WriteLine("Deserialized order:");
                DisplayOrder(deserializedOrder);
            }
            else
            {
                WriteLine("Count or price is not valid!");
            }
            ReadKey();
        }
        static void BinarySerialize(Order dataToSerialize, string path)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, dataToSerialize);
                WriteLine("Serialized to bin");
            }
        }
        static Order BinaryDeserialize(string path)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                Order deserilizedData = (Order)formatter.Deserialize(fs);
                return deserilizedData;
            }
        }

        static void DisplayOrder(Order dataToDisplay)
        {
            WriteLine("Your count is {0}, price is {1}, total price is {2}", dataToDisplay.Count, dataToDisplay.Price, dataToDisplay.TotalPrice);
        }
    }
}
