using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb
{
    public static class SD
    {
        public static string ApiBaseUrl = "http://localhost:51588/";
        public static string NationalParkAPIPath = ApiBaseUrl + "api/Nationalparks/";
        public static string TrailAPIPath = ApiBaseUrl + "api/Trails/";
        public static string AccountAPIPath = ApiBaseUrl + "api/User/";
    }
    
}
