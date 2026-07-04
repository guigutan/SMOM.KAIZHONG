using SIE.Barcodes;
using SIE.Domain;
using SIE.MES.WIP.Products;
using SIE.Security;
using System;
using System.Windows.Controls;

namespace SIE.Wpf.MES.ProductRoutings
{
    /// <summary>
    /// ProductRoutingLayout.xaml 的交互逻辑
    /// </summary>
    public partial class ProductRoutingLayout : UserControl
    {
        /// <summary>
        /// 产品工艺路线 ViewModel
        /// </summary>
        ProductRoutingViewModel model = new ProductRoutingViewModel();

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
                    moduleKey = RT.Service.Resolve<IFindModule>().FindModuleKey(typeof(WipProductRouting));
                return moduleKey;
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public ProductRoutingLayout()
        {
            InitializeComponent();
            InitProductView();
            InitTab();
            design.SelectedActivityChanged += (o) =>
            {
                model.SelectedItem = o;
                model.SelectedNodeChanged();
            };
        }

        /// <summary>
        ///  初始化工单、条码的界面
        /// </summary>
        void InitProductView()
        {
            var template = new ProductRoutingTemplate();
            template.ModuleKey = ModuleKey;
            var ui = template.CreateUI();
            var listView = ui.MainView as ListLogicalView;
            if (listView != null)
            {
                listView.Control.SelectedItemChanged += (o, e) =>
                  {
                      var barcode = listView.Current as Barcode;
                      model.InitInfo(barcode);
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
            tabControl.Items.Add(CreateTabItem(model.KeyItemList));
            tabControl.Items.Add(CreateTabItem(model.TestResultList));
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
            tabControl.Items.Add(CreateTabItem(model.RoutingEventList));
        }

        /// <summary>
        /// 创建页签
        /// </summary>
        /// <param name="lst">数据集合</param>
        /// <returns>返回新的页签</returns>
        TabItem CreateTabItem(EntityList lst)
        {
            var view = AutoUI.ViewFactory.CreateListView(lst.EntityType, ViewConfig.ListView, ModuleKey);
            view.Data = lst;
            var control = view.Control;
            control.Margin = new System.Windows.Thickness(-10);
            var tabItem = new TabItem { Content = control };
            tabItem.SetResourceBinding(TabItem.HeaderProperty, view.Meta.Label);
            return tabItem;
        }
    }
}
