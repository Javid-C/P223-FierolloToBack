using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiorelloBack.Models
{
    public class BasketItem
    {
        public int Id { get; set; }
        public int FlowerId { get; set; }
        public string AppUserId { get; set; }
        public int Count { get; set; }
        public Flower Flower { get; set; }
        public AppUser AppUser { get; set; }
    }
}
