using IronPython.Compiler.Ast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Andon.Andons.IOT
{
    /// <summary>
    /// ITO安灯写入
    /// </summary>
    public class IotWrite
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
        public IotWriteParams @params { get; set; }
    }

    public class IotWriteParams
    {
        /// <summary>
        /// 
        /// </summary>
        public IotWriteArgs args { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IotWriteContext context { get; set; }
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

    public class IotWriteContext
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

    public class IotWriteArgs
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
        public IotWriteParameterMap parameterMap { get; set; }
    }
    public class IotWriteParameterMap
    {
        /// <summary>
        /// 
        /// </summary>
        public int Tag1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Tag2 { get; set; }
    }
}
