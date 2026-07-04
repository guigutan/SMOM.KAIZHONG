using DevExpress.Xpf.Core;

namespace SIE.Wpf.MES.WIP.Inspects
{
    /// <summary>
    /// 检验采集模板
    /// </summary>
    public class InspectByItemUITemplate : CollectionUITemplate
    {
        /// <summary>
        /// 构造函数,指定实体类型
        /// </summary>
        public InspectByItemUITemplate() : base(typeof(InspectByItemViewModel))
        {
        }

        /// <summary>
        /// 添加当前模块中 UI 的初始化逻辑。
        /// 当使用自动生成的 UI 时，此方法会被调用。
        /// </summary>
        /// <param name="ui">控件结果</param>
        protected override void OnUIGenerated(ControlResult ui)
        {
            var model = new InspectByItemViewModel();
            var layout = ui.Control as DockLayout;

            var tabs = new DXTabControl();
            tabs.Items.Add(CreateTabItem("检验项目", CreateDetailListControl(ui.MainView, model.InspectionItemList, canEdit: true)));
            tabs.Items.Add(CreateTabItem("缺陷录入信息", CreateDetailListControl(ui.MainView, model.DefectItemList)));
            tabs.Items.Add(CreateTabItem("采集记录", CreateDetailListControl(ui.MainView, model.CollectDetailList)));
            //如果不用显示消息列表，则注释下面这句
            tabs.Items.Add(CreateTabItem("消息列表", CreateMessagerControl(model)));
            tabs.Margin = new System.Windows.Thickness(5, 5, 5, 0);
            layout.Children.Add(CreateOperationControl(model.Workstation));
            layout.Children.Add(tabs);
            ui.MainView.Data = model;
            base.OnUIGenerated(ui);
        }
    }
}