using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Models
{
    public class FoodDrinkModel
    {

        public class Rootobject
        {
            public Food[]? Food { get; set; }
        }

        public class Food 
        {
            public string? Name { get; set; }
            public string? Price { get; set; }
        }

    }
}
