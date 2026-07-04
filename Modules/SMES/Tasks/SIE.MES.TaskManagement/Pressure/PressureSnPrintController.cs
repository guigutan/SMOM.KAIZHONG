using Irony;
using SIE.Barcodes.WipBatchs;
using SIE.Common.NumberRules;
using SIE.Core.Algorithms.KZ;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.MES.Dispatchs;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Reports;
using SIE.MES.TaskManagement.Reports.Datas;
using SIE.MES.TaskManagement.WorkOrders;
using SIE.MES.WIP.Pressure;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SIE.MES.TaskManagement.Pressure
{
    /// <summary>
    /// 控制器-打印条码(KZ)
    /// </summary>
    public class PressureSnPrintController : SIE.MES.WIP.Pressure.WipPressureController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="printQty"></param>
        /// <param name="task"></param>
        /// <param name="isAllowOver"></param>
        /// <param name="numberRuleId"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual WipPressure GenerateWipPressureSns(int printQty, DispatchTask task, bool isAllowOver, double numberRuleId, WipResource resource)
        {
            var list = new EntityList<WipPressureSn>();
            task = RF.GetById<DispatchTask>(task.Id);
            if (task == null)
                throw new ValidationException("任务单数据异常");
            var maxPrintCount = isAllowOver ? GetMaxPrintCount(task.DispatchQty) : task.DispatchQty;
            if (task.PrintedQty >= maxPrintCount)
                throw new ValidationException("该任务单没有可打印数量");

            //生成工序标签批次
            var dispatchConfig = RT.Service.Resolve<DispatchController>().GetDispatchConfig();
            var tuple = RT.Service.Resolve<ReportController>().CreateWipBatchs(task, dispatchConfig, YesNo.No, printQty, printQty); //不需要按分单数拆分标签

            var dbTime = RF.Find<WipPressureSn>().GetDbTime();

            using (var trans = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                var wipBatch = tuple.Item1.FirstOrDefault();
                //标记为CS打印功能生成，后续其他功能(如A包装采集)可用到
                wipBatch.IsPressureSnPrint = true;
                RF.Save(wipBatch);
                //生成测试批次
                var wipPressure = new WipPressure()
                {
                    WorkOrder = task.WorkOrder,
                    Resource = resource,
                    BatchNo = wipBatch.BatchNo,
                    Product = task?.Product,
                    Qty = printQty,
                    OriginalQty = printQty,
                    BeginTime = dbTime,
                    IsAllowOver = isAllowOver,
                    EndTime = dbTime,
                };
                RF.Save(wipPressure);

                //客户料码数据
                var itemCustomer = RT.Service.Resolve<ItemCusotmerDataController>().GetItemCusotmerData(wipPressure.ProductId.Value, batchNo: wipPressure.BatchNo, lineCode: resource?.Code);
                if (itemCustomer == null)
                    throw new ValidationException("产品[{0}]还未维护客户料码数据".L10nFormat(wipPressure.Product?.Code));

                var snNos = RT.Service.Resolve<NumberRuleController>().GenerateSegment(numberRuleId, printQty, itemCustomer).ToList();
                NumberRule byId = GetById<NumberRule>(numberRuleId);
                var length = byId.DetailList.Sum(p => p.Length);


                //生成SN
                for (int i = 0; i < printQty; i++)
                {
                    if (snNos[i].Length != length)
                        throw new ValidationException("编码规则[{0}],基础数据不准确，编码位数设置为{1}，实际位数为{2}".L10nFormat(byId.Code, length, snNos[i].Length));

                    var sn = new WipPressureSn()
                    {
                        Sn = snNos[i],
                        WipPressure = wipPressure,
                        TestResult = TestResult.PASS,
                        TestTime = dbTime,
                        IsOver = false
                    };
                    list.Add(sn);
                }
                RF.BatchInsert(list);

                DB.Update<DispatchTask>().Set(p => p.PrintedQty, 0).Where(p => p.Id == task.Id && p.PrintedQty == null).Execute();

                DB.Update<DispatchTask>().Set(p => p.PrintedQty, p => p.PrintedQty + wipBatch.Qty).Where(p => p.Id == task.Id).Execute();

                ////自动报工
                //PdaScanSubmitInfo submitInfo = new PdaScanSubmitInfo()
                //{
                //    ScanType = 1,
                //    ResourceId = resource.Id,
                //    ProcessId = task?.ProcessId ?? 0,
                //    DispatchTaskId = task.Id,
                //    WorkOrderId = task?.WorkOrderId ?? 0,
                //    DetailInfos = new List<ScanDetailInfo>() {
                //        new ScanDetailInfo() {
                //            Sn = wipBatch.BatchNo,
                //            Qty = wipBatch.Qty,
                //            GoodQty = wipBatch.Qty,
                //            SuspectQty = 0
                //        }
                //    },
                //    ReportEmployeeId = RT.IdentityId,
                //    IsTaskFinish = true
                //};
                //var printInfos = RT.Service.Resolve<ReportController>().SubmitScanInfo(submitInfo);

                trans.Complete();
                return wipPressure;
            }

            return null;
        }
    }
}
