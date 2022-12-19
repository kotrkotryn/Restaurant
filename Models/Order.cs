using Restaurant.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Models
{
    public class Order
    {
        public int Id { get; set; } 
        public string Table { get; set; }
        public int AmountOfSeats { get; set; }
        public decimal AmountToPay { get; set; }
        public List<FoodDrinkModel.Food> FoodDrink { get; set; } = new List<FoodDrinkModel.Food> { };  
        public DateTime OrderDate { get; set; }

    }
}
