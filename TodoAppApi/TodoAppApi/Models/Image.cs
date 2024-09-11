using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoAppApi.Models
{
    public class Image
    {
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        public byte[]? ImageContent { get; set; }

        public virtual Category Category { get; set; } 
    }
}
