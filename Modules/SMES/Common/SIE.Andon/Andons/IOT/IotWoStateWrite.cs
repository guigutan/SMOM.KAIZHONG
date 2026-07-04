using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Andon.Andons.IOT
{
    /// <summary>
    /// IOT工单状态写入
    /// </summary>
    public class IotWoStateWrite
    {
        /// <summary>
        /// 
        /// </summary>
        public string id { get; set; } = "guid";
        /// <summary>
        /// 
        /// </summary>
        public string jsonrpc { get; set; } = "2.0";
        /// <summary>
        /// 
        /// </summary>
        public string method { get; set; } = "service";
        /// <summary>
        /// 
        /// </summary>
        public IotWoStateWriteParams @params { get; set; }
    }

    public class IotWoStateWriteParams
    {
        /// <summary>
        /// 
        /// </summary>
        public IotWoStateWriteArgs args { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IotWoStateWriteContext context { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string model { get; set; } = "iiot_thing_order";
        /// <summary>
        /// 
        /// </summary>
        public string tag { get; set; } = "master";
        /// <summary>
        /// 
        /// </summary>
        public string service { get; set; } = "executeOrderWithParamsById";
        /// <summary>
        /// 
        /// </summary>
        public string app { get; set; } = "iiot_thing";
    }

    public class IotWoStateWriteContext
    {
        /// <summary>
        /// 
        /// </summary>
        public string uid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int zoneOffset { get; set; } = 8;
        /// <summary>
        /// 
        /// </summary>
        public string lang { get; set; } = "zh-CN";
    }

    public class IotWoStateWriteArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public string entityId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string orderId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IotWoStateWriteParameterMap parameterMap { get; set; }
    }

    public class IotWoStateWriteParameterMap
    {
        /// <summary>
        /// 状态1：开始or恢复 2暂停
        /// </summary>
        public int WoState { get; set; }
    }
}
