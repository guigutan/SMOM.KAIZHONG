using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.CrossPlatform.Collect.Common.Log
{
    public class LogBaseMessage
    {
        public long Id { get; set; }
        public string LogLevel { get; set; }
        public string TraceId { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public string Client { get; set; }
        public string Session { get; set; }
        public string CreateTime { get; set; }
        public object Data { get; set; }
        public string Error { get; set; }
    }
}
