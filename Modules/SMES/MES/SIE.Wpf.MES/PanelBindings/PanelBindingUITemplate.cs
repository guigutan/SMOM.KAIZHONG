using DevExpress.Xpf.Core;
using SIE.Wpf.MES.PanelBindings.Controls;
using SIE.Wpf.MES.WIP;
using System.Windows;

namespace SIE.Wpf.MES.PanelBindings
{
    /// <summary>
    /// 工单条码绑定模板
    /// </summary>
    public class PanelBindingUITemplate : CollectionUITemplate
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PanelBindingUITemplate() : base(typeof(PanelBindingViewModel))
        {
        }

        /// <summary>
        /// UI创建
        /// </summary>
        /// <param name="ui">UI</param>
        protected override void OnUIGenerated(ControlResult ui)
        {
            var model = new PanelBindingViewModel();
            var layout = ui.Control as DockLayout;
            var leftTab = new DXTabControl();
            var leftControl = CreateTabItem("拼板码", CreateDetailListControl(ui.MainView, model.PanelList, ViewConfig.ListView));
            leftTab.Items.Add(leftControl);
            var rightTab = new DXTabControl();
            var rightControl = CreateTabItem("SN条码", CreateDetailListControl(ui.MainView, model.SnList, ViewConfig.ListView));
            rightTab.Items.Add(rightControl);
            var content = new SplitterControl(leftTab, rightTab);
            content.Margin = new Thickness(5);
            layout.Children.Add(content);
            ui.MainView.Data = model;
            base.OnUIGenerated(ui);
        }
    }
}