using _16noyabr.Areas.Admin.ViewModels;
using _16noyabr.DAL;
using _16noyabr.Models;
using _16noyabr.Utilities.Extention;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;

namespace _16noyabr.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
		private readonly IWebHostEnvironment _env;

		public ProductController(AppDbContext context , IWebHostEnvironment env)
        {
           _context = context;
		_env = env;
		}
        public async Task<IActionResult> Index()
        {
            List<Product> products=await _context.Products.Include(p=>p.Category).Include(p => p.ProductImages.Where(pi => pi.IsPrimary == true)).ToListAsync();
			ViewBag.Tags = await _context.Tags.ToListAsync();
			return View(products);
        }
        public async Task<IActionResult> Create()
        {
         ViewBag.Categories =await _context.Categories.ToListAsync();
            ViewBag.Tags= await _context.Tags.ToListAsync();
            return View();
        }
        [HttpPost]
        public  async Task<IActionResult> Create(CreateProductVM productVM)
        {
         if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _context.Categories.ToListAsync();
				ViewBag.Tags = await _context.Tags.ToListAsync();
				return View();
            }
          bool result = await _context.Categories.AnyAsync(c=>c.Id==productVM.CategoryId);
            if (!result)
            {
                ViewBag.Categories = await _context.Categories.ToListAsync();
				ViewBag.Tags = await _context.Tags.ToListAsync();
				ModelState.AddModelError("CategoryId", "Movcud deil");
                return View();
            }
            foreach (int tagid in productVM.TagIds)
            {
                bool tagresult = await _context.Tags.AnyAsync(t => t.Id == tagid);
                if (tagresult)
                {
                    ViewBag.Categories = await _context.Categories.ToListAsync();
					ViewBag.Tags = await _context.Tags.ToListAsync();
                    ModelState.AddModelError("tagIds","yanlis");
                    return View() ;
				}
            }
            if (!productVM.MainPhoto.ValidateType("image/"))
            {
				ViewBag.Categories = await _context.Categories.ToListAsync();
				ViewBag.Tags = await _context.Tags.ToListAsync();
				ModelState.AddModelError("MainPhoto", "yanlis");
				return View();
            }
			if (!productVM.MainPhoto.ValidateSize(600))
			{
				ViewBag.Categories = await _context.Categories.ToListAsync();
				ViewBag.Tags = await _context.Tags.ToListAsync();
				ModelState.AddModelError("MainPhoto", "yanlis");
				return View();
			}
			if (!productVM.HoverPhoto.ValidateType("image/"))
			{
				ViewBag.Categories = await _context.Categories.ToListAsync();
				ViewBag.Tags = await _context.Tags.ToListAsync();
				ModelState.AddModelError("HoverPhoto", "yanlis");
				return View();
			}
            	if (!productVM.HoverPhoto.ValidateSize(600))
			{
				ViewBag.Categories = await _context.Categories.ToListAsync();
				ViewBag.Tags = await _context.Tags.ToListAsync();
				ModelState.AddModelError("HoverPhoto", "yanlis");
				return View();
			}
            ProductImage image = new ProductImage
            {
                IsPrimary = true,
                Url = await productVM.MainPhoto.CreateFileAsync(_env.WebRootPath,"assets","images","website-image")
            };
			ProductImage hoverimage = new ProductImage
			{
				IsPrimary = false,
				Url = await productVM.HoverPhoto.CreateFileAsync(_env.WebRootPath, "assets", "images", "website-image")
			};
			Product product = new Product
			{
				Tittle = productVM.Tittle,
				Price = productVM.Price,
				CategoryId = productVM.CategoryId,
                ProductTags=new List<ProductTag>(),
                ProductImages = new List<ProductImage> { image, hoverimage }
                


			};


		
            foreach (int tagId in productVM.TagIds)
            {
                ProductTag productTag = new ProductTag
                {
                    TagId = tagId,
                 


                };
          product.ProductTags.Add(productTag);
            }
            TempData["Message"] = "";
            foreach (IFormFile photos in productVM.Photos)
            {
				if (photos.ValidateType("image/"))
				{
					TempData["Message"] += $"\n{photos.FileName} uygun deil";
					continue;
				}
				if (photos.ValidateSize(600))
				{
					TempData["Message"] += $"\n{photos.FileName} uygun deil";
					continue;
				}
                product.ProductImages.Add(new ProductImage
                {
                   
                    IsPrimary=null,
                    Url=await photos.CreateFileAsync(_env.WebRootPath, "assets", "images", "website-image")
                });
			}

            await _context.Products.AddAsync(product);  
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Product product = await _context.Products.Include(p=>p.ProductImages).Include(p=>p.ProductTags).FirstOrDefaultAsync(c=>c.Id==id);
            if (product == null) return NotFound();
            UpdateProductVM productVM = new UpdateProductVM
            {
                Tittle = product.Tittle,
                Price = product.Price,
                CategoryId = product.CategoryId,
                TagIds=product.ProductTags.Select(pt=>pt.TagId).ToList(),
                ProductImages=product.ProductImages,
                Categories=await _context.Categories.ToListAsync(),

                //Tags  =await _context.Tags.ToListAsync(),    

            };
            return View(productVM);

        }

        [HttpPost]
        public async Task<IActionResult> Update(int id ,UpdateProductVM productVM)
        {
            if (!ModelState.IsValid)
            {
                productVM.Categories = await _context.Categories.ToListAsync();
		

				return View();
            }
     Product existed=await _context.Products.Include(p=>p.ProductTags).Include(pi=>pi.ProductImages).FirstOrDefaultAsync(p=>p.Id==id);
            if (existed is null) return NotFound();
            bool result = await _context.Categories.AnyAsync(c => c.Id == productVM.CategoryId);
            if(!result) 
            {
                productVM.Categories = await _context.Categories.ToListAsync();
                return View(productVM);
            }

            if (productVM.MainPhoto is not null)
              
            {
                if (productVM.MainPhoto.ValidateType("image/"))
                {
					productVM.Categories = await _context.Categories.ToListAsync();
					//productVM.Tags = await _context.Tags.ToListAsync();
					ModelState.AddModelError("MainPhoto", "yanlis");
					return View();
				}
				if (productVM.MainPhoto.ValidateSize(600))
				{
					productVM.Categories = await _context.Categories.ToListAsync();
					//productVM.Tags = await _context.Tags.ToListAsync();
					ModelState.AddModelError("MainPhoto", "yanlis");
					return View();
				}
			}
			if (productVM.MainPhoto is not null)

			{
				if (productVM.HoverPhoto.ValidateType("image/"))
				{
					productVM.Categories = await _context.Categories.ToListAsync();
					//productVM.Tags = await _context.Tags.ToListAsync();
					ModelState.AddModelError("HoverPhoto", "yanlis");
					return View();
				}
				if (productVM.HoverPhoto.ValidateSize(600))
				{
					productVM.Categories = await _context.Categories.ToListAsync();
					//productVM.Tags = await _context.Tags.ToListAsync();
					ModelState.AddModelError("HoverPhoto", "yanlis");
					return View();
				}

               
			}
            if (productVM.MainPhoto is not null)
            {
                string filename = await productVM.MainPhoto.CreateFileAsync(_env.WebRootPath, "assets", "images", "ebsite-images");
                ProductImage mainImage = existed.ProductImages.FirstOrDefault(pi => pi.IsPrimary == true);
                mainImage.Url.DeleteFile(_env.WebRootPath, "assets", "images", "ebsite-images");
                _context.ProductImages.Remove(mainImage);
                existed.ProductImages.Add(new ProductImage
                {

                    IsPrimary = true,
                    Url = filename

                });
            }
				if (productVM.HoverPhoto is not null)
				{
					string filename = await productVM.HoverPhoto.CreateFileAsync(_env.WebRootPath, "assets", "images", "ebsite-images");
					ProductImage hoverImage = existed.ProductImages.FirstOrDefault(pi => pi.IsPrimary == false);
					hoverImage.Url.DeleteFile(_env.WebRootPath, "assets", "images", "ebsite-images");
					_context.ProductImages.Remove(hoverImage);
					existed.ProductImages.Add(new ProductImage
					{

						IsPrimary = false,
						Url = filename

					});
				}
       List<ProductImage> removeable= existed.ProductImages.Where(pi=>productVM.ImageIds.Exists(ImgId=>ImgId==pi.Id)&&pi.IsPrimary==null).ToList();
            foreach (ProductImage pImage in removeable)
            {
                pImage.Url.DeleteFile(_env.WebRootPath, "assets", "images", "ebsite-images");
                existed.ProductImages.Remove(pImage);

			}
            existed.Tittle = productVM.Tittle;
            existed.Price = productVM.Price;
            existed.CategoryId = productVM.CategoryId;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        
       
    }
}
