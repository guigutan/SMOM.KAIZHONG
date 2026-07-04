using SIE.Barcodes.WipBatchs;
using SIE.Domain;
using SIE.MES.BatchWIP.Products;
using SIE.Security;
using SIE.Wpf.MES.ProductRoutings;
using System;
using System.Windows.Controls;

namespace SIE.Wpf.MES.BatchProductRoutings
{
    /// <summary>
    /// RoutingLayout.xaml 的交互逻辑
    /// </summary>
    public partial class RoutingLayout : UserControl
    {
        BatchRoutingViewModel model = new BatchRoutingViewModel();

        /// <summary>
        /// 模块Key
        /// </summary>
        string moduleKey;

        /// <summary>
        /// 模块Key
        /// </summary>
        string ModuleKey
        {
            get
            {
                if (moduleKey.IsNullOrEmpty())
                    moduleKey = RT.Service.Resolve<IFindModule>().FindModuleKey(typeof(BatchWipProductRouting));
                return moduleKey;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public RoutingLayout()
        {
            InitializeComponent();
            InitProductView();
            InitTab();
            design.SelectedActivityChanged += (activity) =>
            {
                model.SelectedItem = activity;
                model.SelectedNodeChanged();
            };
        }

        /// <summary>
        ///  初始化工单、条码的界面
        /// </summary>
        void InitProductView()
        {
            var template = new BatchRoutingTemplate();
            template.ModuleKey = ModuleKey;
            var ui = template.CreateUI();
            var listView = ui.MainView as ListLogicalView;
            if (listView != null)
            {
                listView.Control.SelectedItemChanged += (o, e) =>
                {
                    var wipBatch = listView.Current as WipBatch;
                    model.InitInfo(wipBatch);
                    design.InitModel(model);
                };
            }

            productGrid.Children.Add(ui.Control);
        }

        /// <summary>
        /// 初始化产品生产关键件、产品测试结果、工序BOM、产品缺陷记录、产品维修记录、产品工艺路线修改事件
        /// </summary>
        void InitTab()
        {
            tabControl.Margin = new System.Windows.Thickness(5);
            tabControl.Items.Add(CreateTabItem(model.KeyItemList));
            var template = new ListUITemplate(typeof(BomViewModel), ViewConfig.ListView, ModuleKey);
            var ui = template.CreateUI();
            ui.MainView.Data = model.BomList;
            ui.MainView["Model"] = model;
            ui.Control.Margin = new System.Windows.Thickness(-10);
            var item = new TabItem() { Content = ui.Control };
            item.SetResourceBinding(TabItem.HeaderProperty, ui.MainView.Meta.Label);
            tabControl.Items.Add(item);
            tabControl.Items.Add(CreateTabItem(model.DefectList));
            tabControl.Items.Add(CreateTabItem(model.RepaireList));
            tabControl.Items.Add(CreateTabItem(model.RoutingEventList, "工艺路线变更事件"));
        }

        /// <summary>
        /// 创建页签
        /// </summary>
        /// <param name="entityList">数据集合</param>
        /// <param name="header">标题</param>
        /// <returns>返回新的页签</returns>
        TabItem CreateTabItem(EntityList entityList, string header = "")
        {
            var view = AutoUI.ViewFactory.CreateListView(entityList.EntityType, ViewConfig.ListView, ModuleKey);
            view.Data = entityList;
            var control = view.Control;
            control.Margin = new System.Windows.Thickness(-10);
            var tabItem = new TabItem { Content = control };
            tabItem.SetResourceBinding(TabItem.HeaderProperty, header.IsNullOrEmpty() ? view.Meta.Label : header);
            return tabItem;
        }
    }
}