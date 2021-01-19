using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyApi.Configurations
{
    public class DatabaseConfigurations : IConfigureOptions<DbContextOptionsBuilder>
    {

        readonly IConfiguration _config;

        public DatabaseConfigurations(IConfiguration config)
        {
            this._config = config;
        }

        public void Configure(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(this._config.GetConnectionString(DataBaseNames.ParkyDb.ToString()));
        }

    }
}
