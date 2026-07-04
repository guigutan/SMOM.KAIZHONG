using DevExpress.Xpf.Core;
using SIE.Domain;
using SIE.MES.WIP.Repairs;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SIE.Wpf.MES.WIP.Repairs
{
    /// <summary>
    /// 维修采集模板
    /// </summary>
    public class RepairUITemplate : CollectionUITemplate
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public RepairUITemplate() : base(typeof(RepairViewModel))
        {
        }

        /// <summary>
        /// 创建UI
        /// </summary>
        /// <param name="ui">控件结果</param>
        protected override void OnUIGenerated(ControlResult ui)
        {
            var model = new RepairViewModel { ModuleKey = ui.MainView.ModuleKey };
            var tabs = new DXTabControl();
            tabs.Margin = new Thickness(5, 5, 5, 0);
            tabs.Items.Add(CreateTabItem("不良信息", CreateDefectControl(ui.MainView, model.RepairDefectList, model.DetailList)));
            tabs.Items.Add(CreateTabItem("上料明细", CreateDetailListControl(ui.MainView, model.LoadItemList)));
            tabs.Items.Add(CreateTabItem("下料明细", CreateDetailListControl(ui.MainView, model.UnloadItemList)));
            tabs.Items.Add(CreateTabItem("采集记录", CreateDetailListControl(ui.MainView, model.CollectDetailList)));
            //如果不用显示消息列表，则注释下面这句
            tabs.Items.Add(CreateTabItem("消息列表", CreateMessagerControl(model)));
            var layout = ui.Control as DockLayout;
            layout.Children.Add(CreateOperationControl(model.Workstation));
            layout.Children.Add(tabs);
            ui.MainView.Data = model;
            base.OnUIGenerated(ui);
        }

        /// <summary>
        /// 创建缺陷控件
        /// </summary>
        /// <param name="mainView">主视图</param>
        /// <param name="defectList">缺陷列表</param>
        /// <param name="details">装配明细</param>
        /// <returns>缺陷控件</returns>
        FrameworkElement CreateDefectControl(LogicalView mainView, EntityList<RepairDefectViewModel> defectList, EntityList<ProductAssemblyDetailViewModel> details)
        {
            var panel = new Grid();
            panel.RowDefinitions.Add(new RowDefinition() { });
            panel.ColumnDefinitions.Add(new ColumnDefinition() { });
            panel.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            panel.ColumnDefinitions.Add(new ColumnDefinition() { });
            var defectContol = CreateDetailListControl(mainView, defectList, RepairDefectViewModelViewConfig.RepairView);
            panel.Children.Add(defectContol);
            GridSplitter splitter = new GridSplitter()
            {
                Width = 5,
                VerticalAlignment = VerticalAlignment.Stretch,
                ShowsPreview = true,
                HorizontalAlignment = HorizontalAlignment.Center,
                Background = new SolidColorBrush(Colors.Transparent)
            };
            panel.Children.Add(splitter);
            var detailControl = CreateDetailListControl(mainView, details, ProductAssemblyDetailViewModelViewConfig.RepairView);
            panel.Children.Add(detailControl);
            Grid.SetColumn(defectContol, 0);
            Grid.SetColumn(splitter, 1);
            Grid.SetColumn(detailControl, 2);

            return panel;
        }
    }
}
