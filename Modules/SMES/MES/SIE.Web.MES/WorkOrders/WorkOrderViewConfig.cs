using SIE;
using SIE.Core.ProjectMaintains;
using SIE.CSM.Customers;
using SIE.Domain;
using SIE.Items;
using SIE.ManagedProperty;
using SIE.MES.WIP.Reworks;
using SIE.MES.WorkOrders;
using SIE.MetaModel.View;
using SIE.Resources.Enterprises;
using SIE.Resources.ProcessSegments;
using SIE.Resources.ProcessTechs;
using SIE.Resources.WipResources;
using SIE.Utils;
using SIE.Web.Items._Extentions_;
using SIE.Web.Items.ViewModels;
using SIE.Web.MES.WorkOrders.Commands;
using SIE.Web.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SIE.Web.MES.WorkOrders
{
    /// <summary>
    /// 工单视图配置
    /// </summary> 
    [CompiledPropertyDeclarer]
    public class WorkOrderViewConfig : WebViewConfig<WorkOrder>
    {
        /// <summary>
        /// 编辑视图ViewGroup
        /// </summary>
        public const string EditView = "EditView";

        /// <summary>
        /// 查看工单视图ViewGroup
        /// </summary>
        public const string ReadonlyView = "ReadonlyView";

        /// <summary>
        /// 工单BOM字符串
        /// </summary>
        private readonly string _bomListLabel = "工单BOM".L10N();

        /// <summary>
        /// 工序BOM字符串
        /// </summary>
        private readonly string _processBomListLabel = "工序BOM".L10N();

        /// <summary>
        /// 帮助信息
        /// </summary>
        private readonly string _woSourceHelpInfo = "工单来源为外部或工单状态是生产中不可编辑".L10N();

        #region 类型名称 TypeName
        /// <summary>
        /// 类型名称
        /// </summary>
        public static readonly Property<string> TypeNameProperty = P<WorkOrder>.RegisterExtensionReadOnly("TypeName", typeof(WorkOrderViewConfig),
            GetTypeName, WorkOrder.TypeProperty);

        /// <summary>
        /// 类型名称
        /// </summary>
        /// <param name="me">WorkOrder</param>
        /// <returns>string</returns>
        public static string GetTypeName(WorkOrder me)
        {
            return me.Type.ToLabel();
        }
        #endregion

        #region 显示状态 DisplayState
        /// <summary>
        /// 显示状态  用一个字段显示工单状态和排产状态，工单状态有值时显示工单状态，排产状态有值时显示排产状态
        /// </summary>
        public static readonly Property<string> DisplayStateProperty = P<WorkOrder>.RegisterExtensionReadOnly("DisplayState", typeof(WorkOrderViewConfig),
            GetDisplayState, WorkOrder.StateProperty, WorkOrder.StateProperty);

        /// <summary>
        /// 显示状态
        /// </summary>
        /// <param name="me">工单</param>
        /// <returns>工单状态/排产状态</returns>
        public static string GetDisplayState(WorkOrder me)
        {
            if (me.IsPause == YesNo.Yes && (me.State == SIE.Core.WorkOrders.WorkOrderState.Release || me.State == SIE.Core.WorkOrders.WorkOrderState.Producing))
                return EnumViewModel.EnumToLabel(me.State).L10N() + "暂停".L10N();
            else
            {
                return EnumViewModel.EnumToLabel(me.State).L10N();
            }
        }
        #endregion

        #region 根据工单来源设置是否只读 IsReadonly
        /// <summary>
        /// 自建工单可以修改主数据，外部来源工单不可修改主数据
        /// </summary>    
        public static Expression<Func<WorkOrder, bool>> IsReadonlyProperty { get; }
            = e => e.Source == SIE.Common.SourceType.External || e.State == SIE.Core.WorkOrders.WorkOrderState.Producing;

        #endregion

        #region 暂停状态下车间资源可编辑 ResourceReadonly
        /// <summary>
        /// 自建工单可以修改主数据，外部来源工单不可修改主数据
        /// </summary>       
        public static Expression<Func<WorkOrder, bool>> ResourceReadonlyProperty { get; } = e => e.State == SIE.Core.WorkOrders.WorkOrderState.Producing && e.IsPause == YesNo.No;
        #endregion

        #region 暂停状态下工艺路线可编辑 RoutingReadonly
        /// <summary>
        /// 自建工单可以修改主数据，外部来源工单不可修改主数据
        /// </summary>       
        public static Expression<Func<WorkOrder, bool>> RoutingReadonlyProperty { get; } = e => e.State != SIE.Core.WorkOrders.WorkOrderState.Release || e.IsPause == YesNo.No;
        #endregion

        #region 使用旧条码只读属性 UseOldSnReadonly
        /// <summary>
        /// 使用旧条码只读属性
        /// </summary>
        public static Expression<Func<WorkOrder, bool>> UseOldSnReadonlyProperty { get; } = e => GetUseOldSnReadonly(e);

        /// <summary>
        /// 使用旧条码只读属性
        /// </summary>
        /// <param name="me">me</param>
        /// <returns>bool</returns>
        public static bool GetUseOldSnReadonly(WorkOrder me)
        {
            bool unionExis = RT.Service.Resolve<ReworkController>().CheckUnionBarcodeExist(me.No);
            return me.PrintedQty > 0 || unionExis;
        }
        #endregion

        #region 使用旧条码显示属性 UseOldSnVisible
        /// <summary>
        /// 使用旧条码显示属性
        /// </summary>    
        public static Expression<Func<WorkOrder, bool>> UseOldSnVisibleProperty { get; } = e => e.Type == SIE.Core.WorkOrders.WorkOrderType.Rework;
        #endregion

        #region 工艺路线版本 RoutingVersion
        /// <summary>
        /// 工艺路线版本 显示工艺路线名称+版本
        /// </summary>
        public static readonly Property<string> RoutingVersionProperty = P<WorkOrder>.RegisterExtensionReadOnly("RoutingVersion", typeof(WorkOrderViewConfig),
            GetRoutingVersion, WorkOrder.VersionIdProperty);

        /// <summary>
        /// 工艺路线版本
        /// </summary>
        /// <param name="me">工单</param>
        /// <returns>工艺路线版本 显示工艺路线名称+版本</returns>
        public static string GetRoutingVersion(WorkOrder me)
        {
            if (me != null && me.VersionId.HasValue && me.Version != null && me.Version.Routing != null)
            {
                return me.Version.Routing.Name + "(" + me.Version.Name + ")";
            }
            return "";
        }
        #endregion

        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(new string[] { EditView, ReadonlyView });
            switch (ViewGroup)
            {
                case EditView:
                    EditConfigView();
                    break;
                case ReadonlyView:
                    ConfigReadonlyView();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 查看工单视图
        /// </summary>
        void ConfigReadonlyView()
        {
            View.AddBehavior("SIE.Web.MES.WorkOrders.EditReadWorkOrderBehavior");
            View.ClearCommands();
            View.UseDetail(4);
            View.Property(p => p.No).Show(ShowInWhere.Detail).Readonly().HasOrderNo(10);
            View.Property(p => p.Factory).Show(ShowInWhere.Detail).Readonly().HasOrderNo(20);
            View.Property(p => p.CustomerName).Show(ShowInWhere.Detail).Readonly().HasOrderNo(30);
            View.Property(p => p.CustomerOrderNo).Show(ShowInWhere.Detail).Readonly().HasOrderNo(40);
            View.Property(p => p.ErpWorkOrderNo).ShowInDetail().Readonly().HasOrderNo(45);
            View.Property(p => p.SaleOrderNo).Show(ShowInWhere.Detail).Readonly().HasOrderNo(50);
            View.Property(TypeNameProperty).Show(ShowInWhere.Detail).Readonly().HasLabel("类型").HasOrderNo(60);
            //View.Property(p => p.WorkShopCode).Show(ShowInWhere.Detail).Readonly().HasOrderNo(70);
            View.Property(p => p.WorkShopName).Show(ShowInWhere.Detail).Readonly().HasOrderNo(70);
            View.Property(p => p.ResourceName).Show(ShowInWhere.Detail).Readonly().HasOrderNo(80);
            View.Property(p => p.ProjectMaintainCode).Show(ShowInWhere.Detail).Readonly().HasOrderNo(85);
            View.Property(p => p.WorkOrderProductCode).Show(ShowInWhere.Detail).Readonly().HasOrderNo(90);
            View.Property(P => P.WorkOrderProductName).Show(ShowInWhere.Detail).Readonly().HasOrderNo(100);
            View.Property(P => P.PlanQty).UseSpinEditor(p => p.AllowDecimals = false).Show(ShowInWhere.Detail).Readonly().HasOrderNo(110);
            View.Property(P => P.OrderQty).UseSpinEditor(p => p.AllowDecimals = false).Show(ShowInWhere.Detail).Readonly().HasOrderNo(120);
            View.Property(P => P.ProcessTech).Show(ShowInWhere.Detail).Readonly().HasOrderNo(122);
            View.Property(P => P.ProcessSegmentName).Show(ShowInWhere.Detail).Readonly().HasOrderNo(124);
            View.Property(P => P.FinishQty).UseSpinEditor(p => p.AllowDecimals = false).Show(ShowInWhere.Detail).Readonly().HasOrderNo(130);
            View.Property(p => p.PlanBeginDate).Show(ShowInWhere.Detail).Readonly().HasOrderNo(140);
            View.Property(p => p.PlanEndDate).Show(ShowInWhere.Detail).Readonly().HasOrderNo(150);
            View.Property(RoutingVersionProperty).HasLabel("工艺路线版本").Show(ShowInWhere.Detail).Readonly().HasOrderNo(160);
            View.Property(p => p.UseOldSn).UseCheckEditor().Show(ShowInWhere.Detail).Readonly().Visibility(UseOldSnVisibleProperty)
                .UseListSetting(e => { e.HelpInfo = "工单类型为返工可见"; }).HasOrderNo(170);
            View.Property(p => p.PanelWorkOrder).Readonly().HasLabel("组合板工单号").Show(ShowInWhere.Detail).HasOrderNo(171);
            View.Property(p => p.PanelQty).Show(ShowInWhere.Detail).Readonly().HasOrderNo(180);
            View.Property(p => p.ItemExtPropName).UseItemExtPropRecordsFieldEditor(p =>
                   {
                       p.IsAllRequired = true;
                       p.SourceEntityType = "WorkOrder";
                       p.ItemIdField = "ProductId";
                       p.DbField = "ItemExtProp";
                   }).Readonly().HasOrderNo(105).Show(ShowInWhere.Detail);
            View.ChildrenProperty(p => p.WorkOrderOutputProductList).Show(ChildShowInWhere.Detail).UseViewGroup(WorkOrderOutputProductViewConfig.ReadonlyView).HasOrderNo(19);
            View.ChildrenProperty(p => p.BomList).Show(ChildShowInWhere.Detail).HasLabel(_bomListLabel).Readonly().UseViewGroup(ReadonlyView).HasOrderNo(20);
            View.ChildrenProperty(p => p.ProcessBomList).Show(ChildShowInWhere.Detail).HasLabel(_processBomListLabel).Readonly().UseViewGroup(WorkOrderViewConfig.ReadonlyView).HasOrderNo(30);
            View.ChildrenProperty(p => p.RoutingProcessList).Show(ChildShowInWhere.Hide).HasLabel("工序清单").Readonly().HasOrderNo(40);
            View.ChildrenProperty(p => p.PackageRuleDetailList).Show(ChildShowInWhere.Detail).HasLabel("包装规则").Readonly().UseViewGroup(WorkOrderViewConfig.ReadonlyView).HasOrderNo(50);
            View.ChildrenProperty(p => p.WorkOrderLogList).Show(ChildShowInWhere.Detail).HasLabel("工单日志").Readonly().HasOrderNo(60);

            View.AttachDetailChildrenProperty(typeof(SIE.Core.Items.LabelPrintTemplate), e =>
            {
                var workOrder = e.Parent as WorkOrder;
                if (workOrder == null)
                {
                    var template = new SIE.Core.Items.LabelPrintTemplate();
                    return template;
                }

                return RT.Service.Resolve<WorkOrderController>().GetLabelPrintTemplateOfWorkOrder(workOrder.Id);
            }, ProductLabelTemplateViewConfig.ReadOnlyView).HasLabel("打印设置").Show(ChildShowInWhere.All).HasOrderNo(70).LazyLoad(false);
            View.AssociateChildrenProperty(WoWipBatchExt.AttacWoWipBatchProperty, (e) =>
            {
                var wo = e.Parent as WorkOrder;
                if (wo == null) return new SIE.Core.WorkOrders.WoWipBatch();
                var batch = RT.Service.Resolve<SIE.Core.WorkOrders.WorkOrderController>().GetWipBatch(wo.Id);
                if (batch == null)
                {
                    batch = new SIE.Core.WorkOrders.WoWipBatch();
                    batch.GenerateId();
                    batch.Qty = 1;
                }

                return batch;
            }, WoWipBatchViewConfig.ReadonlyView).HasLabel("生产批次").Show(ChildShowInWhere.Detail).HasOrderNo(80).LazyLoad(false);
            View.ChildrenProperty(p => p.LayoutInfoList).Show(ChildShowInWhere.Detail).HasOrderNo(90);
        }

        /// <summary>
        /// 修改工单视图
        /// </summary>
        void EditConfigView()
        {
            View.ClearCommands();
            View.AddBehavior("SIE.Web.MES.WorkOrders.EditReadWorkOrderBehavior");
            View.UseCommands("SIE.Web.MES.WorkOrders.SaveWorkOrderCommand");
            View.UseDetail(4);

            View.Property(p => p.No).Show(ShowInWhere.Detail).Readonly().HasOrderNo(10);
            View.Property(p => p.Factory).Show(ShowInWhere.Detail).UseFactoryEditor().Readonly(IsReadonlyProperty)
                .UseListSetting(e => { e.HelpInfo = _woSourceHelpInfo; }).HasOrderNo(20);
            View.Property(p => p.CustomerId).Show(ShowInWhere.Detail).Readonly(IsReadonlyProperty)
                .UseListSetting(e => { e.HelpInfo = _woSourceHelpInfo; }).HasOrderNo(30);
            View.Property(p => p.CustomerOrderNo).Show(ShowInWhere.Detail).Readonly(IsReadonlyProperty)
                .UseListSetting(e => { e.HelpInfo = _woSourceHelpInfo; }).HasOrderNo(40);
            View.Property(p => p.ErpWorkOrder).ShowInDetail().UsePagingLookUpEditor().Readonly(IsReadonlyProperty)
                .UseListSetting(e => { e.HelpInfo = _woSourceHelpInfo; }).HasOrderNo(45);
            View.Property(p => p.SaleOrderNo).Show(ShowInWhere.Detail).Readonly(IsReadonlyProperty)
                .UseListSetting(e => { e.HelpInfo = _woSourceHelpInfo; }).HasOrderNo(50);
            View.Property(p => p.Type).Show(ShowInWhere.Detail).Readonly(IsReadonlyProperty)
                .UseListSetting(e => { e.HelpInfo = _woSourceHelpInfo; }).HasOrderNo(60);
            View.Property(p => p.WorkShop).Show(ShowInWhere.Detail).Readonly(ResourceReadonlyProperty)
                .UseDataSource((source, pagingInfo, keyword) =>
                {
                    var entity = source as WorkOrder;
                    double? factoryId = entity?.FactoryId;
                    var workshop = RT.Service.Resolve<EnterpriseController>().GetResourceWorkShops(pagingInfo, keyword, factoryId);
                    if (workshop == null || workshop.Count <= 0)
                        return new EntityList<Enterprise>();
                    workshop.ForEach(p => p.TreePId = null);
                    return workshop;
                })
                .UseListSetting(e => { e.HelpInfo = "工单来源为外部或工单状态是生产中或工单为暂停都不可编辑"; }).HasOrderNo(70)
                .Cascade(p => p.ResourceId, null);
            View.Property(p => p.Resource).Show(ShowInWhere.Detail).Readonly(ResourceReadonlyProperty).UseDataSource((e, c, r) =>
            {
                var workOrder = e as WorkOrder;
                if (workOrder == null || workOrder.WorkShop == null)
                    return new EntityList<WipResource>();

                var stateList = new List<ResourceState>() { ResourceState.Actived, ResourceState.Stop, ResourceState.Unused };
                var sourceType = new List<SyncSourceType>() { SyncSourceType.Enterprise, SyncSourceType.Equipment };
                return RT.Service.Resolve<WipResourceController>().GetWipResources(stateList, workOrder.WorkShopId.Value, sourceType, c, r);
            }).UsePagingLookUpEditor(p => p.DisplayField = "Name")
            .UseListSetting(e => { e.HelpInfo = "显示当前车间启用状态不失效的生产资源,工单来源为外部或工单状态是生产中或工单为暂停都不可编辑"; }).HasOrderNo(80);
            View.Property(p => p.ProjectMaintain).UseDataSource((e, p, k) =>
            {
                return RT.Service.Resolve<ProjectMaintainController>().GetProjectMaintains(p, k);
            }).Show(ShowInWhere.Detail).HasOrderNo(85);
            View.Property(p => p.Product).UsePagingLookUpEditor().Show(ShowInWhere.Detail).Readonly().HasOrderNo(90);
            View.Property(P => P.WorkOrderProductName).Show(ShowInWhere.Detail).Readonly().HasLabel("产品名称").HasOrderNo(100);
            View.Property(P => P.PlanQty).UseItemUnitEditor(p => { p.ItemIdField = "ProductId"; }).Show(ShowInWhere.Detail).Readonly(IsReadonlyProperty)
                .UseListSetting(e => { e.HelpInfo = _woSourceHelpInfo; }).HasOrderNo(120);
            View.Property(P => P.ProcessSegmentId).Cascade(p => p.ProcessTechId, null).Show(ShowInWhere.Detail).Readonly(IsReadonlyProperty).HasOrderNo(122);
            View.Property(P => P.ProcessTechId).UseDataSource((e, c, r) =>
            {
                var workOrder = e as WorkOrder;
                if (workOrder == null || workOrder.ProcessSegmentId == null)
                    return new EntityList<ProcessTech>();
                return RT.Service.Resolve<ProcessTechBaseController>().GetProcessTechsBySegment(workOrder.ProcessSegmentId.Value, c, r);
            }).Show(ShowInWhere.Detail).Readonly(IsReadonlyProperty).HasOrderNo(124);
            View.Property(P => P.OrderQty).UseSpinEditor(p => p.AllowDecimals = false).Show(ShowInWhere.Detail).Readonly(IsReadonlyProperty)
                .UseListSetting(e => { e.HelpInfo = _woSourceHelpInfo; }).HasOrderNo(130);
            View.Property(P => P.FinishQty).UseSpinEditor(p => p.AllowDecimals = false).Show(ShowInWhere.Detail).Readonly(true).HasOrderNo(130);
            View.Property(p => p.PlanBeginDate).Show(ShowInWhere.Detail).Readonly(IsReadonlyProperty)
                .UseListSetting(e => { e.HelpInfo = _woSourceHelpInfo; }).HasOrderNo(140);
            View.Property(p => p.PlanEndDate).Show(ShowInWhere.Detail).Readonly(IsReadonlyProperty)
                .UseListSetting(e => { e.HelpInfo = _woSourceHelpInfo; }).HasOrderNo(150);
            //View.Property(p => p.VersionId)
            //    .UsePagingLookUpEditor()
            //    .Show(ShowInWhere.Detail).Readonly(RoutingReadonlyProperty)
            //.UseListSetting(e => { e.HelpInfo = _woSourceHelpInfo; }).HasOrderNo(160);
            View.Property(p => p.Version).UsePagingLookUpOldEditor(p =>
            { p.DataSourceProperty = "true"; p.XType = "WorkOrderRoutingVersion"; }).UseListSetting(e => { e.HelpInfo = _woSourceHelpInfo; })
                .Show(ShowInWhere.Detail).Readonly(RoutingReadonlyProperty).HasOrderNo(160);
            View.Property(p => p.UseOldSn).UseCheckEditor().Show(ShowInWhere.Detail).Visibility(UseOldSnVisibleProperty)
                .UseListSetting(e => { e.HelpInfo = "工单类型为返工可见"; }).HasOrderNo(170);
            View.Property(p => p.PanelWorkOrder).UsePagingLookUpEditor().HasLabel("组合板工单号").Show(ShowInWhere.Detail).HasOrderNo(161).Readonly(p => p.Source == SIE.Common.SourceType.External);
            View.Property(p => p.PanelQty).Show(ShowInWhere.Detail).HasOrderNo(180);
            View.Property(p => p.ItemExtPropName).UseItemExtPropRecordsFieldEditor(p =>
            {
                p.IsAllRequired = true;
                p.SourceEntityType = "WorkOrder";
                p.ItemIdField = "ProductId";
                p.DbField = "ItemExtProp";
            }).Readonly().HasOrderNo(105).Show(ShowInWhere.Detail);
            EditConfigViewChildren();
        }

        /// <summary>
        /// 设置EditConfigView子视图属性
        /// </summary>
        private void EditConfigViewChildren()
        {
            View.ChildrenProperty(p => p.WorkOrderOutputProductList).Show(ChildShowInWhere.All).UseViewGroup(WorkOrderOutputProductViewConfig.EditView).HasOrderNo(19);
            View.ChildrenProperty(p => p.BomList).Show(ChildShowInWhere.Detail).HasLabel(_bomListLabel).HasOrderNo(20).LazyLoad(false);
            View.ChildrenProperty(p => p.ProcessBomList).Show(ChildShowInWhere.Detail).HasLabel(_processBomListLabel).UseViewGroup(WorkOrderProcessBomViewConfig.WorkOrderProcessBomView).HasOrderNo(30).LazyLoad(false);
            View.ChildrenProperty(p => p.PackageRuleDetailList).Show(ChildShowInWhere.Detail).HasLabel("包装规则").UseViewGroup(WorkOrderPackageRuleDetailViewConfig.ListView).HasOrderNo(40).LazyLoad(false);
            View.ChildrenProperty(p => p.WorkOrderLogList).Show(ChildShowInWhere.Detail).HasLabel("工单日志").HasOrderNo(50).LazyLoad(false);

            View.AttachDetailChildrenProperty(typeof(SIE.Core.Items.LabelPrintTemplate), e =>
            {
                var workOrder = e.Parent as WorkOrder;
                if (workOrder == null)
                {
                    var template = new SIE.Core.Items.LabelPrintTemplate();
                    return template;
                }

                return RT.Service.Resolve<WorkOrderController>().GetLabelPrintTemplateOfWorkOrder(workOrder.Id);
            }, ProductLabelTemplateViewConfig.WorkderView).HasLabel("打印设置")
            .HasOrderNo(60)
                .Show(ChildShowInWhere.All).LazyLoad(false);

            //View.AssociateChildrenProperty(WOLabelPrintDetailProperty.LabelPrintTemProperty, e =>
            //{
            //    var workOrder = e.Parent as WorkOrder;
            //    Check.NotNull(workOrder, "工单为空".L10N());
            //    var template = RT.Service.Resolve<SIE.Core.WorkOrders.WorkOrderController>().GetTemplate(workOrder.Id);
            //    if (template == null)
            //    {
            //        template = new SIE.Core.Items.LabelPrintTemplate();
            //        template.GenerateId();
            //        workOrder.Template = template;
            //        return template;
            //    }
            //    workOrder.Template = template;
            //    return template;
            //}, ProductLabelTemplateViewConfig.WorkderView)
            //    .HasLabel("打印设置")
            //    .Show(ChildShowInWhere.All)
            //    .HasOrderNo(60).LazyLoad(false);

            View.AssociateChildrenProperty(WoWipBatchExt.AttacWoWipBatchProperty, (e) =>
            {
                var wo = e.Parent as WorkOrder;
                if (wo == null) return new SIE.Core.WorkOrders.WoWipBatch();
                var batch = RT.Service.Resolve<SIE.Core.WorkOrders.WorkOrderController>().GetWipBatch(wo.Id);
                if (batch == null)
                {
                    batch = new SIE.Core.WorkOrders.WoWipBatch();
                    batch.GenerateId();
                    batch.Qty = 1;
                }

                return batch;
            }, DetailsView)
                .HasLabel("生产批次")
                .Show(ChildShowInWhere.Detail)
                .HasOrderNo(70)
                .LazyLoad(false);

            //用于BS暂存工序清单数据，前端JS会隐藏这个页签
            View.ChildrenProperty(p => p.RoutingProcessList).Show(ChildShowInWhere.Detail).HasLabel("工序清单").HasOrderNo(999).LazyLoad(false);
            View.ChildrenProperty(p => p.LayoutInfoList).Show(ChildShowInWhere.All).HasOrderNo(1000).UseViewGroup("EditListView");
        }

        /// <summary>
        /// 默认工单视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.FormEdit();
            View.RequirModuleResource("SIE.Web.MES.WorkOrders.Scripts.WorkOrderBehavior.js");
            View.AddBehavior("SIE.Web.MES.WorkOrders.WorkOrderBehavior");
            View.UseCommands("SIE.Web.MES.WorkOrders.WorkOrderResumeCommand", "SIE.Web.MES.WorkOrders.WorkOrderPauseCommand", "SIE.Web.MES.WorkOrders.WorkOrderCloseCommand", "SIE.Web.MES.WorkOrders.WorkOrderAdvanceCommand");
            View.UseCommands(AddWorkOrderCommand.FullName, "SIE.Web.MES.WorkOrders.EditWorkOrderCommand", CopyWorkOrderCommand.FullName, "SIE.Web.MES.WorkOrders.Commands.UpdateRoutingCommand");
            View.UseCommands("SIE.Web.MES.WorkOrders.ReadonlyWorkOrderCommand", "SIE.Web.MES.WorkOrders.WorkOrderImportCommand", "SIE.Web.MES.WorkOrders.UnionBarcodeCommand");
            View.UseCommands(typeof(WorkOrderCompletionCommand).FullName, typeof(GeneraDispatchTaskCommand).FullName);
            //View.UseCommands(typeof(OneKeyToPrepareCommand).FullName); BarcodeDownLoadCommand.FullName,
            View.UseClientOrder();
            View.UseCommands(WebCommandNames.ExportXls);
            using (View.OrderProperties())
            {
                View.Property(p => p.No).FixColumn().ShowInList(130).UseListSetting(e => { e.HelpInfo = "根据工单编号配置项自动生成"; }).HasOrderNo(10);
                View.Property(p => p.WorkOrderProductCode).FixColumn().ShowInList(120).HasLabel("产品编码").HasOrderNo(20);
                View.Property(p => p.WorkOrderProductName).ShowInList(120).HasLabel("产品名称").HasOrderNo(30);
                View.Property(p => p.PlanQty).UseSpinEditor(p => p.AllowDecimals = true).HasOrderNo(40);
                View.Property(p => p.FinishQty).UseSpinEditor(p => p.AllowDecimals = true).HasOrderNo(50);
                View.Property(p => p.ScrapQty).UseSpinEditor(p => p.AllowDecimals = true).HasOrderNo(51);
                View.Property(p => p.Ztfl).UseSpinEditor(p => p.AllowDecimals = true).HasOrderNo(52);
                View.Property(p => p.WorkOrderMpType).HasOrderNo(53);
                View.Property(p => p.ProjectMaintain).HasOrderNo(54);
                View.Property(P => P.ProcessTech).HasOrderNo(62);
                View.Property(P => P.ProcessSegmentName).HasOrderNo(64);
                View.Property(p => p.Type).HasOrderNo(70);
                View.Property(p => p.ProductShortDescription).HasOrderNo(75);
                View.Property(p => p.State).Show(ShowInWhere.Hide).HasOrderNo(80);
                View.Property(DisplayStateProperty).HasLabel("工单状态").HasOrderNo(90);
                View.Property(p => p.KitType).Show(ShowInWhere.Hide).HasOrderNo(100);
                View.Property(p => p.IsPause).Show(ShowInWhere.Hide).HasOrderNo(110);
                View.Property(p => p.PlanBeginDate).ShowInList(150).HasOrderNo(120);
                View.Property(p => p.PlanEndDate).ShowInList(150).HasOrderNo(130);
                View.Property(p => p.ActuStartDate).ShowInList(150).HasOrderNo(140);
                View.Property(p => p.ActuFinishDate).ShowInList(150).HasOrderNo(150);
                View.Property(p => p.CreateDate).HasOrderNo(151);
                View.Property(p => p.UpdateDate).HasOrderNo(152);
                View.Property(p => p.WorkShopCode).HasOrderNo(70);
                View.Property(p => p.WorkShopName).HasOrderNo(160);
                View.Property(p => p.ResourceName).HasOrderNo(170);
                View.Property(RoutingVersionProperty).HasLabel("工艺路线版本").ShowInList(150).HasOrderNo(180);
                View.Property(p => p.PanelQty).HasOrderNo(185);
                View.Property(p => p.PanelWorkOrderNo).Readonly().HasOrderNo(185);
                View.Property(p => p.IsPanelWorkOrder).Readonly().HasOrderNo(185);
                View.Property(p => p.MakerName).HasOrderNo(190);
                View.Property(p => p.MakeDate).ShowInList(150).HasOrderNo(200);
                View.Property(p => p.Source).HasOrderNo(210);
                View.Property(p => p.ParentId).HasOrderNo(220);
                View.Property(p => p.ErpWorkOrderNo).HasOrderNo(230);
                View.Property(p => p.CustomerName).HasOrderNo(240);
                View.Property(p => p.CustomerOrderNo).HasOrderNo(250);
                View.Property(p => p.SaleOrderNo).HasOrderNo(260);
                View.Property(p => p.IsCommonMode).Readonly().HasOrderNo(270);
                View.Property(p => p.IsMainMaterial).Readonly().HasOrderNo(280);
                View.Property(p => p.PlanNo).Readonly().HasOrderNo(290);
                View.Property(p => p.ProcessTechOrderCode).Readonly().HasOrderNo(300);
                View.Property(p => p.OrderQty).HasOrderNo(310);
                View.Property(p => p.Level).Show(ShowInWhere.Hide).HasOrderNo(320);
                View.Property(p => p.ProductionOrderCode).Readonly().HasOrderNo(325);
                View.Property(p => p.Factory).Show(ShowInWhere.All).HasOrderNo(330);
                View.Property(p => p.ItemExtPropName);
                View.Property(p => p.BatchNo);
                View.Property(p => p.Uebto);
                View.Property(p => p.Fevor);
                View.Property(p => p.Lgort);
                View.Property(p => p.Remark);
                View.Property(p => p.OrderNo);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).ShowInList(150);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).ShowInList(150);
                View.ChildrenProperty(p => p.WorkOrderLogList).Show(ChildShowInWhere.Hide).HasLabel("工单日志").HasOrderNo(20);
                View.ChildrenProperty(p => p.PackageRuleDetailList).Show(ChildShowInWhere.Hide).HasLabel("包装规则").HasOrderNo(30);
                View.ChildrenProperty(p => p.BomList).Show(ChildShowInWhere.Hide).HasLabel(_bomListLabel).HasOrderNo(40);
                View.ChildrenProperty(p => p.ProcessBomList).Show(ChildShowInWhere.Hide).HasLabel(_processBomListLabel).HasOrderNo(50);
                View.ChildrenProperty(p => p.RoutingProcessList).Show(ChildShowInWhere.Hide).HasLabel("工序清单").HasOrderNo(60);
                View.ChildrenProperty(p => p.WorkOrderOutputProductList).Show(ChildShowInWhere.Hide).HasOrderNo(19);
                View.ChildrenProperty(p => p.LayoutInfoList).Show(ChildShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 创建订工单的视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            View.AddBehavior("SIE.Web.MES.WorkOrders.CreateWorkOrderBehavior");
            View.UseCommands("SIE.Web.MES.WorkOrders.SaveAndAddWorkOrderCommand", "SIE.Web.MES.WorkOrders.SaveAndClosedWorkOrderCommand");
            View.UseDetail(4);

            View.Property(p => p.No).Show(ShowInWhere.Detail).Readonly().HasOrderNo(10);
            View.Property(p => p.Factory).Show(ShowInWhere.Detail).UseFactoryEditor().HasOrderNo(20);
            View.Property(p => p.CustomerId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<CustomerController>().GetCustomers(pagingInfo, keyword);
            }).Show(ShowInWhere.Detail).HasOrderNo(30);
            View.Property(p => p.CustomerOrderNo).Show(ShowInWhere.Detail).HasOrderNo(40);
            View.Property(p => p.ErpWorkOrder).HasLabel("ERP工单").ShowInDetail().HasOrderNo(45);
            View.Property(p => p.SaleOrderNo).Show(ShowInWhere.Detail).HasOrderNo(50);
            View.Property(p => p.Type).Show(ShowInWhere.Detail).HasOrderNo(60);
            View.Property(p => p.WorkShop)
                .UseDataSource((source, pagingInfo, keyword) =>
                {
                    var entity = source as WorkOrder;
                    double? factoryId = entity?.FactoryId;
                    var workshop = RT.Service.Resolve<EnterpriseController>().GetResourceWorkShops(pagingInfo, keyword, factoryId);
                    if (workshop == null || workshop.Count <= 0)
                        return new EntityList<Enterprise>();
                    workshop.ForEach(p => p.TreePId = null);
                    return workshop;
                }).Show(ShowInWhere.Detail).Cascade(p => p.ResourceId, null).HasOrderNo(70);
            View.Property(p => p.Resource).Show(ShowInWhere.Detail).UseDataSource((e, c, r) =>
            {
                var workOrder = e as WorkOrder;
                if (workOrder == null || workOrder.WorkShop == null)
                    return new EntityList<WipResource>();
                var stateList = new List<ResourceState>() { ResourceState.Actived, ResourceState.Stop, ResourceState.Unused };
                var sourceType = new List<SyncSourceType>() { SyncSourceType.Enterprise, SyncSourceType.Equipment };
                return RT.Service.Resolve<WipResourceController>().GetWipResources(stateList, workOrder.WorkShopId.Value, sourceType, c, r);
            }).UsePagingLookUpEditor(p => p.DisplayField = "Name")
            .UseListSetting(e => { e.HelpInfo = "显示当前车间启用状态不失效的生产资源"; }).HasOrderNo(80);
            View.Property(p => p.ProjectMaintain).UseDataSource((e, p, k) =>
            {
                return RT.Service.Resolve<ProjectMaintainController>().GetProjectMaintains(p, k);
            }).HasOrderNo(85);
            View.Property(p => p.Product).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add(nameof(e.WorkOrderProductName), nameof(e.Product.Name));
                m.DicLinkField = dic;
            }).UseDataSource((e, p, k) =>
            {
                List<ItemType> itemTypeList = new List<ItemType>() { ItemType.Product, ItemType.SemiFinished };
                List<int> itemTypeValueList = itemTypeList.Select(f => (int)f).ToList();
                return RT.Service.Resolve<ItemController>().GetItemsFormType(itemTypeValueList, State.Enable, k, p);
            }).Show(ShowInWhere.Detail).HasOrderNo(90).Cascade(e => e.ItemExtPropName, null).Cascade(p => p.ItemExtProp, null);
            View.Property(P => P.WorkOrderProductName).Show(ShowInWhere.Detail).Readonly().HasLabel("产品名称").HasOrderNo(100);
            View.Property(P => P.PlanQty).UseItemUnitEditor(p => { p.ItemIdField = "ProductId"; }).Show(ShowInWhere.Detail).HasOrderNo(110);
            View.Property(P => P.OrderQty).HasLabel("订单数量*").UseSpinEditor(p => p.AllowDecimals = false).Show(ShowInWhere.Detail).HasOrderNo(120);
            View.Property(P => P.ProcessSegmentId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<ProcessSegmentController>().GetProcessSegments(pagingInfo, keyword);
            }).Cascade(p => p.ProcessTechId, null).Show(ShowInWhere.Detail).HasOrderNo(122);
            View.Property(P => P.ProcessTechId).UseDataSource((e, c, r) =>
            {
                var workOrder = e as WorkOrder;
                if (workOrder == null || workOrder.ProcessSegmentId == null)
                    return new EntityList<ProcessTech>();
                return RT.Service.Resolve<ProcessTechBaseController>().GetProcessTechsBySegment(workOrder.ProcessSegmentId.Value, c, r);
            }).Show(ShowInWhere.Detail).HasOrderNo(124);
            View.Property(p => p.PlanBeginDate).Show(ShowInWhere.Detail).HasOrderNo(130);
            View.Property(p => p.PlanEndDate).Show(ShowInWhere.Detail).HasOrderNo(140);
            View.Property(p => p.Version).UsePagingLookUpOldEditor(p => { p.DataSourceProperty = "true"; p.XType = "WorkOrderRoutingVersion"; }).Show(ShowInWhere.Detail).HasOrderNo(150);
            View.Property(p => p.UseOldSn).UseCheckEditor().Show(ShowInWhere.Detail).Visibility(UseOldSnVisibleProperty)
                .UseListSetting(e => { e.HelpInfo = "工单类型为返工可见"; }).HasOrderNo(160);
            View.Property(p => p.PanelWorkOrder).UsePagingLookUpEditor().HasLabel("组合板工单号").Show(ShowInWhere.Hide).HasOrderNo(161);
            View.Property(p => p.PanelQty).Show(ShowInWhere.Hide).HasOrderNo(170);
            ConfigDetailsViewChildren();
        }

        /// <summary>
        /// ConfigDetailsView子视图属性
        /// </summary>
        private void ConfigDetailsViewChildren()
        {
            View.Property(p => p.ItemExtPropName).UseItemExtPropRecordsFieldEditor(p =>
            {
                p.IsAllRequired = true;
                p.SourceEntityType = "WorkOrder";
                p.ItemIdField = "ProductId";
                p.DbField = "ItemExtProp";
            }).HasOrderNo(105).Show(ShowInWhere.All);
            View.ChildrenProperty(p => p.WorkOrderOutputProductList).Show(ChildShowInWhere.All).UseViewGroup(WorkOrderOutputProductViewConfig.EditView).HasOrderNo(19);
            View.ChildrenProperty(p => p.BomList).Show(ChildShowInWhere.Detail).HasLabel(_bomListLabel).HasOrderNo(20).LazyLoad(false);
            View.ChildrenProperty(p => p.ProcessBomList).Show(ChildShowInWhere.Detail).HasLabel(_processBomListLabel).UseViewGroup(WorkOrderProcessBomViewConfig.WorkOrderProcessBomView).HasOrderNo(30).LazyLoad(false);
            View.ChildrenProperty(p => p.RoutingProcessList).Show(ChildShowInWhere.Hide).HasLabel("工序清单").Readonly().HasOrderNo(40).LazyLoad(false).IsVisible = true;
            View.ChildrenProperty(p => p.PackageRuleDetailList).Show(ChildShowInWhere.Detail).HasLabel("包装规则").UseViewGroup(WorkOrderPackageRuleDetailViewConfig.ListView).HasOrderNo(50).LazyLoad(false);

            View.AssociateChildrenProperty(WOLabelPrintDetailProperty.LabelPrintTemProperty, e =>
            {
                var workOrder = e.Parent as WorkOrder;
                Check.NotNull(workOrder, "工单为空".L10N());
                if (workOrder.Template == null)
                {
                    var template = new SIE.Core.Items.LabelPrintTemplate();
                    template.GenerateId();
                    workOrder.Template = template;
                    return template;
                }
                return RF.GetById<SIE.Core.Items.LabelPrintTemplate>(workOrder.TemplateId, new EagerLoadOptions().LoadWithViewProperty());
            }, ProductLabelTemplateViewConfig.WorkderView)
                .HasLabel("打印设置")
                .Show(ChildShowInWhere.All)
                .HasOrderNo(60)
                .LazyLoad(false);

            View.AssociateChildrenProperty(WoWipBatchExt.AttacWoWipBatchProperty, (e) =>
            {
                var wo = e.Parent as WorkOrder;
                if (wo == null) return new SIE.Core.WorkOrders.WoWipBatch();
                var batch = RT.Service.Resolve<SIE.Core.WorkOrders.WorkOrderController>().GetWipBatch(wo.Id);
                if (batch == null)
                {
                    batch = new SIE.Core.WorkOrders.WoWipBatch();
                    batch.GenerateId();
                    batch.Qty = 1;
                }
                return batch;
            }, DetailsView).HasLabel("生产批次")
            .Show(ChildShowInWhere.Detail)
            .HasOrderNo(70)
            .LazyLoad(false);

            //用于BS暂存工序清单数据，前端JS会隐藏这个页签
            View.ChildrenProperty(p => p.RoutingProcessList).Show(ChildShowInWhere.Detail).HasLabel("工序清单").HasOrderNo(999).LazyLoad(false);
            View.ChildrenProperty(p => p.LayoutInfoList).Show(ChildShowInWhere.All).HasOrderNo(1000);
        }

        /// <summary>
        /// 选择视图配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.No).Show().HasOrderNo(10);
            View.Property(p => p.WorkOrderProductCode).Readonly().HasOrderNo(11);
            View.Property(p => p.WorkOrderProductName).Readonly().HasOrderNo(12);
            View.Property(p => p.PlanQty).Readonly().HasOrderNo(13);
            View.Property(DisplayStateProperty).HasLabel("状态").Readonly().HasOrderNo(14);
            View.Property(p => p.PlanBeginDate).Show().HasOrderNo(20);
            View.Property(p => p.PlanEndDate).Show().HasOrderNo(30);
            View.Property(P => P.PlanQty).UseSpinEditor(p => p.AllowDecimals = false).Show().HasOrderNo(40);
            View.Property(p => p.ActuFinishDate).Show().HasOrderNo(50);
            View.Property(p => p.Factory).Show().HasOrderNo(60);
        }

        /// <summary>
        /// 导入模板视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.Property(p => p.No).HasLabel("工单编号");
            View.Property(p => p.WorkOrderProductCode).HasLabel("产品编码");
            View.Property(p => p.PlanQty).HasLabel("计划数量");
            View.Property(p => p.Type).HasLabel("工单类型");
            View.Property(p => p.PlanBeginDate);
            View.Property(p => p.PlanEndDate);
            View.Property(p => p.WorkShopName).HasLabel("车间编码");
            View.Property(p => p.ResourceName).HasLabel("资源编码");
            View.Property(p => p.ParentId).HasLabel("上级工单编码");
            View.Property(p => p.ErpWorkOrderNo);
            View.Property(p => p.CustomerOrderNo);
            View.Property(p => p.SaleOrderNo);
            View.Property(p => p.OrderQty);
            View.Property(p => p.Factory);
        }
    }
}
