using FiorelloBack.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FiorelloBack.ViewModels
{
    public class OrderVM
    {
        [Required]
        [StringLength(maximumLength:70)]
        public string Fullname { get; set; }
        [Required]
        [StringLength(maximumLength: 25)]
        public string Username { get; set; }
        [Required]
        [StringLength(maximumLength: 50)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [StringLength(maximumLength: 150)]
        public string Address { get; set; }
        [Required]
        [StringLength(maximumLength: 40)]
        public string Country { get; set; }
        [Required]
        [StringLength(maximumLength: 30)]
        public string State { get; set; }
        public List<BasketItem> BasketItems { get; set; }
    }
}
