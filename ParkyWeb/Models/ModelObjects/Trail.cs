using ParkyWeb.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb.Models.ModelObjects
{
    public class Trail : ErrorDTO
    {
        public int TrailId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Distance { get; set; }
        [Required]
        public double Elevation { get; set; }

        public DifficultiTypes Difficulity { get; set; }
        
        public int NationalParkId { get; set; }
        public NationalPark NationalPark { get; set; }

        public Trail()
        {
            this.NationalPark = new NationalPark();
        }


    }
}
