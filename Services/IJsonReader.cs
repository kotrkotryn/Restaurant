using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Services
{
    public interface IJsonReader
    {
        public FoodDrinkModel.Rootobject ReturnFoodItems();
        public FoodDrinkModel.Rootobject ReturnDrinkItems();
    }
}
