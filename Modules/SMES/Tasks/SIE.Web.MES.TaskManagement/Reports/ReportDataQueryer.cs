using Newtonsoft.Json;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.TaskManagement.Models;
using SIE.MES.TaskManagement.Reports;
using SIE.MES.TaskManagement.TaskConfigs;
using SIE.Web.Common.Prints;
using SIE.Web.Data;
using System;
using System.Collections.Generic;


namespace SIE.Web.MES.TaskManagement.Reports
{
    /// <summary>
    /// 报工数据查询者
    /// </summary>
    public class ReportDataQueryer : DataQueryer
    {
        /// <summary>
        /// 获取报工规则配置
        /// SIE.Web.MES.TaskManagement.Reports.ProductFamilyBehavior.js 引用
        /// </summary>
        /// <param name="familyId">产品族ID</param>
        /// <returns>报工规则配置</returns>
        public ReportRuleConfigInfo GetReportRuleConfig(double familyId)
        {
            ReportRuleConfigInfo info = new ReportRuleConfigInfo() { IsExpendItem = true };
            var config = RT.Service.Resolve<ReportController>().GetReportRuleConfig(familyId);
            if (config != null)
            {
                info.ReportMode = (int)config.ReportMode;
                info.ReportQty = config.ReportQty;
                info.ModReport = config.IsModReport ? 1 : 0;
                info.IsSyntype = config.IsSyntype;
                info.IsExpendItem=config.IsExpendItem;  
                info.Family = 0;
            }
            return info;
        }

        /// <summary>
        /// 获取任务单生成规则配置
        /// SIE.Web.MES.TaskManagement.Reports.ProductFamilyBehavior.js 引用
        /// </summary>
        /// <param name="familyId">产品族ID</param>
        /// <returns>任务单生成规则配置</returns>
        public TaskConfigInfo GetTaskConfig(double familyId)
        {
            TaskConfigInfo info = new TaskConfigInfo();
            var config = RT.Service.Resolve<TaskConfigController>().GetTaskConfig(familyId);
            if (config != null)
            {
                info.Qty = config.Qty;
                info.ByQty = config.ByQty;
                info.ByProcess = config.ByProcess;
                info.ByVirtualPart = config.ByVirtualPart;
                info.BySpecification = config.BySpecification;
                info.Family = 0;
            }
            return info;
        }

        /// <summary>
        /// 保存报工规则配置信息
        /// SIE.Web.MES.TaskManagement.Reports.ProductFamilyBehavior.js 引用
        /// </summary>
        /// <param name="result">规则json</param>
        public void SaveReportRuleConfigs(string result)
        {
            var configs = JsonConvert.DeserializeObject<List<FamilyReportRuleConfig>>(result);
            RT.Service.Resolve<ReportController>().SaveReportRuleConfigs(configs);
        }

        /// <summary>
        /// 保存任务单生成规则配置信息
        /// SIE.Web.MES.TaskManagement.Reports.ProductFamilyBehavior.js 引用
        /// </summary>
        /// <param name="result">规则json</param>
        public void SaveTaskConfigs(string result)
        {
            var configs = JsonConvert.DeserializeObject<List<FamilyTaskConfig>>(result);
            RT.Service.Resolve<TaskConfigController>().SaveTaskConfigs(configs);
        }

        /// <summary>
        /// 验证任务单生成规则配置信息
        /// SIE.Web.MES.TaskManagement.Reports.ProductFamilyBehavior.js 引用
        /// </summary>
        /// <param name="result">规则json</param>
        public string ValidateTaskConfigs(string result)
        {
            var configs = JsonConvert.DeserializeObject<List<FamilyTaskConfig>>(result);
            return RT.Service.Resolve<TaskConfigController>().ValidateTaskConfigs(configs);
        }

        /// <summary>
        /// 获取共模任务单信息
        /// SIE.Web.MES.TaskManagement.Reports.ReportCommon.js调用
        /// </summary>
        public List<ReportTaskInfo> GetCommonModeInfo(double dispatchTaskId)
        {
            return RT.Service.Resolve<ReportController>().GetIsSyntypeTasks(dispatchTaskId);
        }

        /// <summary>
        /// 获取主任务单缺陷列表
        /// SIE.Web.MES.TaskManagement.Reports.ReportGenerator.js调用
        /// </summary>
        /// <param name="dispatchTaskId">任务单Id</param>
        /// <returns>缺陷列表</returns>
        public List<double> GetMainDefectIds(double dispatchTaskId)
        {
            return RT.Service.Resolve<ReportController>().GetMainDefectIds(dispatchTaskId);
        }

        /// <summary>
        /// 获取报工记录
        /// SIE.Web.MES.TaskManagement.Reports.ReportRefreshCommand.js调用
        /// </summary>
        /// <param name="dispatchTaskId">任务单Id</param>
        /// <returns>报工记录</returns>
        public ReportRecord GetOrCreateReportRecord(double dispatchTaskId)
        {
            return RT.Service.Resolve<ReportController>().GetOrCreateMainReportRecord(dispatchTaskId);
        }

        /// <summary>
        /// 判断是否维护了报工打印模板
        /// </summary>
        /// <returns></returns>
        public bool TryGetReportPrintemplate() 
        {
            var printTemplate = RT.Service.Resolve<ReportController>().GetReportPrintemplate();
            if (printTemplate == null)
                throw new ValidationException("请在配置项中维护报工打印模板".L10N());
            return true;
        }

        /// <summary>
        ///报工标签打印
        /// </summary>
        /// <param name="reportRecordId">报工记录Id</param>
        /// <returns>打印结果</returns>
        public ReportLabelPrintResult ReportLabelPrint(double reportRecordId)
        {
            var prtResult = new ReportLabelPrintResult();
            var template = RT.Service.Resolve<ReportController>().GetReportPrintemplate();

            try
            {
                prtResult.ErrMsg = string.Empty;
                prtResult.Type = template.Type;
                var report = ReportFactory.Current.GetReportByExtension(template.Type);
                var reportRecord = RF.GetById<ReportRecord>(reportRecordId);

                if (reportRecord==null|| reportRecord.BatchNo.IsNullOrEmpty()) 
                {
                    throw new ValidationException("打印失败【报工记录没有生成批次号】".L10N());
                }

                prtResult.Url = report.PrintProcess(new ReportRecordPrintable(), template.Id, template.Content, () =>
                {
                    return new EntityList<ReportRecord>() { reportRecord };
                });
            }
            catch (Exception exc)
            {
                prtResult.ErrMsg = exc.Message;
            }
            return prtResult;
        }

        /// <summary>
        /// 报工
        /// SIE.Web.MES.TaskManagement.Reports.ReportCommon.js 调用
        /// </summary>
        /// <param name="record">报工记录</param>
        /// <param name="defectIds">缺陷Id</param>
        /// <param name="assTaskInfo">关联任务单</param>
        /// <param name="isReport">是否直接报工</param>
        /// <returns>true/false</returns>
        public double Report(ReportRecord record, List<double> defectIds, List<ReportTaskInfo> assTaskInfo, bool isReport)
        {
            ReportTaskInfo info = new ReportTaskInfo()
            {
                BatchNo = record.BatchNo,
                RecordId = record.Id,  //0表示新增的
                OkQty = record.OkQty,
                NgQty = record.NgQty,
                ReportNgQty = record.NgQty,
                Remark = record.Remark,
                TaskId = record.DispatchTaskId,
                Hour = record.Hour,
                ProcessId = record.ProcessId,
                StationId = record.StationId,
                WorkOrderId = record.WorkOrderId ?? 0,
                IsTaskFinish = true,
                IsValidatePrepare = true,
            };
            info.DefectIds.AddRange(defectIds);
            info.SyntypeTaskInfos.AddRange(assTaskInfo);
            var reportRecord = RT.Service.Resolve<ReportController>().TaskReport(info, isReport);
            return reportRecord.Id;
        }

        /// <summary>
        /// 获取任务统计报表数据
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>任务统计报表数据列表</returns>

        public List<DispatchReportInfo> GetDispatchReport(ReportDispatchTaskViewModelCriteria criteria)
        {
            List<DispatchReportInfo> resultList = new List<DispatchReportInfo>();
            resultList.Add(new DispatchReportInfo
            {
                WorkOrderNo = "test1111",
                ProcessName = "第一道工序",
                ProductCode = "1200212",
                WorkshopName = "车间1",
                ResourceName = "资源1",
                HeadTitle = "任务数",
                Qty = 1
            });
            resultList.Add(new DispatchReportInfo
            {
                WorkOrderNo = "test1111",
                ProcessName = "第一道工序",
                ProductCode = "1200212",
                WorkshopName = "车间1",
                ResourceName = "资源1",
                HeadTitle = "订单数",
                Qty = 2
            });
            resultList.Add(new DispatchReportInfo
            {
                WorkOrderNo = "test1111",
                ProcessName = "第一道工序",
                ProductCode = "1200212",
                WorkshopName = "车间1",
                ResourceName = "资源1",
                HeadTitle = "完工数",
                Qty = 3
            });
            //RT.Service.Resolve<ReportController>().GetRecordList(criteria);
            return resultList;
        }

        /// <summary>
        /// Lot打印返回结果
        /// </summary>
        [Serializable]
        public class ReportLabelPrintResult
        {
            /// <summary>
            /// 类型
            /// </summary>
            public string Type { get; set; }

            /// <summary>
            /// 错误信息
            /// </summary>
            public string ErrMsg { get; set; }

            /// <summary>
            /// 路径
            /// </summary>
            public string Url { get; set; }
        }

        /// <summary>
        /// 按月导出报工记录审核数据
        /// SIE.Web.MES.TaskManagement.Reports.Commands.ReportExportByMonthCommand.js 调用
        /// </summary>
        /// <param name="criteria">查询条件（包含时间范围）</param>
        /// <returns>报工记录列表</returns>
        public EntityList<ReportRecordExamine> ExportReportRecordExamineByMonth(ReportRecordExamineCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ValidationException("查询条件不能为空".L10N());
            }

            // 导出场景必须有时间范围限制
            if (!criteria.ReportTime.BeginValue.HasValue || !criteria.ReportTime.EndValue.HasValue)
            {
                throw new ValidationException("导出必须选择时间范围".L10N());
            }

            // 计算时间跨度，限制单次导出范围（最多3个月）
            var timeSpan = criteria.ReportTime.EndValue.Value - criteria.ReportTime.BeginValue.Value;
            if (timeSpan.TotalDays > 90)
            {
                throw new ValidationException("导出时间范围不能超过3个月".L10N());
            }

            // 调用 Controller 的导出方法
            return RT.Service.Resolve<ReportController>().ExportReportRecordExamine(criteria);
        }
    }
}