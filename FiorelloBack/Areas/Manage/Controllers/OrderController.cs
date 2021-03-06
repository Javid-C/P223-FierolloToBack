using FiorelloBack.DAL;
using FiorelloBack.Models;
using FiorelloBack.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace FiorelloBack.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Order> orders = _context.Orders.ToList();
            return View(orders);
        }

        public IActionResult Edit(int id)
        {
            Order order = _context.Orders.Include(o => o.OrderItems).Include(o=>o.AppUser).FirstOrDefault(o => o.Id == id);
            if (order == null) return NotFound();
            return View(order);
        }

        public IActionResult Accept(int id,string message)
        {

            Order order = _context.Orders.FirstOrDefault(o => o.Id == id);
            if (order == null) return Json(new { status=400});
            order.Status = true;
            order.Message = message;
            _context.SaveChanges();
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("alialiyevasmoy@gmail.com","Fiorello");
            mail.To.Add(new MailAddress("javid.asadullayev@code.edu.az"));

            mail.Subject = "Order";
            mail.Body = $"{message}";
            mail.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential("alialiyevasmoy@gmail.com","alivasmoy123");
            smtp.Send(mail);
            return Json(new { status = 200});
        }
    }
}
