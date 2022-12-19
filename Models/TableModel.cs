using Restaurant.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Models
{
    public class TableModel
    {
        public int Id { get; set; } 
        public string? Name { get; set; }    
        public int NumberOfSeats { get; set; }
        public bool Occupied { get; set; }

    }
}
