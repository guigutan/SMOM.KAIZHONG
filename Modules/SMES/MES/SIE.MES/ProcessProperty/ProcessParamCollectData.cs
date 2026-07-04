using System.Collections.Generic;

namespace SIE.MES.ProcessProperty
{
    /// <summary>
    /// 工序参数数采 创建数据
    /// </summary>
    public class ProcessParamCollectData
    {
        /// <summary>
        /// SN
        /// </summary>
        public string SN { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// 工序流水码
        /// </summary>
        public string Vornr { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcessCode { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        public string Factory { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 质量状态（True=OK, False=NG）
        /// </summary>
        public string QualityStatus { get; set; }

        /// <summary>
        /// 工艺参数列表
        /// </summary>
        public List<ProcessParamData> ParamList { get; set; } = new List<ProcessParamData>();

        /// <summary>
        /// 子件列表
        /// </summary>
        public List<ProcessParamComponentData> ComponentList { get; set; } = new List<ProcessParamComponentData>();
    }

    /// <summary>
    /// 工艺参数 数据
    /// </summary>
    public class ProcessParamData
    {
        /// <summary>
        /// 参数名
        /// </summary>
        public string ParamName { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        public string ParamValue { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }
    }

    /// <summary>
    /// 子件 数据
    /// </summary>
    public class ProcessParamComponentData
    {
        /// <summary>
        /// 子件SN
        /// </summary>
        public string ComponentSN { get; set; }
    }
}
