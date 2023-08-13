using System.ComponentModel.DataAnnotations;

namespace A1.Models
{
    public class Product
    {
        [Key] public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
    }
}