using System.ComponentModel.DataAnnotations.Schema;

namespace FinalExam.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string Description { get; set; }
        public string? Image { get; set; }
        public Position? Position { get; set; } 
        public int PositionId { get; set; }
        [NotMapped]
        public IFormFile? Photo { get; set; }
    }
}
