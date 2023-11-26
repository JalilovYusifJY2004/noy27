using _16noyabr.Areas.Admin.ViewModels;
using _16noyabr.Areas.Admin.ViewModels;
using _16noyabr.Areas.Admin.ViewModels.Slide;
using _16noyabr.DAL;
using _16noyabr.Models;
using _16noyabr.Utilities.Extention;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _16noyabr.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class SlideController : Controller
    {

        public AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SlideController(AppDbContext context, IWebHostEnvironment env)

        {
            _context = context;
            _env = env;
        }



        public async Task<IActionResult> Index()
        {
            List<Slide> slides = await _context.Slides.ToListAsync();

            return View(slides);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateSlideVM slideVM)
        {
            //if (slideVM.Image is null)
            //{
            //    ModelState.AddModelError("Image", "Sekil secin");
            //    return View();
            //}
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (!slideVM.Image.ValidateType("image/"))
            {
                ModelState.AddModelError("Photo", "yanlis file tipi");
                return View();
            }

            if (slideVM.Image.ValidateSize(2 * 1024*1024))
            {
                ModelState.AddModelError("Image", "2mb olsun");
                return View();
            }

        string fileName = await slideVM.Image.CreateFile(_env.WebRootPath, "assets", "images", "slider");
            Slide slide = new Slide
            {
                //Image = fileName,
                Title = slideVM.Title,
                SubTitle = slideVM.SubTitle,
                Order = slideVM.Order

            };
            await _context.Slides.AddAsync(slide);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Slide existed = await _context.Slides.FirstOrDefaultAsync(slide => slide.Id == id);
            if (existed is null) return NotFound();

            UpdateSlideVM slideVM = new UpdateSlideVM
            {
                Images = existed.Images,
                Title = existed.Title,
                SubTitle = existed.SubTitle,
                Description = existed.Description,
                Order = existed.Order

            };

            return View(slideVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateSlideVM slideVM)
        {
        
          
            if (ModelState.IsValid)
            {
                return View(slideVM);
            }
            Slide existed = await _context.Slides.FirstOrDefaultAsync(s => s.Id == id);
            if (existed is null) return NotFound(nameof(existed));
            if (slideVM.Image is not null)
            {
                if (!slideVM.Image.ValidateType("image/"))
                {
                    ModelState.AddModelError("Photo", "yanlis file tipi");
                    return View(slideVM);
                }

                if (slideVM.Image.ValidateSize(2 * 1024 * 1024))
                {
                    ModelState.AddModelError("Image", "2mb olsun");
                    return View(slideVM);
                }
                string newImage = await slideVM.Image.CreateFile(_env.WebRootPath, "assets", "images", "slider");
                existed.Images.DeleteFile(_env.WebRootPath, "assets", "images", "slider");
                existed.Images = newImage;
                 }
        existed.Title = slideVM.Title;
            existed.Description = slideVM.Description;
            existed.SubTitle = slideVM.SubTitle;
            existed.Order = slideVM.Order;

            _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));



        }





        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0) return BadRequest();
            var slide = await _context.Slides.FirstOrDefaultAsync(s => s.Id == id);
            if (slide is null) return NotFound();

            return View(slide);
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();

            Slide slide = await _context.Slides.FirstOrDefaultAsync(slide => slide.Id == id);
            if (slide is null) return NotFound();
           slide.Images.DeleteFile(_env.WebRootPath,"assets","images","slider");

            _context.Slides.Remove(slide);
           await _context.SaveChangesAsync();
            return RedirectToAction("Index");


        }
    }
 }
   
