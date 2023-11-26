using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _16noyabr.Areas.Admin.ViewModels
{
    public class CreateSlideVM
    {
 
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
   
        public int Order { get; set; }
        [Required]
        public IFormFile? Image { get; set; }
    }
}
