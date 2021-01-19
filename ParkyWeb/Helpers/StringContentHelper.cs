using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ParkyWeb.Helpers
{
    public static class StringContentHelper
    {
        
        public static StringContent CreateStringContent<T>(T obj)
        {
            var jsonString = JsonHelper.ConvertObjToJson<T>(obj);

            return new StringContent(jsonString, Encoding.UTF8, ContentTypes.JsonType);
        }
    }
}
