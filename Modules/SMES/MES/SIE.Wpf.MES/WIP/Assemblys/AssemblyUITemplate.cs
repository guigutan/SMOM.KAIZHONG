using DevExpress.Xpf.Core;

namespace SIE.Wpf.MES.WIP.Assemblys
{
    /// <summary>
    /// 上料采集模板
    /// </summary>
    public class AssemblyUITemplate : CollectionUITemplate
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public AssemblyUITemplate() : base(typeof(AssemblyViewModel))
        {
        }

        /// <summary>
        /// UI创建后
        /// </summary>
        /// <param name="ui">UI控件</param>
        protected override void OnUIGenerated(ControlResult ui)
        {
            var model = new AssemblyViewModel();
            var tabs = new DXTabControl();
            tabs.Margin = new System.Windows.Thickness(5, 5, 5, 0);
            tabs.Items.Add(CreateTabItem("装配清单", CreateDetailListControl(ui.MainView, model.AssemblyDetailList, ViewConfig.ListView)));
            tabs.Items.Add(CreateTabItem("上料明细", CreateDetailListControl(ui.MainView, model.LoadItemList)));
            tabs.Items.Add(CreateTabItem("下料明细", CreateDetailListControl(ui.MainView, model.UnloadItemList)));
            tabs.Items.Add(CreateTabItem("工位配送", CreateDetailListControl(ui.MainView, model.MoveItemList)));
            tabs.Items.Add(CreateTabItem("采集记录", CreateDetailListControl(ui.MainView, model.CollectDetailList)));            
            CreateReportTaskControl(tabs, ui.MainView, model);
            //如果不用显示消息列表，则注释下面这句
            tabs.Items.Add(CreateTabItem("消息列表", CreateMessagerControl(model)));

            var layout = ui.Control as DockLayout;
            layout.Children.Add(CreateOperationControl(model.Workstation));
            layout.Children.Add(tabs);
            ui.MainView.Data = model;
            base.OnUIGenerated(ui);
        }
    }
}
