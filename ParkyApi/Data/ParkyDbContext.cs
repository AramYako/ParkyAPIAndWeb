using Microsoft.EntityFrameworkCore;
using ParkyApi.Models.Instances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ParkyApi.Data
{
    public class ParkyDbContext: DbContext
    {
        public readonly DbContextOptions<ParkyDbContext> _option;
        public ParkyDbContext (DbContextOptions<ParkyDbContext> options)
            :base(options)
        {
            _option = options;
        }

        public DbSet<NationalPark> NationalParks { get; set; }
        public DbSet<Trail> Trails { get; set; }
        public DbSet<User> Users { get; set; }
    }

}
