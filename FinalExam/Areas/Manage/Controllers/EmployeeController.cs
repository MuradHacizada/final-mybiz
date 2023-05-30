using FinalExam.DAL;
using FinalExam.Models;
using FinalExam.Utilities.Extensions;
using FinalExam.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace FinalExam.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin")]

    public class EmployeeController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public EmployeeController(AppDbContext db,IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public async Task<IActionResult> Index(int page = 0)
        {
            List<Employee> employees = await _db.Employees.Skip(page * 5).Take(5).Include(p => p.Position).ToListAsync();
            PaginateVM<Employee> paginate = new PaginateVM<Employee>
            {
                Items = employees,
                TotalPage = Math.Ceiling((decimal)_db.Employees.Count() / 5),
                CurrentPage = page
            };
            return View(paginate);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Position = await _db.Positions.ToListAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            ViewBag.Position = await _db.Positions.ToListAsync();
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "");
                return View();
            }
            if (employee.PositionId == null) { ModelState.AddModelError("PositionId", "Xahis olunur Position secin"); return View(); }
            bool result = await _db.Positions.AnyAsync(p => p.Id == employee.PositionId);
            if (!result) { ModelState.AddModelError("PositionId", "Bele Position Yoxdur"); return View(); }

            if (employee.Photo == null) { ModelState.AddModelError("Photo", "Zehmet olmasa sekil secin"); return View(); }
            if (!employee.Photo.CheckFileType("image/")) { ModelState.AddModelError("Photo", "Sekil tipi uygun deyil"); return View(); }
            if (!employee.Photo.CheckFileSize(400)) { ModelState.AddModelError("Photo", "Sekil uzunlugu uygun deyil"); return View(); }
            employee.Image = await employee.Photo.CreateFileAsync(_env.WebRootPath, "assets/img/team");
            await _db.Employees.AddAsync(employee);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int? id)
        {
            ViewBag.Position = await _db.Positions.ToListAsync();
            if (id < 1 || id == null) { return BadRequest(); }
            Employee existed = await _db.Employees.Include(p => p.Position).FirstOrDefaultAsync(p => p.Id == id);
            if (existed == null) { return NotFound(); }
            return View(existed);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Update(int? id, Employee employee)
        {
            ViewBag.Position = await _db.Positions.ToListAsync();
            if (id < 1 || id == null) { return BadRequest(); }
            Employee existed = await _db.Employees.Include(p => p.Position).FirstOrDefaultAsync(p => p.Id == id);
            if (existed == null) { return NotFound(); }
            bool result = await _db.Positions.AnyAsync(p => p.Id == employee.PositionId);
            if (!result) { ModelState.AddModelError("Position", "Bele Position Yoxdur"); return View(); }
            if (employee.Photo != null)
            {
                if (!employee.Photo.CheckFileType("image/")) { ModelState.AddModelError("Photo", "Sekil tipi uygun deyil"); return View(); }
                if (!employee.Photo.CheckFileSize(400)) { ModelState.AddModelError("Photo", "Sekil uzunlugu uygun deyil"); return View(); }
                existed.Image.DeleteFile(_env.WebRootPath, "assets/img/team");
                existed.Image = await employee.Photo.CreateFileAsync(_env.WebRootPath, "assets/img/team");
            }
            existed.Fullname = employee.Fullname;
            existed.PositionId = employee.PositionId;
            existed.Description = employee.Description;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id < 1 || id == null) { return BadRequest(); }
            Employee existed = await _db.Employees.FirstOrDefaultAsync(p => p.Id == id);
            if (existed == null) { return NotFound(); }
            existed.Image.DeleteFile(_env.WebRootPath, "assets/img/team");
            _db.Employees.Remove(existed);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
