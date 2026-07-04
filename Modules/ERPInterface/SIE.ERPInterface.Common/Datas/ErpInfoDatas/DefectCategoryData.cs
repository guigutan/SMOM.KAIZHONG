using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.ERPInterface.Common.Datas.ErpInfoDatas
{
    /// <summary>
    /// 缺陷分类信息
    /// </summary>
    [Serializable]
    public class DefectCategoryData : ErpInfoData
    {
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }
}
