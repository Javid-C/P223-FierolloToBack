using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FiorelloBack.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Bura zibil atmayin")]
        [StringLength(maximumLength:20,ErrorMessage = "Kateqoriya adi 20 xarakterden uzun ola bilmez")]
        public string Name { get; set; }
        public List<FlowerCategory> FlowerCategories { get; set; }
    }

}
