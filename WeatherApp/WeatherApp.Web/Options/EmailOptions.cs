using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherApp.Web.Options
{
    public class EmailOptions
    {
        public string SenderAdress { get; set; }
        public string SenderName { get; set; }
        public string SenderPass { get; set; }
        public string SenderHost { get; set; }
        public int SenderPort { get; set; }
        public bool UseSsl { get; set; }
    }
}
