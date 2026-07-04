using SIE.EventMessages.MES.WorkOrders;
using SIE.EventMessages.MES.WorkOrders.Models;
using SIE.EventMessages.WMS.Shipment;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.LES.Interfaces
{
    /// <summary>
    /// 工单接口控制器
    /// </summary>
    public class WoInfoForLesController : DomainController, IIsStartLes
    {
        /// <summary>
        /// 获取工单信息
        /// </summary>
        /// <param name="resourceIds">生产资源Id列表</param>
        public virtual List<WoInfoForLes> GetWoInfoForLes(List<double?> resourceIds)
        {
            return RT.Service.Resolve<IWorkOrderQuery>().GetWoInfoForLes(null, resourceIds, null);
        }

        /// <summary>
        /// 获取工单信息
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        public virtual List<WoInfoForLes> GetWoInfoForLesByWorkOrderId(double workOrderId)
        {
            return RT.Service.Resolve<IWorkOrderQuery>().GetWoInfoForLes(workShopId: null,
                resourceIds: null, workOrderIds: new List<double> { workOrderId });
        }


        /// <summary>
        /// 启用LES
        /// </summary>
        /// <returns></returns>
        public virtual bool IsStartLes()
        {
            return true;
        }
    }
}
