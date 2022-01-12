using FiorelloBack.DAL;
using FiorelloBack.Extensions;
using FiorelloBack.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FiorelloBack.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private IWebHostEnvironment _env;
        public SliderController(AppDbContext context, IWebHostEnvironment env )
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            List<Slider> model = _context.Sliders.ToList();
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Slider slider)
        {
            if (!ModelState.IsValid) return View();

            if (slider.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Shekil daxil edin");
                return View();
            }
            if(!slider.ImageFile.IsSizeOkay(2))
            {
                ModelState.AddModelError("ImageFile", "Shekilin olcusu maximum 2MB ola biler");
                return View();
            }
            if (!slider.ImageFile.IsImage())
            {
                ModelState.AddModelError("ImageFile", "Image file sec");
                return View();
            }

            slider.Image = slider.ImageFile.SaveImg(_env.WebRootPath, "assets/images");
            _context.Sliders.Add(slider);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
