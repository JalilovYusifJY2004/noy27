using _16noyabr.Models;

namespace _16noyabr.Areas.Admin.ViewModels
{
    public class UpdateProductVM
    {
        public string Tittle { get; set; }


        public double Price { get; set; }
     
        public string Image { get; set; }


        public int? CategoryId { get; set; }
        public List<int> TagIds { get; set; }
        public List<Category>? Categories { get; set; }
        public List<int>? Tags { get; }
    }
}
