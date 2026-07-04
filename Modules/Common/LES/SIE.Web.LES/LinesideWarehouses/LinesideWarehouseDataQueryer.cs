using SIE.LES.LinesideWarehouses;
using SIE.LES.LinesideWarehouses.Models;
using SIE.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.LES.LinesideWarehouses
{
    /// <summary>
    /// 产线线边仓数据请求
    /// </summary>
    public class LinesideWarehouseDataQueryer : DataQueryer
    {
        /// <summary>
        /// 获取生产资源的工厂车间
        /// </summary>
        /// <param name="wipId"></param>
        /// <returns></returns>
        public virtual WipEnterpriseInfo GetWipResourceInfo(double wipId)
        {
            return RT.Service.Resolve<LinesideWarehouseController>().GetWipResourceInfo(wipId);
        }

        /// <summary>
        /// 获取车间的工厂
        /// </summary>
        /// <param name="workShopId"></param>
        /// <returns></returns>
        public virtual WipEnterpriseInfo GetWorkShopInfo(double workShopId)
        {
            return RT.Service.Resolve<LinesideWarehouseController>().GetWorkShopInfo(workShopId);
        }
    }
}
