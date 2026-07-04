using SIE.ManagedProperty;
using SIE.MES.WorkOrders;

namespace SIE.Wpf.MES.WorkOrders
{
    /// <summary>
    /// 工单视图配置
    /// </summary> 
    [CompiledPropertyDeclarer]
    internal class WorkOrderViewConfig : WPFViewConfig<WorkOrder>
    {
        ///// <summary>
        ///// 编辑视图ViewGroup
        ///// </summary>
        //public const string EditView = "EditView";

        /// <summary>
        /// 查看工单视图ViewGroup
        /// </summary>
        public const string ReadonlyView = "ReadonlyView";

        //#region 类型名称 TypeName
        ///// <summary>
        ///// 类型名称
        ///// </summary>
        //public static readonly Property<string> TypeNameProperty = P<WorkOrder>.RegisterExtensionReadOnly("TypeName", typeof(WorkOrderViewConfig),
        //    GetTypeName, WorkOrder.TypeProperty);

        ///// <summary>
        ///// 类型名称
        ///// </summary>
        ///// <param name="me">WorkOrder</param>
        ///// <returns>string</returns>
        //public static string GetTypeName(WorkOrder me)
        //{
        //    return me.Type.ToLabel();
        //}
        //#endregion

        //#region 显示状态 DisplayState
        ///// <summary>
        ///// 显示状态  用一个字段显示工单状态和排产状态，工单状态有值时显示工单状态，排产状态有值时显示排产状态
        ///// </summary>
        //public static readonly Property<string> DisplayStateProperty = P<WorkOrder>.RegisterExtensionReadOnly("DisplayState", typeof(WorkOrderViewConfig),
        //    GetDisplayState, WorkOrder.StateProperty, WorkOrder.StateProperty);

        ///// <summary>
        ///// 显示状态
        ///// </summary>
        ///// <param name="me">工单</param>
        ///// <returns>工单状态/排产状态</returns>
        //public static string GetDisplayState(WorkOrder me)
        //{
        //    if (me.IsPause == YesNo.Yes && (me.State == Core.WorkOrders.WorkOrderState.Release || me.State == Core.WorkOrders.WorkOrderState.Producing))
        //        return EnumViewModel.EnumToLabel(me.State) + "暂停";
        //    else
        //    {
        //        return EnumViewModel.EnumToLabel(me.State);
        //    }
        //}
        //#endregion

        //#region 根据工单来源设置是否只读 IsReadonly
        ///// <summary>
        ///// 自建工单可以修改主数据，外部来源工单不可修改主数据
        ///// </summary>
        //public static readonly Property<bool> IsReadonlyProperty = P<WorkOrder>.RegisterExtensionReadOnly("IsReadonly", typeof(WorkOrderViewConfig),
        //    GetIsReadonly, WorkOrder.SourceProperty);

        ///// <summary>
        ///// 自建工单可以修改主数据，外部来源工单不可修改主数据
        ///// </summary>
        ///// <param name="me">工单</param>
        ///// <returns>true/false</returns>
        //public static bool GetIsReadonly(WorkOrder me)
        //{
        //    if (me.Source == SIE.Common.SourceType.External)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        if (me.State == Core.WorkOrders.WorkOrderState.Producing)
        //        {
        //            return true;
        //        }
        //    }

        //    return false;
        //}
        //#endregion

        //#region 暂停状态下车间资源可编辑 ResourceReadonly
        ///// <summary>
        ///// 自建工单可以修改主数据，外部来源工单不可修改主数据
        ///// </summary>
        //public static readonly Property<bool> ResourceReadonlyProperty = P<WorkOrder>.RegisterExtensionReadOnly("ResourceReadonly", typeof(WorkOrderViewConfig),
        //    GetResourceReadonly, WorkOrder.IsPauseProperty);

        ///// <summary>
        ///// 自建工单可以修改主数据，外部来源工单不可修改主数据
        ///// </summary>
        ///// <param name="me">工单</param>
        ///// <returns>true/false</returns>
        //public static bool GetResourceReadonly(WorkOrder me)
        //{
        //    if (me.Source == SIE.Common.SourceType.External)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        if (me.State == Core.WorkOrders.WorkOrderState.Producing && me.IsPause == YesNo.No)
        //        {
        //            return true;
        //        }
        //    }

        //    return false;
        //}
        //#endregion

        //#region 使用旧条码只读属性 UseOldSnReadonly
        ///// <summary>
        ///// 使用旧条码只读属性
        ///// </summary>
        //[Label("使用旧条码只读属性")]
        //public static readonly Property<bool> UseOldSnReadonlyProperty = P<WorkOrder>.RegisterExtensionReadOnly("UseOldSnReadonly", typeof(WorkOrderViewConfig),
        //    GetUseOldSnReadonly, WorkOrder.SourceProperty);

        ///// <summary>
        ///// 使用旧条码只读属性
        ///// </summary>
        //public static bool GetUseOldSnReadonly(WorkOrder me)
        //{
        //    bool unionExis = RT.Service.Resolve<ReworkController>().CheckUnionBarcodeExist(me.No);
        //    return me.PrintedQty > 0 || unionExis;
        //}
        //#endregion

        //#region 使用旧条码显示属性 UseOldSnVisible
        ///// <summary>
        ///// 使用旧条码显示属性
        ///// </summary>
        //[Label("使用旧条码显示属性")]
        //public static readonly Property<bool> UseOldSnVisibleProperty = P<WorkOrder>.RegisterExtensionReadOnly("UseOldSnVisible", typeof(WorkOrderViewConfig),
        //    GetUseOldSnVisible, WorkOrder.TypeProperty);

        ///// <summary>
        ///// 使用旧条码显示属性
        ///// </summary>
        ///// <returns>工单类型为返工返回true，否则返回false</returns>
        //public static bool GetUseOldSnVisible(WorkOrder me)
        //{
        //    bool visible = true;
        //    if (me.Type == SIE.Core.WorkOrders.WorkOrderType.Rework)
        //        visible = true;
        //    else
        //        visible = false;
        //    return visible;
        //}
        //#endregion

        ///// <summary>
        ///// 默认视图配置
        ///// </summary>
        //protected override void ConfigView()
        //{
        //    View.DeclareExtendViewGroup(new string[] { EditView, ReadonlyView });
        //    switch (ViewGroup)
        //    {
        //        case EditView:
        //            EditConfigView();
        //            break;
        //        case ReadonlyView:
        //            ConfigReadonlyView();
        //            break;
        //    }
        //}

        ///// <summary>
        ///// 查看工单视图
        ///// </summary>
        //void ConfigReadonlyView()
        //{
        //    View.UseDetail(dialogMax: true, dialogHeight: 800);
        //    View.AddBehavior(typeof(WorkOrderBehavior));
        //    using (View.OrderProperties())
        //    {
        //        using (View.DeclareGroup("基本信息", 4, false))
        //        {
        //            View.Property(p => p.No).Show(ShowInWhere.Detail).Readonly();
        //            View.Property(p => p.ErpWorkOrder).UsePagingLookUpEditor(p => p.DisplayMember = ErpWorkOrder.NoProperty.Name).ShowInDetail().Readonly();
        //            View.Property(p => p.CustomerName).Show(ShowInWhere.Detail).Readonly();
        //            View.Property(p => p.CustomerOrderNo).Show(ShowInWhere.Detail).Readonly();
        //            View.Property(p => p.SaleOrderNo).Show(ShowInWhere.Detail).Readonly();
        //            View.Property(TypeNameProperty).Show(ShowInWhere.Detail).Readonly().HasLabel("类型");
        //            View.Property(p => p.WorkShop).Show(ShowInWhere.Detail).Readonly();
        //            View.Property(p => p.Resource).Show(ShowInWhere.Detail).Readonly();
        //            View.Property(p => p.Product).UsePagingLookUpEditor(p => p.DisplayMember = Item.CodeProperty.Name).Show(ShowInWhere.Detail).Readonly();
        //            View.Property(P => P.WorkOrderProductName).Show(ShowInWhere.Detail).Readonly();
        //            View.Property(P => P.PlanQty).UseSpinEditor(p => p.Decimals = 0).Show(ShowInWhere.Detail).Readonly();
        //            View.Property(P => P.OrderQty).UseSpinEditor(p => p.Decimals = 0).Show(ShowInWhere.Detail).Readonly();
        //            View.Property(P => P.FinishQty).UseSpinEditor(p => p.Decimals = 0).Show(ShowInWhere.Detail).Readonly();
        //            View.Property(p => p.PlanBeginDate).Show(ShowInWhere.Detail).Readonly();
        //            View.Property(p => p.PlanEndDate).Show(ShowInWhere.Detail).Readonly();
        //            View.Property(p => p.Version).Show(ShowInWhere.Detail).Readonly();
        //            View.Property(p => p.UseOldSn).UseCheckEditor().Show(ShowInWhere.Detail).Visibility(UseOldSnVisibleProperty).Readonly();
        //        }

        //        View.AttachChildrenProperty(typeof(PropertyValueViewModel), (o) =>
        //        {
        //            var workOrder = o.Parent as WorkOrder;
        //            if (workOrder == null) return null;
        //            var list = workOrder.LocalContext.GetPropertyOrDefault<EntityList<PropertyValueViewModel>>("PropertyValueViewModel");
        //            if (list == null)
        //            {
        //                var result = workOrder.PropertyValueList.GroupBy(p => p.DefinitionId).Select(f => new PropertyValueViewModel { DefinitionId = f.Key, Values = f.Select(p => p.Value).ToList(), Type = f.Select(p => p.WorkOrder).FirstOrDefault().GetType(), ParentId = f.Select(p => p.WorkOrderId).FirstOrDefault() });
        //                list = new EntityList<PropertyValueViewModel>();
        //                list.AddRange(result);
        //                foreach (var value in list)
        //                    value.ItemId = workOrder.ProductId;
        //                workOrder.LocalContext.SetExtendedProperty("PropertyValueViewModel", list);
        //            }

        //            list.MarkSaved();
        //            return list;
        //        }, WorkOrderPropertyValueExtendViewConfig.WorkerReadOnlyView).Show(ChildShowInWhere.All).HasLabel("属性值").OrderNo = -1;
        //        View.ChildrenProperty(p => p.BomList).Show(ChildShowInWhere.Detail).HasLabel("BOM信息").Readonly().UseViewGroup(WorkOrderViewConfig.ReadonlyView);
        //        View.ChildrenProperty(p => p.ProcessBomList).Show(ChildShowInWhere.Detail).HasLabel("工序BOM").Readonly().UseViewGroup(WorkOrderViewConfig.ReadonlyView);
        //        View.ChildrenProperty(p => p.RoutingProcessList).Show(ChildShowInWhere.Hide).HasLabel("工序清单").Readonly();
        //        View.ChildrenProperty(p => p.PackageRuleDetailList).Show(ChildShowInWhere.Detail).HasLabel("包装规则").Readonly().UseViewGroup(WorkOrderViewConfig.ReadonlyView);
        //        View.ChildrenProperty(p => p.WorkOrderLogList).Show(ChildShowInWhere.Detail).HasLabel("工单日志").Readonly();
        //        View.AttachDetailChildrenProperty(typeof(SIE.Core.Items.LabelPrintTemplate), e =>
        //        {
        //            return (e.Parent as WorkOrder).Template;
        //        }, ProductLabelTemplateViewConfig.ReadOnlyView).HasLabel("打印设置").Show(ChildShowInWhere.All).OrderNo = 50;
        //        View.AssociateChildrenProperty(WoWipBatchExt.AttacWoWipBatchProperty, (e) =>
        //        {
        //            var wo = e.Parent as WorkOrder;
        //            if (wo == null) return new Core.WorkOrders.WoWipBatch();
        //            var batch = RT.Service.Resolve<Core.WorkOrders.WorkOrderController>().GetWipBatch(wo.Id);
        //            if (batch == null)
        //            {
        //                batch = new Core.WorkOrders.WoWipBatch();
        //                batch.GenerateId();
        //                batch.Qty = 1;
        //            }

        //            return batch;
        //        }, WoWipBatchViewConfig.ReadonlyView).HasLabel("生产批次").Show(ChildShowInWhere.Detail).OrderNo = 68;
        //    }
        //}

        ///// <summary>
        ///// 修改工单视图
        ///// </summary>
        //void EditConfigView()
        //{
        //    View.UseCommands(typeof(SaveWorkOrderCommand));
        //    View.AddBehavior(typeof(WorkOrderBehavior));
        //    using (View.OrderProperties())
        //    {
        //        using (View.DeclareGroup("基本信息", 4, false))
        //        {
        //            View.Property(p => p.No).Show(ShowInWhere.Detail).Readonly();
        //            View.Property(p => p.ErpWorkOrder).ShowInDetail().UsePagingLookUpEditor(p => p.DisplayMember = ErpWorkOrder.NoProperty.Name).Readonly(IsReadonlyProperty);
        //            View.Property(p => p.CustomerName).Show(ShowInWhere.Detail).Readonly(IsReadonlyProperty);
        //            View.Property(p => p.CustomerOrderNo).Show(ShowInWhere.Detail).Readonly(IsReadonlyProperty);
        //            View.Property(p => p.SaleOrderNo).Show(ShowInWhere.Detail).Readonly(IsReadonlyProperty);
        //            View.Property(p => p.Type).Show(ShowInWhere.Detail).Readonly(IsReadonlyProperty);
        //            View.Property(p => p.WorkShop).Show(ShowInWhere.Detail).Readonly(ResourceReadonlyProperty).UseResourceWorkShopEditor();
        //            View.Property(p => p.Resource).Show(ShowInWhere.Detail).Readonly(ResourceReadonlyProperty).UseDataSource((e, c, r) =>
        //            {
        //                var workOrder = e as WorkOrder;
        //                if (workOrder == null || workOrder.WorkShop == null)
        //                    return new EntityList<WipResource>();

        //                var stateList = new List<ResourceState>() { ResourceState.Actived, ResourceState.Stop, ResourceState.Unused };
        //                var sourceType = new List<SyncSourceType>() { SyncSourceType.Enterprise, SyncSourceType.Equipment };
        //                return RT.Service.Resolve<WipResourceController>().GetWipResources(stateList, workOrder.WorkShopId.Value, sourceType, c, r);
        //            }).UsePagingLookUpEditor(p => { p.ReloadDataOnPopping = true; });
        //            View.Property(p => p.Product).UsePagingLookUpEditor(p => p.DisplayMember = Item.CodeProperty.Name).Show(ShowInWhere.Detail).Readonly();
        //            View.Property(P => P.Product.Name).Show(ShowInWhere.Detail).Readonly().HasLabel("产品名称");
        //            View.Property(P => P.PlanQty).UseSpinEditor(p => p.Decimals = 0).Show(ShowInWhere.Detail).Readonly(IsReadonlyProperty).UseSpinEditor(e => e.Decimals = 0);
        //            View.Property(P => P.OrderQty).UseSpinEditor(p => p.Decimals = 0).Show(ShowInWhere.Detail).Readonly(IsReadonlyProperty).UseSpinEditor(e => e.Decimals = 0);
        //            View.Property(P => P.FinishQty).UseSpinEditor(p => p.Decimals = 0).Show(ShowInWhere.Detail).Readonly(true).UseSpinEditor(e => e.Decimals = 0);
        //            View.Property(p => p.PlanBeginDate).Show(ShowInWhere.Detail).Readonly(IsReadonlyProperty);
        //            View.Property(p => p.PlanEndDate).Show(ShowInWhere.Detail).Readonly(IsReadonlyProperty);
        //            View.Property(p => p.Version).UseDataSource((e, c, r) =>
        //            {
        //                var workOrder = e as WorkOrder;
        //                return RT.Service.Resolve<RoutingSettingController>().GetRoutingVersions(workOrder.Type, workOrder.PlanBeginDate, workOrder.ProductId, workOrder.ResourceId);
        //            }).Show(ShowInWhere.Detail).Readonly(IsReadonlyProperty);
        //            View.Property(p => p.UseOldSn).UseCheckEditor().Show(ShowInWhere.Detail).Visibility(UseOldSnVisibleProperty).Readonly(UseOldSnReadonlyProperty);
        //        }

        //        View.AttachChildrenProperty(typeof(PropertyValueViewModel), (o) =>
        //        {
        //            var workOrder = o.Parent as WorkOrder;
        //            if (workOrder == null) return null;
        //            var list = workOrder.LocalContext.GetPropertyOrDefault<EntityList<PropertyValueViewModel>>("PropertyValueViewModel");
        //            if (list == null)
        //            {
        //                var result = workOrder.PropertyValueList.GroupBy(p => p.DefinitionId).Select(f => new PropertyValueViewModel { DefinitionId = f.Key, Values = f.Select(p => p.Value).ToList(), Type = f.Select(p => p.WorkOrder).FirstOrDefault().GetType(), ParentId = f.Select(p => p.WorkOrderId).FirstOrDefault() });
        //                list = new EntityList<PropertyValueViewModel>();
        //                list.AddRange(result);
        //                foreach (var value in list)
        //                    value.ItemId = workOrder.ProductId;
        //                workOrder.LocalContext.SetExtendedProperty("PropertyValueViewModel", list);
        //            }

        //            list.MarkSaved();
        //            return list;
        //        }, WorkOrderPropertyValueExtendViewConfig.WorkOrderPropertyValueExtendView).Show(ChildShowInWhere.All).HasLabel("属性值").OrderNo = -1;
        //        View.ChildrenProperty(p => p.BomList).Show(ChildShowInWhere.Detail).HasLabel("BOM信息");
        //        View.ChildrenProperty(p => p.ProcessBomList).Show(ChildShowInWhere.Detail).HasLabel("工序BOM").UseViewGroup(WorkOrderProcessBomViewConfig.WorkOrderProcessBomView);
        //        View.ChildrenProperty(p => p.RoutingProcessList).Show(ChildShowInWhere.Hide).HasLabel("工序清单").Readonly();
        //        View.ChildrenProperty(p => p.PackageRuleDetailList).Show(ChildShowInWhere.Detail).HasLabel("包装规则").UseViewGroup(WorkOrderPackageRuleDetailViewConfig.ListView);

        //        View.ChildrenProperty(p => p.WorkOrderLogList).Show(ChildShowInWhere.Detail).HasLabel("工单日志");

        //        //View.AttachDetailChildrenProperty(typeof(SIE.Core.Items.LabelPrintTemplate), (e) =>
        //        //{
        //        //    return (e.Parent as WorkOrder).Template;
        //        //}, ProductLabelTemplateViewConfig.WorkderView).HasLabel("打印设置").Show(ChildShowInWhere.All).OrderNo = 50;
        //        View.AssociateChildrenProperty(WOLabelPrintDetailProperty.LabelPrintTemProperty, e =>
        //        {
        //            var workOrder = e.Parent as WorkOrder;
        //            if (workOrder == null)
        //                return new Core.Items.LabelPrintTemplate();
        //            if (workOrder.Template == null)
        //            {
        //                var template = new Core.Items.LabelPrintTemplate();
        //                template.GenerateId();
        //                workOrder.Template = template;
        //            }
        //            return workOrder.Template;
        //        }, ProductLabelTemplateViewConfig.WorkderView).HasLabel("打印设置").Show(ChildShowInWhere.All).OrderNo = 50;


        //        View.AssociateChildrenProperty(WoWipBatchExt.AttacWoWipBatchProperty, (e) =>
        //        {
        //            var wo = e.Parent as WorkOrder;
        //            if (wo == null) return new Core.WorkOrders.WoWipBatch();
        //            var batch = RT.Service.Resolve<Core.WorkOrders.WorkOrderController>().GetWipBatch(wo.Id);
        //            if (batch == null)
        //            {
        //                batch = new Core.WorkOrders.WoWipBatch();
        //                batch.GenerateId();
        //                batch.Qty = 1;
        //            }

        //            return batch;
        //        }).HasLabel("生产批次").Show(ChildShowInWhere.Detail).OrderNo = 68;
        //    }
        //}

        ///// <summary>
        ///// 默认工单视图
        ///// </summary>
        //protected override void ConfigListView()
        //{
        //    View.FormEdit().UseDefaultBehaviors();
        //    View.UseCommands(
        //        typeof(WorkOrderResumeCommand), typeof(WorkOrderPauseCommand),
        //        typeof(WorkOrderCloseCommand), typeof(AddWorkOrderCommand), typeof(UpdateWorkOrderCommand),
        //        typeof(WorkOrderListCopyCommand), typeof(ViewWorkOrderCommand), typeof(DownloadWorkOrderBarcodeCommand),
        //        typeof(ImportBarcodeCommand), typeof(UnionBarcodeCommand), typeof(UpdateRoutingLayoutCommand), typeof(ImportWorkOrderCommand));
        //    View.UseCommands(WPFCommandNames.Export);

        //    using (View.OrderProperties())
        //    {
        //        View.Property(p => p.No).FixColumn();
        //        View.Property(p => p.WorkOrderProductCode).FixColumn();
        //        View.Property(p => p.WorkOrderProductName);
        //        View.Property(p => p.PlanQty).UseSpinEditor(p => p.Decimals = 0);
        //        View.Property(p => p.FinishQty).UseSpinEditor(p => p.Decimals = 0);
        //        View.Property(p => p.StorageQty).UseSpinEditor(p => p.Decimals = 0);
        //        View.Property(p => p.Type);
        //        View.Property(p => p.State).Show(ShowInWhere.Hide);
        //        View.Property(DisplayStateProperty).HasLabel("工单状态");
        //        View.Property(p => p.KitType).Show(ShowInWhere.Hide);
        //        View.Property(p => p.IsPause).Show(ShowInWhere.Hide);
        //        View.Property(p => p.PlanBeginDate);
        //        View.Property(p => p.PlanEndDate);
        //        View.Property(p => p.ActuStartDate);
        //        View.Property(p => p.ActuFinishDate);
        //        View.Property(p => p.WorkShopName);
        //        View.Property(p => p.ResourceName);
        //        View.Property(p => p.VersionName);
        //        View.Property(p => p.MakerName);
        //        View.Property(p => p.MakeDate);
        //        View.Property(p => p.Source);
        //        View.Property(p => p.ParentId);
        //        View.Property(p => p.ErpWorkOrder).UsePagingLookUpEditor(p => p.DisplayMember = ErpWorkOrder.NoProperty.Name);
        //        View.Property(p => p.CustomerName);
        //        View.Property(p => p.CustomerOrderNo);
        //        View.Property(p => p.SaleOrderNo);
        //        View.Property(p => p.ProcessTechOrderCode).Readonly();
        //        View.Property(p => p.OrderQty);
        //        View.Property(p => p.Level).Show(ShowInWhere.Hide);
        //        View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
        //        View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
        //        View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
        //        View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        //        View.ChildrenProperty(p => p.PropertyValueList).Show(ChildShowInWhere.Hide).HasLabel("属性值");
        //        View.ChildrenProperty(p => p.WorkOrderLogList).Show(ChildShowInWhere.Hide).HasLabel("工单日志");
        //        View.ChildrenProperty(p => p.PackageRuleDetailList).Show(ChildShowInWhere.Hide).HasLabel("包装规则");
        //        View.ChildrenProperty(p => p.BomList).Show(ChildShowInWhere.Hide).HasLabel("工单BOM信息");
        //        View.ChildrenProperty(p => p.ProcessBomList).Show(ChildShowInWhere.Hide).HasLabel("工序BOM");
        //        View.ChildrenProperty(p => p.RoutingProcessList).Show(ChildShowInWhere.Hide).HasLabel("工序清单");
        //    }
        //}

        ///// <summary>
        ///// 创建订工单的视图
        ///// </summary>
        //protected override void ConfigDetailsView()
        //{
        //    View.UseDefaultCommands();
        //    View.AddBehavior(typeof(WorkOrderBehavior));
        //    View.ReplaceCommands(WPFCommandNames.FormSave, typeof(WorkOrderFormSaveThenAddCommand));
        //    View.RemoveCommands(WPFCommandNames.FormCopy, WPFCommandNames.FormAdd);
        //    View.UseDetail(dialogMax: true, dialogHeight: 800);
        //    using (View.OrderProperties())
        //    {
        //        using (View.DeclareGroup("基本信息", 4, false))
        //        {
        //            View.Property(p => p.No).Show(ShowInWhere.Detail).Readonly(f => f.No.IsNotEmpty());
        //            View.Property(p => p.ErpWorkOrder).HasLabel("ERP工单").ShowInDetail().UsePagingLookUpEditor(p => p.DisplayMember = ErpWorkOrder.NoProperty.Name);
        //            View.Property(p => p.CustomerName).Show(ShowInWhere.Detail);
        //            View.Property(p => p.CustomerOrderNo).Show(ShowInWhere.Detail);
        //            View.Property(p => p.SaleOrderNo).Show(ShowInWhere.Detail);
        //            View.Property(p => p.Type).Show(ShowInWhere.Detail);
        //            View.Property(p => p.WorkShop).Show(ShowInWhere.Detail).UseResourceWorkShopEditor();
        //            View.Property(p => p.Resource).Show(ShowInWhere.Detail).UseDataSource((e, c, r) =>
        //            {
        //                var workOrder = e as WorkOrder;
        //                if (workOrder == null || workOrder.WorkShop == null)
        //                    return new EntityList<WipResource>();

        //                var stateList = new List<ResourceState>() { ResourceState.Actived, ResourceState.Stop, ResourceState.Unused };
        //                var sourceType = new List<SyncSourceType>() { SyncSourceType.Enterprise, SyncSourceType.Equipment };
        //                return RT.Service.Resolve<WipResourceController>().GetWipResources(stateList, workOrder.WorkShopId.Value, sourceType, c, r);
        //            }).UsePagingLookUpEditor(p => { p.ReloadDataOnPopping = true; });
        //            View.Property(p => p.Product).UseProductCombinationEditor(p => p.DisplayMember = Item.CodeProperty.Name).Show(ShowInWhere.Detail);
        //            View.Property(P => P.Product.Name).Show(ShowInWhere.Detail).HasLabel("产品名称");
        //            View.Property(P => P.PlanQty).UseSpinEditor(p => p.Decimals = 0).Show(ShowInWhere.Detail).UseSpinEditor(e => e.Decimals = 0);
        //            View.Property(P => P.OrderQty).UseSpinEditor(p => p.Decimals = 0).Show(ShowInWhere.Detail).UseSpinEditor(e => e.Decimals = 0);
        //            View.Property(p => p.PlanBeginDate).Show(ShowInWhere.Detail);
        //            View.Property(p => p.PlanEndDate).Show(ShowInWhere.Detail);
        //            View.Property(p => p.Version).UseDataSource((e, c, r) =>
        //            {
        //                var workOrder = e as WorkOrder;
        //                return RT.Service.Resolve<RoutingSettingController>().GetRoutingVersions(workOrder.Type, workOrder.PlanBeginDate, workOrder.ProductId, workOrder.ResourceId);
        //            }).UsePagingLookUpEditor(p => { p.ReloadDataOnPopping = true; }).Show(ShowInWhere.Detail);
        //            View.Property(p => p.UseOldSn).UseCheckEditor().Show(ShowInWhere.Detail).Visibility(UseOldSnVisibleProperty).Readonly(UseOldSnReadonlyProperty);
        //        }

        //        View.AttachChildrenProperty(typeof(PropertyValueViewModel), (o) =>
        //        {
        //            var workOrder = o.Parent as WorkOrder;
        //            if (workOrder == null) return null;
        //            var list = workOrder.LocalContext.GetPropertyOrDefault<EntityList<PropertyValueViewModel>>("WorkOrderValueList");
        //            if (list == null)
        //            {
        //                var result = workOrder.PropertyValueList.GroupBy(p => p.DefinitionId).Select(f => new PropertyValueViewModel { DefinitionId = f.Key, Values = f.Select(p => p.Value).ToList(), Type = f.Select(p => p.WorkOrder).FirstOrDefault().GetType(), ParentId = f.Select(p => p.WorkOrderId).FirstOrDefault() });
        //                list = new EntityList<PropertyValueViewModel>();
        //                list.AddRange(result);
        //                foreach (var value in list)
        //                    value.ItemId = workOrder.ProductId;
        //                workOrder.LocalContext.SetExtendedProperty("PropertyValueViewModel", list);
        //            }

        //            list.MarkSaved();
        //            return list;
        //        }, WorkOrderPropertyValueExtendViewConfig.WorkOrderPropertyValueExtendView).Show(ChildShowInWhere.All).HasLabel("属性值").OrderNo = -1;
        //        View.ChildrenProperty(p => p.BomList).Show(ChildShowInWhere.Detail).HasLabel("BOM信息");
        //        View.ChildrenProperty(p => p.ProcessBomList).Show(ChildShowInWhere.Detail).HasLabel("工序BOM").UseViewGroup(WorkOrderProcessBomViewConfig.WorkOrderProcessBomView);
        //        View.ChildrenProperty(p => p.RoutingProcessList).Show(ChildShowInWhere.Hide).HasLabel("工序清单").Readonly().IsVisible = true;
        //        View.ChildrenProperty(p => p.PackageRuleDetailList).Show(ChildShowInWhere.Detail).HasLabel("包装规则").UseViewGroup(WorkOrderPackageRuleDetailViewConfig.ListView);
        //        View.AssociateChildrenProperty(WOLabelPrintDetailProperty.LabelPrintTemProperty, e =>
        //        {
        //            var workOrder = e.Parent as WorkOrder;
        //            if (workOrder == null) return new Core.Items.LabelPrintTemplate();
        //            if (workOrder.Template == null)
        //            {
        //                var template = new Core.Items.LabelPrintTemplate();
        //                template.GenerateId();
        //                workOrder.Template = template;
        //            }

        //            return workOrder.Template;
        //        }, ProductLabelTemplateViewConfig.WorkderView).HasLabel("打印设置").Show(ChildShowInWhere.All).OrderNo = 50;
        //        View.AssociateChildrenProperty(WoWipBatchExt.AttacWoWipBatchProperty, (e) =>
        //        {
        //            var wo = e.Parent as WorkOrder;
        //            if (wo == null) return new Core.WorkOrders.WoWipBatch();
        //            var batch = RT.Service.Resolve<Core.WorkOrders.WorkOrderController>().GetWipBatch(wo.Id);
        //            if (batch == null)
        //            {
        //                batch = new Core.WorkOrders.WoWipBatch();
        //                batch.GenerateId();
        //                batch.Qty = 1;
        //            }

        //            return batch;
        //        }).HasLabel("生产批次").Show(ChildShowInWhere.Detail).OrderNo = 68;
        //    }
        //}

        /// <summary>
        /// 选择视图配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.No);
            View.Property(p => p.WorkOrderProductCode).HasLabel("产品编码").Show();
            View.Property(p => p.WorkOrderProductName).HasLabel("产品名称").Show();
            View.Property(p => p.PlanBeginDate);
            View.Property(p => p.PlanEndDate);
            View.Property(P => P.PlanQty).UseSpinEditor(p => p.Decimals = 0);
            View.Property(p => p.ActuFinishDate);
            View.Property(p => p.State);
        }
    }
}