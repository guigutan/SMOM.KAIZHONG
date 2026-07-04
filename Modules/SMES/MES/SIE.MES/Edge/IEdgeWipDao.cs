using SIE.Barcodes;
using SIE.Defects;
using SIE.Defects.Measures;
using SIE.Domain;
using SIE.Items;
using SIE.MES.Edge.Models;
using SIE.MES.InspectionStandards;
using SIE.MES.PackingPrints;
using SIE.MES.WorkOrders;
using SIE.Resources.Employees;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.Edge
{
    /// <summary>
    /// 边缘在制数据访问
    /// </summary>
    public interface IEdgeWipDao
    {
        /// <summary>
        /// 按工单编码获取工单
        /// </summary>
        /// <param name="WorkOrderNo">工单编码</param>
        /// <returns></returns>
        WorkOrder GetWorkOrder(string WorkOrderNo);

        /// <summary>
        /// 按生产条码获取工单
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <returns></returns>
        WorkOrder GetWorkOrderByBarcode(string barcode);

        /// <summary>
        /// 取资源的工位数据
        /// </summary>
        /// <param name="resourceIds">资源ID</param>
        /// <returns></returns>
        List<Tech.Stations.Station> GetStationsByResourceIds(List<double> resourceIds);

        /// <summary>
        /// 根据登陆用户获取工序列表
        /// </summary>
        /// <param name="employeeId">员工ID</param>
        /// <returns>工序列表</returns>
        List<Tech.Processs.Process> GetProcesssByEmployeeId(double employeeId);

        /// <summary>
        /// 根据工单ID获取已打印的条码
        /// </summary>
        /// <param name="workorderId">工单ID</param> 
        /// <returns>条码信息</returns>
        List<Barcode> GetBarcodes(double workorderId);

        /// <summary>
        /// 根据工单ID获取已打印的包装号
        /// </summary>
        /// <param name="workorderId">工单ID</param> 
        /// <returns>条码信息</returns>
        List<PackingBarcode> GetPackingBarcodes(double workorderId);

        /// <summary>
        /// 获取员工关联的生产资源列表
        /// </summary>
        /// <param name="employeeId">员工ID</param>
        /// <returns>生产资源列表</returns>
        EntityList<WipResource> GetWipResources(double employeeId);

        /// <summary>
        /// 取员工信息
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        Employee GetEmployeeById(double employeeId);

        /// <summary>
        /// 取生产资源班次信息
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="currentTime"></param>
        /// <returns></returns>
        Resources.ShiftTypes.Shift GetShift(double resourceId, DateTime currentTime);

        /// <summary>
        /// 获取物料信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        List<Item> GetItems(List<double> ids);

        /// <summary>
        /// 取机型检验项目
        /// </summary>
        /// <returns></returns>
        List<ModelInspectionItem> GetInspectionItems();

        /// <summary>
        /// 取缺陷代码
        /// </summary>
        /// <returns></returns>
        List<Defect> GetAllDefects();

        /// <summary>
        /// 取缺陷分类
        /// </summary>
        /// <returns></returns>
        List<DefectCategory> GetAllDefectCategory();

        /// <summary>
        /// 获取所有的缺陷责任
        /// </summary>
        /// <returns></returns>
        List<DefectResponsibility> GetAllDefectResponsibility();

        /// <summary>
        /// 获取所有的缺陷责任分类
        /// </summary>
        /// <returns></returns>
        List<DefectResponsibilityCategory> GetAllDefectResponsibilityCategory();

        /// <summary>
        /// 获取所有维修措施
        /// </summary>
        /// <returns></returns>
        List<RepairMeasure> GetAllRepairMeasure();

        /// <summary>
        /// 获取工序缺陷信息
        /// </summary>
        /// <param name="processIds">工序ID</param>
        /// <returns>缺陷代码列表</returns>
        EntityList<ProcessDefect> GetProcessDefects(IList<double> processIds);

        /// <summary>
        /// 获取工序缺陷分类
        /// </summary>
        /// <param name="defects">工序缺陷</param>
        /// <returns>缺陷分类列表</returns>
        List<EdgeDefectCategory> GetProcessDefectCategorys(IList<EdgeDefect> defects);

       /// <summary>
       /// 更新条码
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
        /// 通过工序Id获取采集步骤列表
        /// </summary>
        /// <param name="processIds">工序Id</param>
        /// <returns>采集步骤列表</returns>
        List<ProcessCollectStep> GetProcessCollectSteps(List<double> processIds);


        /// <summary>
        /// 获取当前时间计划排产的有效在制工单信息
        /// </summary>
        /// <returns></returns>
        List<WorkOrder> GetPlannedWipWorkOrders(List<string> resourceNos);


        /// <summary>
        /// 获取当前有效在制工单信息
        /// </summary>
        /// <param name="workOrderNo">工单编码</param>
        /// <returns></returns>
        WorkOrder GetWipWorkOrderByNo(string workOrderNo);


        /// <summary>
        /// 获取包装号
        /// </summary>
        /// <param name="ruleId">包装规则Id</param> 
        /// <returns></returns>
        string GetPackCode(double ruleId);
    }
}
