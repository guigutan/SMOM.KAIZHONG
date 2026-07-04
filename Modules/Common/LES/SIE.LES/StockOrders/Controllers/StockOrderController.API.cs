using SIE.Api;
using SIE.LES.StockOrders.APIModels;
using SIE.LES.StockOrders.Service;
using SIE.Warehouses;
using System.Collections.Generic;

namespace SIE.LES.StockOrders.Controllers
{
    /// <summary>
    /// 备料单API
    /// </summary>
    public partial class StockOrderController : DomainController
    {
        /// <summary>
        /// 获取员工权限下的工厂信息
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [ApiService("获取员工权限下的工厂信息")]
        [return: ApiReturn("返回工厂数据集合：BaseInfo")]
        public virtual List<BaseInfo> GetFactoryInfos([ApiParameter("登录人ID")] double employeeId)
        {
            return RT.Service.Resolve<StockOrderService>().GetFactoryInfos(employeeId);
        }

        /// <summary>
        /// 获取工厂下的车间
        /// </summary>
        /// <param name="factoryId"></param>
        /// <returns></returns>
        [ApiService("获取员工工厂下的车间信息")]
        [return: ApiReturn("返回车间数据集合：BaseInfo")]
        public virtual List<BaseInfo> GetWorkShopInfos([ApiParameter("工厂ID")] double factoryId)
        {
            return RT.Service.Resolve<StockOrderService>().GetWorkShopInfos(factoryId);
        }

        /// <summary>
        /// 获取员工工厂下的资源信息
        /// </summary>
        /// <param name="factoryId"></param>
        /// <param name="workShopId"></param>
        /// <returns></returns>
        [ApiService("获取员工工厂下的资源信息")]
        [return: ApiReturn("返回资源数据集合以及车间信息：BaseInfo")]
        public virtual List<BaseInfo> GetWipRecourceInfos([ApiParameter("工厂ID")] double factoryId, [ApiParameter("工厂ID")] double? workShopId)
        {
            return RT.Service.Resolve<StockOrderService>().GetWipResourceInfos(factoryId, workShopId);
        }

        /// <summary>
        /// 获取资源下的线边仓仓库和库位
        /// </summary>
        /// <param name="wipId"></param>
        /// <returns></returns>
        [ApiService("获取资源下的线边仓仓库和库位")]
        [return: ApiReturn("返回资源下的线边仓仓库和库位信息：WareAndStorage")]
        public virtual WareAndStorage GetWareAndStorage([ApiParameter("生产资源ID")] double? wipId)
        {
            return RT.Service.Resolve<StockOrderService>().GetWareAndStorage(wipId);
        }


        /// <summary>
        /// 获取生产资源下的产线线边仓
        /// </summary>
        /// <param name="wipId"></param>
        /// <returns></returns>
        [ApiService("获取生产资源下的产线线边仓")]
        [return: ApiReturn("返回线边仓信息：BaseInfo")]
        public virtual List<BaseInfo> GetLineWarehouse([ApiParameter("生产资源ID")] double? wipId)
        {
            return RT.Service.Resolve<StockOrderService>().GetLineWarehouse(wipId);
        }

        /// <summary>
        /// 获取生产资源下的产线线边仓库位
        /// </summary>
        /// <param name="wipId"></param>
        /// <param name="wareId"></param>
        /// <returns></returns>
        [ApiService("获取生产资源下的产线线边仓库位")]
        [return: ApiReturn("返回线边仓库位信息：BaseInfo")]
        public virtual List<BaseInfo> GetLineStorage([ApiParameter("生产资源ID")] double? wipId, [ApiParameter("仓库资源ID")] double wareId)
        {
            return RT.Service.Resolve<StockOrderService>().GetLineStorage(wipId, wareId);
        }

        /// <summary>
        /// 获取备料单接收方式
        /// </summary>
        /// <returns></returns>
        [ApiService("获取备料单接收方式")]
        [return: ApiReturn("备料单接收方式StockReceiveType")]
        public virtual StockReceiveType GetStockReceiveType()
        {
            return RT.Service.Resolve<StockOrderService>().GetStockReceiveType();
        }

        /// <summary>
        /// 更新用户的备料单APP基本信息
        /// </summary>
        [ApiService("更新用户的备料单APP基本信息")]
        [return: ApiReturn("更新用户的备料单APP基本信息")]
        public virtual void UpdataUserInfo([ApiParameter("创建备料单基本信息")] StockBaseInfo stockBaseInfo)
        {
            RT.Service.Resolve<StockOrderService>().UpdataUserInfo(stockBaseInfo);
        }

        /// <summary>
        /// 获取用户的备料单APP基本信息
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [ApiService("获取用户的备料单APP基本信息")]
        [return: ApiReturn("用户的备料单APP基本信息")]
        public virtual StockBaseInfo GetUserInfo([ApiParameter("登录人ID")] double employeeId)
        {
            return RT.Service.Resolve<StockOrderService>().GetUserInfo(employeeId);
        }

        /// <summary>
        /// 获取物料信息
        /// </summary>
        /// <param name="itemCode">物料编码</param>
        /// <param name="warehouseId">仓库ID</param>
        /// <returns></returns>
        [ApiService("获取物料信息")]
        [return: ApiReturn("物料信息")]
        public virtual BaseInfo GetItemInfo([ApiParameter("物料编码")] string itemCode, [ApiParameter("仓库ID")] double? warehouseId)
        {
            return RT.Service.Resolve<StockOrderService>().GetItemInfo(itemCode, warehouseId);
        }

        /// <summary>
        /// 根据物料获取工单
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="workShopId"></param>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        [ApiService("获取工单信息")]
        [return: ApiReturn("工单信息")]
        public virtual List<BaseInfo> GetWorkOders([ApiParameter("物料Id")] double? itemId, [ApiParameter("车间Id")] double? workShopId, [ApiParameter("资源Id")] double? resourceId)
        {
            return RT.Service.Resolve<StockOrderService>().GetWorkOders(itemId, workShopId, resourceId);
        }

        /// <summary>
        /// 创建备料单
        /// </summary>
        /// <param name="baseInfo"></param>
        /// <param name="itemId"></param>
        /// <param name="stockQty"></param>
        /// <param name="workOrderId">工单Id</param>
        /// <param name="itemExtProp">扩展属性值</param>
        /// <param name="itemExtPropName">扩展属性显示名称</param>
        /// <param name="isEnableExtProp">是否启用扩展属性</param>
        /// <returns></returns>
        [ApiService("创建备料单")]
        [return: ApiReturn("创建备料单")]
        public virtual string CreateStockOrder(StockBaseInfo baseInfo, double itemId, decimal stockQty, double? workOrderId, string itemExtProp, string itemExtPropName, bool isEnableExtProp)
        {
            return RT.Service.Resolve<StockOrderService>().CreateStockOrder(baseInfo, itemId, itemExtProp, itemExtPropName, isEnableExtProp, stockQty, workOrderId);
        }
    }
}
