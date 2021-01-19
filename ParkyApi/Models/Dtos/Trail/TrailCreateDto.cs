using ParkyApi.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyApi.Models.Dtos.Trail
{
    public class TrailCreateDto
    {
        public int TrailId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Distance { get; set; }

        public DifficultiTypes Difficulity { get; set; }
        [Required]
        public double Elevation { get; set; }

        public int NationalParkId { get; set; }
    }
}
