using AutoMapper.Configuration.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyApi.Models.Dtos
{
    public class NationalParkDto
    {
        public int NationalParkId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string State { get; set; }
        public byte[] Picture { get; set; }
        public DateTime Created { get; set; }
        public DateTime Established { get; set; }

    }

    public class NationalParkDtoUpdateDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a Id bigger than 0")]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string State { get; set; }
        public byte[] Picture { get; set; }

    }
}

