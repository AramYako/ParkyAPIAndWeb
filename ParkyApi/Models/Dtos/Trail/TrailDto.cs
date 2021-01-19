using ParkyApi.Models.Enums;
using ParkyApi.Models.Instances;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyApi.Models.Dtos.Trail
{
    public class TrailDto
    {
        public int TrailId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Distance { get; set; }
        [Required]
        public double Elevation { get; set; }

        public DifficultiTypes Difficulity { get; set; }
        public NationalPark NationalPark { get; set; }
    }
}
