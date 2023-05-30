using FinalExam.DAL;
using FinalExam.Models;
using FinalExam.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace FinalExam.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;
        public HomeController(AppDbContext db)
        {
            _db= db;
        }
        public async Task<IActionResult> Index()
        {
            HomeVM homeVM = new HomeVM
            {
                Employees =await _db.Employees.Take(4).Include(p=>p.Position).ToListAsync(),
                Settings=await _db.Settings.ToDictionaryAsync(k=>k.Key,k=>k.Value)
            };
            return View(homeVM);
        }
    }
}