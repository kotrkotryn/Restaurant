using Newtonsoft.Json;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Services
{
    public class JsonReader : IJsonReader
    {
        private string DrinkJsonPath = "C:/Users/Kotryna/Desktop/Restaurant/JsonFiles/Drinks.json";
        private string FoodJsonPath = "C:/Users/Kotryna/Desktop/Restaurant/JsonFiles/Food.json";
        private string WritePath = "C:/Users/Kotryna/Desktop/Restaurant/ReceiptFolder/TodaysOrders.json";

        public FoodDrinkModel.Rootobject ReturnFoodItems()
        {
            using (StreamReader r = new StreamReader(FoodJsonPath))
            {
                string json = r.ReadToEnd();
                var items = JsonConvert.DeserializeObject<FoodDrinkModel.Rootobject>(json);
                return items;
            }
        }

        public FoodDrinkModel.Rootobject ReturnDrinkItems()
        {
            using (StreamReader r = new StreamReader(DrinkJsonPath))
            {
                string json = r.ReadToEnd();
                var items = JsonConvert.DeserializeObject<FoodDrinkModel.Rootobject>(json);
                return items;
            }
        }

        public void CreateOrderFile(Order order)
        {
            string json = JsonConvert.SerializeObject(order, Formatting.Indented);
            System.IO.File.WriteAllText(WritePath, json);
        }

    }
}
