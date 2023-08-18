using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace A1.Models
{
    public class Comment
    {
        [Key] public int Id { get; set; }

        [Required] public string UserComment { get; set; }

        [Required] public string Name { get; set; }

        public string Time { get; set; }

        public string IP { get; set; }
    }
}