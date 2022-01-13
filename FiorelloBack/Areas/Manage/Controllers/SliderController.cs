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

        public IActionResult Edit(int id)
        {
            Slider slider = _context.Sliders.FirstOrDefault(s=>s.Id == id);
            if (slider == null) return NotFound();
            return View(slider);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Slider slider)
        {
            Slider existSlider = _context.Sliders.FirstOrDefault(s => s.Id == slider.Id);

            if (existSlider == null) return NotFound();
            if (!ModelState.IsValid) return View(existSlider);

            if(slider.ImageFile != null && slider.SignatureFile == null)
            {
                if (!slider.ImageFile.IsImage())
                {
                    ModelState.AddModelError("ImageFile", "Please select image file");
                    return View(existSlider);
                }
                if (!slider.ImageFile.IsSizeOkay(2))
                {
                    ModelState.AddModelError("ImageFile", "Image size must be max 2MB");
                    return View(existSlider);
                }

                Helpers.Helper.DeleteImg(_env.WebRootPath, "assets/images", existSlider.Image);
                existSlider.Image = slider.ImageFile.SaveImg(_env.WebRootPath, "assets/images");
            }else if(slider.ImageFile == null && slider.SignatureFile != null)
            {

                if (!slider.SignatureFile.IsImage())
                {
                    ModelState.AddModelError("SignatureFile", "Please select image file");
                    return View(existSlider);
                }
                if (!slider.SignatureFile.IsSizeOkay(2))
                {
                    ModelState.AddModelError("SignatureFile", "Image size must be max 2MB");
                    return View(existSlider);
                }

                if(existSlider.Signature != null)
                {
                    Helpers.Helper.DeleteImg(_env.WebRootPath, "assets/images", existSlider.Signature);
                }
                existSlider.Signature = slider.SignatureFile.SaveImg(_env.WebRootPath, "assets/images");
            }else if(slider.ImageFile !=null && slider.SignatureFile != null)
            {

                if (!slider.SignatureFile.IsImage())
                {
                    ModelState.AddModelError("SignatureFile", "Please select image file");
                    return View(existSlider);
                }
                if (!slider.SignatureFile.IsSizeOkay(2))
                {
                    ModelState.AddModelError("SignatureFile", "Image size must be max 2MB");
                    return View(existSlider);
                }
                if (!slider.ImageFile.IsImage())
                {
                    ModelState.AddModelError("ImageFile", "Please select image file");
                    return View(existSlider);
                }
                if (!slider.ImageFile.IsSizeOkay(2))
                {
                    ModelState.AddModelError("ImageFile", "Image size must be max 2MB");
                    return View(existSlider);
                }

                if(existSlider.Signature != null)
                {
                    Helpers.Helper.DeleteImg(_env.WebRootPath, "assets/images", existSlider.Signature);
                }
                existSlider.Signature = slider.SignatureFile.SaveImg(_env.WebRootPath, "assets/images");

                Helpers.Helper.DeleteImg(_env.WebRootPath, "assets/images", existSlider.Image);
                existSlider.Image = slider.ImageFile.SaveImg(_env.WebRootPath, "assets/images");
            }
            existSlider.Title = slider.Title;
            existSlider.SubTitle = slider.SubTitle;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }
    }
}
