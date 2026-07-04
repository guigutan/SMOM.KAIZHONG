using IronPython.Compiler.Ast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Andon.Andons.IOT
{
    /// <summary>
    /// IOT读取数据
    /// </summary>
    public class IotWorkRead
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
        public IotWorkReadparams @params { get; set; }
    }

    public class IotWorkReadparams
    {
        /// <summary>
        /// 
        /// </summary>
        public IotWorkReadArgs args { get; set; }
        /// <summary>
        public IotWorkReadContext context { get; set; }
        /// 
        /// </summary>
        /// <summary>
        /// 
        /// </summary>
        public string model { get; set; } = "iiot_thing_property";
        /// <summary>
        /// 
        /// </summary>
        public string tag { get; set; } = "master";
        /// <summary>
        /// 
        /// </summary>
        public string service { get; set; } = "searchPropertyRealData";
        /// <summary>
        /// 
        /// </summary>
        public string app { get; set; } = "iiot_thing";
    }

    public class IotWorkReadContext
    {
        /// <summary>
        /// 
        /// </summary>
        public string uid { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string lang { get; set; } = "zh-CN";
    }

    public class IotWorkReadArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public List<EntityIdCustomPropertyIdsItem> entityIdCustomPropertyIds { get; set; }
    }

    public class EntityIdCustomPropertyIdsItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string entityId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> customPropertyId { get; set; }
    }
}
