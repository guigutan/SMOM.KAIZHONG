using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.ERPInterface.Common.Datas.ErpInfoDatas.InspStandards
{
    /// <summary>
    /// 物料检验标准数据
    /// </summary>
    [Serializable]
    public class ItemInspStandardData : InspStandardDataBase
    {
       

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

       
    }
}
