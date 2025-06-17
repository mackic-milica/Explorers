using System.ComponentModel.DataAnnotations;

namespace SredaZadatak.Models
{
    public class Explorer
    {
        public int Id  { get; set; }
        [Required]
        [StringLength(50, MinimumLength =2)]
        public string Name { get; set; }
        [Required]
        [StringLength(80, MinimumLength =2)]
        public string LastName { get; set; }
        [Required]
        [Range(minimum:1900, maximum:2024)]
        public  int BirthYear { get; set; }
        [Required]
        [Range(typeof(decimal), "10000.0", "500000.0")]
        public decimal Salary { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
