using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Andon.Andons.IOT
{
    public class IotWriteReturn
    {
        /// <summary>
        /// 
        /// </summary>
        public IotWriteReturnResult result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string jsonrpc { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Error error { get; set; }
    }

    public class IotWriteReturnResult
    {
        /// <summary>
        /// 
        /// </summary>
        public bool data { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IotWriteReturnContext context { get; set; }
    }

    public class IotWriteReturnContext
    {
        /// <summary>
        /// 
        /// </summary>
        public string @Class { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string uid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string tenantId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string skin { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string lang { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string curSg { get; set; }
    }

    public class Error
    {
        /// <summary>
        /// 
        /// </summary>
        public int code { get; set; }
        /// <summary>
        /// 指令信息有误，当前获取到的指令数量为0
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Data data { get; set; }
    }
    public class Data
    {
        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string debug { get; set; }
    }
}
