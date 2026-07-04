using SIE.MES.Edge.Models;
using System;
using System.Collections.Generic;

namespace SIE.MES.Edge
{
    /// <summary>
    /// 边缘采集接口类
    /// </summary>
    public interface IEdgeWipService
    {
        /// <summary>
        /// 获取在制工单信息
        /// </summary>
        /// <param name="WorkOrderNo"></param>
        /// <returns></returns>
        WipWorkOrder GetWipWorkOrder(string WorkOrderNo);

        /// <summary>
        /// 获取在制工单信息
        /// </summary>
        /// <param name="barcode">生产条码</param>
        /// <returns></returns>
        WipWorkOrder GetWipWorkOrderByBarcode(string barcode);

        /// <summary>
        /// 获取在制用户基础信息，包括工序、工位、资源权限等
        /// </summary>
        /// <param name="employeeId"></param>
        WipEmployeeInfo GetWipUserInfo(double employeeId);

        /// <summary>
        /// 取机型检验项目
        /// </summary>
        /// <returns></returns>
        EdgeInspectionItemInfo GetInspectionItemInfo();

        /// <summary>
        /// 取缺陷代码、缺陷分类等信息
        /// </summary>
        /// <returns></returns>
        EdgeDefectInfo GetDefectInfo();

        /// <summary>
        /// 取生产资源班次信息
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="count">班次数量</param>
        /// <returns></returns>
        EdgeShiftInfo GetShifts(double resourceId, int count);

        /// <summary>
        /// 更新条码信息
        /// </summary>
        /// <param name="edgeMaterials"></param>
        /// <returns></returns>
        bool SetBarcodes(List<EdgeMaterial> edgeMaterials);

        /// <summary>
        /// 下料更新条码信息
        /// </summary>
        /// <param name="edgeMaterials">下料来源信息</param>
        /// <returns></returns>
        bool UpdateUnLoadItemBarcodes(List<EdgeMaterial> edgeMaterials);

        /// <summary>
        /// 获取当前时间计划排产的有效在制工单信息
        /// </summary>
        /// <returns></returns>
        List<WipWorkOrder> GetPlannedWipWorkOrders(List<string> resourceNos);


        /// <summary>
        /// 获取当前时间计划排产的有效在制工单信息
        /// </summary>
        /// <returns></returns>
        WipWorkOrder GetWipWorkOrderByNo(string workOrderNo);


        /// <summary>
        /// 获取包装规则单号
        /// </summary>
        /// <param name="ruleId">规则Id</param>
        /// <returns></returns>
        [Api.ApiService]
        string GetPackCode(double ruleId);
    }
}
