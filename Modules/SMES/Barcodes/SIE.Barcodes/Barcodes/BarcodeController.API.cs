using SIE.Api;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.MES.Barcodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Barcodes.Barcodes
{
    /// <summary>
    /// 外部条码信息
    /// </summary>
    [Serializable]
    public class OutwayBarcodeInfo
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Qty { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WoNo { get; set; }
    }

    /// <summary>
    /// 条码API
    /// </summary>
    public partial class BarcodeController : DomainController
    {
        /// <summary>
        /// 外部生成条码
        /// </summary>
        /// <param name="outwayBarcodeInfoList"></param>
        [ApiService("外部生成条码")]
        [return: ApiReturn("void:保存生成的条码到表")]
        public virtual void GenerateBarcode([ApiParameter] List<OutwayBarcodeInfo> outwayBarcodeInfoList)
        {
            if (outwayBarcodeInfoList == null || outwayBarcodeInfoList.Count == 0)
            {
                throw new ValidationException("打印信息不能为空！".L10N());
            }
            if (outwayBarcodeInfoList.Exists(p => p.Sn.IsNullOrEmpty()))
            {
                throw new ValidationException("条码信息不能为空！".L10N());
            }
            if (outwayBarcodeInfoList.Exists(p => p.WoNo.IsNullOrEmpty()))
            {
                throw new ValidationException("工单信息不能为空！".L10N());
            }
            if (outwayBarcodeInfoList.Exists(p => p.Qty <= 0))
            {
                throw new ValidationException("打印数量必须大于0！".L10N());
            }
            if (outwayBarcodeInfoList.GroupBy(p => p.Sn).Any(p => p.Count() > 1)) 
            {
                throw new ValidationException("外部不能生产相同条码！".L10N());
            }
            // 条码集合
            var snList = outwayBarcodeInfoList.Select(x => x.Sn).ToList();
            // 全库存组织条码
            var invorgBarList = snList.SplitContains(tempSns =>
            {
                return Query<UniqueBarcode>().Where(p => tempSns.Contains(p.Sn)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            // 包含重复条码
            if (invorgBarList.Any())
            {
                var barcode = invorgBarList[0];
                throw new ValidationException("库存组织{0}存在条码{1}".L10nFormat(barcode.InvOrgName, barcode.Sn));
            }
            // 工单号集合
            var woNoList = outwayBarcodeInfoList.Select(x => x.WoNo).ToList();
            // 工单
            var printWoList = woNoList.SplitContains(tempNos =>
            {
                return Query<PrintWorkOrder>().Where(p => tempNos.Contains(p.No)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            if (printWoList.Any(p => p.State == Core.WorkOrders.WorkOrderState.CancelRelease))
            {
                throw new ValidationException("取消发放的工单不能打印条码！".L10N());
            }
            // 改动工单(save保存)
            var savePrintWoList = new EntityList<PrintWorkOrder>();
            // 条码(save保存)
            var saveBarcodeList = new EntityList<Barcode>();
            using (var tran = DB.TransactionScope(BarcodeEntityDataProvider.ConnectionStringName))
            {
                foreach(var barcodeInfo in outwayBarcodeInfoList)
                {
                    var printWoOrder = printWoList.FirstOrDefault(p => p.No == barcodeInfo.WoNo);
                    if (printWoOrder == null)
                    {
                        throw new ValidationException("工单{0}信息有误！".L10nFormat(barcodeInfo.WoNo));
                    }
                    printWoOrder.PrintedQty += barcodeInfo.Qty;
                    // 改动过的工单保存
                    savePrintWoList.Add(printWoOrder);
                    var barcode = new Barcode
                    {
                        Sn = barcodeInfo.Sn,
                        Qty = barcodeInfo.Qty,
                        WorkOrder = printWoOrder,
                        PrintedState = BarcodeState.Notprint,
                        PrintTimes = 0,
                        PrintDate = null,
                        Range = null,
                        PrintById = RT.IdentityId,
                    };
                    saveBarcodeList.Add(barcode);
                }
                RF.Save(savePrintWoList);
                RF.BatchInsert(saveBarcodeList);
                // 推送打印条码消息到边端
                SendPrintEvents(saveBarcodeList, printWoList);
                tran.Complete();
            }
        }

        /// <summary>
        /// 推送打印条码消息到边端
        /// </summary>
        /// <param name="saveBarcodeList"></param>
        /// <param name="printWoList"></param>
        private void SendPrintEvents(EntityList<Barcode> saveBarcodeList, EntityList<PrintWorkOrder> printWoList)
        {
            var groupByWoList = saveBarcodeList.GroupBy(p => p.WorkOrderId).ToDictionary(p => p.Key, p => p.ToList());
            groupByWoList.ForEach(w =>
            {
                var printBarcodeInfo = new PrintBarcodeInfo();
                printBarcodeInfo.MsgType = "3";
                printBarcodeInfo.WorkOrderNo = printWoList.FirstOrDefault(x => x.Id == w.Key)?.No;
                printBarcodeInfo.BarcodeList.AddRange(w.Value);
                RT.EventBus.Publish<PrintBarcodeInfo>(printBarcodeInfo);
            });
        }
    }
}
