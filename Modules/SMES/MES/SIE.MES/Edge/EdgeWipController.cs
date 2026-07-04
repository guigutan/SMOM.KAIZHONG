using SIE.Common.Queues;
using SIE.Domain.Validation;
using SIE.EventMessages.MES.Barcodes;
using SIE.EventMessages.MES.WorkOrders.Models;
using SIE.MES.Edge.Models;
using SIE.Security;
using SIE.Utils;
using System;
using System.Collections.Generic;
using System.Threading;

namespace SIE.MES.Edge
{
    /// <summary>
    /// 边缘在制数据处理
    /// </summary>
    public class EdgeWipController : DomainController
    {
        /// <summary>
        /// 边缘在制Service
        /// </summary>
        private readonly IEdgeWipService edgeWipService;
        /// <summary>
        /// 边缘在制Service
        /// </summary>
        private readonly ILoadItemService loadItemService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="edgeWipService"></param>
        /// <param name="loadItemService"></param>
        public  EdgeWipController(IEdgeWipService edgeWipService, ILoadItemService loadItemService)
        {
            this.edgeWipService = edgeWipService;
            this.loadItemService = loadItemService;
        }

        /// <summary>
        /// 获取在制工单信息
        /// </summary>
        /// <param name="workOrderNo"></param>
        /// <returns></returns>
        [Api.ApiService]
        public virtual WipWorkOrder GetWipWorkOrder(string workOrderNo)
        {
            return edgeWipService.GetWipWorkOrder(workOrderNo);

        }

        /// <summary>
        /// 获取包装规则单号
        /// </summary>
        /// <param name="ruleId">规则Id</param>
        /// <returns></returns>
        [Api.ApiService]
        public virtual string GetPackCode(double ruleId)
        {
            return edgeWipService.GetPackCode(ruleId);
        }

        /// <summary>
        /// 获取在制工单信息
        /// </summary>
        /// <param name="barcode">生产条码</param>
        /// <returns></returns>
        [Api.ApiService]
        public virtual WipWorkOrder GetWipWorkOrderByBarcode(string barcode)
        {
            return edgeWipService.GetWipWorkOrderByBarcode(barcode);
        }
        /// <summary>
        /// 获取在制条码物料信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Api.ApiService]
        public virtual List<MaterialInfo> GetBarcodeInfo(EdgeCollectData data)
        {
            return loadItemService.GetBarcodeInfo(data);
        }

        /// <summary>
        /// 上料
        /// </summary>
        [Api.ApiService]
        public virtual List<MaterialInfo> LoadItem(EdgeCollectData data)
        {
            return loadItemService.LoadItem(data);
        }
        /// <summary>
        /// 获取在制工单信息
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [Api.ApiService]
        public virtual WipEmployeeInfo GetWipUserInfo(double employeeId)
        {
            return edgeWipService.GetWipUserInfo(employeeId);
        }

        /// <summary>
        /// 取机型检验项目
        /// </summary>
        /// <returns></returns>
        [Api.ApiService]
        public virtual EdgeInspectionItemInfo GetInspectionItemInfo()
        {
            return edgeWipService.GetInspectionItemInfo();
        }

        /// <summary>
        /// 取缺陷代码、缺陷分类等信息
        /// </summary>
        /// <returns></returns>
        [Api.ApiService]
        public virtual EdgeDefectInfo GetDefectInfo()
        {
            return edgeWipService.GetDefectInfo();
        }

        /// <summary>
        /// 根据工序Id获取缺陷代码、缺陷分类等信息
        /// </summary>
        /// <returns></returns>
        [Api.ApiService]
        public virtual EdgeDefectInfo GetDefectInfoByProcessId(string processId)
        {
            try
            {
                return new EdgeDefectInfo();
            }
            catch (Exception ex)
            {
                throw new ValidationException(ex.Message);
            }
        }

        /// <summary>
        /// 获取当前时间往后的多个班次信息
        /// </summary>
        /// <param name="resourceId">资源Id</param>
        /// <param name="count">班次数量</param>
        /// <returns></returns>
        [Api.ApiService]
        public virtual EdgeShiftInfo GetShifts(string resourceId, int count)
        {
            if (count < 1)
            {
                count = 1;
            }
            try
            {
                double resId = double.Parse(resourceId);
                return edgeWipService.GetShifts(resId, count);
            }
            catch (Exception ex)
            {
                throw new ValidationException(ex.Message);
            }
        }

        /// <summary>
        /// 更新物料（SN/单体条码/配送单）条码剩余数量
        /// </summary>
        /// <param name="edgeMaterials"></param>
        /// <returns></returns>
        [Api.ApiService]
        public virtual bool SetBarcodes(List<EdgeMaterial> edgeMaterials)
        {
            try
            {
                return edgeWipService.SetBarcodes(edgeMaterials);

            }
            catch (Exception ex)
            {
                throw new ValidationException(ex.Message);
            }
        }

        /// <summary>
        /// 下料
        /// </summary>
        /// <param name="edgeMaterials">下料来源信息</param>
        /// <returns></returns>
        [Api.ApiService]
        public virtual bool UnLoadItems(List<EdgeMaterial> edgeMaterials)
        {
            try
            {
                return edgeWipService.UpdateUnLoadItemBarcodes(edgeMaterials);
            }
            catch (Exception ex)
            {
                throw new ValidationException(ex.Message);
            }
        }

        /// <summary>
        /// 获取当前时间计划排产的有效在制工单信息
        /// </summary>
        /// <returns></returns>
        [Api.ApiService]
        public virtual List<WipWorkOrder> GetPlannedWipWorkOrders(List<string> resourceNos)
        {
            try
            {
                return edgeWipService.GetPlannedWipWorkOrders(resourceNos);
            }
            catch (Exception ex)
            {
                throw new ValidationException(ex.Message);
            }

        }

        /// <summary>
        /// 获取工单基础信息
        /// </summary>
        /// <param name="workOrderNo">工单编码</param>
        /// <returns></returns>
        [Api.ApiService]
        public virtual WipWorkOrder GetWipWorkOrderByNo(string workOrderNo)
        {
            try
            {
                return edgeWipService.GetWipWorkOrderByNo(workOrderNo);
            }
            catch (Exception ex)
            {
                throw new ValidationException(ex.Message);
            }

        }

        /// <summary>
        /// 创建边端在制工单消息并发布
        /// </summary>
        public virtual void CreateAndPublishWipWoInfo(WorkOrderInfo woInfo)
        {
            Thread.Sleep(30);
            var wipWorkOrder = GetWipWorkOrder(woInfo.WorkOrderNo);
            if (wipWorkOrder == null) 
            {
                return;
            }
            wipWorkOrder.BarcodeList.Clear();
            wipWorkOrder.PackBarcodeList.Clear();
            wipWorkOrder.ProcessStationList.Clear();
            PublishEdgeMessage(woInfo.MsgType, wipWorkOrder);
        }

        /// <summary>
        /// 创建边端在制工单条码消息并发布
        /// </summary>
        public virtual void CreateAndPublishWipBarcodeInfo(PrintBarcodeInfo printBarcodeInfo)
        {
            var wipWorkOrder = GetWipWorkOrderByNo(printBarcodeInfo.WorkOrderNo);
            if (wipWorkOrder == null)
            {
                return;
            }
            foreach (var barcode in printBarcodeInfo.BarcodeList)
            {
                var edgeBarcode = new EdgeBarcode();
                edgeBarcode.Barcode = barcode.Sn;
                edgeBarcode.IsScraped = barcode.IsScraped;
                edgeBarcode.IsPending = barcode.IsPending;
                edgeBarcode.BoxesQty = barcode.BoxesQty;
                edgeBarcode.Qty = barcode.Qty;
                wipWorkOrder.BarcodeList.Add(edgeBarcode);
            }
            PublishEdgeMessage(printBarcodeInfo.MsgType, wipWorkOrder);
        }

        /// <summary>
        /// 创建边端在制工单包装号消息并发布
        /// </summary>
        public virtual void CreateAndPublishWipPackCodeInfo(PackingBarcodeInfo packingBarcodeInfo)
        {
            var wipWorkOrder = GetWipWorkOrderByNo(packingBarcodeInfo.WorkOrderNo);
            if (wipWorkOrder == null)
            {
                return;
            }
            foreach (var packingCode in packingBarcodeInfo.PackingCodeList)
            {
                var edgePackingBarcode = new EdgePackingBarcode();
                edgePackingBarcode.Code = packingCode.Code;
                edgePackingBarcode.PackUnitName = packingCode.PackUnitName;
                edgePackingBarcode.IsUse = packingCode.IsUse;
                wipWorkOrder.PackBarcodeList.Add(edgePackingBarcode);
            }
            PublishEdgeMessage(packingBarcodeInfo.MsgType, wipWorkOrder);
        }


        /// <summary>
        /// 发布SMOM数据到边端
        /// </summary>
        /// <param name="type">数据类型</param>
        /// <param name="messageObject">消息体</param>
        public virtual void PublishEdgeMessage(string type, object messageObject)
        {
            EdgeSendMessage sendMessage = new EdgeSendMessage();
            sendMessage.Id = Guid.NewGuid().ToString();
            sendMessage.Name = "edge_data_upload";
            sendMessage.Type = type;
            sendMessage.Body = messageObject;
            sendMessage.InvOrg = RT.InvOrg.ToString();
            new EventQueue("Edge_").Publish(sendMessage);
        }
    }

}
