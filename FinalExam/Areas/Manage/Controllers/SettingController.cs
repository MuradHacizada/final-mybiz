using FinalExam.DAL;
using FinalExam.Models;
using FinalExam.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace FinalExam.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class SettingController : Controller
    {
        private readonly AppDbContext _context;

        public SettingController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var existed = await _context.Settings.ToListAsync();
            return View(existed);
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id < 1) return BadRequest();

            var existed = await _context.Settings.FirstOrDefaultAsync(s => s.Id == id);
            if (existed == null) return NotFound();

            return View(existed);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Setting setting)
        {
            var existed = await _context.Settings.FirstOrDefaultAsync(s => s.Id == setting.Id);
            if (existed == null) return NotFound();

            existed.Value = setting.Value;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
    }
}
