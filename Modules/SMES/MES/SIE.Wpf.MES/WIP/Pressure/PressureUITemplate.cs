using DevExpress.Xpf.Core;
using System.Linq;
using System.Windows;

namespace SIE.Wpf.MES.WIP.Pressure
{
    /// <summary>
    /// 耐压采集模板
    /// </summary>
    public class PressureUITemplate : KZCollectionUITemplate
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        public PressureUITemplate() : base(typeof(PressureViewModel))
        {
        }

        /// <summary>
        /// 创建UI
        /// </summary>
        /// <param name="ui"></param>
        protected override void OnUIGenerated(ControlResult ui)
        {
            var model = new PressureViewModel();
            var tabs = new DXTabControl();
            tabs.Margin = new Thickness(5, 5, 5, 0);
            tabs.Items.Add(CreateTabItem("SN明细", CreateDetailListControl(ui.MainView, model.SnDetailList, WPFViewConfig.ListView)));
            var layout = ui.Control as DockLayout;
            layout.Children.Add(CreateOperationControl(model.KZWorkstation));
            layout.Children.Add(tabs);
            ui.MainView.Data = model;
            model.SnDetailListView = ui.MainView.Relations.FirstOrDefault(p => p.View.EntityType == typeof(SIE.MES.WIP.Pressure.WipPressureSn))?.View;
            base.OnUIGenerated(ui);
        }
    }
}
