using SIE.MES.LoadItems;
using SIE.Web.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.MES.LoadItems.DeductItems.DataQuerys
{
    /// <summary>
    /// 工单耗用单前后端数据请求
    /// </summary>
    public class WoCostItemDataQuery : DataQueryer
    {
        /// <summary>
        /// 获取工单对应的工厂资源
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        public FactoryInfo GetFactoryInfoByWoId(double workOrderId)
        {
            return RT.Service.Resolve<WoCostItemController>().GetFactoryInfo(workOrderId);
        }
    }
}
