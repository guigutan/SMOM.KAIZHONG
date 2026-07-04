using SIE.Api;
using SIE.Core.ApiModels;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.EventMessages.MES.Models;
using SIE.EventMessages.MES.WorkOrders.Models;
using SIE.Services;
using System;
using System.Collections.Generic;

namespace SIE.EventMessages.MES.WorkOrders
{
    /// <summary>
    /// 工单关联接口
    /// </summary>
    [Service(FallbackType = typeof(DefaultWorkOrderQuery))]
    public interface IWorkOrderQuery
    {
        /// <summary>
        /// 根据工单获取工艺路线(拼接)
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        string GetWorkOrderLayoutByWoId(double Id);

        /// <summary>
        /// 根据工单获取工艺路线按照工序编码(拼接)
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        string GetWorkOrderLayoutByWoCodeId(double Id);

        /// <summary>
        /// 根据车间和产线获取工单列表
        /// </summary>
        /// <param name="workShopId">车间Id</param>
        /// <param name="resourceId">产线Id</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        /// <param name="isFilterPause">是否过滤暂停工单</param>
        /// <returns>工单列表</returns>
        EntityList<WorkOrder> GetWorkOrderList(double? workShopId, double? resourceId, PagingInfo pagingInfo, string keyword,bool isFilterPause=false);

        /// <summary>
        /// 根据工单Id获取产线和车间
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <returns>工单信息</returns>
        WorkOrderInfo GetWorkOrderResource(double workOrderId);

        /// <summary>
        /// 获取工单工厂Id
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        double? GetWorkOrderFactoryId(double workOrderId);

        /// <summary>
        /// 获取工单工段数据
        /// </summary>
        /// <returns></returns>
        Dictionary<double, List<double>> GetDicWoProcessSegment(List<double> workIds);

        /// <summary>
        /// 获取API分页工单信息
        /// </summary>
        /// <param name="queryInfo"></param>
        /// <returns></returns>
        PagingBaseDataInfo GetPagingWorkOrdertInfos([ApiParameter("工单查询信息")] WorkOrderQueryInfo queryInfo);

        /// <summary>
        /// 获取工单信息提供给LES备料计算使用
        /// </summary>
        /// <param name="workShopId">车间Id</param>
        /// <param name="resourceIds">生产资源Id列表</param>
        /// <param name="workOrderIds">工单ID列表</param>
        /// <returns>工单信息列表</returns>
        List<WoInfoForLes> GetWoInfoForLes(double? workShopId, List<double?> resourceIds, List<double> workOrderIds);

        /// <summary>
        /// 根据产线Ids获取在制工单Ids
        /// </summary>
        /// <param name="resourceIds"></param>
        /// <returns></returns>
        List<WorkOrderInfo> GetWipWorkOrderIds(List<double?> resourceIds);

        /// <summary>
        /// 根据工单Ids获取工序bom
        /// </summary>
        /// <param name="woOrderIds"></param>
        /// <returns></returns>
        List<WoProcessBomInfo> GetWoProcessBomInfos(List<double> woOrderIds);

        /// <summary>
        /// 根据工单Ids获取工单耗用单
        /// </summary>
        /// <param name="woOrderIds"></param>
        /// <returns></returns>
        List<WoOrderCostInfo> GetWoOrderCostInfos(List<double> woOrderIds);

        /// <summary>
        /// 根据工单Ids获取单体关键件
        /// </summary>
        /// <param name="woOrderIds"></param>
        /// <returns></returns>
        List<WipProductKeyItem> GetSingleWipProductKeyItems(List<double> woOrderIds);

        /// <summary>
        /// 根据工单Ids获取批次关键件
        /// </summary>
        /// <param name="woOrderIds"></param>
        /// <returns></returns>
        List<WipProductKeyItem> GetBatchWipProductKeyItems(List<double> woOrderIds);

        /// <summary>
        /// 根据工单号获取在制数量(单体+批次条码数)
        /// </summary>
        /// <param name="planTaskIds"></param>
        /// <returns></returns>
        IReadOnlyList<WoOrderTaskInfo> GetProductBarcodeCount(IReadOnlyList<string> planTaskIds);

        /// <summary>
        /// APS触发完工
        /// </summary>
        /// <param name="planTaskIds"></param>
        IReadOnlyList<WoOrderTaskInfo> APSFinishWorkOrder(IReadOnlyList<string> planTaskIds);

        /// <summary>
        /// APS触发取消完工
        /// </summary>
        /// <param name="planTaskIds"></param>
        /// <returns></returns>
        IReadOnlyList<WoOrderTaskInfo> APSCancelFinishWoOrder(IReadOnlyList<string> planTaskIds);

        /// <summary>
        /// 备料单弹框查询工单
        /// </summary>
        /// <param name="no">工单号</param>
        /// <param name="factoryId">工厂</param>
        /// <param name="workshopId">车间</param>
        /// <param name="resourceId">资源</param>
        /// <param name="pcode">产品编码</param>
        /// <param name="pname">产品名称</param>
        /// <param name="exceptClose">排除关闭状态</param>
        /// <param name="exceptCancel">排除取消状态</param>
        /// <param name="planBeginStart">计划开始时间</param>
        /// <param name="planBeginEnd">计划开始时间</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns></returns>
        WorkOrderLesInfoWithCount GetWorkOrderList(string no, double? factoryId, double? workshopId, double? resourceId, string pcode, string pname, bool exceptClose = false, bool exceptCancel = false, DateTime? planBeginStart = null, DateTime? planBeginEnd = null, PagingInfo pagingInfo = null);

        /// <summary>
        /// LES获取工单信息
        /// </summary>
        /// <param name="woIds">工单Ids</param>
        /// <returns></returns>
        Dictionary<double, WorkOrderInfo> GetWorkOrderList(List<double> woIds);

        /// <summary>
        /// 获取工单简要信息
        /// </summary>
        /// <param name="no">工单号</param>
        /// <returns></returns>
        List<WorkOrderSimpleInfo> GetWorkOrderSimpleInfos(string no);

        /// <summary>
        /// 获取产线下工单bom(含节拍)
        /// </summary>
        /// <param name="resourceIds">工单产线Ids</param>
        /// <param name="itemIds">工单Bom物料Ids</param>
        /// <returns></returns>
        List<WoBomPushPreInfo> GetWoBomPushPreInfos(List<double> resourceIds, List<double> itemIds);
    }
    /// <summary>
    /// 工单关联默认实现
    /// </summary>
    public class DefaultWorkOrderQuery : IWorkOrderQuery
    {

        /// <summary>
        /// 根据工单获取工艺路线(拼接)
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public string GetWorkOrderLayoutByWoId(double Id)
        {
            return string.Empty;
        }

        /// <summary>
        /// 根据工单获取工艺路线工序编码(拼接)
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public string GetWorkOrderLayoutByWoCodeId(double Id)
        {
            return string.Empty;
        }

        /// <summary>
        /// 根据车间和产线获取工单列表
        /// </summary>
        /// <param name="workShopId">车间Id</param>
        /// <param name="resourceId">产线Id</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        /// <param name="isFilterPause">是否过滤暂停工单</param>
        /// <returns>工单列表</returns>
        public EntityList<WorkOrder> GetWorkOrderList(double? workShopId, double? resourceId, PagingInfo pagingInfo, string keyword,bool isFilterPause= false)
        {

            var wos = new EntityList<WorkOrder>();
            return wos;
        }

        /// <summary>
        /// 根据工单id获取产线和车间
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <returns></returns>
        public WorkOrderInfo GetWorkOrderResource(double workOrderId)
        {
            return new WorkOrderInfo();
        }

        /// <summary>
        /// 获取工单工厂Id
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>

        public double? GetWorkOrderFactoryId(double workOrderId)
        {
            return null;
        }

        /// <summary>
        /// 获取工单集合的工段集合
        /// </summary>
        /// <param name="workIds">工单ID集合</param>
        /// <returns></returns>
        public Dictionary<double, List<double>> GetDicWoProcessSegment(List<double> workIds)
        {
            return new Dictionary<double, List<double>>();
        }

        /// <summary>
        /// 获取API分页工单信息
        /// </summary>
        /// <param name="queryInfo"></param>
        /// <returns></returns>
       
        public PagingBaseDataInfo GetPagingWorkOrdertInfos([ApiParameter("工单查询信息")] WorkOrderQueryInfo queryInfo)
        {
            return new PagingBaseDataInfo();
        }

        /// <summary>
        /// 获取工单信息提供给LES备料计算使用
        /// </summary>
        /// <param name="workShopId">车间Id</param>
        /// <param name="resourceIds">生产资源Id列表</param>
        /// <param name="workOrderIds">工单Id列表</param>
        /// <returns>工单信息列表</returns>        
        public List<WoInfoForLes> GetWoInfoForLes(double? workShopId, List<double?> resourceIds, List<double> workOrderIds)
        {
            return new List<WoInfoForLes>();
        }

        /// <summary>
        /// 根据产线Ids获取在制工单Ids
        /// </summary>
        /// <param name="resourceIds"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public List<WorkOrderInfo> GetWipWorkOrderIds(List<double?> resourceIds)
        {
            return new List<WorkOrderInfo>();
        }

        /// <summary>
        /// 根据工单Ids获取工序bom
        /// </summary>
        /// <param name="woOrderIds"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public List<WoProcessBomInfo> GetWoProcessBomInfos(List<double> woOrderIds)
        {
            return new List<WoProcessBomInfo>();
        }

        /// <summary>
        /// 根据工单Ids获取工单耗用单
        /// </summary>
        /// <param name="woOrderIds"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public List<WoOrderCostInfo> GetWoOrderCostInfos(List<double> woOrderIds)
        {
            return new List<WoOrderCostInfo>();
        }

        /// <summary>
        /// 根据工单Ids获取单体关键件
        /// </summary>
        /// <param name="woOrderIds"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public List<WipProductKeyItem> GetSingleWipProductKeyItems(List<double> woOrderIds)
        {
            return new List<WipProductKeyItem>();
        }

        /// <summary>
        /// 根据工单Ids获取批次关键件
        /// </summary>
        /// <param name="woOrderIds"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public List<WipProductKeyItem> GetBatchWipProductKeyItems(List<double> woOrderIds)
        {
            return new List<WipProductKeyItem>();
        }

        /// <summary>
        /// 根据工单号获取在制数量(单体+批次条码数)
        /// </summary>
        /// <param name="planTaskIds"></param>
        /// <returns></returns>
        public IReadOnlyList<WoOrderTaskInfo> GetProductBarcodeCount(IReadOnlyList<string> planTaskIds)
        {
            return new List<WoOrderTaskInfo>();
        }

        /// <summary>
        /// APS触发完工
        /// </summary>
        /// <param name="planTaskIds"></param>
        /// <returns></returns>
        public IReadOnlyList<WoOrderTaskInfo> APSFinishWorkOrder(IReadOnlyList<string> planTaskIds)
        { 
            return new List<WoOrderTaskInfo>();
        }

        /// <summary>
        /// APS触发取消完工
        /// </summary>
        /// <param name="planTaskIds"></param>
        /// <returns></returns>
        public IReadOnlyList<WoOrderTaskInfo> APSCancelFinishWoOrder(IReadOnlyList<string> planTaskIds)
        {
            return new List<WoOrderTaskInfo>();
        }

        /// <summary>
        /// 备料单弹框查询工单
        /// </summary>
        /// <param name="no">工单号</param>
        /// <param name="factoryId">工厂</param>
        /// <param name="workshopId">车间</param>
        /// <param name="resourceId">资源</param>
        /// <param name="pcode">产品编码</param>
        /// <param name="pname">产品名称</param>
        /// <param name="exceptClose">排除关闭状态</param>
        /// <param name="exceptCancel">排除取消状态</param>
        /// <param name="planBeginStart">计划开始时间</param>
        /// <param name="planBeginEnd">计划开始时间</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns></returns>
        public WorkOrderLesInfoWithCount GetWorkOrderList(string no, double? factoryId, double? workshopId, double? resourceId, string pcode, string pname, bool exceptClose = false, bool exceptCancel = false, DateTime? planBeginStart = null, DateTime? planBeginEnd = null, PagingInfo pagingInfo = null)
        {
            return new WorkOrderLesInfoWithCount();
        }

        /// <summary>
        /// LES获取工单信息
        /// </summary>
        /// <param name="woIds">工单Ids</param>
        /// <returns></returns>
        public Dictionary<double, WorkOrderInfo> GetWorkOrderList(List<double> woIds)
        {
            return new Dictionary<double, WorkOrderInfo>();
        }

        /// <summary>
        /// 获取工单简要信息
        /// </summary>
        /// <param name="no">工单号</param>
        /// <returns></returns>
        public List<WorkOrderSimpleInfo> GetWorkOrderSimpleInfos(string no)
        {
            return new List<WorkOrderSimpleInfo>();
        }

        /// <summary>
        /// 获取产线下工单bom(含节拍)
        /// </summary>
        /// <param name="resourceIds">工单产线Ids</param>
        /// <param name="itemIds">工单Bom物料Ids</param>
        /// <returns></returns>
        public List<WoBomPushPreInfo> GetWoBomPushPreInfos(List<double> resourceIds, List<double> itemIds)
        {
            return new List<WoBomPushPreInfo>();
        }
    }

}