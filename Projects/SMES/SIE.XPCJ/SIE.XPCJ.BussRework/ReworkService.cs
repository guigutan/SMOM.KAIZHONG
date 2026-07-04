using SIE.XPCJ.Common.ApiCall;
using SIE.XPCJ.Models;
using SIE.XPCJ.Models.Enums;
using SIE.XPCJ.Models.Exceptions;
using SIE.XPCJ.Models.WIP;
using SIE.XPCJ.Models.WIP.Entity;
using SIE.XPCJ.Models.WIP.Packing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SIE.XPCJ.BussRework
{
    /// <summary>
    /// 新包装采集服务
    /// </summary>
    public static class ReworkService
    {

        private static readonly string Controller = "WinFormReworkApiController";


        /// <summary>
        /// 根据Workcell获取当前工单的相关信息
        /// </summary>
        /// <returns></returns>
        public static XPApiResultRework GetCurrentInfo(Workcell workcell)
        {
            var result = ApiHelper.Post<XPApiResultRework>(Controller, "GetCurrentInfo", workcell.EmployeeId, workcell.ProcessId, workcell.StationId, workcell.ResourceId);
            if (result.Success)
            {
                return result.Result;
            }
            else
            {
                throw new ValidationException(result.Message);
            }
        }


        /// <summary>
        /// 获取产线线边仓库
        /// </summary>
        /// <param name="wipKeyItemProcessResourceId">工序过站记录资源ID</param>
        /// <returns></returns>
        public static List<XPLinesideWarehouse> GetWarehouse(double wipKeyItemProcessResourceId)
        {
            var result = ApiHelper.Post<List<XPLinesideWarehouse>>(Controller, "GetWarehouse", wipKeyItemProcessResourceId);
            if (result.Success)
            {
                return result.Result;
            }
            else
            {
                throw new ValidationException(result.Message);
            }
        }

        /// <summary>
        /// 条码置换
        /// </summary>
        /// <param name="barcode">条码号</param>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="employeeId">操作人ID</param>
        /// <param name="processId">工序ID</param>
        /// <param name="stationId">工位ID</param>
        /// <param name="resourceId">资源ID</param>
        /// <param name="step">采集步骤，控制条码录入</param>
        /// <param name="selectedBlankingWay">是否选择了置换后不良下料</param>
        /// <param name="selectedWarehouse">下料目标线边仓</param>
        /// <param name="listKeyItemIds">当前的关键件ID列表</param>
        /// <returns></returns>
        public static XPApiResultRework PermuteAssemblyCollect(string barcode, double workOrderId, Workcell workcell, XPReworkStep step, List<double> listKeyItemIds, List<double> listCheckedKeyItemIds)
        {
            var result = ApiHelper.Post<XPApiResultRework>(Controller, "PermuteAssemblyCollect", barcode, workOrderId,
                workcell.EmployeeId, workcell.ProcessId, workcell.StationId, workcell.ResourceId,
                step, listKeyItemIds, listCheckedKeyItemIds);
            if (result.Success)
            {
                return result.Result;
            }
            else
            {
                throw new ValidationException(result.Message);
            }
        }

        /// <summary>
        /// 条码置换解绑关健件采集
        /// </summary>
        /// <param name="barcode">条码号</param>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="employeeId">操作人ID</param>
        /// <param name="processId">工序ID</param>
        /// <param name="stationId">工位ID</param>
        /// <param name="resourceId">资源ID</param>
        /// <param name="step">采集步骤，控制条码录入</param>
        /// <param name="selectedBlankingWay">是否选择了置换后不良下料</param>
        /// <param name="selectedLinesideWarehouse">下料目标线边仓</param>
        /// <param name="listKeyItemIds">当前的关键件ID列表</param>
        /// <returns></returns>
        public static XPApiResultRework PermuteUnboundAssemblyCollect(string barcode, double workOrderId, Workcell workcell, XPReworkStep step,
            bool selectedBlankingWay, XPLinesideWarehouse selectedLinesideWarehouse, List<double> listKeyItemIds, List<double> listCheckedKeyItemIds, ReplaceItemHandleMethod? replaceItemHandleMethod)
        {
            var result = ApiHelper.Post<XPApiResultRework>(Controller, "PermuteUnboundAssemblyCollect", barcode, workOrderId,
                workcell.EmployeeId, workcell.ProcessId, workcell.StationId, workcell.ResourceId,
                step, selectedBlankingWay,
                selectedLinesideWarehouse == null ? 0 : selectedLinesideWarehouse.Id,
                listKeyItemIds, listCheckedKeyItemIds, replaceItemHandleMethod);

            if (result.Success)
            {
                return result.Result;
            }
            else
            {
                throw new ValidationException(result.Message);
            }
        }

        /// <summary>
        /// 条码置换解绑关健件采集
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="employeeId">操作人ID</param>
        /// <param name="processId">工序ID</param>
        /// <param name="stationId">工位ID</param>
        /// <param name="resourceId">资源ID</param>
        /// <param name="step">采集步骤，控制条码录入</param>
        /// <param name="selectedBlankingWay">是否选择了置换后不良下料</param>
        /// <param name="selectedLinesideWarehouse">下料目标线边仓</param>
        /// <param name="listKeyItemIds">当前的关键件ID列表</param>
        /// <returns></returns>
        public static XPApiResultRework SubmitPermuteUnboundAssemblyCollect(double workOrderId, Workcell workcell, XPReworkStep step,
            bool selectedBlankingWay, XPLinesideWarehouse selectedLinesideWarehouse, List<double> listKeyItemIds, List<double> listCheckedKeyItemIds, ReplaceItemHandleMethod? changeItemHandleMethod)
        {

            var result = ApiHelper.Post<XPApiResultRework>(Controller, "SubmitPermuteUnboundAssemblyCollect", workOrderId,
                workcell.EmployeeId, workcell.ProcessId, workcell.StationId, workcell.ResourceId,
                step, selectedBlankingWay,
                selectedLinesideWarehouse == null ? 0 : selectedLinesideWarehouse.Id,
                listKeyItemIds, listCheckedKeyItemIds, changeItemHandleMethod);

            if (result.Success)
            {
                return result.Result;
            }
            else
            {
                throw new ValidationException(result.Message);
            }
        }

        /// <summary>
        /// 关健件解绑-前置
        /// </summary>
        /// <param name="barcode">条码号</param>
        /// <returns></returns>
        public static XPWipProductProcessKeyItem PreKeyItemUnbound(string barcode)
        {
            var result = ApiHelper.Post<XPWipProductProcessKeyItem>(Controller, "PreKeyItemUnbound", barcode);
            return result.Result;
        }

        /// <summary>
        /// 关健件解绑
        /// </summary>
        /// <param name="barcode">条码号</param>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="employeeId">操作人ID</param>
        /// <param name="processId">工序ID</param>
        /// <param name="stationId">工位ID</param>
        /// <param name="resourceId">资源ID</param>
        /// <param name="step">采集步骤，控制条码录入</param>
        /// <param name="listKeyItemIds">当前的关键件ID列表</param>
        /// <returns></returns>
        public static XPApiResultRework KeyItemUnbound(string barcode, double workOrderId, Workcell workcell, XPReworkStep step,
            bool selectedBlankingWay, XPLinesideWarehouse selectedLinesideWarehouse, List<double> listKeyItemIds, List<double> listCheckedKeyItemIds, ReplaceItemHandleMethod? changeItemHandleMethod)
        {
            var result = ApiHelper.Post<XPApiResultRework>(Controller, "KeyItemUnbound", barcode, workOrderId,
                workcell.EmployeeId, workcell.ProcessId, workcell.StationId, workcell.ResourceId,
                step, selectedBlankingWay,
                selectedLinesideWarehouse == null ? 0 : selectedLinesideWarehouse.Id,
                listKeyItemIds, listCheckedKeyItemIds, changeItemHandleMethod);

            if (result.Success)
            {
                return result.Result;
            }
            else
            {
                throw new ValidationException(result.Message);
            }
        }
    }
}