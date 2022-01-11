using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FiorelloBack.Models
{
    public class Tag
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength:15)]
        public string Name { get; set; }
        public List<FlowerTag> FlowerTags{ get; set; }
    }
}
