using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Andon.Andons.IOT
{
    /// <summary>
    /// IOT工单写入
    /// </summary>
    public class IotWorkWrite
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
        public IotWorkWriteParams @params { get; set; }
    }

    public class IotWorkWriteParams
    {
        /// <summary>
        /// 
        /// </summary>
        public IotWorkWriteArgs args { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IotWorkWriteContext context { get; set; }
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

    public class IotWorkWriteContext
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

    public class IotWorkWriteArgs
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
        public IotWorkWriteParameterMap parameterMap { get; set; }
    }

    public class IotWorkWriteParameterMap
    {
        /// <summary>
        /// 工单
        /// </summary>
        public string WorkOrder { get; set; }
        /// <summary>
        /// 设备编码
        /// </summary>
        public string DeviceCode { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int OutPutNum { get; set; }
    }
}