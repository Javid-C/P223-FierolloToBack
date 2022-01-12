using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FiorelloBack.Models
{
    public class Slider
    {
        public int Id { get; set; }
        [StringLength(maximumLength:100)]
        public string Image { get; set; }
        [StringLength(maximumLength:60)]
        public string Title { get; set; }
        [StringLength(maximumLength:100)]
        public string SubTitle { get; set; }
        [StringLength(maximumLength:100)]
        public string Signature { get; set; }
        public int Order { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        [NotMapped]
        public IFormFile SignatureFile { get; set; }
    }
}
