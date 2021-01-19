using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyApi
{
    public class AppSettings
    {
        public static readonly string AppSettingsSection = "AppSettingsJWT";
        public string Secret { get; set; }
    }
}
