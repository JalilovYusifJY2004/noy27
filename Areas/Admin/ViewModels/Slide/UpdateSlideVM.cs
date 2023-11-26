using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _16noyabr.Areas.Admin.ViewModels.Slide
{
    public class UpdateSlideVM
    {
  
        [Required]
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public string Images { get; set; }
        public int Order { get; set; }
        [NotMapped]
        public IFormFile? Image { get; set; }
    }
}
