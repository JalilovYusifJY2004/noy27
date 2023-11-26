using System.ComponentModel.DataAnnotations;

namespace _16noyabr.Models
{
    public class Tag
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Ad mutleq daxil edilmelidir!")]
        [MaxLength(10, ErrorMessage = "Uzunlugu 10 xarakterden cox olmamalidir.")]
        public string Name { get; set; }

        public List<ProductTag>? ProductTags { get; set; }
    }
}
