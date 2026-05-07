using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsoleApp1
{
    public class TriangleRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        public float Side1 { get; set; }

        [Required]
        public float Side2 { get; set; }

        [Required]
        public float Side3 { get; set; }

        [Required]
        [StringLength(50)]
        public string TriangleType { get; set; } = string.Empty;

        [StringLength(500)]
        public string ErrorMessage { get; set; } = string.Empty;

        [StringLength(200)]
        public string Coordinates { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}