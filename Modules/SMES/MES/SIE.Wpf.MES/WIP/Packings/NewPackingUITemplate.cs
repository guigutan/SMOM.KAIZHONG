using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel.View;
using SIE.Packages;
using SIE.Packages.ItemLabels;
using SIE.Packages.Packings;
using SIE.Packages.Packings.Strategys;
using System;
using System.Linq;
using System.Windows;
using static Stimulsoft.Report.WpfDesign.StiBuilder;

namespace SIE.Wpf.MES.WIP.Packings
{
    /// <summary>
    /// 包装采集模板
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NewPackingUITemplate<T> : CollectionUITemplate where T : NewPackingViewModel
    {
        /// <summary>
        /// 包装关系视图
        /// </summary>
        ListLogicalView packingRelationView;

        /// <summary>
        /// 包装ViewModel
        /// </summary>
        NewPackingViewModel packingViewData;

        /// <summary>
        /// 构造函数
        /// </summary>
        public NewPackingUITemplate() : base(typeof(T))
        {
            ViewGroup = "Packing";
        }

        /// <summary>
        /// 定义块
        /// </summary>
        /// <returns>聚合块</returns>
        protected override AggtBlocks DefineBlocks()
        {
            var block = base.DefineBlocks();
            block.Layout = new LayoutMeta(typeof(PackingLayout));
            return block;
        }

        /// <summary>
        /// UI创建后
        /// </summary>
        /// <param name="ui">ui</param>
        protected override void OnUIGenerated(ControlResult ui)
        {
            packingViewData = new NewPackingViewModel();
            var packingView = ui.MainView as DetailLogicalView;
            packingView.Data = packingViewData;
            var layout = ui.Control as PackingLayout;
            //工作单元
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
        /// 初始化子视图
        /// </summary>
        /// <param name="mainView">主视图</param>
        /// <param name="model">包装ViewModel</param>
        /// <returns>Dev原生Tab控件</returns>
        private DXTabControl InitChildrenView(DetailLogicalView mainView, NewPackingViewModel model)
        {
            DXTabControl tab = new DXTabControl();
            tab.Margin = new Thickness(5, 5, 5, 0);
            tab.Items.Add(CreateTabItem("物料标签", CreateChildListControl(mainView, model.ItemLabelList)));
            tab.Items.Add(CreateTabItem("包装规则明细", CreateChildListControl(mainView, model.PackageRuleDetailList)));
            //如果不用显示消息列表，则注释下面这句
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
            packingRelationView.Data = packingViewData.PackingRelations;
            if (packingView.ChildrenViews.Any(f => f.EntityType == typeof(PackageRuleDetail)))
                throw new ValidationException("[包装规则]块无权限,请联系管理员角色权限配置".L10N());
            var itemLableView = packingView.ChildrenViews.FirstOrDefault(f => f.EntityType == typeof(ItemLabel)) as ListLogicalView;
            if (itemLableView == null)
                throw new ValidationException("[物料标签]块无权限,请联系管理员配置".L10N());
        }

        /// <summary>
        /// 块定义
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        protected override void Template_BlocksDefined(object sender, CodeBlocksDefinedEventArgs e)
        {
            if (e.Blocks.MainBlock.EntityType == typeof(ItemLabel))
                e.Blocks.Surrounders.Clear();
        }

        private void SubscribeEventBus()
        {
            ////订阅打包事件           
            RT.EventBus.Subscribe<NewPackingStrategyEvent>(this, PackingStrategy);
        }

        private void PackingStrategy(NewPackingStrategyEvent packingEvent)
        {
            if (packingEvent.Group != "MesPacking") return;

            if (packingEvent.IsSucess)
            {
                if (packingEvent.StrategyType == ScanStrategyMode.ScanSingle)
                {
                    //正常模式
                    packingViewData.Helper.SucceedEventHandler(packingEvent);
                }
                else if (packingEvent.StrategyType == ScanStrategyMode.ScanOneJoinToMany)
                {
                    //加入模式
                    packingViewData.Helper.ScanChildPackageSucceedEventHandler(packingEvent);
                }

                //级联打包了不定位了
                if (RF.GetById<PackingRelation>(packingEvent.OuterPackingRelation.Id).GetTreePId() != null)
                {
                    return;
                }

                ExpandNode(packingEvent);

                //将扫描的包装，设为当前节点
                var packingRelation = packingViewData.PackingRelations
                    .FirstOrDefault(p => p.Id == packingEvent.OuterPackingRelation.Id);

                if (packingRelation != null)
                {
                    packingRelationView.Current = packingRelation;
                }
            }
            else
            {
                packingViewData.Helper.FailedEventHandler(packingEvent);
            }
        }

        /// <summary>
        /// 展开节点
        /// </summary>
        /// <param name="packingEvent"></param>
        private void ExpandNode(NewPackingStrategyEvent packingEvent)
        {
            var treeView = packingRelationView.Control.View as TreeListView;

            if (treeView == null)
            {
                return;
            }

            treeView.CollapseAllNodes();

            var tn = treeView.Nodes.FirstOrDefault(p => (p.Content as PackingRelation).Id == packingEvent.OuterPackingRelation.Id);

            var node = treeView.GetNodeByContent(tn);

            packingRelationView.Current = packingEvent.OuterPackingRelation;

            treeView.ExpandNode(node == null ? 0 : node.RowHandle);
        }

        /// <summary>
        /// 取消事件总线订阅
        /// </summary>
        /// <param name="ui">控件结果</param>
        private void UnsubscribebeEventBus(ControlResult ui)
        {
            ui.MainView.Closed += (s, e) =>
            {
                RT.EventBus.Unsubscribe<NewPackingStrategyEvent>(this);
            };
        }
    }
}
