using _16noyabr.Areas.Admin.ViewModels;
using _16noyabr.DAL;
using _16noyabr.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;

namespace _16noyabr.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
           _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Product> products=await _context.Products.Include(p=>p.Category).Include(p => p.ProductImages.Where(pi => pi.IsPrimary == true)).ToListAsync();   
            return View(products);
        }
        public async Task<IActionResult> Create()
        {
         ViewBag.Categories =await _context.Categories.ToListAsync();
            return View();
        }
        [HttpPost]
        public  async Task<IActionResult> Create(CreateProductVM productVM)
        {
         if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _context.Categories.ToListAsync();
                return View(productVM);
            }
          bool result = await _context.Categories.AnyAsync(c=>c.Id==productVM.CategoryId);
            if (!result)
            {
                ViewBag.Categories = await _context.Categories.ToListAsync();
                ModelState.AddModelError("CategoryId", "Movcud deil");
                return View();
            }
            Product product = new Product
            {
                Tittle = productVM.Tittle,
                Price = productVM.Price,
                CategoryId = productVM.CategoryId,


            };
            await _context.Products.AddAsync(product);  
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
