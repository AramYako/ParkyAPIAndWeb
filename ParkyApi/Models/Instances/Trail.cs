using ParkyApi.Models.Enums;
using ParkyApi.Models.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyApi.Models.Instances
{
    public class Trail
    {
        [Key]
        public int TrailId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Distance { get; set; }
        [Required]
        public double Elevation { get; set; }
        public DifficultiTypes Difficulity { get; set; }
        [Required]
        public int NationalParkId { get; set; }
        public NationalPark NationalPark { get; set; }

        public DateTime DateCreated { get; set; }


    }
}
