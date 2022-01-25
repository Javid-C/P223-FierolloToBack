using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiorelloBack.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int? FlowerId { get; set; }
        public string AppUserId { get; set; }
        public int OrderId { get; set; }
        public Flower Flower { get; set; }
        public AppUser AppUser { get; set; }
        public Order Order { get; set; }
    }
}
