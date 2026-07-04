using DevExpress.Xpf.Core;
using DevExpress.Xpf.LayoutControl;
using SIE.Wpf.MES.WIP;
using System.Windows;
using System.Windows.Controls;

namespace SIE.Wpf.MES.BatchWIP.Packings.Layouts
{
    /// <summary>
    /// BatchPackingLayout.xaml 的交互逻辑
    /// </summary>
    public partial class BatchPackingLayout : UserControl, ILayoutControl
    {
        /// <summary>
        /// 批次包装采集布局构造函数
        /// </summary>
        public BatchPackingLayout()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 页面内容编排
        /// </summary>
        /// <param name="components">内容组件</param>
        public void Arrange(UIComponents components)
        {
            if (components.CommandsContainer != null)
            {
                toolBar.Margin = new Thickness(5, 5, 5, 0);
                toolBar.Content = components.CommandsContainer.Control;
            }
            var control = components.Main.Control as LayoutControl;
            WipLayoutHelper.SetPanelMargin(control);
            mainView.Content = control;
        }

        /// <summary>
        /// 初始化进站批次视图
        /// </summary>
        /// <param name="ui">视图结果</param>
        public void InitBatchListView(ControlResult ui)
        {
            batchAndPacking.InitBatchListView(ui);
        }

        /// <summary>
        /// 初始化包装关系
        /// </summary>
        /// <param name="ui">视图结果</param>
        public void InitPackingListView(ControlResult ui)
        {
            batchAndPacking.InitPackingListView(ui);
        }

        /// <summary>
        /// 初始化包装规则明细
        /// </summary>
        /// <param name="ui">视图结果</param>
        public void InitPkgRuleDetailView(ControlResult ui)
        {
            ui.Control.Margin = new Thickness(-10);
            var item = new DXTabItem
            {
                Content = ui.Control,
            };
            item.SetResourceBinding(DXTabItem.HeaderProperty, ui.MainView.Meta.Label);
            childrenView.Items.Add(item);
        }

        /// <summary>
        /// 初始化工作站信息
        /// </summary>
        /// <param name="workstation">工作站</param>
        public void InitWorkstation(FrameworkElement workstation)
        {
            workStation.Content = workstation;
        }
    }
}