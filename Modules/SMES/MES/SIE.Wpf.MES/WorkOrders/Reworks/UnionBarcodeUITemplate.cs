using SIE.Domain;
using SIE.MES.WorkOrders;
using SIE.MetaModel.View;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SIE.Wpf.MES.WorkOrders.Reworks
{
    /// <summary>
    /// 关联条码界面模板
    /// </summary>
    public class UnionBarcodeUITemplate : DetailsUITemplate
    {
        /// <summary>
        /// 返工工单
        /// </summary>
        private WorkOrder workOrder;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="workOrder">返工工单</param>
        public UnionBarcodeUITemplate(WorkOrder workOrder) : base(typeof(WorkOrderUnionBarcode))
        {
            this.workOrder = workOrder;
        }

        /// <summary>
        /// UI创建后
        /// </summary>
        /// <param name="ui">UI控件</param>
        protected override void OnUIGenerated(ControlResult ui)
        {
            var model = new WorkOrderUnionBarcode(workOrder);

            var panel = new DockPanel();
            panel.Margin = new Thickness(12, 10, 12, 10);
            panel.Children.Add(CreateTwoListShowControl(ui.MainView, model.BarcodeList, model.KeyItemList, ViewConfig.ListView, ViewConfig.ListView));
            var layout = ui.Control as DockLayout;
            layout.Children.Add(panel);
            ui.MainView.Data = model;
            base.OnUIGenerated(ui);
        }

        /// <summary>
        /// 创建两个平行显示的List组件
        /// </summary>
        /// <param name="mainView">主视图</param>
        /// <param name="first">第一个List</param>
        /// <param name="second">第二个List</param>
        /// <param name="firstViewGroup">第一个ViewGroup</param>
        /// <param name="secondViewGroup">第二个ViewGroup</param>
        /// <returns>明细控件</returns>
        protected virtual FrameworkElement CreateTwoListShowControl(LogicalView mainView, EntityList first, EntityList second, string firstViewGroup = null, string secondViewGroup = null)
        {
            var ui1 = CreateListControl(mainView, first, firstViewGroup);
            var ui2 = CreateListControl(mainView, second, secondViewGroup);
            return new TwoListControl(ui1, "关联的条码".L10N(), ui2, "关键件解绑".L10N());
        }

        /// <summary>
        /// 获取当前模板的结构定义。
        /// 结构定义包括：块间的结构、布局、块对应的视图的扩展名。
        /// </summary>
        /// <returns>聚合块</returns>
        protected override AggtBlocks DefineBlocks()
        {
            var blocks = base.DefineBlocks();
            blocks.Layout = new LayoutMeta(typeof(DockLayout));
            return blocks;
        }

        /// <summary>
        /// 创建明细列表控件
        /// </summary>
        /// <param name="mainView">视图对象</param>
        /// <param name="data">实体集合</param>
        /// <param name="viewGroup">视图组名称</param>
        /// <returns>返回UI</returns>
        protected ControlResult CreateListControl(LogicalView mainView, EntityList data, string viewGroup = null)
        {
            var moduleKey = RT.Service.Resolve<SIE.Security.IFindModule>().FindModuleKey(typeof(WorkOrder));
            UITemplate uiTemplate = new ListUITemplate(data.EntityType, viewGroup.IsNullOrEmpty() ? ViewGroup : viewGroup, moduleKey);
            var ui = uiTemplate.CreateUI();
            ui.MainView.Data = data;
            ui.MainView.Relations.Add(new RelationView("mainView", mainView));
            ui.Control.Margin = new Thickness(-10);
            return ui;
        }
    }

    /// <summary>
    /// DockLayout布局控件
    /// </summary>
    internal class DockLayout : DockPanel, ILayoutControl
    {
        /// <summary>
        /// 重置布局
        /// </summary>
        /// <param name="components">控件容器</param>
        public void Arrange(UIComponents components)
        {
            var toolBar = components.CommandsContainer;
            if (toolBar != null)
            {
                toolBar.Control.Margin = new Thickness(5);
                SetDock(toolBar.Control, Dock.Top);
                Children.Add(toolBar.Control);
            }

            var control = components.Main.Control as DevExpress.Xpf.LayoutControl.LayoutControl;
            if (control != null)
            {
                SetDock(control, Dock.Top);
                Children.Add(control);
            }
        }
    }
}