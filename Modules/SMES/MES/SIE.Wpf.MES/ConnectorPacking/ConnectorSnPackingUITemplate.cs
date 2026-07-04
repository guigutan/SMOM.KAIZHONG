using DevExpress.Xpf.Core;
using SIE.Wpf.MES.ConnectorPackings;
using SIE.Wpf.MES.WIP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SIE.Wpf.MES.ConnectorPacking
{
    /// <summary>
    /// 装箱QC
    /// </summary>
    public class ConnectorSnPackingUITemplate : KZCollectionUITemplate
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ConnectorSnPackingUITemplate() : base(typeof(ConnectorSnPackingViewModel))
        {

        }
        /// <summary>
        /// UI创建
        /// </summary>
        /// <param name="ui"></param>
        protected override void OnUIGenerated(ControlResult ui)
        {
            var model = new ConnectorSnPackingViewModel();
            var layout = ui.Control as DockLayout;
            var tabs = new DXTabControl();
            tabs.Margin = new Thickness(5, 5, 5, 0);
            tabs.Items.Add(CreateTabItem("包装明细", CreateDetailListControl(ui.MainView, model.PackageSnRecordList, WPFViewConfig.ListView)));
            layout.Children.Add(CreateOperationControl(model.KZWorkstation));
            layout.Children.Add(tabs);
            ui.MainView.Data = model;
            base.OnUIGenerated(ui);
        }
    }
}
