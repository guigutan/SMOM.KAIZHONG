using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Andon.Andons.IOT
{
    [Serializable]
    public class IotReadReturn
    {
        /// <summary>
        /// 
        /// </summary>
        public IotReadResult result { get; set; }
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
        public string error { get; set; }
    }

    [Serializable]
    public class IotReadResult
    {
        /// <summary>
        /// 
        /// </summary>
        public List<IotReadDataItem> data { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IotReadContext context { get; set; }
    }
    [Serializable]
    public class IotReadContext
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

    public class IotReadDataItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string entity_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string custom_property_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string property_id { get; set; }
    }
}
