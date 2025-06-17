using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SredaZadatak.Models
{
    public class Project
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength:150)]
        public string Name { get; set; }
        [Required]
        [Range(minimum:2000, maximum:2023)]
        public int StartYear { get; set; }
        [Required]
        [Range(minimum:2023, maximum:2030)]
        public int EndYear { get; set; }
        public ICollection <Explorer> Explorers { get; set; } = new List<Explorer>();
    }
}
