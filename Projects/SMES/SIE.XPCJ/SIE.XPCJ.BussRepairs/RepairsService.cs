using SIE.XPCJ.Common.ApiCall;
using SIE.XPCJ.Common.Exceptions;
using SIE.XPCJ.Models;
using SIE.XPCJ.Models.WIP;
using SIE.XPCJ.Models.WIP.Repairs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.XPCJ.BussRepairs
{
    /// <summary>
    /// 维修服务
    /// </summary>
    public static class RepairsService
    {
        private static readonly string repairApiController = "WinFormRepairApiController";

        /// <summary>
        /// 获取或创建维修主记录
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <param name="submitBarcodeCode"></param>
        public static void GetRepairRecord(double workOrderId, string submitBarcodeCode)
        {
            object[] parameters = new object[2];
            parameters[0] = workOrderId;
            parameters[1] = submitBarcodeCode;
            var result = ApiHelper.Post<string>(repairApiController, "GetRepairRecord", parameters);
            if (!result.Success)
            {
                throw new ValidationException(result.Message);
            }
        }

        /// <summary>
        /// 获取加载关键项
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="isItem"></param>
        public static List<WipProductProcessKeyItem> GetLoadkeyItems(string barcode, bool isItem)
        {
            object[] parameters = new object[2];
            parameters[0] = barcode;
            parameters[1] = isItem;
            var result = ApiHelper.Post<List<WipProductProcessKeyItem>>(repairApiController, "GetLoadkeyItems", parameters);
            if (!result.Success)
            {
                throw new ValidationException(result.Message);
            }
            else
            {
                return result.Result;
            }
        }

        /// <summary>
        /// 验证工艺路线并加载产品未维修的缺陷
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="workcell"></param>
        /// <returns></returns>
        public static List<RepairDefectViewModel> GetLoadDefects(string barcode, Workcell workcell)
        {
            object[] parameters = new object[2];
            parameters[0] = barcode;
            parameters[1] = workcell;
            var result = ApiHelper.Post<List<RepairDefectViewModel>>(repairApiController, "GetLoadDefects", parameters);
            if (!result.Success)
            {
                throw new ValidationException(result.Message);
            }
            else
            {
                return result.Result;
            }
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
            var result = ApiHelper.Post<string>(repairApiController, "CollectApi", parameters);
            if (!result.Success)
            {
                throw new ValidationException(result.Message);
            }
        }

        /// <summary>
        /// 维修采集
        /// </summary>
        /// <param name="barcodes"></param>
        /// <param name="collectData"></param>
        /// <param name="workcell"></param>
        public static void RepairCollect(string barcode, CollectData collectData, Workcell workcell, double? uplineProcess = null)
        {
            object[] parameters = new object[4];
            parameters[0] = barcode;
            parameters[1] = collectData;
            parameters[2] = workcell;
            parameters[3] = uplineProcess;
            var result = ApiHelper.Post<string>(repairApiController, "RepairCollect", parameters);
            if (!result.Success)
            {
                throw new ValidationException(result.Message);
            }
        }

        /// <summary>
        /// 获取所有维修措施
        /// </summary>
        public static List<RepairMeasure> GetAllRepairMeasure()
        {
            object[] parameters = new object[0];
            var result = ApiHelper.Post<List<RepairMeasure>>(repairApiController, "GetAllRepairMeasure", parameters);
            if (!result.Success)
            {
                throw new ValidationException(result.Message);
            }
            return result.Result;
        }

        /// <summary>
        /// 获取所有缺陷责任
        /// </summary>
        public static List<DefectResponsibility> GetAllDefectResponsibility()
        {
            object[] parameters = new object[0];
            var result = ApiHelper.Post<List<DefectResponsibility>>(repairApiController, "GetAllDefectResponsibility", parameters);
            if (!result.Success)
            {
                throw new ValidationException(result.Message);
            }
            return result.Result;
        }

        /// <summary>
        /// 获取所有缺陷责任分类
        /// </summary>
        /// <returns></returns>
        public static List<DefectResponsibilityCategory> GetAllDefectResponsibilityCategory()
        {
            object[] parameters = new object[0];
            var result = ApiHelper.Post<List<DefectResponsibilityCategory>>(repairApiController, "GetAllDefectResponsibilityCategory", parameters);
            if (!result.Success)
            {
                throw new ValidationException(result.Message);
            }
            return result.Result;
        }
        /// <summary>
        /// 获取可选工序工艺路线
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="workcell"></param>
        /// <returns></returns>
        public static List<GotoProcessViewModel> GetRepariRoutingProcessList(string barcode, Workcell workcell)
        {
            object[] parameters = new object[2];
            parameters[0] = barcode;
            parameters[1] = workcell;
            var result = ApiHelper.Post<List<GotoProcessViewModel>>(repairApiController, "GetRepariRoutingProcessList", parameters);
            if (!result.Success)
            {
                throw new ValidationException(result.Message);
            }
            return result.Result;
        }

        /// <summary>
        /// 获取可选工艺路线
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="workcell"></param>
        /// <returns></returns>
        public static List<GotoProcessViewModel> GetRepariOptionalPaths(string barcode, Workcell workcell)
        {
            object[] parameters = new object[2];
            parameters[0] = barcode;
            parameters[1] = workcell;
            var result = ApiHelper.Post<List<GotoProcessViewModel>>(repairApiController, "GetRepariOptionalPaths", parameters);
            if (!result.Success)
            {
                throw new ValidationException(result.Message);
            }
            return result.Result;
        }

        /// <summary>
        /// 存在可选路径时候提交
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="workcell"></param>
        public static void RepairOptionalSubmit(string barcode, Workcell workcell, double routingProcessId)
        {
            object[] parameters = new object[3];
            parameters[0] = barcode;
            parameters[1] = workcell;
            parameters[2] = routingProcessId;
            var result = ApiHelper.Post<string>(repairApiController, "RepairOptionalSubmit", parameters);
            if (!result.Success)
            {
                throw new ValidationException(result.Message);
            }
        }

        /// <summary>
        /// 保存维修记录
        /// </summary>
        /// <param name="defects"></param>
        public static void SaveRepairRecord(List<WipProductDefect> defects)
        {
            object[] parameters = new object[1];
            parameters[0] = defects;
            var result = ApiHelper.Post<List<GotoProcessViewModel>>(repairApiController, "SaveRepairRecord", parameters);
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
            var result = ApiHelper.Post<ValidateResult>(repairApiController, "ValidateApiBarcode", parameters);
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
