using _16noyabr.DAL;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace _16noyabr.Areas.Admin.Controllers
{
    [Area("Admin")]
	public class ColorController : Controller
	{
        private readonly AppDbContext _context;

        public ColorController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
		{
            //List<Color> colors = await _context.Color.Include().ToListAsync();
			return View();
		}
	}
}
