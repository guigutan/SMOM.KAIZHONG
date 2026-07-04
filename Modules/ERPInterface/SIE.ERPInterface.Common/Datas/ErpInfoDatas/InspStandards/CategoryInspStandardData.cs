using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.ERPInterface.Common.Datas.ErpInfoDatas.InspStandards
{
    /// <summary>
    /// 分类检验标准数据
    /// </summary>
    [Serializable]
    public class CategoryInspStandardData : InspStandardDataBase
    {
        /// <summary>
        /// 质量分类
        /// </summary>
        public string QualityCategory { get; set; }
    }
}
