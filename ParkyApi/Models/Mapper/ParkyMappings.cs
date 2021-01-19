using AutoMapper;
using ParkyApi.Models.Dtos;
using ParkyApi.Models.Dtos.Trail;
using ParkyApi.Models.Instances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyApi.Models.Mapper
{
    public class ParkyMappings : Profile
    {
        public ParkyMappings()
        {
            //With reverseMap we can map both ways
            CreateMap<NationalParkDto, NationalPark>().ReverseMap();
            CreateMap<NationalParkDtoUpdateDto, NationalPark>().ReverseMap();

            CreateMap<TrailDto, Trail>().ReverseMap();
            CreateMap<TrailUpdateDto, Trail>().ReverseMap();
            CreateMap<TrailCreateDto, Trail>().ReverseMap();


        }
    }
}
