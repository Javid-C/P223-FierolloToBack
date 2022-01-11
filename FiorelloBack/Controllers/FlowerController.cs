using FiorelloBack.DAL;
using FiorelloBack.Models;
using FiorelloBack.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiorelloBack.Controllers
{
    public class FlowerController : Controller
    {
        private readonly AppDbContext _context;
        public FlowerController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int page=1)
        {

            ViewBag.CurrentPage = page;
            ViewBag.TotalPage = Math.Ceiling((decimal)_context.Flowers.Count() / 1);
            
            List<Flower> model = _context.Flowers.Include(f=>f.FlowerCategories).ThenInclude(fc=>fc.Category).Include(f=>f.Campaign).Include(f=>f.FlowerImages).Skip((page - 1)*1).Take(1).ToList();
            return View(model);
        }

        public IActionResult Details(int id)
        {

            Flower flower = _context.Flowers.Include(f => f.Campaign).Include(f => f.FlowerCategories).ThenInclude(fc => fc.Category).Include(f => f.FlowerTags).ThenInclude(ft => ft.Tag).Include(f => f.FlowerImages).FirstOrDefault(f => f.Id == id);
            if (flower == null) return NotFound();
            return View(flower);
        }

        #region Session

        //public IActionResult setSession(int id)
        //{
        //    Flower flower = _context.Flowers.FirstOrDefault(f => f.Id == id);

        //    HttpContext.Session.SetString("Session", flower.Name);

        //    return RedirectToAction("Index", "Home");
        //}
        //public IActionResult ShowSession()
        //{
        //    var basket = HttpContext.Session.GetString("Session");
        //    return Content(basket);
        //}

        #endregion


        #region Cookie

        //public IActionResult SetCookie(int id)
        //{
        //    Flower flower = _context.Flowers.FirstOrDefault(f => f.Id == id);

        //    HttpContext.Response.Cookies.Append("Cookie", flower.Name);
        //    return RedirectToAction("Index", "Home");
        //}

        //public IActionResult ShowCookie()
        //{
        //    return Content(HttpContext.Request.Cookies["Cookie"]);
        //}
        //public IActionResult DeleteCookie(string key)
        //{
        //    HttpContext.Response.Cookies.Delete(key);
        //    return RedirectToAction("Index", "Home");
        //}

        #endregion

        public IActionResult AddBasket(int id)
        {
            Flower flower = _context.Flowers.Include(f=>f.Campaign).FirstOrDefault(f => f.Id == id);

            string basket = HttpContext.Request.Cookies["Basket"];

            if(basket == null)
            {
                List<BasketCookieItemVM> basketCookieItems = new List<BasketCookieItemVM>();

                basketCookieItems.Add(new BasketCookieItemVM
                {
                    Id = flower.Id,
                    Count = 1
                });



                //BasketVM basketVM = new BasketVM
                //{
                //    BasketItems = new List<BasketItemVM>(),
                //    TotalPrice = 0,
                //    Count = 1
                //};

                //BasketItemVM basketItem = new BasketItemVM
                //{
                //    Flower = flower,
                //    Count = 1
                //};
                //basketVM.BasketItems.Add(basketItem);
                //if(flower.CampaignId == null)
                //{
                //    basketVM.TotalPrice = flower.Price;
                //}
                //else
                //{
                //    basketVM.TotalPrice = flower.Price * (100 - flower.Campaign.DiscountPercent) / 100;
                //}
                //Math.Round(basketVM.TotalPrice, 3);
                
                string basketStr = JsonConvert.SerializeObject(basketCookieItems);
                

                HttpContext.Response.Cookies.Append("Basket", basketStr);
            }
            else
            {
                List<BasketCookieItemVM> basketCookieItems = JsonConvert.DeserializeObject<List<BasketCookieItemVM>>(basket);

                BasketCookieItemVM cookieItem = basketCookieItems.FirstOrDefault(c => c.Id == flower.Id);

                if(cookieItem == null)
                {
                    cookieItem = new BasketCookieItemVM
                    {
                        Id = flower.Id,
                        Count = 1
                    };
                    basketCookieItems.Add(cookieItem);
                }
                else
                {
                    cookieItem.Count++;
                }


                //BasketVM basketVM = JsonConvert.DeserializeObject<BasketVM>(basket);
                //BasketItemVM basketItem = basketVM.BasketItems.FirstOrDefault(i => i.Flower.Id == flower.Id);
                //if(basketItem == null)
                //{
                //    basketItem = new BasketItemVM
                //    {
                //        Flower = flower,
                //        Count = 1
                //    };
                //    basketVM.Count++;
                //    basketVM.BasketItems.Add(basketItem);
                //}
                //else
                //{
                //    basketItem.Count++;
                //}
                //if (flower.CampaignId == null)
                //{
                //    basketVM.TotalPrice += basketItem.Flower.Price;
                //}
                //else
                //{
                //    basketVM.TotalPrice += basketItem.Flower.Price * (100 - basketItem.Flower.Campaign.DiscountPercent) / 100;
                //}
                string basketStr = JsonConvert.SerializeObject(basketCookieItems);

                HttpContext.Response.Cookies.Append("Basket", basketStr);

            }
            
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ShowBasket()
        {
            string basketStr = HttpContext.Request.Cookies["Basket"];
            if (!string.IsNullOrEmpty(basketStr))
            {
                List<BasketCookieItemVM> basket = JsonConvert.DeserializeObject<List<BasketCookieItemVM>>(basketStr);
                return Json(basket);
            }
            return Content("Basket is empty");
        }
    }
}
