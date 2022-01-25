using FiorelloBack.DAL;
using FiorelloBack.Models;
using FiorelloBack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiorelloBack.Controllers
{
    [Authorize(Roles = "Member")]
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public OrderController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Checkout()
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            OrderVM model = new OrderVM
            {
                Fullname = user.Fullname,
                Username = user.UserName,
                Email = user.Email,
                BasketItems = _context.BasketItems.Include(b=>b.Flower).ThenInclude(f=>f.Campaign).Where(b => b.AppUserId == user.Id).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(OrderVM orderVM)
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            OrderVM model = new OrderVM
            {
                Fullname = orderVM.Fullname,
                Username = orderVM.Username,
                Email = orderVM.Email,
                BasketItems = _context.BasketItems.Include(b => b.Flower).ThenInclude(f => f.Campaign).Where(b => b.AppUserId == user.Id).ToList()
            };
            if (!ModelState.IsValid) return View(model);

            TempData["Succeeded"] = false;

            if (model.BasketItems.Count == 0) return RedirectToAction("index", "home");
            Order order = new Order
            {
                Country = orderVM.Country,
                State = orderVM.State,
                Address = orderVM.Address,
                TotalPrice = 0,
                Date = DateTime.Now,
                AppUserId = user.Id
            };
            
            foreach (BasketItem item in model.BasketItems)
            {
                order.TotalPrice += item.Flower.CampaignId == null ? item.Count * item.Flower.Price : item.Count * item.Flower.Price * (100 - item.Flower.Campaign.DiscountPercent) / 100;
                OrderItem orderItem = new OrderItem
                {
                    Name = item.Flower.Name,
                    Price = item.Flower.CampaignId == null ? item.Count * item.Flower.Price : item.Count * item.Flower.Price * (100 - item.Flower.Campaign.DiscountPercent) / 100,
                    AppUserId = user.Id,
                    FlowerId = item.Flower.Id,
                    Order = order
                };
                _context.OrderItems.Add(orderItem);
            }
            _context.BasketItems.RemoveRange(model.BasketItems);
            _context.Orders.Add(order);
            _context.SaveChanges();
            TempData["Succeeded"] = true;

            return RedirectToAction("index", "home");
        }
    }
}
