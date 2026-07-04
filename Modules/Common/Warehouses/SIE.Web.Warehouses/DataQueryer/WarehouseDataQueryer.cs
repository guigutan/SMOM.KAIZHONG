using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using SIE.Warehouses;
using SIE.Core.Enums;

namespace SIE.Web.Warehouses.DataQueryer
{
    /// <summary>
    /// 仓库查询
    /// </summary>
    public class WarehouseDataQueryer : Data.DataQueryer
    {
        /// <summary>
        /// 获取库存组织
        /// </summary>
        /// <param name="isOnlyCurOrg"></param>
        /// <param name="keyword"></param>
        /// <param name="AllotModel">发运单调拨模式</param>
        /// <returns></returns>
        public EntityList GetWarehouseInvorg(bool isOnlyCurOrg, int? AllotModel, string keyword)
        {
            if (isOnlyCurOrg)
                return RT.Service.Resolve<WarehouseController>().GetEnableWarehousesWithOrgName(null, keyword, allotModel: AllotModel);
            else
                return RT.Service.Resolve<WarehouseController>().GetWarehouseByOtherInvOrg(null, keyword, allotModel: AllotModel);
        }

        /// <summary>
        /// 获取仓库数据，ASN跨组织入库、转仓入库
        /// </summary>
        /// <param name="orderType">单据大类</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        public EntityList GetWarehouseInvorgByAsn(OrderType orderType, string keyword)
        {
            if (orderType == OrderType.CrossOrgTransferIn)
            {
                //关联仓库档案中的数据，过滤掉禁用的或冻结的或“不管库存=False”的数据.当是跨组织调拨入库时，仓库数据只保留非当前登录库存组织的数据
                return RT.Service.Resolve<WarehouseController>().GetSourceWarehouseByOtherInvOrg(null, keyword, true);
            }
            else if (orderType == OrderType.WhTransferIn)
            {
                //关联仓库档案中的数据，过滤掉禁用的或冻结的或“不管库存=False”的数据.转仓入库、跨组织调拨入库需要维护来源仓库，其他订单类型时来源仓库、来源仓库名称栏位隐藏
                return RT.Service.Resolve<WarehouseController>().GetEnableWarehousesWithOrgName(null, keyword, true);
            }
            else
                return new EntityList<Warehouse>();
        }
    }
}
