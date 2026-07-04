using SIE.Kit.UrgentOrder.ItemUrgentOrders;
using SIE.Web.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Kit.UrgentOrder.ItemUrgentOrders.DataQuery
{
    /// <summary>
    /// 物料加急单查询器
    /// </summary>
    public class ItemUrgentOrderDataQueryer : DataQueryer
    {
        /// <summary>
        /// 获取物料加急单时间配置项值
        /// </summary>
        /// <returns>物料加急单时间配置项值</returns>
        public double GetItemUrgentOrderDate()
        {
            var time = RT.Service.Resolve<ItemUrgentOrderController>().GetItemUrgentOrderDateConfig();
            return time;
        }
    }
}
