using _16noyabr.Models;

namespace _16noyabr.Areas.Admin.ViewModels
{
    public class CreateProductVM
    {
      
        public string Tittle { get; set; }


        public double Price { get; set; }
        public int Order { get; set; }
        public string Image { get; set; }

        public IFormFile MainPhoto { get; set; }
		public IFormFile HoverPhoto { get; set; }

		public List<IFormFile>? Photos { get; set; }


		public int? CategoryId { get; set; }

        public List<int>  TagIds  { get; set; }


    }
}
