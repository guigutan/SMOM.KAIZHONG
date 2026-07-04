using DevExpress.Xpf.Core;
using System.Windows;

namespace SIE.Wpf.MES.WIP.Moves
{
    /// <summary>
    /// 过站采集模板
    /// </summary>
    public class MoveUITemplate : CollectionUITemplate
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MoveUITemplate() : base(typeof(MoveViewModel))
        {
        }

        /// <summary>
        /// UI创建
        /// </summary>
        /// <param name="ui">UI</param>
        protected override void OnUIGenerated(ControlResult ui)
        {
            var model = new MoveViewModel();
            var layout = ui.Control as DockLayout;
            var tabs = new DXTabControl();
            tabs.Margin = new Thickness(5, 5, 5, 0);
            tabs.Items.Add(CreateTabItem("采集记录", CreateDetailListControl(ui.MainView, model.CollectDetailList)));
            CreateReportTaskControl(tabs, ui.MainView, model);
            //如果不用显示消息列表，则注释下面这句
            tabs.Items.Add(CreateTabItem("消息列表", CreateMessagerControl(model)));
            layout.Children.Add(CreateOperationControl(model.Workstation));
            layout.Children.Add(tabs);
            ui.MainView.Data = model;
            base.OnUIGenerated(ui);
        }
    }
}