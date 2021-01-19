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
using Microsoft.EntityFrameworkCore;
using ParkyApi.Models.Dtos.Trail;

namespace ParkyApi.Models.Repository
{
    public class TrailRepository :Repository.Repository<Trail>, ITrailRepository
    {
        private IMapper _mapper;
        public TrailRepository(ParkyDbContext db, IMapper mapper)
            :base(db)
        {
            this._mapper = mapper;
        }

        
        #region Get

        public TrailDto GetTrail(int id)
        {
            using (var dbCtx = new ParkyDbContext(this.db._option))
            {
                var nPark = dbCtx.Trails.Where(t => t.TrailId == id)
                    .Include(c => c.NationalPark).FirstOrDefault();

                return _mapper.Map<TrailDto>(nPark);
            }
        }

        public IEnumerable<TrailDto> GetTrails()
        {
            using (var dbCtx = new ParkyDbContext(this.db._option))
            {
                var nParks = dbCtx.Trails
                    .Include(c => c.NationalPark).ToList();

                return _mapper.Map<List<TrailDto>>(nParks);
            }
        }

        public IEnumerable<TrailDto> GetTrailsInNationalPark(int nationalParkId)
        {
            using (var dbCtx = new ParkyDbContext(this.db._option))
            {
                var nParks = dbCtx.Trails
                    .Include(c => c.NationalPark)
                    .Where(c => c.NationalParkId == nationalParkId).ToList();

                return _mapper.Map<List<TrailDto>>(nParks);
            }
        }
        #endregion

        #region Exist
        public bool TrailExists(string name)
        {
            using (var dbCtx = new ParkyDbContext(this.db._option))
            {
                return dbCtx.Trails.Any(a => a.Name.Equals(name));
            }
        }

        #endregion


    }
}
