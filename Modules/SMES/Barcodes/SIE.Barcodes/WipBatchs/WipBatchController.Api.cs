using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using Org.BouncyCastle.Asn1.Ocsp;
using SIE.Api;
using SIE.Barcodes.WipBatchs.Datas;
using SIE.Core.Common;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.MES.SuspectProductLabel;
using SIE.Security;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Barcodes.WipBatchs
{
    public partial class WipBatchController : DomainController
    {
        /// <summary>
        /// 委外同步可疑品标签
        /// </summary>
        /// <param name="wipBatches"></param>
        /// <param name="invOrg"></param>
        /// <returns></returns>
        [ApiService("")]
        [AllowAnonymous]
        public virtual SyncWipBatchResponse SyncWipBatch(List<WipBatch> wipBatches, string invOrg)
        {
            SyncWipBatchResponse response = new SyncWipBatchResponse();
            RT.Service.Resolve<LoginController>().Login(invOrg);
            var WorkOrderNos = wipBatches.Select(p => p.WorkOrderNo).Distinct().ToList();
            var workOrders = RT.Service.Resolve<WorkOrderController>().GetWorkOrders(WorkOrderNos);

            var batchNos = wipBatches.Select(p => p.BatchNo).Distinct().ToList();
            var wbs = RT.Service.Resolve<WipBatchController>().GetWipBatches(batchNos);

            foreach (var wipBatche in wipBatches)
            {
                //记录旧Id，用于报错返回，更新事务上传
                var oldId = wipBatche.Id;
                try
                {
                    using (var tran = DB.TransactionScope(BarcodeEntityDataProvider.ConnectionStringName))
                    {
                        var wip = wbs.FirstOrDefault(p => p.BatchNo == wipBatche.BatchNo);
                        if (wip != null)
                        {
                            if (wip.Qty != wipBatche.Qty)
                            {
                                wip.EditQtyProcessCode = wipBatche.EditQtyProcessCode;
                            }
                            wip.Qty = wipBatche.Qty;
                            wip.IsSuspectProduct = wipBatche.IsSuspectProduct;
                            wip.IsScraped = wipBatche.IsScraped;
                            wip.ScrapQty = wipBatche.ScrapQty;
                        }
                        else
                        {
                            wip = new WipBatch();
                            wip.Clone(wipBatche, new CloneOptions(CloneActions.NormalProperties));
                            wip.GenerateId();
                            wip.PersistenceStatus = Domain.PersistenceStatus.New;
                            var workOrder = workOrders.FirstOrDefault(p => p.No == wipBatche.WorkOrderNo);
                            if (workOrder == null)
                            {
                                throw new ValidationException("工单不存在".L10N());
                            }
                            wip.WorkOrderId = workOrder.Id;

                            BarcodeRange barcodeRange = new BarcodeRange();
                            barcodeRange.StartSn = wip.BatchNo;
                            barcodeRange.EndSn = wip.BatchNo;
                            barcodeRange.State = ReceiveState.NoReceive;
                            barcodeRange.PrintQty = 0;
                            barcodeRange.WorkOrderId = workOrder.Id;
                            barcodeRange.ReceiveById = null;
                            barcodeRange.RuleId = null;
                            barcodeRange.TemplateId = null;
                            barcodeRange.GenerateId();
                            barcodeRange.PersistenceStatus = PersistenceStatus.New;
                            RF.Save(barcodeRange);

                            wip.Range = barcodeRange;
                            wip.PrintById = null;
                            wip.ReportRecordIds = null;
                            wip.DispatchTaskId = null;

                        }
                        RF.Save(wip);

                        tran.Complete();
                    }
                }
                catch (Exception ex)
                {
                    response.failResponses.Add(new SyncWipBatchFailResponse()
                    {
                        Id = oldId,
                        Msg = ex.GetBaseException()?.Message
                    });
                }
            }
            return response;
        }

    }
}
