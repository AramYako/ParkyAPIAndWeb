using ParkyApi.Models.Repository.IRepository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParkyApi.Models.Instances;
using ParkyApi.Data;
using AutoMapper;
using ParkyApi.Models.Dtos;

namespace ParkyApi.Models.Repository
{
    public class NationalParkRepository : Repository.Repository<NationalPark>, INationalParkRepository
    {
    

        public NationalParkRepository(ParkyDbContext db)
            :base (db)
        {
        }

        #region Exists
        public bool NationalParkExists(string name)
        {
            using (var dbCtx = new ParkyDbContext(this.db._option))
            {
                return dbCtx.NationalParks.Any(a => a.Name.Equals(name));
            }
        }
        #endregion
    }
}
