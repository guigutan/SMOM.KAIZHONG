using DotLiquid.Util;
using Microsoft.Scripting.Utils;
using Newtonsoft.Json;
using SIE.Api;
using SIE.Barcodes.WipBatchs;
using SIE.Barcodes.WipBatchs.Datas;
using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Core.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.ErpCommon;
using SIE.EventMessages.ErpCommon.Datas;
using SIE.Items;
using SIE.KZ.Base.Interfaces;
using SIE.KZ.Base.Interfaces.Enums;
using SIE.KZ.Base.SmomControl;
using SIE.MES.Outsourcing;
using SIE.MES.TaskManagement.Configs;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Models;
using SIE.MES.TaskManagement.Reports;
using SIE.MES.TaskManagement.Reports.Datas;
using SIE.MES.TaskManagement.Reports.Enums;
using SIE.MES.TaskManagement.SuspectProductLabels.ApiModels;
using SIE.MES.WIP.Pressure;
using SIE.Rbac.InvOrgs;
using SIE.Security;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace SIE.MES.TaskManagement.SuspectProductLabels
{
    /// <summary>
    /// 可疑品标签 API控制器
    /// </summary>
    public partial class SuspectProductLabelController
    {
        #region 获取待处理可疑标签
        /// <summary>
        /// 获取待处理可疑标签
        /// </summary>
        /// <param name="batchNo"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        [ApiService("获取待处理可疑标签")]
        [return: ApiReturn("可疑标签信息")]
        public virtual SuspectProductLabelInfo GetUnprocessedSuspectProduct([ApiParameter("标签号")] string batchNo)
        {
            if (batchNo.IsNullOrEmpty())
                throw new ValidationException("标签号不可空".L10N());
            var label = Query<SuspectProductLabel>().Where(p => p.BatchNo == batchNo && p.HandleState != SuspectHandleState.Processed).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            if (label == null)
                throw new ValidationException("可疑品标签[{0}]不存在或已处理完成".L10nFormat(batchNo));
            //获取父级旧料号
            var parentItem = RT.Service.Resolve<ItemController>().GetParentItemByItemId(label.ProductId);

            var result = new SuspectProductLabelInfo()
            {
                SuspectProductLabelId = label.Id,
                BatchNo = label.BatchNo,
                ItemName = label.ProductName,
                SuspectQty = label.Qty - label.GoodQty - label.ScrapQty - label.RepairQty,
                NeedMrbReport = label.NeedMrbReport && label.AttachmentList.Count == 0,
                ItemShortDescription = label.ProductShortDescription,
                Bismt = parentItem?.Bismt
            };
            return result;
        }
        #endregion

        #region 提交可疑品标签处理结果

        /// <summary>
        /// 提交可疑品标签处理结果验证
        /// </summary>
        /// <param name="data"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ValidationException"></exception>
        /// <exception cref="EntityNotFoundException"></exception>
        [ApiService("提交可疑品标签处理结果验证")]
        public virtual string SubmitSuspectValid(SuspectProductLabelData data)
        {
            var label = GetSuspectProductLabel(data.SuspectProductLabelId);
            var dispatchTask = RF.GetById<DispatchTask>(label.DispatchTaskId, new EagerLoadOptions().LoadWithViewProperty());

            //var processPty = RT.Service.Resolve<ProcessPtyController>().GetProcessPtysByProcessId(dispatchTask.ProcessId.Value, dispatchTask.ProductId);
            //var qty = dispatchTask.ReportQty + (dispatchTask.SuspectQty - data.GoodQty - data.RepairQty - data.ScrapQty);
            //decimal Uebto = 0;
            //decimal.TryParse(dispatchTask.WorkOrder.Uebto, out Uebto);

            //if (qty >= dispatchTask.DispatchQty && qty < dispatchTask.DispatchQty * (1 + Uebto / 100))
            ////if (qty >= dispatchTask.DispatchQty * (1 + Uebto))
            //{
            //    if (processPty.IsReportValid == true)
            //    {
            //        var tasks = RT.Service.Resolve<DispatchController>().GetDispatchTasks(dispatchTask.WorkOrderId.Value);
            //        var dic = tasks.GroupBy(p => p.Seq).ToDictionary(p => p.Key, p => p.ToList());
            //        decimal lastReportQty = 0;
            //        decimal lastSuspectQty = 0;
            //        if (dic != null && dic.Count > 0)
            //        {
            //            lastReportQty = dic.OrderBy(p => p.Key).FirstOrDefault().Value.Sum(p => p.ReportQty);
            //            lastSuspectQty = dic.OrderBy(p => p.Key).FirstOrDefault().Value.Sum(p => p.SuspectQty);
            //        }
            //        return "首工序已报工数量{0}，可疑品数量{1}，当前任务数量{2},已报工数量{3}，可疑品数量{4}，是否将任务单已完成".L10nFormat(lastReportQty, lastSuspectQty, dispatchTask?.DispatchQty ?? 0, dispatchTask?.ReportQty ?? 0, dispatchTask?.SuspectQty ?? 0);
            //        //return "已报工数{0}已达到任务数量{1}是否将任务更新为已完成".L10nFormat(qty, dispatchTask.DispatchQty);
            //    }
            //}
            return null;
        }

        /// <summary>
        /// 提交可疑品标签处理结果
        /// </summary>
        /// <param name="data"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ValidationException"></exception>
        /// <exception cref="EntityNotFoundException"></exception>
        [ApiService("提交可疑品标签处理结果")]
        public virtual List<PdaPrintInfo> SubmitSuspectProductHandleResult([ApiParameter("可疑品标签数据")] SuspectProductLabelData data)
        {
            var printDatas = new List<PdaPrintInfo>();

            var newWipBatchs = new EntityList<WipBatch>();
            var newDtls = new EntityList<SuspectProductLabelDetail>();

            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (data.GoodQty < 0 || data.ScrapQty < 0 || data.RepairQty < 0)
                throw new ValidationException("数量不可小于0".L10N());
            if (data.ScrapDetailList.IsNotEmpty() && data.ScrapDetailList.Any(p => p.Qty < 0))
                throw new ValidationException("数量不可小于0".L10N());
            if (data.RepairDetailList.IsNotEmpty() && data.RepairDetailList.Any(p => p.Qty < 0))
                throw new ValidationException("数量不可小于0".L10N());
            if (data.GoodQty <= 0 && data.ScrapQty <= 0 && data.RepairQty <= 0)
                throw new ValidationException("数量都为0，无可提交数据".L10N());
            if (data.ScrapQty > 0 && (data.ScrapDetailList.IsNullOrEmpty() || data.ScrapQty != data.ScrapDetailList.Sum(p => p.Qty)))
                throw new ValidationException("报废数量与报废明细数量综合不等，请检查".L10N());
            if (data.RepairQty > 0 && (data.RepairDetailList.IsNullOrEmpty() || data.RepairQty != data.RepairDetailList.Sum(p => p.Qty)))
                throw new ValidationException("返工数量与返工明细数量综合不等，请检查".L10N());

            var label = GetSuspectProductLabel(data.SuspectProductLabelId);
            if (null == label)
                throw new EntityNotFoundException(typeof(SuspectProductLabel), data.SuspectProductLabelId);

            var task = RF.GetById<DispatchTask>(label.DispatchTaskId, new EagerLoadOptions().LoadWithViewProperty());
            bool IsPackingSuspect = task.ProcessCode.Contains("包装") || task.ProcessName.Contains("包装"); //是否包装采集功能提交可疑品

            #region 包装可疑品处理
            if (IsPackingSuspect && label.LabelType == LabelType.WipSn)
            {
                printDatas = WipSnReportProcess(label,data, IsPackingSuspect,task);
                ////可疑品处理
                //HandleSuspectProductLabel(data, label, null, newWipBatchs, newDtls);
                ////更新任务单可疑品数
                //RT.Service.Resolve<ReportController>().UpdateSuspectQty(task.Id);
                return printDatas;
            }
            #endregion

            var wipBatch = RT.Service.Resolve<WipBatchController>().GetWipBatch(label.BatchNo);
            if (null == wipBatch)
                throw new ValidationException("批次号[{0}]不存在".L10nFormat(label.BatchNo));

            if (label.HandleState == SuspectHandleState.Processed)
                throw new ValidationException("可疑品标签[{0}]已处理完毕，无需提交".L10nFormat(label.BatchNo));
            if ((label.Qty - data.GoodQty - data.ScrapQty - data.RepairQty) != 0)
                throw new ValidationException("可疑品标签[{0}]提交的数量总和必须与数量相等, 请检查".L10nFormat(label.BatchNo));

            var suspectQty = label.Qty - label.GoodQty - label.ScrapQty - label.RepairQty;
            if (data.GoodQty + data.ScrapQty + data.RepairQty > suspectQty)
                throw new ValidationException("可疑品标签[{0}]剩余可疑品数量[{1}]，小于提交的数量总和".L10nFormat(label.BatchNo, suspectQty));
            if (data.RepairQty > 0 && !RT.Service.Resolve<DispatchController>().IsEndProcess(label.WorkOrderId, label.ProcessId))
                throw new ValidationException("可疑品标签[{0}]非最后工序不允许返工".L10nFormat(label.BatchNo));


            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                DB.Update<SuspectProductLabel>().Set(p => p.UpdateDate, DateTime.Now).Where(p => p.Id == data.SuspectProductLabelId).Execute();

                //可疑品处理
                HandleSuspectProductLabel(data, label, wipBatch, newWipBatchs, newDtls);

                #region 报工

                //要参与报工的标签
                var beReportWipBatchs = new EntityList<WipBatch>();

                //调用报工生成报工记录
                if (wipBatch.IsSuspectProduct == YesNo.No && label.DispatchTaskId > 0)
                {
                    beReportWipBatchs.AddRange(newWipBatchs);

                    //如果是包装采集功能提交的可疑品,暂不需要处理良品报工逻辑,后续后会进行包装提交报工
                    if (label.ReportRecordId > 0 || !IsPackingSuspect)   //已有可疑品报工记录或非包装采集工序,需要处理良品报工
                        beReportWipBatchs.Add(wipBatch);

                    ReportTaskInfo taskInfo = RT.Service.Resolve<ReportController>().GetReportTaskRecordInfo(label.DispatchTaskId.Value);

                    ReportRecord record1 = null;
                    if (label.ReportRecordId > 0)
                    {
                        record1 = RF.GetById<ReportRecord>(label.ReportRecordId);
                    }
                    foreach (var w in beReportWipBatchs)
                    {
                        var recordId = label.ReportRecordId ?? 0; //已有报工记录时,不再进行报工扣料 2025.11.26
                        if (recordId == 0)
                        {
                            //保留此逻辑,目的是兼容2025.11.26日前的旧数据处理
                            var info = taskInfo.Copy();
                            info.IsSuspect = true;
                            info.OkQty = 0;
                            info.NgQty = 0;
                            info.SuspectQty = 0;
                            info.ReworkQty = 0;
                            info.BatchNo = RT.Service.Resolve<ReportController>().GetReportBatchNo();
                            info.IsTaskFinish = data.IsTaskFinish;
                            info.IsValidatePrepare = false;
                            info.SourceType = Reports.Enums.SourceType.Report_SuspectProduct;
                            if (w.IsScraped)
                                info.NgQty = w.Qty;
                            else if (w.IsRework)
                                info.ReworkQty = w.Qty;
                            else
                                info.OkQty = w.Qty;

                            var defectIds = newDtls.Where(p => p.SubBatchNo == w.BatchNo).Select(p => p.DefectId).Distinct();
                            foreach (var defectId in defectIds)
                            {
                                if (defectId != null && info.DefectIds.All(p => p != defectId))
                                    info.DefectIds.Add(defectId.Value);
                            }
                            //调用报工接口
                            var record = RT.Service.Resolve<ReportController>().TaskReport(info, true);
                            recordId = record.Id;
                        }
                        else
                        {
                            var defectIds = newDtls.Where(p => p.SubBatchNo == w.BatchNo && p.DefectId > 0).Select(p => p.DefectId).Distinct();
                            foreach (var defectId in defectIds)
                            {
                                record1.Defects.Add(new ReportDefect() { DefectId = defectId.Value, ReportRecordId = recordId });
                            }
                            if (w.IsScraped)
                                record1.NgQty += w.Qty;
                            else if (w.IsRework)
                                record1.ReworkQty += w.Qty;
                            else
                                record1.OkQty += w.Qty;

                            record1.ReportQty += w.Qty;
                            record1.SuspectQty -= w.Qty;
                        }

                        //良品标签关联报工记录
                        var reportWipBatch = new ReportWipBatch()
                        {
                            ReportRecordId = recordId,
                            WipBatch = w,
                            BatchNo = w.BatchNo,
                            Qty = w.Qty,
                        };

                        RF.Save(reportWipBatch);

                        //关联报工记录ID
                        var ids = w.ReportRecordIds.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList();
                        ids.Add(recordId.ToString());
                        w.ReportRecordIds = ids.Distinct().Concat(",");

                        DB.Update<WipBatch>().Set(p => p.ReportRecordIds, w.ReportRecordIds).Where(p => p.Id == w.Id).Execute();
                    }

                    if (record1 != null)
                    {
                        //更新审核状态
                        record1.ExamineState = record1.SuspectQty <= 0 ? ReportRecordExamineState.Confirmed : ReportRecordExamineState.ToConfirm;
                        record1.SourceType = Reports.Enums.SourceType.Report_SuspectProduct;
                        RF.Save(record1);

                        RT.Service.Resolve<ReportController>().UpdateDispatchTaskQty(record1);
                    }
                    //更新任务单可疑品数
                    RT.Service.Resolve<ReportController>().UpdateSuspectQty(task.Id);
                    RT.Service.Resolve<ReportController>().UpdateDispatchTaskState(task.Id, DateTime.Now, true, Reports.Enums.SourceType.Report_SuspectProduct);
                }
                //末工序,良品与返工批次,生成物料标签
                if (task.EndProcess == true)
                {
                    RT.Service.Resolve<ReportController>().GenerateItemLabels(task.Id, beReportWipBatchs);
                }


                #endregion

                var dispatchConfig = RT.Service.Resolve<DispatchController>().GetDispatchConfig();
                printDatas = RT.Service.Resolve<ReportController>().CreatePrintDatas(true, PrintLabelType.Good, task, dispatchConfig.GoodLabel, dispatchConfig.SuspectLabel, beReportWipBatchs, label.WipResource);

                tran.Complete();
            }
            //委外可疑品标签需要同步
            if (task.IsOutsourcing == true)
            {
                var ids = newWipBatchs.Select(p => p.Id).Distinct().ToList();
                ids.Add(wipBatch.Id);
                var nwbs = Query<WipBatch>().Where(p => ids.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                //调用接口同步数据
                SyncWipBatchToOtherFactory(nwbs.ToList(), null, task);
            }

            return printDatas;
        }

        /// <summary>
        /// 耐压可疑品标签处理
        /// </summary>
        private List<PdaPrintInfo> WipSnReportProcess(SuspectProductLabel  label, SuspectProductLabelData data, bool IsPackingSuspect, DispatchTask task)
        {
            var printDatas = new List<PdaPrintInfo>();
            var newWipBatchs = new EntityList<WipBatch>();
            var newDtls = new EntityList<SuspectProductLabelDetail>();

            var wipBatch = RT.Service.Resolve<WipBatchController>().GetWipBatch(label.ProcessBatchNo);
            if (null == wipBatch)
                throw new ValidationException("批次号[{0}]不存在".L10nFormat(label.ProcessBatchNo));

            if (label.HandleState == SuspectHandleState.Processed)
                throw new ValidationException("可疑品标签[{0}]已处理完毕，无需提交".L10nFormat(label.BatchNo));
            if ((label.Qty - data.GoodQty - data.ScrapQty - data.RepairQty) != 0)
                throw new ValidationException("可疑品标签[{0}]提交的数量总和必须与数量相等, 请检查".L10nFormat(label.BatchNo));

            var suspectQty = label.Qty - label.GoodQty - label.ScrapQty - label.RepairQty;
            if (data.GoodQty + data.ScrapQty + data.RepairQty > suspectQty)
                throw new ValidationException("可疑品标签[{0}]剩余可疑品数量[{1}]，小于提交的数量总和".L10nFormat(label.BatchNo, suspectQty));
            if (data.RepairQty > 0 && !RT.Service.Resolve<DispatchController>().IsEndProcess(label.WorkOrderId, label.ProcessId))
                throw new ValidationException("可疑品标签[{0}]非最后工序不允许返工".L10nFormat(label.BatchNo));


            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                DB.Update<SuspectProductLabel>().Set(p => p.UpdateDate, DateTime.Now).Where(p => p.Id == data.SuspectProductLabelId).Execute();

                //可疑品处理
                HandleSuspectProductLabel(data, label, wipBatch, newWipBatchs, newDtls);

                #region 报工

                //要参与报工的标签
                var beReportWipBatchs = new EntityList<WipBatch>();

                //调用报工生成报工记录
                if (wipBatch.IsSuspectProduct == YesNo.No && label.DispatchTaskId > 0)
                {
                    beReportWipBatchs.AddRange(newWipBatchs);

                    //如果是包装采集功能提交的可疑品,暂不需要处理良品报工逻辑,后续后会进行包装提交报工
                    if (label.ReportRecordId > 0 || !IsPackingSuspect)   //已有可疑品报工记录或非包装采集工序,需要处理良品报工
                        beReportWipBatchs.Add(wipBatch);

                    ReportTaskInfo taskInfo = RT.Service.Resolve<ReportController>().GetReportTaskRecordInfo(label.DispatchTaskId.Value);

                    ReportRecord record1 = null;
                    if (label.ReportRecordId > 0)
                    {
                        record1 = RF.GetById<ReportRecord>(label.ReportRecordId);
                    }
                    foreach (var w in beReportWipBatchs)
                    {
                        var recordId = label.ReportRecordId ?? 0; //已有报工记录时,不再进行报工扣料 2025.11.26
                        if (recordId == 0)
                        {
                            //保留此逻辑,目的是兼容2025.11.26日前的旧数据处理
                            var info = taskInfo.Copy();
                            info.IsSuspect = true;
                            info.OkQty = 0;
                            info.NgQty = 0;
                            info.SuspectQty = 0;
                            info.ReworkQty = 0;
                            info.BatchNo = RT.Service.Resolve<ReportController>().GetReportBatchNo();
                            info.IsTaskFinish = data.IsTaskFinish;
                            info.IsValidatePrepare = false;
                            info.SourceType = Reports.Enums.SourceType.Report_SuspectProduct;
                            if (w.IsScraped)
                                info.NgQty = w.Qty;
                            else if (w.IsRework)
                                info.ReworkQty = w.Qty;
                            else
                                info.OkQty = w.Qty;

                            var defectIds = newDtls.Where(p => p.SubBatchNo == w.BatchNo).Select(p => p.DefectId).Distinct();
                            foreach (var defectId in defectIds)
                            {
                                if (defectId != null && info.DefectIds.All(p => p != defectId))
                                    info.DefectIds.Add(defectId.Value);
                            }
                            //调用报工接口
                            var record = RT.Service.Resolve<ReportController>().TaskReport(info, true);
                            recordId = record.Id;
                        }
                        else
                        {
                            var defectIds = newDtls.Where(p => p.SubBatchNo == w.BatchNo && p.DefectId > 0).Select(p => p.DefectId).Distinct();
                            foreach (var defectId in defectIds)
                            {
                                record1.Defects.Add(new ReportDefect() { DefectId = defectId.Value, ReportRecordId = recordId });
                            }
                            if (w.IsScraped)
                                record1.NgQty += w.Qty;
                            else if (w.IsRework)
                                record1.ReworkQty += w.Qty;
                            else
                                record1.OkQty += w.Qty;

                            record1.ReportQty += w.Qty;
                            record1.SuspectQty -= w.Qty;
                        }

                        //良品标签关联报工记录
                        var reportWipBatch = new ReportWipBatch()
                        {
                            ReportRecordId = recordId,
                            WipBatch = w,
                            BatchNo = w.BatchNo,
                            Qty = w.Qty,
                        };

                        RF.Save(reportWipBatch);

                        //关联报工记录ID
                        var ids = w.ReportRecordIds.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList();
                        ids.Add(recordId.ToString());
                        w.ReportRecordIds = ids.Distinct().Concat(",");

                        DB.Update<WipBatch>().Set(p => p.ReportRecordIds, w.ReportRecordIds).Where(p => p.Id == w.Id).Execute();
                    }

                    if (record1 != null)
                    {
                        //更新审核状态
                        record1.ExamineState = record1.SuspectQty <= 0 ? ReportRecordExamineState.Confirmed : ReportRecordExamineState.ToConfirm;
                        record1.SourceType = Reports.Enums.SourceType.Report_SuspectProduct;
                        RF.Save(record1);

                        RT.Service.Resolve<ReportController>().UpdateDispatchTaskQty(record1);
                    }
                    //更新任务单可疑品数
                    RT.Service.Resolve<ReportController>().UpdateSuspectQty(task.Id);
                    RT.Service.Resolve<ReportController>().UpdateDispatchTaskState(task.Id, DateTime.Now, true, Reports.Enums.SourceType.Report_SuspectProduct);
                }
                //末工序,良品与返工批次,生成物料标签
                if (task.EndProcess == true)
                {
                    RT.Service.Resolve<ReportController>().GenerateItemLabels(task.Id, beReportWipBatchs);
                }

                var isRework = data.RepairDetailList.Count > 0;
                var isScrap = data.ScrapDetailList.Count > 0;
                RT.Service.Resolve<WipPressureController>().SetSnSuspectState(label.BatchNo, false, isRework, isScrap);

                #endregion

                var dispatchConfig = RT.Service.Resolve<DispatchController>().GetDispatchConfig();
                printDatas = RT.Service.Resolve<ReportController>().CreatePrintDatas(true, PrintLabelType.Good, task, dispatchConfig.GoodLabel, dispatchConfig.SuspectLabel, beReportWipBatchs, label.WipResource);

                tran.Complete();
            }
            //委外可疑品标签需要同步
            if (task.IsOutsourcing == true)
            {
                var ids = newWipBatchs.Select(p => p.Id).Distinct().ToList();
                ids.Add(wipBatch.Id);
                var nwbs = Query<WipBatch>().Where(p => ids.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                //调用接口同步数据
                SyncWipBatchToOtherFactory(nwbs.ToList(), null, task);
            }
            return printDatas;
        }


        /// <summary>
        /// 可疑品处理
        /// </summary>
        /// <param name="data"></param>
        /// <param name="label"></param>
        /// <param name="wipBatch"></param>
        /// <param name="newWipBatchs"></param>
        /// <param name="newDtls"></param>
        void HandleSuspectProductLabel(SuspectProductLabelData data, SuspectProductLabel label, WipBatch wipBatch, EntityList<WipBatch> newWipBatchs, EntityList<SuspectProductLabelDetail> newDtls)
        {
            var task = RF.GetById<DispatchTask>(label.DispatchTaskId, new EagerLoadOptions().LoadWithViewProperty());

            var now = DateTime.Now;
            var newBatchCount = 0;
            if (data.ScrapDetailList.IsNotEmpty())
            {
                newBatchCount += data.ScrapDetailList.Count;
            }
            if (data.RepairDetailList.IsNotEmpty())
            {
                newBatchCount += data.RepairDetailList.Count;
            }
            var newBatchNos = new List<string>();
            var newBatchNoIndex = 0;
            if (newBatchCount > 0)
            {

                var invOrg = RT.Service.Resolve<InvOrgController>().GetByCode(RT.InvOrg.Value);

                var dispatchConfig = RT.Service.Resolve<DispatchController>().GetDispatchConfig();
                if (dispatchConfig == null)
                    throw new ValidationException("派工管理配置项不存在.".L10N());
                //if (!dispatchConfig.NumberRuleId.HasValue)
                //    throw new ValidationException("派工管理编码规则配置项未配置.".L10N());
                if (!dispatchConfig.NumberRuleId.HasValue)
                    throw new ValidationException("派工管理编码规则配置项未配置.".L10N());

                double numberRuleId = 0;
                //如果工序配置了编码规则,优先使用该规则
                if (task.Process?.NumberRuleId > 0)
                    numberRuleId = task.Process.NumberRuleId.Value;

                if (numberRuleId == 0)
                {
                    //根据产品的物料类型，找到配置项
                    TypeConfigValue configValue = Query<TypeConfigValue>().Where(p => p.Type == task.Product.Mtart).FirstOrDefault();
                    var config = ConfigService.GetConfig(new ItemTypeConfig(), typeof(DispatchTask), configValue);
                    if (config != null && config.ProcessNumberRuleId != null)
                    {
                        numberRuleId = config.ProcessNumberRuleId.Value;
                    }
                }

                if (numberRuleId == 0)
                {
                    //2810用的是绕包线编码规则以及非绕包线编码规则
                    if (invOrg.ExternalId != "2810")
                    {
                        numberRuleId = dispatchConfig.NumberRuleId.GetValueOrDefault();
                    }
                    else
                    {
                        if (label.WorkOrder.Product.Zmc.Contains("绕包"))
                        {
                            numberRuleId = dispatchConfig.EntangleNumberRuleId.GetValueOrDefault();
                        }
                        else
                        {
                            numberRuleId = dispatchConfig.UnEntangleNumberRuleId.GetValueOrDefault();
                        }
                    }
                }
                newBatchNos = RT.Service.Resolve<NumberRuleController>().GenerateSegment(numberRuleId, newBatchCount, label.DispatchTask).ToList();
            }

            if (data.ScrapDetailList.IsNotEmpty())
            {
                foreach (var dtlData in data.ScrapDetailList)
                {
                    var dtl = CreateSuspectProductLabelDetail(label.Id, dtlData, now, SuspectJudgeResult.Scrap);
                    dtl.SubBatchNo = newBatchNos[newBatchNoIndex++];
                    newDtls.Add(dtl);
                    if (wipBatch != null)
                    {
                        var newWipBatch = CreateWipBatch(wipBatch, dtl, true, task);
                        newWipBatch.ScrapQty = dtl.Qty;
                        newWipBatch.IsScraped = true;
                        newWipBatchs.Add(newWipBatch);
                    }
                }
            }
            if (data.RepairDetailList.IsNotEmpty())
            {
                foreach (var dtlData in data.RepairDetailList)
                {
                    var dtl = CreateSuspectProductLabelDetail(label.Id, dtlData, now, SuspectJudgeResult.Repair);
                    dtl.SubBatchNo = newBatchNos[newBatchNoIndex++];
                    newDtls.Add(dtl);
                    if (wipBatch != null)
                    {
                        var newWipBatch = CreateWipBatch(wipBatch, dtl, false, task);
                        newWipBatch.IsRework = true;
                        newWipBatchs.Add(newWipBatch);
                    }
                }
            }
            if (data.GoodQty > 0)
            {
                var suspectProductLabel = GetById<SuspectProductLabel>(data.SuspectProductLabelId);
                var dtlDetail = RT.Service.Resolve<SuspectProductLabelController>().GetSuspectProductLabelDetail(suspectProductLabel.BatchNo);
                if (dtlDetail == null)
                {
                    SuspectProductLabelDetailData dtlData = new SuspectProductLabelDetailData();
                    dtlData.Qty = data.GoodQty;
                    dtlData.DefectId = null;
                    var dtl = CreateSuspectProductLabelDetail(label.Id, dtlData, DateTime.Now, SuspectJudgeResult.Good);
                    dtl.SubBatchNo = suspectProductLabel.BatchNo;
                    newDtls.Add(dtl);
                }
                else
                {
                    dtlDetail.Qty = label.GoodQty;
                    newDtls.Add(dtlDetail);
                }
            }
            if (data.AttachmentIdList.IsNotEmpty())
            {
                DB.Update<SuspectProductLabelAttachment>().Set(p => p.OwnerId, data.SuspectProductLabelId)
                    .Where(p => data.AttachmentIdList.Contains(p.Id)).Execute();
            }

            label.GoodQty += data.GoodQty;
            label.ScrapQty += data.ScrapQty;
            label.RepairQty += data.RepairQty;

            if (label.GoodQty + label.ScrapQty + label.RepairQty >= label.Qty)
            {
                label.HandleState = SuspectHandleState.Processed;
            }

            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                RF.Save(label);
                RF.Save(newDtls);
                RF.Save(newWipBatchs);
                if (wipBatch != null)
                {
                    wipBatch.IsSuspectProduct = label.HandleState == SuspectHandleState.Processed ? YesNo.No : YesNo.Yes;
                    var wipBatchQty = label.Qty - label.ScrapQty - label.RepairQty;
                    if (wipBatch.Qty != wipBatchQty)
                        wipBatch.EditQtyProcessCode = label.Process?.Code;
                    wipBatch.Qty = wipBatchQty;
                    RF.Save(wipBatch);
                }
                tran.Complete();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void SyncWipBatchToOtherFactory(List<WipBatch> wipBatches, WipBatch wipBatch, DispatchTask task)
        {
            OutsourcingRequest request = Query<OutsourcingRequest>().Where(p => p.WorkOrderId == task.WorkOrderId && p.BeginProcess.Process.Code == task.Process.Code).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            if (request == null)
                return;
            if (wipBatch != null)
                wipBatches.Add(wipBatch);
            var invOrg = RT.Service.Resolve<InvOrgController>().GetByCode(RT.InvOrg.Value);

            EntityList<OutsourcingReportLog> logs = new EntityList<OutsourcingReportLog>();
            foreach (var newWipBatch in wipBatches)
            {
                var processingType = RT.Service.Resolve<OutsourcingApiController>().GetProcessingTypeByWipBatch(newWipBatch);
                OutsourcingReportLog log = request.OutsourcingReportLogList.FirstOrDefault(p => p.SN == newWipBatch.BatchNo);
                if (log != null)
                {
                    log.Qty = newWipBatch.Qty;
                    log.ProcessingType = processingType;
                    logs.Add(log);
                }
                else
                {
                    log = new OutsourcingReportLog();
                    log.OutsourcingRequestId = request.Id;
                    log.SN = newWipBatch.BatchNo;
                    log.LotNo = newWipBatch.BatchNo;
                    log.Qty = newWipBatch.Qty;
                    log.PassQty = 0;
                    log.NgQty = 0;
                    log.State = OutsourcingDetailState.Submitted;
                    log.PersistenceStatus = PersistenceStatus.New;
                    log.ProcessingType = processingType;//ProcessingType.Good;
                    log.ReportFactory = invOrg.ExternalId;
                    RF.Save(log);
                    logs.Add(log);
                }
            }

            //根据创建的记录，判断是否需要拆分发料明细和收货明细
            EntityList<ProcessingOutbound> processingOutbounds = new EntityList<ProcessingOutbound>();
            foreach (var p in logs)
            {
                //可能会拆标签,发料明细数据要重新校对
                var outbound = request.ProcessingOutsourcingOutboundList.FirstOrDefault(item => item.SN == p.SN);
                if (outbound != null)
                {
                    outbound.Qty = p.Qty;
                }
                else
                {
                    outbound = new ProcessingOutbound();

                    outbound.SourceId = 0;
                    outbound.PersistenceStatus = PersistenceStatus.New;
                    outbound.GenerateId();
                    outbound.Qty = p.Qty;
                    outbound.SN = p.SN;
                    outbound.LotNo = p.LotNo;
                    outbound.State = OutsourcingDetailState.Submitted;
                    outbound.OutsourcingRequestId = request.Id;
                }
                RF.Save(outbound);
                processingOutbounds.Add(outbound);
            }

            var req = new OutsourcingRequest();
            req.Clone(request, new CloneOptions(CloneActions.NormalProperties));
            req.OutsourcingReportLogList.Clear();
            req.ProcessingOutsourcingInStockList.Clear();
            req.ProcessingOutsourcingOutboundList.Clear();
            req.OutsourcingReportLogList.AddRange(logs);
            req.ProcessingOutsourcingOutboundList.AddRange(processingOutbounds);

            //创建事务上传
            //CreateTrans(wipBatches, task, request);
            //调用接口传可疑品标签
            SyncOtherFactoryInterface(wipBatches, task, request);

            //调用接口传报工记录
            //调用接口
            RT.Service.Resolve<OutsourcingApiController>().SyncOutsourcingRequestToOtherFactory(req, 3, req.InitiatorFactory);
        }

        public virtual void SyncOtherFactoryInterface(List<WipBatch> wipBatches, DispatchTask task, OutsourcingRequest request)
        {
            //获取指定的接口地址
            var smomControlSetting = RT.Service.Resolve<SmomBaseController>().GetSmomControlSettingByFactoryCode(request.InitiatorFactory);
            SyncWipBatchResponse response = new SyncWipBatchResponse();

            var erpDataInfLog = RT.Service.Resolve<InfDataLogController>().SaveErpDataInfLog(InfType.OutsourcingSupWipBatch, "", DateTime.Now, CallDirection.FactoryToFactory, CallResult.Success, 1);
            erpDataInfLog.RequestContent = JsonConvert.SerializeObject(wipBatches);

            try
            {
                if (smomControlSetting == null || smomControlSetting.FactoryUrl.IsNullOrEmpty())
                    throw new ValidationException("未配置总控Url地址!".L10N());

                var smomParam = new List<SmomParam>()
                {
                    new SmomParam { Value =wipBatches },
                                    new SmomParam { Value =request.InitiatorFactory }
                                 }.ToArray();
                response = SmomControlHepler.SmomPost<SyncWipBatchResponse>("WipBatchController", "SyncWipBatch", smomControlSetting.FactoryUrl, smomParam);

                erpDataInfLog.ResponseContent = JsonConvert.SerializeObject(response);

            }
            catch (Exception ex)
            {
                response.errMsg = ex.GetBaseException().Message;

                erpDataInfLog.CallResult = CallResult.Fail;
                erpDataInfLog.ErrorMsg = ex.GetBaseException().Message;
                erpDataInfLog.ResponseContent = ex.GetBaseException().Message;
            }
            finally
            {

                erpDataInfLog.EndDate = DateTime.Now;
                erpDataInfLog.PersistenceStatus = PersistenceStatus.Modified;
                RF.Save(erpDataInfLog);

                //更新事务上传
                //如果统一报错 ，那么就全部更新为同一错误
                if (!response.errMsg.IsNullOrEmpty())
                {
                    foreach (var wipBatche in wipBatches)
                    {
                        DB.Update<WipBatch>().Set(p => p.ReLoadCount, (wipBatche.ReLoadCount ?? 0) + 1).Where(p => p.Id == wipBatche.Id).Execute();

                        //RT.Service.Resolve<IUploadLogControllercs>().UpdateWipBatchCreateTransaction(wipBatche.Id, response.errMsg);
                    }
                }
                else
                {
                    foreach (var wipBatche in wipBatches)
                    {
                        var rs = response.failResponses.Where(p => p.Id == wipBatche.Id).ToList();
                        //存在错误信息的时候
                        if (rs.Count > 0)
                        {
                            foreach (var r in rs)
                            {
                                DB.Update<WipBatch>().Set(p => p.ReLoadCount, (wipBatche.ReLoadCount ?? 0) + 1).Where(p => p.Id == r.Id).Execute();
                                //RT.Service.Resolve<IUploadLogControllercs>().UpdateWipBatchCreateTransaction(r.Id, r.Msg);
                            }
                        }
                        else
                        {
                            DB.Update<WipBatch>().Set(p => p.IsUpload, true).Where(p => p.Id == wipBatche.Id).Execute();
                            //RT.Service.Resolve<IUploadLogControllercs>().UpdateWipBatchCreateTransaction(wipBatche.Id, string.Empty);
                        }
                    }
                }
            }
        }

        public virtual void CreateTrans(List<WipBatch> wipBatches, DispatchTask task, OutsourcingRequest request)
        {
            var curDate = RF.Find<OutsourcingRequest>().GetDbTime();
            List<SyncWipBatchData> datas = new List<SyncWipBatchData>();
            foreach (var item in wipBatches)
            {
                SyncWipBatchData data = new SyncWipBatchData();
                data.Id = item.Id;
                data.WorkOrderId = task.WorkOrderId.Value;
                data.WorkOrderNo = task.WorkOrder?.No;
                data.TransactionDate = curDate;
                data.Qty = item.Qty;
                data.LotNo = item.BatchNo;
                data.WERKS = request.InitiatorFactory;
                datas.Add(data);
            }

            //创建事务上传
            RT.Service.Resolve<IUploadLogControllercs>().SyncWipBatchCreateTransaction(datas);
        }

        /// <summary>
        /// 创建生产批次
        /// </summary>
        /// <param name="wipBatch"></param>
        /// <param name="dtl"></param>
        /// <returns></returns>
        private WipBatch CreateWipBatch(WipBatch wipBatch, SuspectProductLabelDetail dtl, bool IsScraped, DispatchTask dispatchTask)
        {
            return new WipBatch
            {
                WorkOrderId = wipBatch.WorkOrderId,
                BatchNo = dtl.SubBatchNo,
                BatchState = wipBatch.BatchState,
                BoxesQty = wipBatch.BoxesQty,
                IsGenerate = wipBatch.IsGenerate,
                IsScraped = IsScraped,
                IsMantissa = true,
                Qty = dtl.Qty,
                RangeId = wipBatch.RangeId,
                DispatchTaskId = dispatchTask?.Id,
                ResourceCode = dispatchTask.ResourceCode,
                ProcessCode = dispatchTask.ProcessCode,
                GenerateProcessCode = dispatchTask.ProcessCode,
                IsOutsourcing = wipBatch.IsOutsourcing,
                EditQtyProcessCode = dispatchTask.ProcessCode
            };
        }

        /// <summary>
        /// 创建可疑品标签明细
        /// </summary>
        /// <param name="suspectProductLabelId"></param>
        /// <param name="dtlData"></param>
        /// <param name="now"></param>
        /// <param name="judgeResult"></param>
        /// <returns></returns>
        private SuspectProductLabelDetail CreateSuspectProductLabelDetail(double suspectProductLabelId, SuspectProductLabelDetailData dtlData,
            DateTime now, SuspectJudgeResult judgeResult)
        {
            return new SuspectProductLabelDetail
            {
                Qty = dtlData.Qty,
                HandleById = RT.IdentityId,
                HandleDate = now,
                DefectId = dtlData.DefectId,
                SuspectJudgeResult = judgeResult,//SuspectJudgeResult.Scrap,
                SuspectProductLabelId = suspectProductLabelId
            };
        }
        #endregion

        #region 上传可疑品处理标签附件
        /// <summary>
        /// 上传可疑品处理标签附件
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [ApiService("上传可疑品处理标签附件")]
        public virtual double UploadSuspectProductLabelAttachment([ApiParameter("附件数据")] AttachmentData data)
        {
            string base64 = "base64,";
            var attachment = new SuspectProductLabelAttachment();
            attachment.FileName = data.FileName;
            attachment.FileSize = data.FileSize;
            attachment.FileExtesion = data.FileExtesion;
            int num = data.Content.IndexOf(base64) + base64.Length;
            if (data.Content.Length > num)
            {
                attachment.Content = Convert.FromBase64String(data.Content.Substring(num));
            }
            attachment.GenerateId();
            RF.Save(attachment);
            return attachment.Id;
        }
        #endregion
    }
}
