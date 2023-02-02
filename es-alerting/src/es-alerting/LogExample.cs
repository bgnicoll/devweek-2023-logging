using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace es_alerting
{
    public class LogExample
    {
        public DateTime timestamp { get; set; }
        public string message { get; set; }
        public bool raiseTheAlarm { get; set; }
    }
}
