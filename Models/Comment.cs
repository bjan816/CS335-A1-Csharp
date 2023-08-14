using System.ComponentModel.DataAnnotations;

namespace A1.Models
{
    public class Comment
    {
        [Key] public int Id { get; set; }

        [Required] public string UserComment { get; set; }

        [Required] public string Name { get; set; }

        [Required] public DateTime Time { get; set; }

        [Required] public string IP { get; set; }
    }
}