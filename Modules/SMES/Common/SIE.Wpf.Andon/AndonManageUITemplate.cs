using DevExpress.Xpf.Core;
using SIE.Wpf.Andon.Controls;
using SIE.Wpf.MES.Controls;
using SIE.Wpf.MES.WIP;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using static SIE.Wpf.Andon.Controls.AndonButtonControl;

namespace SIE.Wpf.Andon
{
    /// <summary>
    /// 检验采集模板
    /// </summary>
    public class AndonManageUITemplate : KZCollectionUITemplate
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public AndonManageUITemplate() : base(typeof(AndonManageViewModel))
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public AndonManageUITemplate(AndonManageViewModel viewModel) : base(typeof(AndonManageViewModel))
        {
            model = viewModel;
        }

        public AndonManageViewModel model { get; set; } = null;

        public AndonButtonControl andonButtonControl { get; set; } = null;

        /// <summary>
        /// 添加当前模块中 UI 的初始化逻辑。
        /// 当使用自动生成的 UI 时，此方法会被调用。
        /// </summary>
        /// <param name="ui">控件结果</param>
        protected override void OnUIGenerated(ControlResult ui)
        {
            if (model == null)
                model = new AndonManageViewModel();
            var layout = ui.Control as DockLayout;

            //
            foreach (var item in layout.Children)
            {
                if (item.GetType() == typeof(DevExpress.Xpf.LayoutControl.LayoutControl))
                {
                    var layoutControl = item as DevExpress.Xpf.LayoutControl.LayoutControl;
                    layoutControl.Height = 0;
                }
            }

            layout.Children.Add(CreateOperationControl(model.KZWorkstation));
            layout.Children.Add(CreateAndonEventControl(model));
            layout.Children.Add(CreateAndonButtonControl(model));
            ui.MainView.Data = model;
            base.OnUIGenerated(ui);
        }

        /// <summary>
        /// 创建安灯事件控件
        /// </summary>
        /// <param name="model">检验采集对象</param>
        /// <returns>返回创建的UI</returns>
        public virtual FrameworkElement CreateAndonEventControl(AndonManageViewModel model)
        {
            var ctl = AndonEventControlFactory.CreateControl();

            //显示安灯管理对话框的委托
            ctl.ShowAndonManageDialog
                = new ShowAndonManageDialogDelegate(model.ShowAndonManageDialog);

            ctl.DataContext = model;
            ctl.SetBinding(AndonEventControl.AndonEventsProperty, new Binding("AndonEventList"));
            ctl.Margin = new Thickness(10, 5, 5, 5);
            DockPanel.SetDock(ctl, Dock.Top);
            return ctl;
        }

        /// <summary>
        /// 创建缺陷控件
        /// </summary>
        /// <param name="model">检验采集对象</param>
        /// <returns>返回创建的UI</returns>
        public virtual FrameworkElement CreateAndonButtonControl(AndonManageViewModel model)
        {
            var ctl = AndonButtonControlFactory.CreateControl();
            //显示安灯触发对话框的委托
            ctl.ShowAndonTriggerDialog
                = new ShowAndonTriggerDialogDelegate(model.ShowAndonTriggerDialog);

            ctl.DataContext = model;
            ctl.SetBinding(AndonButtonControl.AndonsProperty, new Binding("AndonList"));

            andonButtonControl = ctl as AndonButtonControl;
            //ctl.Margin = new Thickness(5);
            return ctl;
        }
    }
}
