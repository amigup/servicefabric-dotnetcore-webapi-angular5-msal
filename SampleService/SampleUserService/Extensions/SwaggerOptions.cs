using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleUserService.Extensions
{
    public class SwaggerOptions
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Realm { get; set; }
        public string AppName { get; set; }
        public string Audience { get; set; }
    }
}
