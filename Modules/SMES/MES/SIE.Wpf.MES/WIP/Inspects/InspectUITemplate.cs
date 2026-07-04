using DevExpress.Xpf.Core;
using SIE.Wpf.MES.Controls;
using System.Windows;
using System.Windows.Data;

namespace SIE.Wpf.MES.WIP.Inspects
{
    /// <summary>
    /// 检验采集模板
    /// </summary>
    public class InspectUITemplate : CollectionUITemplate
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public InspectUITemplate() : base(typeof(InspectViewModel))
        {
        }

        /// <summary>
        /// 添加当前模块中 UI 的初始化逻辑。
        /// 当使用自动生成的 UI 时，此方法会被调用。
        /// </summary>
        /// <param name="ui">控件结果</param>
        protected override void OnUIGenerated(ControlResult ui)
        {
            var model = new InspectViewModel();
            var layout = ui.Control as DockLayout;

            var tabs = new DXTabControl();
            tabs.Items.Add(CreateTabItem("缺陷录入", CreateDefectControl(model)));
            tabs.Items.Add(CreateTabItem("采集记录", CreateDetailListControl(ui.MainView, model.CollectDetailList)));
            CreateReportTaskControl(tabs, ui.MainView, model);
            //如果不用显示消息列表，则注释下面这句
            tabs.Items.Add(CreateTabItem("消息列表", CreateMessagerControl(model)));

            tabs.Margin = new Thickness(5, 5, 5, 0);
            layout.Children.Add(CreateOperationControl(model.Workstation));
            layout.Children.Add(tabs);
            ui.MainView.Data = model;
            base.OnUIGenerated(ui);
        }

        /// <summary>
        /// 创建缺陷控件
        /// </summary>
        /// <param name="model">检验采集对象</param>
        /// <returns>返回创建的UI</returns>
        public virtual FrameworkElement CreateDefectControl(InspectViewModel model)
        {
            var ctl = DefectControlFactory.CreateControl();
            ctl.AllowMultiple = true;
            ctl.DataContext = model;
            ctl.SetBinding(DefectControl.SelectedValueProperty, new Binding("DefectItemList"));
            model.DefectControl = ctl;
            ctl.Margin = new Thickness(-8);
            return ctl;
        }
    }
}
