using SIE.Defects;
using System;

namespace SIE.ERPInterface.Common.Datas.ErpInfoDatas
{
    /// <summary>
    /// 缺陷代码信息
    /// </summary>
    [Serializable]
    public class DefectCodeData : ErpInfoData
    {
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 缺陷分类编码
        /// </summary>
        public string DefectCategoryCode { get; set; }

        /// <summary>
        /// 质量类型
        /// </summary>
        public string DefectQualityType { get; set; }

        /// <summary>
        /// 缺陷等级
        /// </summary>
        public string DefectGradeName { get; set; }
    }
}
