using SIE.AbnormalInfo.AbnormalMonitors;
using SIE.AbnormalInfo.AbnormalMonitors.AbnormalMonitorTasks;
using SIE.Defects;
using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Web.AbnormalInfo.AbnormalMonitors.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.AbnormalInfo.AbnormalMonitors
{
    /// <summary>
    /// 异常监控任务视图配置
    /// </summary>
    internal class AbnormalMonitorTaskViewConfig : WebViewConfig<AbnormalMonitorTask>
    {
        /// <summary>
        /// 异常处理视图
        /// </summary>
        const string ProcessView = "ProcessView";
        /// <summary>
        /// 只读视图
        /// </summary>
        const string ReadonlyView = "ReadonlyView";

        /// <summary>
        /// 默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.FormEdit();
            View.DeclareExtendViewGroup(new string[] { ProcessView, ReadonlyView });
            View.AssignAuthorize(typeof(AbnormalMonitorTask));
            switch (ViewGroup)
            {
                case ProcessView:
                    ConfigProcessView();
                    break;
                case ReadonlyView:
                    ConfigReadonlyView();
                    break;
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior("SIE.Web.AbnormalInfo.AbnormalMonitors.Behaviors.AbnormalMonitorTaskBehavior");
            View.UseDefaultCommands();
            View.DisableEditing();
            View.FormEdit();
            View.RemoveCommands(WebCommandNames.Copy, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll, WebCommandNames.ExportXls);
            View.ReplaceCommands(WebCommandNames.Add, "SIE.Web.AbnormalInfo.AbnormalMonitors.Commands.AddTaskCommand");
            View.ReplaceCommands(WebCommandNames.Edit, "SIE.Web.AbnormalInfo.AbnormalMonitors.Commands.EditTaskCommand");
            View.ReplaceCommands(WebCommandNames.Delete, "SIE.Web.AbnormalInfo.AbnormalMonitors.Commands.DeleteTaskCommand");
            View.UseCommands("SIE.Web.AbnormalInfo.AbnormalMonitors.Commands.ViewTaskCommand", "SIE.Web.AbnormalInfo.AbnormalMonitors.Commands.ProcessCommand", "SIE.Web.AbnormalInfo.AbnormalMonitors.Commands.CancelReasonCommand");
            View.UseCommand(typeof(ExportCommand).FullName);
            View.Property(p => p.Code);
            View.Property(p => p.AbnormalName);
            View.Property(p => p.WarnTimes).HasLabel("预警标识".L10N());
            View.Property(p => p.AbnormalWarnDefineId).HasLabel("异常预警").Readonly();
            View.Property(p => p.AbnormalDefineId).DisableSort();
            View.Property(p => p.TaskState);
            View.Property(p => p.TaskType);
            View.Property(p => p.WorkShopId).DisableSort();
            View.Property(p => p.LineId).DisableSort();
            View.Property(p => p.PushMethord).DisableSort();
            View.Property(p => p.TaskHandlerId).DisableSort();
            View.Property(p => p.TriggerNo).DisableSort();
            View.Property(p => p.CancelReason).ShowInList(width: 200).DisableSort();
            View.ChildrenProperty(p => p.AttachmentList).Show(ChildShowInWhere.Hide);
            View.ChildrenProperty(p => p.CustomPushTargetList).Show(ChildShowInWhere.Hide);
            View.AttachChildrenProperty(typeof(AbnormalMonitorTaskLog), (e) =>
            {
                var args = e as ChildPagingDataArgs;
                var item = args.Parent as AbnormalMonitorTask;
                if (item == null)
                {
                    return new EntityList<AbnormalMonitorTaskLog>();
                }
                return RT.Service.Resolve<AbnormalMonitorTaskLogService>().GetList(item.Id, args.PagingInfo);
            }).HasLabel("日志").Show(ChildShowInWhere.All);

        }

        /// <summary>
        /// 异常处理视图
        /// </summary>
        protected void ConfigProcessView()
        {
            View.ClearCommands();
            View.UseCommand("SIE.Web.AbnormalInfo.AbnormalMonitors.Commands.SaveProcessCommand");
            View.UseCommand("SIE.Web.AbnormalInfo.AbnormalMonitors.Commands.SubmitProcessCommand");
            View.AddBehavior("SIE.Web.AbnormalInfo.AbnormalMonitors.Behaviors.ProcessTaskBehavior");
            View.HasDetailColumnsCount(4);
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Readonly().ShowInDetail();
                View.Property(p => p.AbnormalName).Readonly().ShowInDetail();
                View.Property(p => p.AbnormalDefineId).Readonly().ShowInDetail();
                View.Property(p => p.TaskState).Readonly().ShowInDetail();

                View.Property(p => p.WorkShopId).Readonly().ShowInDetail();
                View.Property(p => p.LineId).Readonly().ShowInDetail();
                View.Property(p => p.AbnormalWarnDefineId).HasLabel("异常预警").Readonly().ShowInDetail();
                View.Property(p => p.PushMethord).ShowInDetail();
                View.Property(p => p.TaskHandlerId).ShowInDetail().Readonly(p => !(p.PushMethord == SIE.AbnormalInfo.Common.PushMethordEnum.EightD || p.PushMethord == SIE.AbnormalInfo.Common.PushMethordEnum.PDCA));
                View.Property(p => p.JoinDefectCodes).ShowInDetail().UsePagingLookUpGridPopupEditor(p =>
                {
                    p.Model = typeof(Defect).FullName;
                    p.DisplayField = Defect.CodeProperty.Name;
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add(AbnormalMonitorTask.JoinDefectIdsProperty.Name, Defect.IdProperty.Name);
                    dic.Add(AbnormalMonitorTask.JoinDefectCodeDescriptionsProperty.Name, Defect.DescriptionProperty.Name);
                    p.MutiLinkField = dic.ToJsonString();
                    p.XType = "defectcombopopup";
                }).Readonly(p => !(p.PushMethord == SIE.AbnormalInfo.Common.PushMethordEnum.EightD || p.PushMethord == SIE.AbnormalInfo.Common.PushMethordEnum.PDCA));
                View.Property(p => p.JoinDefectCodeDescriptions).ShowInDetail(columnSpan: 2).Readonly();

                View.Property(p => p.ProblemDescription).UseMemoEditor().Readonly().ShowInDetail(columnSpan: 2);
                View.Property(p => p.AbnormalReason).UseMemoEditor().ShowInDetail(height: "80", columnSpan: 2);
                View.Property(p => p.TempMeasures).UseMemoEditor().ShowInDetail(height: "80", columnSpan: 2);
                View.Property(p => p.LongMeasures).UseMemoEditor().ShowInDetail(height: "80", columnSpan: 2);
                View.ChildrenProperty(p => p.AttachmentList).Show(ChildShowInWhere.All);
            }
        }

        /// <summary>
        /// 只读视图
        /// </summary>
        protected void ConfigReadonlyView()
        {
            View.DisableEditing();
            View.ClearCommands();
            View.AddBehavior("SIE.Web.AbnormalInfo.AbnormalMonitors.Behaviors.ProcessTaskBehavior");
            View.HasDetailColumnsCount(4);
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Readonly().ShowInDetail();
                View.Property(p => p.AbnormalName).Readonly().ShowInDetail();
                View.Property(p => p.AbnormalDefineId).Readonly().ShowInDetail();
                View.Property(p => p.TaskState).Readonly().ShowInDetail();

                View.Property(p => p.WorkShopId).Readonly().ShowInDetail();
                View.Property(p => p.LineId).Readonly().ShowInDetail();
                View.Property(p => p.AbnormalWarnDefineId).HasLabel("异常预警").Readonly().ShowInDetail();
                View.Property(p => p.PushMethord).ShowInDetail();
                View.Property(p => p.TaskHandlerId).ShowInDetail();
                View.Property(p => p.TriggerNo).ShowInDetail();
                View.Property(p => p.JoinDefectCodes).ShowInDetail().Readonly();
                View.Property(p => p.JoinDefectCodeDescriptions).ShowInDetail().Readonly();
                View.Property(p => p.ProblemDescription).UseMemoEditor().Readonly().ShowInDetail(columnSpan: 2);
                View.Property(p => p.AbnormalReason).UseMemoEditor().ShowInDetail(height: "80", columnSpan: 2);
                View.Property(p => p.TempMeasures).UseMemoEditor().ShowInDetail(height: "80", columnSpan: 2);
                View.Property(p => p.LongMeasures).UseMemoEditor().ShowInDetail(height: "80", columnSpan: 2);
                View.ChildrenProperty(p => p.AttachmentList).Show(ChildShowInWhere.All).UseViewGroup("ReadonlyView");
            }
        }

        protected override void ConfigDetailsView()
        {
            View.AddBehavior("SIE.Web.AbnormalInfo.AbnormalMonitors.Behaviors.AddOrEditAbnormalMonitorTaskBehavior");
            View.ClearCommands();
            View.UseCommand(typeof(SaveTaskCommand).FullName);
            View.HasDetailColumnsCount(2);
            View.Property(p => p.Code).Readonly();
            View.Property(p => p.AbnormalName);
            View.Property(p => p.AbnormalDefineId)
            .UsePagingLookUpEditor((m, e) =>
             {
                 Dictionary<string, string> dic = new Dictionary<string, string>();
                 dic.Add(nameof(e.AbnormalWarnDefineId), nameof(e.AbnormalDefine.AbnormalWarnDefineId));
                 dic.Add(nameof(e.AbnormalWarnDefineCode), nameof(e.AbnormalDefine.AbnormalWarnDefineCode));
                 m.DicLinkField = dic;
             });
            View.Property(p => p.AbnormalWarnDefineCode).Readonly();

            View.Property(p => p.WorkShopId).UseDataSource((source, pagingInfo, keyword) =>
            {
                var result = RT.Service.Resolve<WorkShopController>().GetEnterprises(EnterpriseType.Shop, pagingInfo, keyword);
                if (result == null) return new EntityList<WorkShop>();
                return result;
            }).UsePagingLookUpEditor();
            View.Property(p => p.LineId)
                .UseDataSource((source, pagingInfo, keyword) =>
                {

                    double? workShopId = null;
                    if (source is AbnormalMonitorTask bill)
                        workShopId = bill.WorkShopId;

                    var stateList = new List<ResourceState>() { ResourceState.Actived };
                    var sourceType = new List<SyncSourceType>() { SyncSourceType.Enterprise, SyncSourceType.Equipment };
                    return RT.Service.Resolve<WipResourceController>().GetWipResources(stateList, workShopId, sourceType, pagingInfo, keyword);
                })
                .Readonly(p => p.WorkShopId == null || p.WorkShopId == 0);
            View.Property(p => p.ProblemDescription).UseMemoEditor().ShowInDetail(height: "800");
            View.ChildrenProperty(p => p.AttachmentList).Show(ChildShowInWhere.Hide);
            View.ChildrenProperty(p => p.CustomPushTargetList).Show(ChildShowInWhere.Hide);
        }

        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.AbnormalName);
            View.Property(p => p.AbnormalDefineId);
            View.Property(p => p.TaskState);
            View.Property(p => p.TaskType);
            View.Property(p => p.WorkShopId).UseDataSource((source, pagingInfo, keyword) =>
            {
                var result = RT.Service.Resolve<WorkShopController>().GetEnterprises(EnterpriseType.Shop, pagingInfo, keyword);
                if (result == null) return new EntityList<WorkShop>();
                return result;
            }).UsePagingLookUpEditor();
            View.Property(p => p.LineId);
            View.Property(p => p.ProblemDescription);
            View.Property(p => p.CreateDate).UseDateRangeEditor(p => { p.DateFormat = "Y/m/d"; p.DateRangeType = ObjectModel.DateRangeType.Month; });
        }
    }
}