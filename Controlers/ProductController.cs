using _16noyabr.DAL;
using _16noyabr.Models;
using _16noyabr.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _16noyabr.Controlers
{
    public class ProductController:Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}

        public IActionResult Detail(int? id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            Product product = _context.Products
                     .Include(p => p.ProductColors).ThenInclude(pc => pc.Color)
                .Include(p => p.ProductSizes).ThenInclude(ps => ps.Size)
                .Include(p=>p.Category)
                .Include(p => p.ProductImages).
                Include(p=>p.ProductTags).ThenInclude(pt=>pt.Tag).
                FirstOrDefault(p => p.Id == id);






            if (product is null)
            {
                return NotFound();
            }
            List<Product> products = _context.Products.Include(p => p.ProductImages.Where(pi => pi.IsPrimary != null)).Where(p => p.CategoryId == product.CategoryId && p.Id != product.Id).ToList();




            DetailVM detailVM = new DetailVM
            {
                Product = product,
                RelatedProducts = products
            };

            return View(detailVM);
        }
}
}
