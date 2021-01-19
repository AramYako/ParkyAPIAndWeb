using ParkyApi.Data;
using ParkyApi.Models.Dtos;
using ParkyApi.Models.Dtos.Trail;
using ParkyApi.Models.Enums;
using ParkyApi.Models.Instances;
using ParkyApi.Models.Repository.IRepository;
using System;
using System.Collections.Generic;
public interface ITrailRepository: IRepository<Trail>
{
  
    IEnumerable<TrailDto> GetTrailsInNationalPark(int nationalParkId);

    TrailDto GetTrail(int id);

    IEnumerable<TrailDto> GetTrails();

    bool TrailExists(string name);


}