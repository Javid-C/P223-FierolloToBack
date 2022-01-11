using FiorelloBack.DAL;
using FiorelloBack.Models;
using FiorelloBack.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FiorelloBack.Services
{
    public class LayoutServices
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public LayoutServices(AppDbContext context,IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContext = httpContextAccessor;
        }

        public Setting getSettingDatas()
        {
            Setting data = _context.Settings.FirstOrDefault();
            return data;
        }
        public BasketVM ShowBasket()
        {
            string basket = _httpContext.HttpContext.Request.Cookies["Basket"];

            //BasketVM basketVM = new BasketVM();
            BasketVM basketData = new BasketVM
            {
                TotalPrice = 0,
                BasketItems = new List<BasketItemVM>(),
                Count = 0
            };
            if (!string.IsNullOrEmpty(basket))
            {
                List<BasketCookieItemVM> basketCookieItems = JsonConvert.DeserializeObject<List<BasketCookieItemVM>>(basket);

                foreach (BasketCookieItemVM item in basketCookieItems)
                {
                    Flower flower = _context.Flowers.FirstOrDefault(f => f.Id == item.Id);
                    if(flower != null)
                    {
                        BasketItemVM basketItem = new BasketItemVM
                        {
                            Flower = _context.Flowers.Include(f => f.Campaign).Include(f => f.FlowerImages).FirstOrDefault(f => f.Id == item.Id),
                            Count = item.Count
                          
                        };
                        basketItem.Price = basketItem.Flower.CampaignId == null ? basketItem.Flower.Price: basketItem.Flower.Price * (100 - basketItem.Flower.Campaign.DiscountPercent) / 100;
                        basketData.BasketItems.Add(basketItem);
                        basketData.Count++;
                        basketData.TotalPrice += basketItem.Price * basketItem.Count;
                    }
                }

                //foreach (BasketItemVM item in basketVM.BasketItems)
                //{
                //    item.Flower = _context.Flowers.FirstOrDefault(f => f.Id == item.Flower.Id);
                //    if(item.Flower != null)
                //    {
                //        basketData.BasketItems.Add(item);
                //        if (item.Flower.CampaignId == null)
                //        {
                //            basketData.TotalPrice += item.Flower.Price * item.Count;
                //        }
                //        else
                //        {
                //            basketData.TotalPrice += item.Flower.Price * (100 - item.Flower.Campaign.DiscountPercent) / 100 * item.Count;
                //        }
                //        basketData.Count++;
                //    }
                //}
            }
            return basketData;

        }
    }
}
