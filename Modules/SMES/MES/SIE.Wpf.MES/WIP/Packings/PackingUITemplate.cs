using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.WIP.Packings;
using SIE.MES.WorkOrders;
using SIE.MetaModel.View;
using SIE.Packages;
using SIE.Packages.ItemLabels;
using SIE.Packages.Packings;
using SIE.Packages.Packings.Strategys;
using SIE.Wpf.MES.WIP.Packings.Commands;
using SIE.Wpf.MES.WIP.Packings.Layouts;
using System;
using System.Linq;
using System.Windows;

namespace SIE.Wpf.MES.WIP.Packings
{
    /// <summary>
    /// 包装采集模板
    /// </summary>
    public class PackingUITemplate : CollectionUITemplate
    {
        /// <summary>
        /// 包装关系视图
        /// </summary>
        ListLogicalView packingRelationView;

        PackingViewModel packingViewData;

        /// <summary>
        /// 构造函数
        /// </summary>
        public PackingUITemplate() : base(typeof(PackingViewModel))
        {
            ViewGroup = PackingViewModelViewConfig.PackingView;
        }

        /// <summary>
        /// 定义块
        /// </summary>
        /// <returns>聚合块</returns>
        protected override AggtBlocks DefineBlocks()
        {
            var block = base.DefineBlocks();
            block.Layout = new LayoutMeta(typeof(SIE.Wpf.MES.WIP.Packings.Layouts.PackingLayout));
            return block;
        }

        /// <summary>
        /// UI创建后
        /// </summary>
        /// <param name="ui">ui</param>
        protected override void OnUIGenerated(ControlResult ui)
        {
            packingViewData = new PackingViewModel();
            var packingView = ui.MainView as DetailLogicalView;
            packingView.Data = packingViewData;
            var layout = ui.Control as SIE.Wpf.MES.WIP.Packings.Layouts.PackingLayout;
            var workstation = CreateOperationControl(packingViewData.Workstation);
            var childControl = InitChildrenView(packingView, packingViewData);
            layout.InitWorkstation(workstation);
            layout.InitChildrenView(childControl);
            IsExistRelationView(packingView);
            //消息订阅和取消订阅
            SubscribeEventBus();
            UnsubscribebeEventBus(ui);
            base.OnUIGenerated(ui);
        }

        /// <summary>
        /// 设置变量
        /// </summary>
        /// <exception cref="ValidationException">
        /// 1、包装关系未配置权限
        /// 2、包装规则未配置权限
        /// 3、物料标签未配置权限
        /// </exception>
        private void IsExistRelationView(DetailLogicalView packingView)
        {
            packingRelationView = packingView.ChildrenViews.FirstOrDefault(f => f.EntityType == typeof(PackingRelation)) as ListLogicalView;
            if (packingRelationView == null)
                throw new ValidationException("[包装关系]块无权限,请联系管理员配置".L10N());
            packingRelationView.Data = packingViewData.PackingRelationList;
            if (packingView.ChildrenViews.Any(f => f.EntityType == typeof(PackageRuleDetail)))
                throw new ValidationException("[包装规则]块无权限,请联系管理员角色权限配置".L10N());
            var itemLableView = packingView.ChildrenViews.FirstOrDefault(f => f.EntityType == typeof(ItemLabel)) as ListLogicalView;
            if (itemLableView == null)
                throw new ValidationException("[物料标签]块无权限,请联系管理员配置".L10N());
        }

        /// <summary>
        /// 初始化子视图
        /// </summary>
        /// <param name="mainView">主视图</param>
        /// <param name="model">包装ViewModel</param>
        /// <returns>Dev原生Tab控件</returns>
        private DXTabControl InitChildrenView(DetailLogicalView mainView, PackingViewModel model)
        {
            DXTabControl tab = new DXTabControl();
            tab.Margin = new Thickness(5, 5, 5, 0);
            tab.Items.Add(CreateTabItem("物料标签", CreateChildListControl(mainView, model.ItemLabelList)));
            tab.Items.Add(CreateTabItem("包装规则明细", CreateChildListControl(mainView, model.PackageRuleDetailList)));
            CreateReportTaskControl(tab, mainView, model, "CollectionView");
            tab.Items.Add(CreateTabItem("消息列表", CreateMessagerControl(model)));
            return tab;
        }

        /// <summary>
        /// 创建明细列表控件
        /// </summary>
        /// <param name="mainView">视图对象</param>
        /// <param name="data">实体集合</param>
        /// <returns>返回UI</returns>
        private FrameworkElement CreateChildListControl(DetailLogicalView mainView, EntityList data)
        {
            var listView = AutoUI.ViewFactory.CreateListView(data.EntityType, ViewGroup, mainView.ModuleKey);
            listView.Data = data;
            listView.Relations.Add(new RelationView("mainView", mainView));
            listView.Control.Margin = new Thickness(-10);
            return listView.Control;
        }

        private void SubscribeEventBus()
        {
            ////订阅打包事件
            RT.EventBus.Subscribe<DoPackingEvent>(this, DoPacking);
            RT.EventBus.Subscribe<PackingStrategyEvent>(this, PackingStrategy);
        }

        private void PackingStrategy(PackingStrategyEvent packingEvent)
        {
            if (packingEvent.Group != "MesPacking") return;
            if (packingEvent.IsSucess)
            {
                if (packingEvent.StrategyType == ScanStrategyMode.ScanSingle)
                {
                    packingViewData.Helper.SucceedEventHandler(packingEvent);
                }
                else if (packingEvent.StrategyType == ScanStrategyMode.ScanOneJoinToMany)
                {
                    if (packingEvent.InsiderBarcode == null)
                        packingViewData.Helper.ScanParentPackageSucceedEventHandler(packingEvent);
                    else
                        packingViewData.Helper.ScanChildPackageSucceedEventHandler(packingEvent);
                }
                ////级联打包了不定位了
                if (RF.GetById<PackingRelation>(packingEvent.OuterPackingRelation.Id).GetTreePId() != null) return;
                packingRelationView.Current = packingEvent.OuterPackingRelation;
                ExpandNode(packingEvent);
            }
            else
            {
                packingViewData.Helper.FailedEventHandler(packingEvent);
            }
        }

        private void ExpandNode(PackingStrategyEvent packingEvent)
        {
            var treeView = packingRelationView.Control.View as TreeListView;
            treeView?.CollapseAllNodes();
            var node = treeView?.GetNodeByContent(treeView.Nodes.FirstOrDefault(p => (p.Content as PackingRelation).Id == packingEvent.OuterPackingRelation.Id));
            treeView?.ExpandNode(node == null ? 0 : node.RowHandle);
        }

        private void DoPacking(DoPackingEvent packingEvent)
        {
            if (packingEvent.Group != "MesPacking") return;
            try
            {
                if (packingEvent.DoPackingAction == DoPackingAction.DoPacking)
                {
                    packingRelationView.Current = packingEvent.OuterPackingRelations.Last();
                    if (packingRelationView.SelectedEntities.Count == 0)
                    {
                        var pack = packingEvent.OuterPackingRelations.Last();
                        SetSelectedItems(pack);
                    }
                    packingRelationView.Commands.Find<PackingCommand>()?.TryExecute();
                }
                else
                {
                    if (packingViewData.IsAutoPrintLabel)
                    {
                        packingRelationView[PrintBarcodeCommand.NotPrintThrowException] = false;
                        packingRelationView[PrintBarcodeCommand.EnableMultiple] = true;
                        for (int i = 0; i < packingEvent.OuterPackingRelations.Length; i++)
                        {
                            var pack = packingEvent.OuterPackingRelations[i];
                            if (!GetPackageRule(pack).IsPrint) continue;
                            SetSelectedItems(pack);
                        }
                        packingRelationView.Commands.Find<PrintBarcodeCommand>()?.TryExecute();
                    }
                }
            }
            finally
            {
                packingRelationView[PrintBarcodeCommand.NotPrintThrowException] = true;
                packingRelationView[PrintBarcodeCommand.EnableMultiple] = false;
                packingRelationView.RefreshControl();
                packingRelationView.Data.MarkSaved();
            }
        }

        /// <summary>
        /// 设置选中项
        /// </summary>
        /// <param name="pack">旧包装关系</param>
        private void SetSelectedItems(PackingRelation pack)
        {
            var oldPack = packingRelationView.Control.SelectedItems.OfType<PackingRelation>().FirstOrDefault(p => p.Id == pack?.Id);
            if (oldPack != null)
                packingRelationView.Control.SelectedItems.Remove(oldPack);
            packingRelationView.Control.SelectedItems.Add(pack);
        }

        /// <summary>
        /// 获取包装规则
        /// </summary>
        /// <param name="doPacked">包装关系</param>
        /// <returns>工单包装规则明细</returns>
        private WorkOrderPackageRuleDetail GetPackageRule(PackingRelation doPacked)
        {
            var wo = RF.GetById<WorkOrder>(doPacked.WorkOrderId);
            return RT.Service.Resolve<WipPackingController>().GetPackageRuleDetail(doPacked.PackageUnit, wo);
        }

        /// <summary>
        /// 取消事件总线订阅
        /// </summary>
        /// <param name="ui">控件结果</param>
        private void UnsubscribebeEventBus(ControlResult ui)
        {
            ui.MainView.Closed += (s, e) =>
            {
                RT.EventBus.Unsubscribe<DoPackingEvent>(this);
                RT.EventBus.Unsubscribe<PackingStrategyEvent>(this);
            };
        }
    }
}