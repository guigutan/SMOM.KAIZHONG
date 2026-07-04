using SIE.XPCJ.Common.ApiCall;
using SIE.XPCJ.Common.Exceptions;
using SIE.XPCJ.Models;
using SIE.XPCJ.Models.WIP;
using SIE.XPCJ.Models.WIP.Entity;
using SIE.XPCJ.WIP.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.XPCJ.Common.Services
{
    public static class WipService
    {
        private static readonly string workorderController = "WinFormWorkOrderController";

        private static readonly string moveController = "WinFormMoveApiController";

        private static readonly string loadItemController = "WinFormLoadItemApiController";

        /// <summary>
        /// 一键下料
        /// </summary>
        /// <param name="loadItemIds"></param>
        public static void UnloadAllItem(List<double> loadItemIds)
        {
            object[] parameters = new object[1];
            parameters[0] = loadItemIds;

            var result = ApiHelper.Post<string>(loadItemController, "UnloadAllItem", parameters);
            if (!result.Success)
            {
                throw new ValidationException(result.Message);
            }
        }

        /// <summary>
        /// 获取下料明细
        /// </summary>
        /// <param name="processId"></param>
        /// <param name="resourceId"></param>
        /// <param name="stationId"></param>
        /// <returns></returns>
        public static List<UnloadItem> GetUnloadItemList(double processId, double resourceId, double stationId)
        {
            object[] parameters = new object[3];
            parameters[0] = processId;
            parameters[1] = resourceId;
            parameters[2] = stationId;

            var result = ApiHelper.Post<List<UnloadItem>>(loadItemController, "GetUnloadItemList", parameters);
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
        /// 上料
        /// </summary>
        /// <param name="barcodeInfo"></param>
        /// <param name="workcell"></param>
        public static void NewLoadItem(LoadItemBarcodeInfo barcodeInfo, Workcell workcell, bool validateCurrentProcessBom=true)
        {
            object[] parameters = new object[3];
            parameters[0] = barcodeInfo;
            parameters[1] = workcell;
            parameters[2] = validateCurrentProcessBom;
            var result = ApiHelper.Post<string>(loadItemController, "NewLoadItem", parameters);
            if (!result.Success)
            {
                throw new ValidationException(result.Message);
            }
        }

        /// <summary>
        /// 获取上料记录
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="stationId"></param>
        /// <returns></returns>
        public static List<Models.WIP.Entity.LoadItem> GetLoadItemList(double resourceId, double stationId)
        {
            object[] parameters = new object[2];
            parameters[0] = resourceId;
            parameters[1] = stationId;
            var result = ApiHelper.Post<List<Models.WIP.Entity.LoadItem>>(loadItemController, "GetLoadItemList", parameters);
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
        /// 获取物料信息
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public static Item GetItemInfo(double itemId)
        {
            object[] parameters = new object[1];
            parameters[0] = itemId;
            var result = ApiHelper.Post<Item>(loadItemController, "GetItemInfo", parameters);
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
        /// 获取工单信息列表
        /// </summary>
        /// <param name="queryInfo"></param>
        /// <returns></returns>
        public static List<WorkOrder> GetWorkOrdertInfos(WorkOrderQueryInfo queryInfo)
        {
            object[] parameters = new object[1];
            parameters[0] = queryInfo;
            var result = ApiHelper.Post<List<WorkOrder>>(workorderController, "GetWorkOrdertInfos", parameters);
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
        /// 获取当前工作单元在制工单
        /// </summary>
        /// <param name="workcell"></param>
        /// <returns></returns>
        public static WorkOrder GetWipResourceWorkOrder(Workcell workcell)
        {
            object[] parameters = new object[1];
            parameters[0] = workcell;
            var result = ApiHelper.Post<WorkOrder>(workorderController, "GetWipResourceWorkOrder", parameters);
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
        /// 获取当前工序的缺陷
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        public static List<Defect> GetProcessDefects(double processId)
        {
            object[] parameters = new object[1];
            parameters[0] = processId;
            var result = ApiHelper.Post<List<Defect>>(moveController, "GetProcessDefects", parameters);
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
        /// 获取缺陷所有分类
        /// </summary>
        /// <returns></returns>
        public static List<DefectCategory> GetDefectCategory()
        {
            var result = ApiHelper.Post<List<DefectCategory>>(moveController, "GetDefectCategory");
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
        /// 切换在制资源工单
        /// </summary>
        /// <param name="woId"></param>
        /// <param name="workcell"></param>
        /// <returns></returns>
        public static WorkOrder ChangeWipResourceWorkOrder(double woId, Workcell workcell)
        {
            object[] parameters = new object[2];
            parameters[0] = woId;
            parameters[1] = workcell;
            var result = ApiHelper.Post<WorkOrder>(workorderController, "ChangeWipResourceWorkOrder", parameters);
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
        /// 获取并删除拼板码已绑定待打印的条码列表
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <param name="panelCode">拼版码</param>
        /// <returns></returns>
        public static List<Barcode> GetAndDeleteToBePrintedSnList(double workOrderId, string panelCode)
        {
            object[] parameters = new object[2];
            parameters[0] = workOrderId;
            parameters[1] = panelCode;
            var result = ApiHelper.Post<List<Barcode>>(moveController, "GetAndDeleteToBePrintedSnList");
            if (!result.Success)
            {
                throw new ValidationException(result.Message);
            }
            return result.Result;
        }


        /// <summary>
        /// 获取工单条码打印信息
        /// </summary>
        /// <param name="woId"></param>
        /// <returns></returns>
        public static BarcodePrintInfo GetWorkOrderBarcodePrintInfo(double woId)
        {
            object[] parameters = new object[1];
            parameters[0] = woId;
            var result = ApiHelper.Post<BarcodePrintInfo>(moveController, "GetWorkOrderBarcodePrintInfo", woId);
            if (!result.Success)
            {
                throw new ValidationException(result.Message);
            }
            return result.Result;
        }

        /// <summary>
        /// 获取工序信息
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public static Process GetProcessInfo(double processId)
        {
            object[] parameters = new object[1];
            parameters[0] = processId;
            var result = ApiHelper.Post<Process>(moveController, "GetProcessInfo", parameters);
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
        /// 是否生成任务单
        /// </summary>
        /// <returns></returns>
        public static bool IsGenerateTask()
        {
            var result = ApiHelper.Post<bool>(moveController, "IsGenerateTask");
            if (!result.Success)
            {
                throw new ValidationException(result.Message);
            }
            return result.Result;
        }

        /// <summary>
        /// 采集
        /// </summary>
        /// <param name="barcodes"></param>
        /// <param name="collectData"></param>
        /// <param name="workcell"></param>
        public static void Collect(string[] barcodes, CollectData collectData, Workcell workcell)
        {
            object[] parameters = new object[3];
            parameters[0] = barcodes;
            parameters[1] = collectData;
            parameters[2] = workcell;
            var result = ApiHelper.Post<string>(moveController, "CollectApi", parameters);
            if (!result.Success)
            {
                throw new ValidationException(result.Message);
            }
        }

        /// <summary>
        /// 校验新条码
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="woId"></param>
        /// <returns></returns>

        public static decimal ValidateNewBarcode(string barcode, double woId)
        {
            object[] parameters = new object[2];
            parameters[0] = barcode;
            parameters[1] = woId;
            var result = ApiHelper.Post<decimal>(moveController, "ValidateScanBarcode", parameters);
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
        /// 校验条码
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="workcell"></param>
        public static void ValidateBarcode(CollectBarcode barcode, Workcell workcell)
        {
            object[] parameters = new object[2];
            parameters[0] = barcode;
            parameters[1] = workcell;
            var result = ApiHelper.Post<string>(moveController, "ValidateScanApiBarcode", parameters);
            if (!result.Success)
            {
                throw new ValidationException(result.Message);
            }
        }

        /// <summary>
        /// 校验条码
        /// </summary>
        /// <param name="collectBarcode"></param>
        /// <param name="workcell"></param>
        /// <param name="woId"></param>
        /// <returns></returns>
        public static ValidateResult ValidateBarcode(CollectBarcode collectBarcode, Workcell workcell, double woId)
        {
            object[] parameters = new object[3];
            parameters[0] = collectBarcode;
            parameters[1] = workcell;
            parameters[2] = woId;
            var result = ApiHelper.Post<ValidateResult>(moveController, "ValidateApiBarcode", parameters);
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
        /// 获取工序任务列表
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="retrospectType"></param>
        /// <param name="processId"></param>
        /// <returns></returns>
        public static List<ReportTask> GetReportTasks(double employeeId, RetrospectType retrospectType, double? processId)
        {
            object[] parameters = new object[3];
            parameters[0] = employeeId;
            parameters[1] = retrospectType;
            parameters[2] = processId;
            var result = ApiHelper.Post<List<ReportTask>>(moveController, "GetReportTasks", parameters);
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
        /// 获取当班采集数
        /// </summary>
        /// <param name="queryInfo"></param>
        /// <returns></returns>
        public static Tuple<decimal, decimal> GetQtyPassAndFailed(StatisticsQueryInfo queryInfo)
        {
            object[] parameters = new object[1];
            parameters[0] = queryInfo;
            var result = ApiHelper.Post<Tuple<decimal, decimal>>("WipStatisticsController", "GetQtyPassAndFailed", parameters);
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
        /// 获取所有缺陷
        /// </summary>
        /// <returns></returns>
        public static List<Defect> GetDefects()
        {
            object[] parameters = new object[0];
            var result = ApiHelper.Post<List<Defect>>(moveController, "GetAllDefects", parameters);
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
        /// 上料
        /// </summary>
        /// <param name="loadItemId"></param>
        /// <param name="qty"></param>
        public static void UnloadItem(double loadItemId, decimal qty)
        {
            object[] parameters = new object[2];
            parameters[0] = loadItemId;
            parameters[1] = qty;

            var result = ApiHelper.Post<string>(loadItemController, "UnloadItem", parameters);
            if (!result.Success)
            {
                throw new ValidationException(result.Message);
            }
        }

        /// <summary>
        /// 不良下料
        /// </summary>
        /// <param name="loadItemId"></param>
        /// <param name="defects"></param>

        public static void DefectUnloadItem(double loadItemId, List<DefectData> defects)
        {
            object[] parameters = new object[2];
            parameters[0] = loadItemId;
            parameters[1] = defects;

            var result = ApiHelper.Post<string>(loadItemController, "DefectUnloadItem", parameters);
            if (!result.Success)
            {
                throw new ValidationException(result.Message);
            }
        }

        public static List<LoadItemBarcodeInfo> ValidateLoadItem(string barcode, Workcell workcell,
      Dictionary<LoadItemSourceType, bool> dicLoadItemSourceType, double toLoadItemWorkOrderId)
        {
            object[] parameters = new object[4];
            parameters[0] = barcode;
            parameters[1] = workcell;
            parameters[2] = dicLoadItemSourceType;
            parameters[3] = toLoadItemWorkOrderId;
            var result = ApiHelper.Post<List<LoadItemBarcodeInfo>>(loadItemController, "ValidateLoadItem", parameters);
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
