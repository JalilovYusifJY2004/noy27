using _16noyabr.DAL;
using _16noyabr.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _16noyabr.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class TagController : Controller
    {
              
            private readonly AppDbContext _context;

            public TagController(AppDbContext context)
            {
                _context = context;
            }
            public async Task<IActionResult> Index()
            {
                List<Tag> tags = await _context.Tags.Include(t => t.ProductTags).ToListAsync();
                return View(tags);
            }
            public IActionResult Create()
            {
                return View();
            }
            [HttpPost]
            public async Task<IActionResult> Create(Tag tag)
            {
                if (!ModelState.IsValid) return View();

                bool result = _context.Tags.Any(c => c.Name.ToLower().Trim() == tag.Name.ToLower().Trim());
                if (result)
                {
                    ModelState.AddModelError("Name", "Bu Tag artiq movcuddur.");
                    return View();
                }
                await _context.Tags.AddAsync(tag);
                await _context.SaveChangesAsync();
                return RedirectToAction("index");
            }
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();

            Tag tag = await _context.Tags.FirstOrDefaultAsync(c => c.Id == id);
            if (tag is null) return NotFound();
            return View(tag);

        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, Tag tag)
        {
            if (ModelState.IsValid)
            {
                return View();
            }

            Tag existed = await _context.Tags.FirstOrDefaultAsync(c => c.Id == id);
            if (existed is null) return NotFound();
            bool result = _context.Categories.Any(c => c.Name == tag.Name && c.Id != id);
            if (result)
            {
                ModelState.AddModelError("Name", "Movcuddur");
                return View();
            }

            existed.Name = tag.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0) return BadRequest();
            var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == id);
            if (tag is null) return NotFound();

            return View(tag);
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();

            Tag existed = await _context.Tags.FirstOrDefaultAsync(c => c.Id == id);
            if (existed is null) return NotFound();
            _context.Tags.Remove(existed);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));




        }
    }
}
