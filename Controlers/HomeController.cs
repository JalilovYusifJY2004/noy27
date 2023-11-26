using _16noyabr.DAL;
using _16noyabr.Models;
using _16noyabr.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _16noyabr.Controlers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;

        }
        public IActionResult Index()
        {


            List<Slide> slides = _context.Slides.OrderBy(s => s.Order).ToList();

            List<Product> products = _context.Products.Include(pi => pi.ProductImages).OrderBy(p => p.Order).ToList();
            

            HomeVM home = new HomeVM
            {
                Slides=slides,
                Products = products
            };

            return View(home);
            
        }
        public IActionResult About()
        {
            return View();
        }

    }
}
