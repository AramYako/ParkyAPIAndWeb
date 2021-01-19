using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ParkyWeb.Helpers
{
    public static class JsonHelper
    {
        public static string ConvertObjToJson<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T DeserilizeObjToJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

    }
}
