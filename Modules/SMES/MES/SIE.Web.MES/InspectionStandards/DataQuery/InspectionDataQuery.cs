using SIE.MES.InspectionStandards;
using SIE.Web.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.MES.InspectionStandards.DataQuery
{
    /// <summary>
    /// 检验项目数据操作
    /// </summary>
    public class InspectionDataQuery : DataQueryer
    {
        /// <summary>
        /// 根据机型或产品获取当前最大排序值+1
        /// </summary>
        /// <param name="inspectionId"></param>
        /// <param name="modelId"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public int? GetMaxOrderNum(double inspectionId, double? modelId, double? itemId)
        {
            return RT.Service.Resolve<ModelInspectionItemController>().GetMaxOrderNum(inspectionId, modelId, itemId);
        }
    }
}
