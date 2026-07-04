using System;

namespace SIE.ERPInterface.Common.Datas
{
    /// <summary>
    /// ERP单位数据
    /// </summary>
    [Serializable]
    public class UnitData : ErpInfoData
    {
        /// <summary>
        /// 单位精度
        /// </summary>
        public int Precision { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }
    }
}
