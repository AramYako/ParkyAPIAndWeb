using ParkyApi.Models.Instances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyApi.Models.Repository.IRepository
{
    public interface ITokenRepository
    {
        string GenerateToken(User user);
    }
}
