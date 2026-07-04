using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Andon.Andons.IOT
{
    /// <summary>
    /// Iot冲压工单写入
    /// </summary>
    public class IotPunchWorkWrite
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
        public IotPunchWorkWriteParams @params { get; set; }
    }

    public class IotPunchWorkWriteParams
    {
        /// <summary>
        /// 
        /// </summary>
        public IotPunchWorkWriteArgs args { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IotPunchWorkWriteContext context { get; set; }
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

    public class IotPunchWorkWriteContext
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

    public class IotPunchWorkWriteArgs
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
        public object parameterMap { get; set; }
    }

    /// <summary>
    /// 冲压共模16工单
    /// </summary>
    public class IotPunchData
    {
        /// <summary>
        /// 设备编码
        /// </summary>
        public string DeviceCode { get; set; }
        /// <summary>
        /// 工单1
        /// </summary>
        public string WorkOrder1 { get; set; }
        /// <summary>
        /// 工单2
        /// </summary>
        public string WorkOrder2 { get; set; }
        /// <summary>
        /// 工单3
        /// </summary>
        public string WorkOrder3 { get; set; }
        /// <summary>
        /// 工单4
        /// </summary>
        public string WorkOrder4 { get; set; }
        /// <summary>
        /// 工单5
        /// </summary>
        public string WorkOrder5 { get; set; }
        /// <summary>
        /// 工单6
        /// </summary>
        public string WorkOrder6 { get; set; }
        /// <summary>
        /// 工单7
        /// </summary>
        public string WorkOrder7 { get; set; }
        /// <summary>
        /// 工单8
        /// </summary>
        public string WorkOrder8 { get; set; }
        /// <summary>
        /// 工单9
        /// </summary>
        public string WorkOrder9 { get; set; }
        /// <summary>
        /// 工单10
        /// </summary>
        public string WorkOrder10 { get; set; }
        /// <summary>
        /// 工单11
        /// </summary>
        public string WorkOrder11 { get; set; }
        /// <summary>
        /// 工单12
        /// </summary>
        public string WorkOrder12 { get; set; }
        /// <summary>
        /// 工单13
        /// </summary>
        public string WorkOrder13 { get; set; }
        /// <summary>
        /// 工单14
        /// </summary>
        public string WorkOrder14 { get; set; }
        /// <summary>
        /// 工单15
        /// </summary>
        public string WorkOrder15 { get; set; }
        /// <summary>
        /// 工单16
        /// </summary>
        public string WorkOrder16 { get; set; }

        /// <summary>
        /// 工单数量1
        /// </summary>
        public int OutPutNum1 { get; set; }
        /// <summary>
        /// 工单数量2
        /// </summary>
        public int OutPutNum2 { get; set; }
        /// <summary>
        /// 工单数量3
        /// </summary>
        public int OutPutNum3 { get; set; }
        /// <summary>
        /// 工单数量4
        /// </summary>
        public int OutPutNum4 { get; set; }
        /// <summary>
        /// 工单数量5
        /// </summary>
        public int OutPutNum5 { get; set; }
        /// <summary>
        /// 工单数量6
        /// </summary>
        public int OutPutNum6 { get; set; }
        /// <summary>
        /// 工单数量7
        /// </summary>
        public int OutPutNum7 { get; set; }
        /// <summary>
        /// 工单数量8
        /// </summary>
        public int OutPutNum8 { get; set; }
        /// <summary>
        /// 工单数量9
        /// </summary>
        public int OutPutNum9 { get; set; }
        /// <summary>
        /// 工单数量10
        /// </summary>
        public int OutPutNum10 { get; set; }
        /// <summary>
        /// 工单数量11
        /// </summary>
        public int OutPutNum11 { get; set; }
        /// <summary>
        /// 工单数量12
        /// </summary>
        public int OutPutNum12 { get; set; }
        /// <summary>
        /// 工单数量13
        /// </summary>
        public int OutPutNum13 { get; set; }
        /// <summary>
        /// 工单数量14
        /// </summary>
        public int OutPutNum14 { get; set; }
        /// <summary>
        /// 工单数量15
        /// </summary>
        public int OutPutNum15 { get; set; }
        /// <summary>
        /// 工单数量16
        /// </summary>
        public int OutPutNum16 { get; set; }

        /// <summary>
        /// 设备冲压次数
        /// </summary>
        public int PutNum { get; set; } = 0;
    }
}
