using SIE.XPCJ.Common.ApiCall;
using SIE.XPCJ.Models;
using SIE.XPCJ.Models.Exceptions;
using SIE.XPCJ.Models.WIP;
using SIE.XPCJ.Models.WIP.Entity;
using SIE.XPCJ.Models.WIP.Packing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SIE.XPCJ.BussNewPackage
{

    /// <summary>
    /// 新包装采集服务
    /// </summary>
    public static class NewPackingService
    {

        private static readonly string Controller = "WinFormNewPackageApiApiController";

        /// <summary>
        /// 根据Workcell获取当前工单的相关信息
        /// </summary>
        /// <returns></returns>
        public static XPApiResultNewPackage GetCurrentInfo(Workcell workcell)
        {
            var result = ApiHelper.Post<XPApiResultNewPackage>(Controller, "GetCurrentInfo", workcell.EmployeeId, workcell.ProcessId, workcell.StationId, workcell.ResourceId);
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
        /// 验证是否需要提前输入
        /// </summary>
        /// <returns></returns>
        public static bool AdvanceInputPackageNo(string packageNo, double ruleUnitId, string ruleUnitName, double workOrderId)
        {
            var result = ApiHelper.Post<bool>(Controller, "AdvanceInputPackageNo", packageNo, ruleUnitId, ruleUnitName, workOrderId);
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
        /// 验证输入条码，预算包装层级
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="barcodeType">条码类型</param>
        /// <param name="workcell">工作单元</param>
        /// <returns></returns>
        public static XPApiResultNewPackage AdvanceInputBarcode(WorkOrder workOrder, string barcode, Models.Enums.BarcodeType barcodeType, Workcell workcell)
        {
            double workOrderId = workOrder == null ? 0 : workOrder.Id;
            var result = ApiHelper.Post<XPApiResultNewPackage>(Controller, "AdvanceInputBarcode", barcode, (int)barcodeType, workcell.EmployeeId, workcell.ProcessId, workcell.StationId, workcell.ResourceId, workOrderId);
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
        /// 包装采集
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="workOrderId"></param>
        /// <param name="workcell"></param>
        /// <param name="printMode"></param>
        /// <param name="packageUnitList"></param>
        /// <returns></returns>
        public static XPApiResultNewPackage PackingCollect(string barcode, WorkOrder workOrder, Workcell workcell, int printMode, Queue<string> packageUnitList)
        {
            double workOrderId = workOrder == null ? 0 : workOrder.Id;
            var result = ApiHelper.Post<XPApiResultNewPackage>(Controller, "PackingCollect", barcode, workOrderId, workcell.EmployeeId, workcell.ProcessId, workcell.StationId, workcell.ResourceId, printMode, packageUnitList);
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
        /// 手动打包
        /// </summary>
        /// <param name="workcell"></param>
        /// <param name="printMode"></param>
        /// <param name="targetSnRecords"></param>
        /// <returns></returns>
        public static XPApiResultNewPackage PackageMuanual(Workcell workcell, List<XPPackageSnRecord> targetSnRecords)
        {
            List<string> listSN = new List<string>();
            foreach (XPPackageSnRecord r in targetSnRecords)
                listSN.Add(r.Sn);

            var result = ApiHelper.Post<XPApiResultNewPackage>(Controller, "PackageMuanual", workcell.EmployeeId, workcell.ProcessId, workcell.StationId, workcell.ResourceId, listSN);
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
        /// 重新加载工位工序未完成的包装关系
        /// </summary>
        /// <param name="workcell"></param>
        /// <returns></returns>
        public static XPApiResultNewPackage ReloadPackingRelation(Workcell workcell)
        {
            var result = ApiHelper.Post<XPApiResultNewPackage>(Controller, "ReloadPackingRelation", workcell.ProcessId, workcell.StationId, workcell.ResourceId);
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
        /// 更新包装关系物流状态
        /// </summary>
        /// <param name="workcell"></param>
        /// <returns></returns>
        public static bool UpdateRelationStatePrinted(double relationId)
        {
            var result = ApiHelper.Post<XPApiResultNewPackage>(Controller, "UpdateRelationStatePrinted", relationId);
            if (result.Success)
            {
                return true;
            }
            else
            {
                throw new ValidationException(result.Message);
            }
        }

    }
}