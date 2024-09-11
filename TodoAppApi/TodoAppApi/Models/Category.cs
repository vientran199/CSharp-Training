using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoAppApi.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
