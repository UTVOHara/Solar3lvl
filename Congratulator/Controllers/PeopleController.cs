using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WonnaKnow.Models;
using WonnaKnow.Data;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace WonnaKnow.Controllers
{
    public class PeopleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PeopleController(ApplicationDbContext context)
        {
            _context = context;
        }

       
        public IActionResult Index()
        {
            var viewList = _context.Birthdays.ToList();
            return View(viewList);
        }

        
        public IActionResult TodayAndUpcoming()
        {
            DateTime today = DateTime.Today;

            var model = new TodayAndUpcomingViewModel();

            model.TodayList = _context.Birthdays
                .Where(e => e.DateOfBirth.Day == today.Day && e.DateOfBirth.Month == today.Month && e.DateOfBirth.Year != today.Year)
                .OrderBy(e => e.DateOfBirth)
                .ToList();

            model.UpcomingList = _context.Birthdays
                .Where(b => b.DateOfBirth.Day > today.Day && b.DateOfBirth.Month == today.Month && b.DateOfBirth.Year != today.Year)
                .OrderBy(b => b.DateOfBirth)
                .ToList();

            return View(model);
        }

        
        public IActionResult Add()
        {
            return View();
        }

       
        [HttpPost]
        public IActionResult Add(People birthday, IFormFile photo)
        {
            if (photo != null && photo.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    photo.CopyTo(memoryStream);
                    birthday.Photo = memoryStream.ToArray();
                }
            }
            _context.Birthdays.Add(birthday);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        
        public IActionResult Delete(int id)
        {
            var entryToDelete = _context.Birthdays.FirstOrDefault(e => e.Id == id);
            if (entryToDelete != null)
            {
                _context.Birthdays.Remove(entryToDelete);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        
        public IActionResult Edit(int id)
        {
            var entryToEdit = _context.Birthdays.FirstOrDefault(e => e.Id == id);
            if (entryToEdit != null)
            {
                return View(entryToEdit);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Edit(People updatedEntry, IFormFile photo)
        {
            var birthday = _context.Birthdays.Find(updatedEntry.Id);
            if (photo != null && photo.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    photo.CopyTo(memoryStream);
                    birthday.Photo = memoryStream.ToArray();
                }
            }
            if (birthday == null)
            {
                return NotFound();
            }
            birthday.Name = updatedEntry.Name;
            birthday.DateOfBirth = updatedEntry.DateOfBirth;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}