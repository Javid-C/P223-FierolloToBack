using FiorelloBack.DAL;
using FiorelloBack.Extensions;
using FiorelloBack.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiorelloBack.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class FlowerController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public FlowerController(AppDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            List<Flower> flowers = _context.Flowers.Include(f=>f.FlowerImages).ToList();
            return View(flowers);
        }

        public IActionResult Create()
        {
            ViewBag.Campaigns = _context.Campaigns.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Flower flower)
        {
            ViewBag.Campaigns = _context.Campaigns.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            if (!ModelState.IsValid) return View();

            if(flower.CampaignId == 0)
            {
                flower.CampaignId = null;
            }
            flower.FlowerCategories = new List<FlowerCategory>();
            flower.FlowerImages = new List<FlowerImage>();
            foreach (int id in flower.CategoryIds)
            {
                FlowerCategory fCategory = new FlowerCategory
                {
                    Flower = flower,
                    CategoryId = id
                };
                flower.FlowerCategories.Add(fCategory);
            }
            if(flower.ImageFiles.Count > 5)
            {
                ModelState.AddModelError("ImageFiles", "You can choose only 5 images");
                return View();
            }
            foreach (var image in flower.ImageFiles)
            {
                if (!image.IsImage())
                {
                    ModelState.AddModelError("ImageFiles", "Please choose image file");
                    return View();
                }
                if (!image.IsSizeOkay(2))
                {
                    ModelState.AddModelError("ImageFiles", "Image size must be max 2MB");
                    return View();
                }
              
            }
            foreach (var image in flower.ImageFiles)
            {
                FlowerImage flowerImage = new FlowerImage
                {
                    Image = image.SaveImg(_env.WebRootPath, "assets/images"),
                    isMain = flower.FlowerImages.Count < 1 ? true : false,
                    Flower = flower
                };
                flower.FlowerImages.Add(flowerImage);
            }
            _context.Flowers.Add(flower);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }
        public IActionResult Edit(int id)
        {
            ViewBag.Campaigns = _context.Campaigns.ToList();
            ViewBag.Categories = _context.Categories.ToList();
           
            Flower flower = _context.Flowers.Include(f=>f.FlowerCategories).FirstOrDefault(f => f.Id == id);
            if (flower == null) return NotFound();
            return View(flower);
        }
    }
}
