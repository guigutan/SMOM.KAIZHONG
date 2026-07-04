using SIE.Wpf.MES.TouchScreenHomepage;
using SIE.Wpf.MES.WIP;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SIE.Wpf.MES.TouchScreenHomepage
{
    /// <summary>
    /// 检验采集模板
    /// </summary>
    public class TouchScreenHomepageUITemplate : CollectionUITemplate
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public TouchScreenHomepageUITemplate() : base(typeof(TouchScreenHomepageViewModel))
        {
        }

        /// <summary>
        /// 添加当前模块中 UI 的初始化逻辑。
        /// 当使用自动生成的 UI 时，此方法会被调用。
        /// </summary>
        /// <param name="ui">控件结果</param>
        protected override void OnUIGenerated(ControlResult ui)
        {
            var model = new TouchScreenHomepageViewModel();
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

            layout.Children.Add(CreateMenuControlControl(model));
            ui.MainView.Data = model;
            base.OnUIGenerated(ui);
        }

        /// <summary>
        /// 创建缺陷控件
        /// </summary>
        /// <param name="model">检验采集对象</param>
        /// <returns>返回创建的UI</returns>
        public virtual FrameworkElement CreateMenuControlControl(TouchScreenHomepageViewModel model)
        {
            var ctl = MenuControlFactory.CreateControl();

            //显示安灯管理对话框的委托

            ctl.DataContext = model;
            ctl.Margin = new Thickness(10, 5, 5, 5);
            ctl.SetBinding(MenuControl.MenusProperty, new Binding("MenuList"));
            DockPanel.SetDock(ctl, Dock.Top);
            return ctl;
        }
    }
}
