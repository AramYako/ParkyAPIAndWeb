using Microsoft.AspNetCore.Mvc;
using ParkyWeb.Repository.IRepository.RepositoryClients.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ParkyWeb.Models.Api
{
    public class APIController : ControllerBase
    {
        private readonly INationalParkRepository _npRepo;
        public APIController(INationalParkRepository npRepo)
        {
            this._npRepo = npRepo;
        }

    }
}