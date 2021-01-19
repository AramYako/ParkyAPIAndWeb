using ParkyApi.Data;
using ParkyApi.Models.Dtos;
using ParkyApi.Models.Instances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyApi.Models.Repository.IRepository
{
    public interface INationalParkRepository: IRepository<NationalPark>
    {
        bool NationalParkExists(string name);
    }
}
