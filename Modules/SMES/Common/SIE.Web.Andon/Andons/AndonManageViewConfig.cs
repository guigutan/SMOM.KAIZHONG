using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Scripting.Actions.Calls;
using SIE.Andon.Andons;
using SIE.Defects;
using SIE.Domain;
using SIE.MES.WorkOrders;
using SIE.MetaModel.View;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Tech.Stations;
using SIE.Web.Andon.Andons.Commands;
using SIE.Web.Common.Attachments.Commands;
using SIE.Web.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Andon.Andons
{
    /// <summary>
    /// 安灯管理视图配置
    /// </summary>
    public class AndonManageViewConfig : WebViewConfig<AndonManage>
    {
        /// <summary>
        /// 附加表单事件报告
        /// </summary>
        public const string EventViewGroup = "EventViewGroup";

        /// <summary>
        /// 附加表单事件报告查看视图
        /// </summary>
        public const string LookUpEventViewGroup = "LookUpEventViewGroup";

        /// <summary>
        /// 查看按钮视图
        /// </summary>
        public const string LookUpViewGroup = "LookUpViewGroup";

        /// <summary>
        /// 转派视图
        /// </summary>
        public const string ReassignmentViewGroup = "ReassignmentViewGroup";

        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.FormEdit();
            View.DeclareExtendViewGroup(EventViewGroup, LookUpEventViewGroup, LookUpViewGroup, ReassignmentViewGroup);
            if (ViewGroup == EventViewGroup)
            {
                EventView();
            }
            if (ViewGroup == LookUpViewGroup)
            {
                LookUpView();
            }
            if (ViewGroup == LookUpEventViewGroup)
            {
                LookUpEventView();
            }
            if (ViewGroup == ReassignmentViewGroup)
            {
                ReassignmentView();
            }
        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(/*typeof(AndonManageAddCommand).FullName,*/ "SIE.Web.Andon.Andons.Commands.AndonManageLookUpCommand"
                //typeof(AndonManageCancelCommand).FullName, typeof(AndonManageResponseCommand).FullName, typeof(AndonManageReassignmentCommand).FullName,
                //typeof(AndonManageHandleCommand).FullName, typeof(AndonManageCheckCommand).FullName, typeof(AndonManageRejectCommand).FullName
                );
            View.UseCommands("SIE.Web.Andon.Andons.Commands.AndonManageGoExpCommand", /*typeof(AndonManageImageCommand).FullName, */"SIE.Web.Andon.Andons.Commands.AndonManageExportXlsCommand", "SIE.Web.Andon.Andons.Commands.AndonManageExportXlsAllCommand");
            View.AddBehavior("SIE.Web.Andon.Andons.Behaviors.AndonManageListBehavior");
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.State);
                View.Property(p => p.AndonManageCode);
                View.Property(p => p.AndonType);
                View.Property(p => p.WipResource).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.WipResourceName), nameof(e.WipResource.Name));
                    m.DicLinkField = keyValues;
                    m.BindDisplayField = AndonManage.WipResourceCodeProperty.Name;
                });
                View.Property(p => p.WipResourceName).Show().Readonly();
                View.Property(p => p.ProblemDesc);
                View.Property(p => p.Solution);
                View.Property(p => p.Reason);
                View.Property(p => p.HandleMethod);
                View.Property(p => p.Measures);
                View.Property(p => p.Trigger);
                View.Property(p => p.TriggerTime).ShowInList(width: 150).UseListSetting(p => p.HelpInfo = "点击触发按钮的时间");
                View.Property(p => p.ResponseHour).Show().Readonly().DisableSort();
                View.Property(p => p.HandleHour).Show().Readonly().DisableSort();
                View.Property(p => p.CheckHour).Show().Readonly().DisableSort();
                View.Property(p => p.Duration).Show().Readonly().DisableSort();
                View.Property(p => p.ActualTime).DisableSort();
                View.Property(p => p.Responser).Show().Readonly().DisableSort();
                View.Property(p => p.ResponseTime).Show().Readonly().DisableSort();
                View.Property(p => p.Handler2).Show().Readonly().DisableSort();
                View.Property(p => p.HandleTime).Show().Readonly().DisableSort();
                View.Property(p => p.Checker).Show().Readonly().DisableSort();
                View.Property(p => p.CheckTime).Show().Readonly().DisableSort();


                View.Property(p => p.AndonManageClass);
                View.Property(p => p.Andon);
                View.Property(p => p.WipResourceAndonCode).Show().Readonly();
                View.Property(p => p.LastTime);
                //View.Property(p => p.Solution).ShowInList(width: 200);
                View.Property(p => p.Priority);
                View.Property(p => p.Defect).ShowInList(width: 200);
                View.Property(p => p.Department);
                View.Property(p => p.FaultTime).ShowInList(width:150);
                View.Property(p => p.RespPerson);

                View.Property(p => p.CloseTime).ShowInList(width: 150);
                View.Property(p => p.GeneralProbDtlId).Show();
                View.Property(p => p.Factory);
                View.Property(p => p.WorkShop);
                View.Property(p => p.Station);
                View.Property(p => p.EquipAccount).ShowInList(width: 150);
                View.Property(p => p.EquipAccountName);
                View.Property(p => p.WorkGroup);
                View.Property(p => p.WorkOrder);
                View.Property(p => p.ProductCode);
                View.Property(p => p.ProductName);
                View.Property(p => p.Process);
                View.Property(p => p.BarCode);
                View.Property(p => p.LineStop).Readonly();
                View.Property(p => p.AskMaterial).Readonly();
                View.ChildrenProperty(p => p.OperateLogList).HasOrderNo(1).Show(ChildShowInWhere.List);
                View.ChildrenProperty(p => p.ItemDetail).HasOrderNo(2).Show(ChildShowInWhere.List);
                View.AttachDetailChildrenProperty(typeof(AndonManage), (c) =>
                {
                    var andonManage = c.Parent as AndonManage;
                    andonManage = RF.GetById<AndonManage>(andonManage.Id, new EagerLoadOptions().LoadWithViewProperty());
                    return andonManage;
                }, EventViewGroup).HasLabel("事件报告").HasOrderNo(3).Show(ChildShowInWhere.List);
                View.ChildrenProperty(p => p.MessageSendList).HasOrderNo(4).Show(ChildShowInWhere.List);
            }
        }

        /// <summary>
        /// 表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AddBehavior("SIE.Web.Andon.Andons.Behaviors.AndonManageBehavior");
            View.UseCommands(typeof(AndonManageFormSaveCommand).FullName, "SIE.Web.Andon.Andons.Commands.AndonManageHistoryCommand");
            using (View.OrderProperties())
            {
                InfomationDetailView();
                FactoryDetailView();
                ProductDetailView();
                View.ChildrenProperty(p => p.ItemDetail).HasOrderNo(1).Show(ChildShowInWhere.Detail).ViewGroup = AndonManageCallMaterualViewConfig.ShowViewGroup;
            }
        }

        /// <summary>
        /// 异常信息
        /// </summary>
        protected void InfomationDetailView()
        {
            using (View.DeclareGroup("异常信息", 4,true))
            {
                View.Property(p => p.AndonManageCode).Readonly();
                View.Property(p => p.AndonManageClass).Cascade(p => p.AndonTypeId, null);
                View.Property(p => p.AndonType).UseDataSource((e, p, k) =>
                {
                    var source = e as AndonManage;
                    if (source != null)
                    {
                        return RT.Service.Resolve<AndonManageController>().GetAndonTypeEnable(source, p, k);
                    }
                    return new EntityList<AndonType>();
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.AndonTypeName), nameof(e.AndonType.AndonTypeName));
                    m.DicLinkField = keyValues;
                }).Cascade(p => p.AndonId, null);
                View.Property(p => p.Andon).UseDataSource((e, p, k) =>
                {
                    var source = e as AndonManage;
                    if (source != null)
                    {
                        return RT.Service.Resolve<AndonManageController>().GetAndonEnable(source, p, k);
                    }
                    return new EntityList<SIE.Andon.Andons.Andon>();
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.AndonName), nameof(e.Andon.AndonName));
                    keyValues.Add(nameof(e.Priority), nameof(e.Andon.Priority));
                    keyValues.Add(nameof(e.Department), nameof(e.Andon.DepartmentName));
                    keyValues.Add(nameof(e.Solution), nameof(e.Andon.Solution));
                    keyValues.Add(nameof(e.LineStopFlag), nameof(e.Andon.LineStop));
                    keyValues.Add(nameof(e.AskMaterialFlag), nameof(e.Andon.AskMaterial));
                    m.DicLinkField = keyValues;
                });
                View.Property(p => p.Solution).Readonly();
                View.Property(p => p.Department).Readonly();
                View.Property(p => p.Priority).Readonly();
                View.Property(p => p.Defect).UsePagingLookUpGridPopupEditor(p =>
                {
                    p.Model = typeof(Defect).FullName;
                    p.LinkField = AndonManage.DefectIdsProperty.Name;
                    p.DisplayField = Defect.DescriptionProperty.Name;
                    p.XType = "AndonManageIpSpMultiWhComboPopup";
                    p.Editable = false;
                    p.Separator = ",";
                });
                View.Property(p => p.ProblemDesc);
                View.Property(p => p.FaultTime);
                View.Property(p => p.PhotoFile).UseConfigValueEditor(p =>
                {
                    p.XType = "andonuploadimage";
                    p.AllowBlank = true;
                    p.Editable = false;
                });
            }
        }

        /// <summary>
        /// 位置信息
        /// </summary>
        protected void FactoryDetailView()
        {
            using (View.DeclareGroup("位置信息", 4, true))
            {
                View.Property(p => p.Factory).UseFactoryEditor().UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.FactoryName), nameof(e.Factory.Name));
                    m.DicLinkField = keyValues;
                }).Cascade(p => p.WorkShop, null);
                View.Property(p => p.WorkShop).UseDataSource((e, p, k) =>
                {
                    var source = e as AndonManage;
                    if (source != null)
                    {
                        return RT.Service.Resolve<AndonManageController>().GetWorkShops(source, p, k);
                    }
                    return new EntityList<Enterprise>();
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.WorkShopName), nameof(e.WorkShop.Name));
                    m.DicLinkField = keyValues;
                }).Cascade(p => p.WipResource, null);
                View.Property(p => p.WipResource).UseDataSource((e, p, k) =>
                {
                    var source = e as AndonManage;
                    if (source != null)
                    {
                        return RT.Service.Resolve<AndonManageController>().GetWipResources(source, p, k);
                    }
                    return new EntityList<WipResource>();
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.WipResourceName), nameof(e.WipResource.Name));
                    m.DicLinkField = keyValues;
                    m.BindDisplayField = AndonManage.WipResourceNameProperty.Name;
                }).Cascade(p => p.Station, null).Cascade(p => p.WorkOrderId, null);
                View.Property(p => p.Station).UseDataSource((e, p, k) =>
                {
                    var source = e as AndonManage;
                    if (source != null)
                    {
                        return RT.Service.Resolve<AndonManageController>().GetStations(source, p, k);
                    }
                    return new EntityList<Station>();
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.StationName), nameof(e.Station.Name));
                    m.DicLinkField = keyValues;
                });
                View.Property(p => p.EquipAccount).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.EquipAccountCode), nameof(e.EquipAccount.Code));
                    keyValues.Add(nameof(e.EquipAccountName), nameof(e.EquipAccount.Name));
                    m.DicLinkField = keyValues;
                });
                View.Property(p => p.EquipAccountName).Readonly();
                View.Property(p => p.WorkGroup).Readonly();
            }
        }

        /// <summary>
        /// 生产信息
        /// </summary>
        protected void ProductDetailView()
        {
            using (View.DeclareGroup("生产信息", 4, true))
            {
                View.Property(p => p.WorkOrder).UseDataSource((e, p, k) =>
                {
                    var source = e as AndonManage;
                    if (source != null)
                    {
                        return RT.Service.Resolve<AndonManageController>().GetWorkOrdersByWip(source, p, k);
                    }
                    return new EntityList<WorkOrder>();
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.WoNo), nameof(e.WorkOrder.No));
                    keyValues.Add(nameof(e.ProductCode), nameof(e.WorkOrder.Product.Code));
                    keyValues.Add(nameof(e.ProductName), nameof(e.WorkOrder.Product.Name));
                    m.DicLinkField = keyValues;
                });
                View.Property(p => p.Process).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.ProcessName), nameof(e.Process.Name));
                    m.DicLinkField = keyValues;
                });
                View.Property(p => p.BarCode);
                View.Property(p => p.LineStop)
                    .Readonly(p => p.LineStopFlag != SIE.Andon.Andons.Enum.AndonYesOrNo.Artificial);
                View.Property(p => p.AskMaterial)
                    .Readonly(p => p.AskMaterialFlag != SIE.Andon.Andons.Enum.AndonYesOrNo.Artificial);
            }
        }

        /// <summary>
        /// 附加表单事件报告
        /// </summary>
        protected void EventView()
        {
            View.FormEdit();
            View.ClearCommands();
            View.UseCommands(typeof(AndonManageAddExpCommand).FullName, typeof(AndonManageAttachmentCommand).FullName, typeof(AndonManageEventFormSaveCommand).FullName
                , "SIE.Web.Andon.Andons.Commands.AndonManageAttachmentClearCommand");
            View.HasDetailColumnsCount(2);
            using (View.OrderProperties())
            {
                View.Property(p => p.Solution).Readonly(p => p.State == SIE.Andon.Andons.Enum.AndonManageState.Cancel).ShowInDetail(columnSpan: 2);
                View.Property(p => p.Reason).Readonly(p => p.State == SIE.Andon.Andons.Enum.AndonManageState.Cancel).ShowInDetail(columnSpan: 2);
                View.Property(p => p.HandleMethod).Readonly(p => p.State == SIE.Andon.Andons.Enum.AndonManageState.Cancel).ShowInDetail(columnSpan: 2);
                View.Property(p => p.Measures).Readonly(p => p.State == SIE.Andon.Andons.Enum.AndonManageState.Cancel).ShowInDetail(columnSpan: 2);
                View.Property(p => p.Attachment).Readonly(p => p.State == SIE.Andon.Andons.Enum.AndonManageState.Cancel)
                    .UseConfigValueEditor(p =>
                {
                    p.XType = "andonuploadattachment";
                    p.AllowBlank = true;
                    p.Editable = false;
                }).ShowInDetail(columnSpan: 2);
            }
        }

        /// <summary>
        /// 附加表单事件报告查看视图
        /// </summary>
        protected void LookUpEventView()
        {
            View.ClearCommands();
            View.HasDetailColumnsCount(2);
            using (View.OrderProperties())
            {
                View.Property(p => p.Solution).ShowInDetail(columnSpan: 2).Readonly();
                View.Property(p => p.Reason).ShowInDetail(columnSpan: 2).Readonly();
                View.Property(p => p.HandleMethod).ShowInDetail(columnSpan: 2).Readonly();
                View.Property(p => p.Measures).ShowInDetail(columnSpan: 2).Readonly();
                View.Property(p => p.Attachment).ShowInDetail(columnSpan: 2).Readonly();
            }
        }

        /// <summary>
        /// 查看按钮视图
        /// </summary>
        protected void LookUpView()
        {
            View.DisableEditing();
            View.UseCommands(typeof(AndonManageCancelCommand).FullName, typeof(AndonManageResponseCommand).FullName, typeof(AndonManageReassignmentCommand).FullName
                , typeof(AndonManageHandleCommand).FullName, typeof(AndonManageCheckCommand).FullName, typeof(AndonManageRejectCommand).FullName);
            View.UseCommands("SIE.Web.Andon.Andons.Commands.AndonManageGoExpCommand", "SIE.Web.Andon.Andons.Commands.AndonManageImageCommand");
            using (View.OrderProperties())
            {
                using (View.DeclareGroup("异常信息", 4))
                {
                    View.Property(p => p.AndonManageCode).Readonly();
                    View.Property(p => p.AndonManageClass);
                    View.Property(p => p.AndonType);
                    View.Property(p => p.Andon);
                    View.Property(p => p.ProblemDesc);
                    View.Property(p => p.State);
                    View.Property(p => p.Priority);
                    View.Property(p => p.Defect);
                    View.Property(p => p.Trigger);
                    View.Property(p => p.TriggerTime);
                    View.Property(p => p.CloseTime);
                    View.Property(p => p.LastTime);
                    View.Property(p => p.Department);
                    View.Property(p => p.Handler);
                }
                using (View.DeclareGroup("位置信息", 4))
                {
                    View.Property(p => p.Factory);
                    View.Property(p => p.WorkShop);
                    View.Property(p => p.WipResource);
                    View.Property(p => p.Station);
                    View.Property(p => p.EquipAccount);
                    View.Property(p => p.EquipAccountName);
                    View.Property(p => p.WorkGroup);
                }
                using (View.DeclareGroup("生产信息", 4))
                {
                    View.Property(p => p.WorkOrder);
                    View.Property(p => p.Process);
                    View.Property(p => p.BarCode);
                    View.Property(p => p.LineStop);
                    View.Property(p => p.AskMaterial);
                }
                View.ChildrenProperty(p => p.OperateLogList).UseViewGroup(AndonManageOperateLogViewConfig.LookUpViewGroup).HasOrderNo(1).Show(ChildShowInWhere.Detail);
                View.ChildrenProperty(p => p.ItemDetail).UseViewGroup(AndonManageCallMaterualViewConfig.LookupViewGroup).HasOrderNo(2).Show(ChildShowInWhere.Detail);
                View.AttachDetailChildrenProperty(typeof(AndonManage), (c) =>
                {
                    var andonManage = c.Parent as AndonManage;
                    andonManage = RF.GetById<AndonManage>(andonManage.Id, new EagerLoadOptions().LoadWithViewProperty());
                    return andonManage;
                }, AndonManageViewConfig.LookUpEventViewGroup).HasLabel("事件报告").HasOrderNo(3).Show(ChildShowInWhere.Detail);
                View.ChildrenProperty(p => p.MessageSendList).UseViewGroup(AndonManageMessageSendViewConfig.LookUpViewGroup).HasOrderNo(4).Show(ChildShowInWhere.Detail);
            }
        }

        /// <summary>
        /// 转派视图
        /// </summary>
        protected void ReassignmentView()
        {
            View.HasDetailColumnsCount(2);
            using (View.OrderProperties())
            {
                View.Property(p => p.Andon).UseDataSource((e, p, k) =>
                {
                    var source = e as AndonManage;
                    if (source != null)
                    {
                        return RT.Service.Resolve<AndonManageController>().GetAndonEnable(source, p, k);
                    }
                    return new EntityList<SIE.Andon.Andons.Andon>();
                }).ShowInDetail(columnSpan: 2, width: "200");
                View.Property(p => p.Handler).ShowInDetail(columnSpan: 2, width: "200");
            }
        }
    }
}
