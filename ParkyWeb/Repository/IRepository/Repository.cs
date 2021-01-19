using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ParkyWeb.Helpers;
using ParkyWeb.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ParkyWeb.Repository.IRepository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IHttpClientFactory _clientFactory;

        public Repository(IHttpClientFactory clientFactory)
        {
            this._clientFactory = clientFactory;
        }
        public async Task<bool> CreateAsync(string url, T objToCreate, string token="")
        {
            if (objToCreate == null)
                return false; 

            var request = new HttpRequestMessage(HttpMethod.Post, url);

            request.Content = StringContentHelper.CreateStringContent<T>(objToCreate);

            var httpClient = _clientFactory.CreateClient();

            SetDefaultRequestHeadersAuthentication(token, httpClient);

            HttpResponseMessage r =  await httpClient.SendAsync(request);

            if (r.StatusCode.Equals(HttpStatusCode.Created))
                return true;
            else
                return false;

        }

        public async Task<bool> DeleteAsync(string url, int id, string token = "")
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, url + id);

            var httpClient = _clientFactory.CreateClient();

            SetDefaultRequestHeadersAuthentication(token, httpClient);

            HttpResponseMessage r = await httpClient.SendAsync(request);

            if (r.StatusCode.Equals(HttpStatusCode.NoContent))
                return true;
            else
                return false;
        }

        public async Task<IEnumerable<T>> GetAllAsync(string url, string token = "")
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var httpClient = _clientFactory.CreateClient();


            SetDefaultRequestHeadersAuthentication(token, httpClient);

            HttpResponseMessage r = await httpClient.SendAsync(request);

            if (r.StatusCode.Equals(HttpStatusCode.OK))
            {
                string jsonString = await r.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<T>>(jsonString);
            }
            
            return null;
        }

        public async Task<T> GetAsync(string url, int id, string token = "")
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url +id);

            var httpClient = _clientFactory.CreateClient();


            SetDefaultRequestHeadersAuthentication(token, httpClient);

            HttpResponseMessage r = await httpClient.SendAsync(request);

            if (r.StatusCode.Equals(HttpStatusCode.OK))
            {
                string jsonString = await r.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(jsonString);
            }

            return null;
        }

        public async Task<bool> UpdateAsync(string url, T objToCreate, string token = "")
        {
            var request = new HttpRequestMessage(HttpMethod.Patch, url);

            request.Content = StringContentHelper.CreateStringContent<T>(objToCreate);

            var httpClient = _clientFactory.CreateClient();


            SetDefaultRequestHeadersAuthentication(token, httpClient);

            HttpResponseMessage r = await httpClient.SendAsync(request);

            if (r.StatusCode.Equals(HttpStatusCode.NoContent))
            {
                return true;
            }
            return false;
        }

        protected void SetDefaultRequestHeadersAuthentication(string token, HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(AuthenticationSchemas.Bearer.ToString(), token);
        }
    }
}
