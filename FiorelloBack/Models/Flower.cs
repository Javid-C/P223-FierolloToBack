﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FiorelloBack.Models
{
    public class Flower
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength:70)]
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        [StringLength(maximumLength: 500)]
        public string Description { get; set; }
        [Required]
        public int SKUCode { get; set; }
        [StringLength(maximumLength: 10)]
        public string  Weight { get; set; }
        [StringLength(maximumLength: 30)]
        public string Dimension { get; set; }
        public bool InStock { get; set; }
        public int? CampaignId { get; set; }
        public Campaign Campaign { get; set; }
        public List<FlowerImage> FlowerImages { get; set; }
        public List<FlowerCategory> FlowerCategories { get; set; }
        public List<FlowerTag> FlowerTags { get; set; }
        [NotMapped]
        public List<int> CategoryIds { get; set; }
        [NotMapped]
        public List<IFormFile> ImageFiles { get; set; }


    }
}
