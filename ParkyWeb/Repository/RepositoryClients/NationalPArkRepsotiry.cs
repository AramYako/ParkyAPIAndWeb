using ParkyWeb.Models;
using ParkyWeb.Models.ModelObjects;
using ParkyWeb.Repository.IRepository;
using ParkyWeb.Repository.IRepository.RepositoryClients.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ParkyWeb.Repository.RepositoryClients
{
    public class NationalParkRepository: Repository<NationalPark>, INationalParkRepository
    {
        private readonly IHttpClientFactory _clientFactory;

        public NationalParkRepository(IHttpClientFactory clientFactory)
            : base(clientFactory)
        {
            this._clientFactory = clientFactory;
        }
    }
}
