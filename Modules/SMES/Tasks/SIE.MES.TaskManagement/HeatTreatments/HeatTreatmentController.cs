using DocumentFormat.OpenXml.EMMA;
using Microsoft.Scripting.Utils;
using Newtonsoft.Json;
using SIE.Andon.Andons;
using SIE.Andon.Andons.IOT;
using SIE.Barcodes.WipBatchs;
using SIE.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.DeviceControls.ApiModels;
using SIE.EventMessages.MES.Dispatchs;
using SIE.Items;
using SIE.KZ.Base.Interfaces;
using SIE.KZ.Base.Interfaces.Enums;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.HeatTreatments.Datas;
using SIE.MES.TaskManagement.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.HeatTreatments
{
    /// <summary>
    /// 控制器
    /// </summary>
    public partial class HeatTreatmentController : DomainController
    {
        /// <summary>
        /// 根据标签号查询数据
        /// </summary>
        /// <param name="batchNos"></param>
        /// <param name="eagerLoad"></param>
        /// <returns></returns>
        public virtual EntityList<HeatTreatment> GetHeatTreatments(List<string> batchNos, EagerLoadOptions eagerLoad = null)
        {
            return Query<HeatTreatment>().Where(p => batchNos.Contains(p.Barcode)).ToList(null, eagerLoad);
        }
        /// <summary>
        /// 热处理报工
        /// </summary>
        /// <param name="heatTreatments"></param>
        /// <returns></returns>
        public virtual (int, int, string) HeatTreatmentReport(EntityList<HeatTreatment> heatTreatments)
        {
            var msg = new List<string>();
            var totalCount = 0;
            var successCount = 0;

            var barcodes = heatTreatments.Select(p => p.Barcode).ToList();
            var wipBatchs = barcodes.SplitContains(temp =>
            {
                return RT.Service.Resolve<WipBatchController>().GetWipBatches(temp.ToList());
            });
            if (wipBatchs.Count == 0)
                throw new ValidationException("没有要报工的数据");

            totalCount = wipBatchs.Count;

            var woIds = wipBatchs.Select(p => p.WorkOrderId).Distinct().ToList();
            var tasks = RT.Service.Resolve<DispatchController>().GetDispatchTasksByWorkOrderIds(woIds);

            foreach (var wipBatch in wipBatchs)
            {
                var ht = heatTreatments.FirstOrDefault(p => p.Barcode == wipBatch.BatchNo);
                if (ht == null && ht.Count00 <= 0)
                    continue;
                string process = "热处理";
                bool isReported = false;
                decimal reportQty = 0;
                string error = string.Empty;
                try
                {
                    var task = tasks.FirstOrDefault(p => p.WorkOrderId == wipBatch.WorkOrderId && (p.ProcessCode.Contains(process) || p.ProcessName.Contains(process)));
                    if (task == null)
                        throw new ValidationException("工序标签[{0}]未匹配到对应的[{1}]任务单".L10nFormat(ht.Barcode, process));
                    //校验当前工序是否已报工
                    isReported = RT.Service.Resolve<ReportController>().ValidateProcessHasReport(wipBatch.BatchNo, process, false);
                    if (isReported)
                    {
                        throw new ValidationException("标签[{0}]已存在工序[{1}]任务的报工记录,请确认".L10nFormat(wipBatch.BatchNo, process));
                    }

                    //报工
                    var reportInfo = new ReportInfo()
                    {
                        Sn = ht.Barcode,
                        WorkOrderId = wipBatch.WorkOrderId,
                        GoodQty = wipBatch.Qty,
                        ResourceId = task.ResourceId ?? 0,
                    };
                    RT.Service.Resolve<ITaskReportKZ>().HeatTreatmentReport(new List<ReportInfo>() { reportInfo });
                    isReported = true;
                    reportQty = wipBatch.Qty;
                    successCount++;
                }
                catch (Exception ex)
                {
                    error = ex.GetExceptionMessage();
                    //msg.Add(error);
                }
                //更新报工状态
                if (ht != null)
                {
                    DB.Update<HeatTreatment>()
                    .Set(p => p.IsReported, isReported)
                    .Set(p => p.ReportQty, reportQty)
                    .Set(p => p.Remark, error)
                    .Where(p => p.Id == ht.Id).Execute();
                }

            }

            return (totalCount, successCount, msg.Concat(";"));
        }
        /// <summary>
        /// 热处理报工
        /// </summary>
        public virtual void HeatTreatmentReport(List<double> heatTreatmentIds = null)
        {
            var q = Query<HeatTreatment>().Where(p => p.OperationType == OperationType.Out && (p.IsReported == null || p.IsReported == false));
            if (heatTreatmentIds != null)
                q.Where(p => heatTreatmentIds.Contains(p.Id));
            var list = q.ToList();
            if (list.Count == 0)
                return;
            //HeatTreatmentReport(list);
            var (totalCount, successCount, result) = HeatTreatmentReport(list);
            if (result.IsNotEmpty())
                throw new ValidationException(result);
        }

        /// <summary>
        /// 热处理报工对象锁
        /// </summary>
        private readonly object _lockReport = new object();
        /// <summary>
        /// 热处理报工
        /// </summary>
        /// <param name="days"></param>
        /// <returns></returns>
        public virtual (int, int, string) HeatTreatmentReport(int days = 1)
        {
            lock (_lockReport)
            {
                var date = DateTime.Now.AddDays(-days);

                var q = Query<HeatTreatment>().Where(p => p.OperationType == OperationType.Out && (p.IsReported == null || p.IsReported == false) && p.UpdateDate > date);
                var list = q.ToList();
                if (list.Count == 0)
                    return (0, 0, "没有要报工的数据");

                return HeatTreatmentReport(list);
            }
        }

        /// <summary>
        /// 上传的工序标签到SCADA (存在‘热处理’工序的前置工序报工记录的标签数据)
        /// </summary>
        /// <param name="process">热处理工序</param>
        /// <param name="days">多少天前</param>
        /// <returns></returns>
        public virtual (int, int, string) UploadBarcodeToScada(string process, int days = 1)
        {
            var msg = string.Empty;
            var totalCount = 0;
            var successCount = 0;
            var date = DateTime.Now.AddDays(-days);
            //报工记录
            var reportWipBatchs = Query<ReportWipBatch>()
                .Exists<WipBatch>((x, y) => y.Where(p => p.Id == x.WipBatchId && (p.IsUploadIot == null || p.IsUploadIot == false)))
                .Where(p => p.CreateDate > date).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var woIds = reportWipBatchs.Select(p => (double?)p.WorkOrderId).Distinct().ToList();
            var tasks = woIds.SplitContains(ids =>
            {
                return Query<DispatchTask>().Where(p => ids.Contains(p.WorkOrderId)).ToList(null, new EagerLoadOptions().LoadWith(DispatchTask.ProcessProperty));
            });              
            var taskIds = new List<double>();//热处理"的前置工序
            tasks.GroupBy(p => p.WorkOrderId).ForEach(g =>
            {
                var task = g.FirstOrDefault(p => p.Process.Code.Contains(process)); //查询"热处理"的工序任务
                if (task != null)
                {
                    var preSeq = g.OrderBy(p => p.Seq).LastOrDefault(p => p.Seq < task.Seq)?.Seq;  //查询"热处理"的前置工序任务
                    if (preSeq > 0)
                    {
                        var preTaskIds = g.Where(p => p.Seq == preSeq).Select(p => p.Id);
                        taskIds.AddRange(preTaskIds);
                    }
                }
            });
            //过滤出"热处理"的前置工序对应报工标签
            var wipBatchIds = reportWipBatchs.Where(p => taskIds.Contains(p.DispatchTaskId)).Select(p => (double)p.WipBatchId).Distinct().ToList();
            var list = RT.Service.Resolve<WipBatchController>().GetWipBatchsByBatchIds<WipBatch>(wipBatchIds, new EagerLoadOptions().LoadWithViewProperty());
            list = list.Where(p => !p.IsScraped && p.IsSuspectProduct != YesNo.Yes).AsEntityList(); //报废,可疑品不上传
            totalCount = list.Count;
            if (totalCount == 0)
                return (totalCount, successCount, "没有要推送的数据");

            var invOrgs = RF.GetAll<Rbac.InvOrgs.InvOrg>();
            var invOrg = invOrgs.FirstOrDefault(p => p.Code == RT.InvOrg);
            //构建上传数据结构
            var data = new
            {
                RpcPara = new
                {
                    Method = "SendBarcodes",
                    Paras = new
                    {
                        Datas = list.Select(p => new
                        {
                            //DevId = "",
                            Factory = invOrg?.ExternalId,
                            barcode = p.BatchNo,
                            model = p.ShortDescription,
                            cardnum = "",
                            PLANNUM = "",
                            PRODUCTNUM = p.WorkOrderNo,
                            AMOUNT = p.Qty.ToString()
                        }),
                    }
                }
            };

            //调用SMDC接口
            var url = RT.Config.Get<string>("SCADA.URL");
            url += "/api/Rpcinvoke";
            var requeststr = JsonConvert.SerializeObject(data);

            var log = RT.Service.Resolve<InfDataLogController>().SaveErpDataInfLog(InfType.BarcodeToSmdc, requeststr, DateTime.Now, CallDirection.MESToScada, CallResult.UnSave, totalCount);

            var responsestr = RT.Service.Resolve<AndonManageController>().GetHttpPost(url, requeststr, "", false);

            log.ResponseContent = responsestr;
            log.EndDate = DateTime.Now;
            log.CallResult = CallResult.Fail;
            msg = responsestr;
            //RT.Logger.Info($"老化标签下发IOT-UploadBarcodeToIot: [{requeststr}] [{responsestr}]");

            try
            {
                //responsestr = "{\"Data\":true,\"RequestId\":\"a2a76c19-bae2-46d5-a6e0-bd13fe182c57\",\"Requester\":null,\"Error\":null,\"ModuleName\":\"凯中精密.exe\",\"MethodFullName\":\"SCADA_DAQ.Customer.Service.CustomerService\",\"Method\":\"SendBarcodes\",\"MsgType\":0,\"Message\":null,\"IsSucceed\":true,\"Timestamp\":\"2025-09-29 09:32:16\",\"Duration\":16.0}";
                var result = JsonConvert.DeserializeObject<IotApiResponse>(responsestr);
                if (result.IsSucceed)
                {
                    //更新标签上传标识
                    wipBatchIds.SplitDataExecute(temp =>
                    {
                        DB.Update<WipBatch>().Set(p => p.IsUploadIot, true).Where(p => temp.Contains(p.Id)).Execute();
                    });
                    successCount = list.Count;
                    log.TipMsg = $"推送成功[{successCount}]";
                    log.CallResult = CallResult.Success;
                }
                else
                {
                    log.ErrorMsg = result.Error;
                }
            }
            catch (Exception ex)
            {
                msg = "结果解析失败:[{0}][{1}]".L10nFormat(responsestr, ex.Message);
                log.ErrorMsg = msg;
            }
            finally
            {
                RF.Save(log);
            }
            return (totalCount, successCount, msg);
        }

        /// <summary>
        /// 根据产口获取老化炉标签进出炉记录
        /// </summary>
        /// <param name="productCodes"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public virtual List<KzHeatTreatmentInfo> GetHeatTreatmentList(List<string> productCodes, DateTime startTime, DateTime endTime)
        {
            List<KzHeatTreatmentInfo> datas = new List<KzHeatTreatmentInfo>();
            productCodes.SplitDataExecute(temp =>
            {
                var list = Query<HeatTreatment>().Where(p => temp.Contains(p.WipBatch.WorkOrder.Product.Code)
            && p.CreateDate > startTime && p.CreateDate < endTime).Select(p => new
            {
                Barcode = p.Barcode,
                Count00 = p.Count00,
                CreateDate = p.CreateDate,
                MaterialCode = p.WipBatch.WorkOrder.Product.Code,
                DevName = p.DevName,
                Model = p.Model,
                OperationType = p.OperationType
            }).ToList<KzHeatTreatmentInfo>().ToList();
                datas.AddRange(list);
            });
            return datas;
        }

        /// <summary>
        /// 根据条码号获取老化炉标签进出炉记录
        /// </summary>
        /// <param name="lables"></param>
        /// <returns></returns>
        public virtual EntityList<HeatTreatment> GetHeatTreatmentList(List<string> lables)
        {
            return lables.SplitContains(barcodes =>
            {
                return Query<HeatTreatment>().Where(p => barcodes.Contains(p.Barcode)).ToList();
            });
        }
    }
}
