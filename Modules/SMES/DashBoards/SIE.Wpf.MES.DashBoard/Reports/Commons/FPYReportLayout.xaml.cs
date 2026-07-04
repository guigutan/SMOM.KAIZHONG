using System;
using System.Windows.Controls;

namespace SIE.Wpf.MES.DashBoard.Reports.Commons
{
    /// <summary>
    /// SchedulingLayout.xaml 的交互逻辑
    /// </summary>
    public partial class FPYReportLayout : UserControl, ILayoutControl
    {
        /// <summary>
        /// 直通率报表通用控件
        /// </summary>
        DirectRateBasePivotGridControl _reportControl;

        /// <summary>
        /// 构造函数
        /// </summary>
        public FPYReportLayout()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 布局
        /// </summary>
        /// <param name="components">控件容器</param>
        public virtual void Arrange(UIComponents components)
        {
            var mainView = components.Main.MainView as ListLogicalView;
            _reportControl = new DirectRateBasePivotGridControl();
            mainContainer.Content = _reportControl;
            mainView.DataChanged += DataChanged;
            if (components.Condition != null)
            {
                criteriaContainer.Content = components.Condition.Control;
            }
        }

        /// <summary>
        /// 查询事件触发数据源变更
        /// </summary>
        /// <param name="sender">对应主视图</param>
        /// <param name="e">事件参数</param>
        private void DataChanged(object sender, EventArgs e)
        {
            var mainView = sender as ListLogicalView;
            var viewModel = mainView.Data[0];
            _reportControl.DataContext = viewModel;
        }
    }
}