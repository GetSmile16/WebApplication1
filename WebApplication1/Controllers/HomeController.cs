using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public class ViewModel
        {
            public IEnumerable<Film> films { get; set; }
            public IEnumerable<Serial> serials { get; set; }
        }

        private ApplicationContext db;
        private IWebHostEnvironment env;
        public HomeController(ApplicationContext context, IWebHostEnvironment appEnvironment)
        {
            db = context;
            env = appEnvironment;
        }
        public async Task<IActionResult> ViewFilms(string? type, string name, Sort sortOrder = Sort.NameAsc)
        {
            IQueryable<Film> films = db.Films.Include(p => p);
            if (!String.IsNullOrEmpty(type))
            {
                films = films.Where(p => p.Type.Equals(type));
            }
            if (!String.IsNullOrEmpty(name))
            {
                films = films.Where(p => p.Name.Contains(name));
            }

            List<String> types = new List<string>() {"Ужасы", "Комедия", "Триллер", "Боевик", "Драмма"};
            types.Insert(0, "");

            ViewData["NameSort"] = sortOrder == Sort.NameAsc ? Sort.NameDesc : Sort.NameAsc;
            ViewData["RatingSort"] = sortOrder == Sort.RatingAsc ? Sort.RatingDesc : Sort.RatingAsc;

            films = sortOrder switch
            {
                Sort.NameDesc => films.OrderByDescending(s => s.Name),
                Sort.RatingAsc => films.OrderBy(s => s.Rating),
                Sort.RatingDesc => films.OrderByDescending(s => s.Rating),
                _ => films.OrderBy(s => s.Name),
            };

            UserView<Film> viewModel = new UserView<Film>
            {
                Items = await films.AsNoTracking().ToListAsync(),
                Types = new SelectList(types),
                Name = name
            };
            return View(viewModel);
        }
        public async Task<IActionResult> ViewSerials(string? type, string name, Sort sortOrder = Sort.NameAsc)
        {
            IQueryable<Serial> serials = db.Serials.Include(p => p);
            if (!String.IsNullOrEmpty(type))
            {
                serials = serials.Where(p => p.Type.Equals(type));
            }
            if (!String.IsNullOrEmpty(name))
            {
                serials = serials.Where(p => p.Name.Contains(name));
            }

            List<String> types = new List<string>() { "Ужасы", "Комедия", "Триллер", "Боевик", "Драмма" };
            types.Insert(0, "");

            ViewData["NameSort"] = sortOrder == Sort.NameAsc ? Sort.NameDesc : Sort.NameAsc;
            ViewData["RatingSort"] = sortOrder == Sort.RatingAsc ? Sort.RatingDesc : Sort.RatingAsc;

            serials = sortOrder switch
            {
                Sort.NameDesc => serials.OrderByDescending(s => s.Name),
                Sort.RatingAsc => serials.OrderBy(s => s.Rating),
                Sort.RatingDesc => serials.OrderByDescending(s => s.Rating),
                _ => serials.OrderBy(s => s.Name),
            };

            UserView<Serial> viewModel = new UserView<Serial>
            {
                Items = await serials.AsNoTracking().ToListAsync(),
                Types = new SelectList(types),
                Name = name
            };
            return View(viewModel);
        }
        public async Task<IActionResult> ViewEditFilm()
        {
            return View(await db.Films.ToListAsync());
        }
        public async Task<IActionResult> ViewEditSerial()
        {
            return View(await db.Serials.ToListAsync());
        }
        public async Task<IActionResult> Index()
        {
            var films = await db.Films.OrderByDescending(p => p.Rating).Take(5).ToListAsync();
            var serials = await db.Serials.OrderByDescending(p => p.Rating).Take(5).ToListAsync();
            var model = new ViewModel { films = films, serials = serials};
            return View(model);
        }
        public IActionResult CreateFilm()
        {
            return View();
        }
        public IActionResult CreateSerial()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateFilm(Film film, IFormFile image1)
        {   
            if (image1 != null)
            {
                string path = "/Files/" + image1.FileName;
                using (var fileStream = new FileStream(env.WebRootPath + path, FileMode.Create))
                {
                    await image1.CopyToAsync(fileStream);
                }
                film.Image = path;
            }
            db.Films.Add(film);
            await db.SaveChangesAsync();  
            return RedirectToAction("ViewEditFilm");
        }
        [HttpPost]
        public async Task<IActionResult> CreateSerial(Serial serial, IFormFile image1)
        {
            if (image1 != null)
            {
                string path = "/Files/" + image1.FileName;
                using (var fileStream = new FileStream(env.WebRootPath + path, FileMode.Create))
                {
                    await image1.CopyToAsync(fileStream);
                }
                serial.Image = path;
            }
            db.Serials.Add(serial);
            await db.SaveChangesAsync();
            return RedirectToAction("ViewEditSerial");
        }
        public async Task<IActionResult> EditFilm(int? id)
        {
            if (id != null)
            {
                Film film = await db.Films.FirstOrDefaultAsync(p => p.Id == id);
                if (film != null)
                    return View(film);
            }
            return NotFound();
        }
        public async Task<IActionResult> EditSerial(int? id)
        {
            if (id != null)
            {
                Serial serial = await db.Serials.FirstOrDefaultAsync(p => p.Id == id);
                if (serial != null)
                    return View(serial);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> EditFilm(Film film, IFormFile image1)
        {
            if (image1 != null)
            {
                string path = "/Files/" + image1.FileName;
                using (var fileStream = new FileStream(env.WebRootPath + path, FileMode.Create))
                {
                    await image1.CopyToAsync(fileStream);
                }
                film.Image = path;
            }
            db.Films.Update(film);
            await db.SaveChangesAsync();
            return RedirectToAction("ViewEditFilm");
        }
        [HttpPost]
        public async Task<IActionResult> EditSerial(Serial serial, IFormFile image1)
        {
            if (image1 != null)
            {
                string path = "/Files/" + image1.FileName;
                using (var fileStream = new FileStream(env.WebRootPath + path, FileMode.Create))
                {
                    await image1.CopyToAsync(fileStream);
                }
                serial.Image = path;
            }
            db.Serials.Update(serial);
            await db.SaveChangesAsync();
            return RedirectToAction("ViewEditSerial");
        }
        [HttpGet]
        [ActionName("DeleteFilm")]
        public async Task<IActionResult> ConfirmDeleteFilm(int? id)
        {
            if (id != null)
            {
                Film film = await db.Films.FirstOrDefaultAsync(p => p.Id == id);
                if (film != null)
                    return View(film);
            }
            return NotFound();
        }
        [HttpGet]
        [ActionName("DeleteSerial")]
        public async Task<IActionResult> ConfirmDeleteSerial(int? id)
        {
            if (id != null)
            {
                Serial serial = await db.Serials.FirstOrDefaultAsync(p => p.Id == id);
                if (serial != null)
                    return View(serial);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFilm(int? id)
        {
            if (id != null)
            {
                Film film = await db.Films.FirstOrDefaultAsync(p => p.Id == id);
                if (film != null)
                {
                    db.Films.Remove(film);
                    await db.SaveChangesAsync();
                    return RedirectToAction("ViewEditFilm");
                }
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteSerial(int? id)
        {
            if (id != null)
            {
                Serial serial = await db.Serials.FirstOrDefaultAsync(p => p.Id == id);
                if (serial != null)
                {
                    db.Serials.Remove(serial);
                    await db.SaveChangesAsync();
                    return RedirectToAction("ViewEditSerial");
                }
            }
            return NotFound();
        }
    }
}
