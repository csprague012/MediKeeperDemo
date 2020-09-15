using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediKeeperDemo.Models
{
    public class ItemResponse
    {
        public List<Item> items { get; set; } = new List<Item>();
        public string message { get; set; }
    }
}
