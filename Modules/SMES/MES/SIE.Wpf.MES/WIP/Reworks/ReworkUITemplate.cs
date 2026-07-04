using DevExpress.Xpf.Core;
using SIE.Domain;
using SIE.MES.WIP.Products;
using System.Windows;

namespace SIE.Wpf.MES.WIP.Reworks
{
    /// <summary>
    /// 返工采集模板
    /// </summary>
    public class ReworkUITemplate : CollectionUITemplate
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public ReworkUITemplate() : base(typeof(ReworkViewModel))
        {
        }

        /// <summary>
        /// 返工采集UI创建后的调用方法
        /// </summary>
        /// <param name="ui">UI控件</param>
        protected override void OnUIGenerated(ControlResult ui)
        {
            ////base.OnUIGenerated(ui);
            var model = new ReworkViewModel();
            model.ShowTips("返工采集");
            var layout = ui.Control as DockLayout;
            layout.Children.Add(CreateOperationControl(model.Workstation));
            layout.Children.Add(CreateReworkTabControl(ui.MainView, model));

            ui.MainView.Data = model;
            base.OnUIGenerated(ui);
        }

        /// <summary>
        /// 创建返工UI控件
        /// </summary>
        /// <param name="mainView">视图对象</param>
        /// <param name="model">采集视图模型</param>
        /// <returns>返回UI</returns>
        protected FrameworkElement CreateReworkTabControl(LogicalView mainView, ReworkViewModel model)
        {
            EntityList collectDetails = model.CollectDetailList;
            var leftThickness = new Thickness(7, 5, 0, 0);
            var leftContent = CreateDetailListControl(mainView, collectDetails, CollectDetailViewModelViewConfig.ReworkViewGroup);
            var tabLeft = CreateTabControl(leftThickness, "采集记录", leftContent);

            var rightThickness = new Thickness(5, 5, 5, 0);
            var rightContent = new ReworkKeyItemControl(model);
            var tabRight = CreateTabControl(rightThickness, "关键件", rightContent);

            var reworkCtl = CreateReworkControl(tabLeft, tabRight, model);
            return reworkCtl;
        }

        /// <summary>
        /// 创建UI--DXTabControl
        /// </summary>
        /// <param name="thickness">Marggin值</param>
        /// <param name="header">标题</param>
        /// <param name="content">主内容</param>
        /// <returns>DXTabControl</returns>
        private DXTabControl CreateTabControl(Thickness thickness, string header, FrameworkElement content)
        {
            var newTab = new DXTabControl();
            newTab.Margin = thickness;
            newTab.Items.Add(CreateTabItem(header, content));
            return newTab;
        }

        /// <summary>
        /// 创建返工控件(含2个TabControl)
        /// </summary>
        /// <param name="tabLeft">左TabControl</param>
        /// <param name="tabRight">右TabControl</param>
        /// <param name="model">返工采集视图</param>
        /// <returns>返工采集UI</returns>
        private FrameworkElement CreateReworkControl(DXTabControl tabLeft, DXTabControl tabRight, ReworkViewModel model)
        {
            #region 注释掉
            /*var grdPanel = new Grid();
            grdPanel.ColumnDefinitions.Add(new ColumnDefinition());
            grdPanel.ColumnDefinitions.Add(new ColumnDefinition());
            Grid.SetColumn(tabLeft, 0);
            Grid.SetColumn(tabRight, 1);
            grdPanel.Children.Add(tabLeft);
            grdPanel.Children.Add(tabRight);
            return grdPanel;*/
            #endregion
            var reworkCtl = new ReworkControl(tabLeft, tabRight, model);
            return reworkCtl;
        }
    }
}
