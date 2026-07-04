using SIE.Defects.Defects;
using System;

namespace SIE.ERPInterface.Common.Datas.ErpInfoDatas
{
    /// <summary>
    /// 缺陷等级信息
    /// </summary>
    [Serializable]
    public class DefectGradeData : ErpInfoData
    {
        /// <summary>
        /// 严重度
        /// </summary>
        public string DefectSeverity { get; set; }
    }
}
