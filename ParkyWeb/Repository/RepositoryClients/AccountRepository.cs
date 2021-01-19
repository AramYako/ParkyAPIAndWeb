using Newtonsoft.Json;
using ParkyWeb.Helpers;
using ParkyWeb.Models.ModelObjects;
using ParkyWeb.Repository.IRepository;
using ParkyWeb.Repository.IRepository.RepositoryClients.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ParkyWeb.Repository.RepositoryClients
{
    public class AccountRepository : Repository<User>, IAccountRepository
    {
        private readonly IHttpClientFactory _clientFactory;
        public AccountRepository(IHttpClientFactory clientFactory)
            :base(clientFactory)
        {
            this._clientFactory = clientFactory;
        }
        public async Task<User> LoginAsync(string url, User user)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);

            if (user is null)
                return new User(); 

            request.Content = StringContentHelper.CreateStringContent<User>(user);

            var httpClient = _clientFactory.CreateClient();

            HttpResponseMessage r = await httpClient.SendAsync(request);

            if (r.StatusCode.Equals(HttpStatusCode.OK))
            {
                string jsonString = await r.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<User>(jsonString);
            }
            else
                return new User();
        }

        public async Task<bool> RegisterAsync(string url, User user)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);

            if (user is null)
                return false;

            request.Content = StringContentHelper.CreateStringContent<User>(user);

            var httpClient = _clientFactory.CreateClient();

            HttpResponseMessage r = await httpClient.SendAsync(request);

            if (r.StatusCode.Equals(HttpStatusCode.OK))
            {
                return true;
            }
            else
                return false;
        }
    }
}
