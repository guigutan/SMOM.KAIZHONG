using DevExpress.Xpf.Core;
using SIE.Wpf.MES.WIP;
using System.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Wpf.MES.OnOffDutyB
{

    /// <summary>
    /// UITemplate
    /// </summary>
    public class OnOffDutyBUITemplate : KZCollectionUITemplate //CollectionUITemplate
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public OnOffDutyBUITemplate() : base(typeof(OnOffDutyBViewModel))
        {
        }
        /// <summary>
        /// 创建UI
        /// </summary>
        /// <param name="ui">控件结果</param>
        protected override void OnUIGenerated(ControlResult ui)
        {
            var model = new OnOffDutyBViewModel { ModuleKey = ui.MainView.ModuleKey };
            var tabs = new DXTabControl();
            tabs.Margin = new Thickness(5, 5, 5, 0);
            tabs.Items.Add(CreateTabItem("采集记录", CreateDetailListControl(ui.MainView, model.OnOffDutyBCollectDetailViewModelList)));
            var layout = ui.Control as DockLayout;
            layout.Children.Add(tabs);
            ui.MainView.Data = model;
            base.OnUIGenerated(ui);
        }

    }
}
