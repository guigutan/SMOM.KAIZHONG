using DevExpress.Xpf.Core;
using SIE.Wpf.MES.WIP;
using System.Windows;

namespace SIE.Wpf.MES.OnOffDuty
{
    /// <summary>
    /// 维修采集模板
    /// </summary>
    public class OnOffDutyUITemplate : CollectionUITemplate
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public OnOffDutyUITemplate() : base(typeof(OnOffDutyViewModel))
        {
        }

        /// <summary>
        /// 创建UI
        /// </summary>
        /// <param name="ui">控件结果</param>
        protected override void OnUIGenerated(ControlResult ui)
        {
            var model = new OnOffDutyViewModel { ModuleKey = ui.MainView.ModuleKey };
            var tabs = new DXTabControl();
            tabs.Margin = new Thickness(5, 5, 5, 0);
            tabs.Items.Add(CreateTabItem("采集记录", CreateDetailListControl(ui.MainView, model.OnOffDutyCollectDetailViewModelList)));
            //如果不用显示消息列表，则注释下面这句
            //tabs.Items.Add(CreateTabItem("消息列表", CreateMessagerControl(model)));
            var layout = ui.Control as DockLayout;
            layout.Children.Add(CreateOperationControl(model.Workstation));
            layout.Children.Add(tabs);
            ui.MainView.Data = model;
            base.OnUIGenerated(ui);
        }
    }
}
