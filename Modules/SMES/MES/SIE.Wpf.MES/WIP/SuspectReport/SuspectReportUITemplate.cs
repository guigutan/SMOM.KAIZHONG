using DevExpress.Xpf.Core;
using System.Windows;

namespace SIE.Wpf.MES.WIP.SuspectReport
{
    /// <summary>
    /// 可疑品报工
    /// </summary>
    public class SuspectReportUITemplate : KZCollectionUITemplate
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SuspectReportUITemplate() : base(typeof(SuspectReportViewModel))
        {

        }

        /// <summary>
        /// UI创建
        /// </summary>
        /// <param name="ui"></param>
        protected override void OnUIGenerated(ControlResult ui)
        {
            var model = new SuspectReportViewModel();
            var layout = ui.Control as DockLayout;
            var tabs = new DXTabControl();
            tabs.Margin = new Thickness(5, 5, 5, 0);
            //tabs.Items.Add(CreateTabItem("包装明细",CreateDetailListControl(ui.MainView,model.PackageSnRecordList,WPFViewConfig.ListView)));
            layout.Children.Add(CreateOperationControl(model.KZWorkstation));
            layout.Children.Add(tabs);
            ui.MainView.Data = model;
            base.OnUIGenerated(ui);
        }
    }
}
