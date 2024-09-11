using System.ComponentModel.DataAnnotations;

namespace TodoAppApi.Dtos.Image
{
    public class GetImageDto
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public byte[]? ImageContent { get; set; }
        public int CategoryId { get; set; }
        public virtual TodoAppApi.Models.Category Category { get; set; }
    }
}
