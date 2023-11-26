namespace _16noyabr.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Tittle { get; set; }
       
 
        public double Price { get; set; }
        public int Order { get; set; }
        public string Image { get; set; }

      
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
        public List<ProductImage>? ProductImages { get; set; }
        public List<ProductTag>? ProductTags { get; set; }
        public List<ProductColor>? ProductColors { get; set; }
        public List<ProductSize>? ProductSizes { get; set; }
    }
    }
