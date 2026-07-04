using SIE.XPCJ.Common.ApiCall;
using SIE.XPCJ.Models;
using SIE.XPCJ.Models.Enums;
using SIE.XPCJ.Models.Exceptions;
using SIE.XPCJ.Models.WIP;
using SIE.XPCJ.Models.WIP.Entity;
using SIE.XPCJ.WIP.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.XPCJ.BussLoadItems
{
    /// <summary>
    /// 上料服务
    /// </summary>
    public static class LoadItemSevice
    {
        private static readonly string workorderController = "WinFormWorkOrderController";

        private static readonly string loadItemController = "WinFormLoadItemApiController";


        /// <summary>
        /// 获取工单工序BOM列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<WorkOrderProcessBom> GetWorkerProcessBomList(double workorderId)
        {
            object[] parameters = new object[1];
            parameters[0] = workorderId;
            var result = ApiHelper.Post<List<WorkOrderProcessBom>>(workorderController, "GetWorkerProcessBomList", parameters);
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
        /// 验证工序BOM扣料
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="workcell"></param>
        public static void ValidateProcessBomApi(CollectBarcode barcode, Workcell workcell)
        {
            object[] parameters = new object[2];
            parameters[0] = barcode;
            parameters[1] = workcell;
            var result = ApiHelper.Post<string>(loadItemController, "ValidateProcessBomApi", parameters);
            if (!result.Success)
            {
                throw new LackItemException(result.Message);
            }
        }

        /// <summary>
        /// 根据条码和条码类型获取产品运行时
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="barcodeType"></param>
        /// <returns></returns>
        public static ProductInfo FindProduct(string barcode, BarcodeType barcodeType)
        {
            object[] parameters = new object[2];
            parameters[0] = barcode;
            parameters[1] = barcodeType;
            var result = ApiHelper.Post<ProductInfo>(loadItemController, "FindProduct", parameters);
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
        /// 获取下一工序
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="barcodeType"></param>
        /// <param name="processId"></param>
        /// <returns></returns>
        public static Models.WIP.Runtime.process GetProductRoutingGetNext(string barcode, BarcodeType barcodeType, double processId)
        {
            object[] parameters = new object[3];
            parameters[0] = barcode;
            parameters[1] = barcodeType;
            parameters[2] = processId;
            var result = ApiHelper.Post<Models.WIP.Runtime.process>(loadItemController, "GetProductRoutingGetNext", parameters);
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
        /// 获取指定工单的工单BOM
        /// </summary>
        /// <param name="workeOrderId"></param>
        /// <returns></returns>

        public static List<WorkOrderBom> GetWorkOrderBom(double workeOrderId)
        {
            object[] parameters = new object[1];
            parameters[0] = workeOrderId;
            var result = ApiHelper.Post<List<WorkOrderBom>>(workorderController, "GetWorkOrderBom", parameters);
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
