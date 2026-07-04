using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.MetaModel.View;
using SIE.Wpf.MES.WorkOrders.Routings;
using System.Windows.Controls;

namespace SIE.Wpf.MES.WorkOrders
{
    /// <summary>
    /// 工单明细模板
    /// </summary>
    public class WorkOrderDetailTemplate : DetailsUITemplate
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="viewGroup">视图组</param>
        public WorkOrderDetailTemplate(string viewGroup) : base(typeof(WorkOrder))
        {
            ViewGroup = viewGroup;
        }

        /// <summary>
        /// 定义块
        /// </summary>
        /// <returns>聚合块</returns>
        protected override AggtBlocks DefineBlocks()
        {
            var blocks = base.DefineBlocks();
            blocks.Layout = new LayoutMeta(typeof(DockLayout));
            return blocks;
        }

        /// <summary>
        /// 定义字块
        /// </summary>
        /// <param name="entityMeta">实体元数据</param>
        /// <param name="aggtBlocks">聚合块</param>
        /// <param name="mainBlock">主块</param>
        protected override void DefineChildBlocks(EntityMeta entityMeta, AggtBlocks aggtBlocks, Block mainBlock)
        {
            base.DefineChildBlocks(entityMeta, aggtBlocks, mainBlock);
        }

        /// <summary>
        /// UI创建后
        /// </summary>
        /// <param name="ui">UI控件</param>
        protected override void OnUIGenerated(ControlResult ui)
        {
            var layout = ui.Control as DockLayout;
            var control = new WorkOrderRoutingView();
            control.Margin = new System.Windows.Thickness(-10);
            layout.TabControl.Items.Add(new TabItem() { Content = control, Header = "工艺路线" });
            //var packageRuleView = ui.MainView.ChildrenViews.FirstOrDefault(p => p.EntityType == typeof(WorkOrderPackageRuleDetail));
            //packageRuleView.Control.Width = 908;
            ui.MainView.DataChanged += (s, e) =>
            {
                foreach (var child in ui.MainView.ChildrenViews)
                {
                    child.IsActive = true;
                }

                var workOrder = ui.MainView.Data as WorkOrder;
                control.container.LoadFromXmlString(GetLayout(workOrder));
                workOrder.PropertyChanged += (o, ee) =>
                  {
                      if (ee.PropertyName == WorkOrder.VersionIdProperty.Name)
                          control.container.LoadFromXmlString(GetLayout(workOrder));
                  };
            };
        }

        /// <summary>
        /// 获取布局
        /// </summary>
        /// <param name="workOrder">工单</param>
        /// <returns>布局</returns>
        string GetLayout(WorkOrder workOrder)
        {
            if (workOrder.Layout != null)
                return workOrder.Layout.Layout;
            return workOrder.Version?.Layout?.Layout;
        }
    }

    /// <summary>
    /// Dock布局
    /// </summary>
    public class DockLayout : DockPanel, ILayoutControl
    {
        /// <summary>
        /// tab控件
        /// </summary>
        public TabControl TabControl = new TabControl();

        /// <summary>
        /// 布局
        /// </summary>
        /// <param name="components">UI组件</param>
        public void Arrange(UIComponents components)
        {
            var toolBar = components.CommandsContainer;
            if (toolBar != null)
            {
                toolBar.Control.Margin = new System.Windows.Thickness(5, 5, 5, 0);
                SetDock(toolBar.Control, Dock.Top);
                Children.Add(toolBar.Control);
            }

            var control = components.Main;
            if (control != null)
            {
                SetDock(control.Control, Dock.Top);
                Children.Add(control.Control);
            }

            if (components.Children.Count == 0)
                return;

            foreach (var item in components.Children)
            {
                var child = item.ControlResult.Control;
                child.Margin = new System.Windows.Thickness(-10);
                TabControl.Items.Add(new TabItem
                {
                    Content = child,
                    Header = item.Label,
                    DataContext = item.ControlResult.MainView.Data
                });
            }
            TabControl.Margin = new System.Windows.Thickness(5);
            Children.Add(TabControl);
        }
    }
}
