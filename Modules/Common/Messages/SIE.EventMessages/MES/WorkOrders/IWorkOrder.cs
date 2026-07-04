using SIE.EventMessages.MES.WorkOrders.Models;
using SIE.ObjectModel;
using System.Collections.Generic;

namespace SIE.EventMessages.MES.WorkOrders
{
    /// <summary>
    /// 工单接口
    /// </summary>
    [Services.Service(FallbackType = typeof(DefaultWorkOrderInterface))]
    public interface IWorkOrder
    {
        /// <summary>
        /// 根据工单号获取工单信息
        /// </summary>
        /// <param name="woNos">工单号</param>
        /// <returns>工单列表</returns>
        List<WorkOrderSimpleInfo> GetWorkOrderList(List<string> woNos);

        /// <summary>
        /// 根据合并工单号列表获取工单信息
        /// </summary>
        /// <param name="mergeWoNos">合并工单号列表</param>
        /// <returns>工单信息</returns>
        List<WorkOrderSimpleInfo> GetWorkOrders(List<string> mergeWoNos);

        /// <summary>
        /// 根据工单号和物料汇总消耗数量
        /// </summary>
        /// <param name="woNo">工单号</param>
        /// <param name="itemId">物料ID</param>
        /// <returns>消耗数量</returns>
        decimal GetSumConsumedQty(string woNo, double itemId);

        /// <summary>
        /// 根据工单号列表（可空），工单状态（可空）获取工单信息
        /// </summary>
        /// <param name="woNos">工单号列表（可空）</param>
        /// <param name="states">工单状态（可空）</param>
        /// <returns>工单信息</returns>
        List<WipWoSimpleInfo> GetWipWoList(List<string> woNos, List<int> states);

        /// <summary>
        /// 根据工单号列表获取工单产量信息
        /// </summary>
        /// <param name="nos">工单号列表</param>
        /// <returns>工单产量信息列表</returns>
        List<OutputInfo> GetOutputInfos(List<string> nos);

        /// <summary>
        /// 获取工单数据
        /// </summary>
        /// <param name="workShopId">车间ID</param>
        /// <param name="resourceId">资源ID</param>
        /// <param name="productId">产品ID</param>
        /// <param name="planStartDate">计划开始时间</param>
        /// <param name="planFinishDate">计划完成时间</param>
        /// <returns>工单数据</returns>
        List<WorkOrderData> GetWorkOrderDatas(double? workShopId, double? resourceId, double? productId, DateRange planStartDate, DateRange planFinishDate);
    }

    /// <summary>
    /// 工单接口的默认实现
    /// </summary>
    class DefaultWorkOrderInterface : IWorkOrder
    {
        /// <summary>
        /// 根据工单号和物料获取消耗数量汇总
        /// </summary>
        /// <param name="woNo">工单号</param>
        /// <param name="itemId">物料ID</param>
        /// <returns></returns>
        public decimal GetSumConsumedQty(string woNo, double itemId)
        {
            return 0;
        }

        /// <summary>
        /// 根据工单号和物料汇总消耗数量
        /// </summary>
        /// <param name="woNos">工单号</param>
        /// <returns>消耗数量</returns>
        public List<WorkOrderSimpleInfo> GetWorkOrderList(List<string> woNos)
        {
            return new List<WorkOrderSimpleInfo>();
        }

        /// <summary>
        /// 根据工单号列表（可空），工单状态（可空）获取工单信息
        /// </summary>
        /// <param name="woNos">工单号列表（可空）</param>
        /// <param name="states">工单状态（可空）</param>
        /// <returns>工单信息</returns>
        public List<WipWoSimpleInfo> GetWipWoList(List<string> woNos, List<int> states)
        {
            return new List<WipWoSimpleInfo>();
        }

        /// <summary>
        /// 根据合并工单号列表获取工单信息
        /// </summary>
        /// <param name="mergeWoNos">合并工单号列表</param>
        /// <returns>工单信息</returns>
        public List<WorkOrderSimpleInfo> GetWorkOrders(List<string> mergeWoNos)
        {
            return new List<WorkOrderSimpleInfo>();
        }

        /// <summary>
        /// 根据工单号列表获取工单产量信息
        /// </summary>
        /// <param name="nos">工单号列表</param>
        /// <returns>工单产量信息列表</returns>
        public List<OutputInfo> GetOutputInfos(List<string> nos)
        {
            return new List<OutputInfo>();
        }

        /// <summary>
        /// 获取工单数据
        /// </summary>
        /// <param name="workShopId">车间ID</param>
        /// <param name="resourceId">资源ID</param>
        /// <param name="productId">产品ID</param>
        /// <param name="planStartDate">计划开始时间</param>
        /// <param name="planFinishDate">计划完成时间</param>
        /// <returns>工单数据</returns>
        public List<WorkOrderData> GetWorkOrderDatas(double? workShopId, double? resourceId, double? productId, DateRange planStartDate, DateRange planFinishDate)
        {
            return new List<WorkOrderData>();
        }
    }
}
