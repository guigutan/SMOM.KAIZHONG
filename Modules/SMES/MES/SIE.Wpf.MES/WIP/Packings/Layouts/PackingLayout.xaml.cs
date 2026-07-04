using DevExpress.Xpf.Core;
using DevExpress.Xpf.LayoutControl;
using SIE.Packages;
using SIE.Wpf.Controls;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SIE.Wpf.MES.WIP.Packings.Layouts
{
    /// <summary>
    /// PackingLayout.xaml 的交互逻辑
    /// </summary>
    public partial class PackingLayout : UserControl, ILayoutControl
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public PackingLayout()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 布局
        /// </summary>
        /// <param name="components">UI组件</param>
        public virtual void Arrange(UIComponents components)
        {
            if (components.CommandsContainer != null)
            {
                toolBar.Margin = new Thickness(5, 5, 5, 0);
                toolBar.Content = components.CommandsContainer.Control;
            }
            var control = components.Main.Control as LayoutControl;
            SetPanelMargin(control);
            mainView.Content = control;
            mainView.Margin = new Thickness(-5, -10, -5, 0);
            packingRelation.Content = components.Children.FirstOrDefault(f => typeof(PackingRelation).IsAssignableFrom(f.ControlResult.MainView.EntityType))?.ControlResult?.Control;
        }

        /// <summary>
        /// 初始化子视图
        /// </summary>
        /// <param name="tab">Dev原生Tab控件</param>
        public void InitChildrenView(DXTabControl tab)
        {
            childrenView.Content = tab;
        }

        /// <summary>
        /// 初始化工作站信息
        /// </summary>
        /// <param name="workstation">工作站</param>
        public void InitWorkstation(FrameworkElement workstation)
        {
            workStation.Content = workstation;
        }

        /// <summary>
        /// 设置容器边距
        /// </summary>
        /// <param name="control">控件</param>
        private void SetPanelMargin(LayoutControl control)
        {
            if (control == null)
                return;
            var groups = control.Children.OfType<FrameworkElement>().Where(p => p.GetType() == typeof(LayoutGroup)).ToList();
            for (int i = 0; i < groups.Count; i++)
            {
                if (i == 0) continue; //第一个组为提示信息框，不要设置边距
                var formPanel = groups[i].GetLogicalChild<FormPanel>();
                if (formPanel != null)
                    formPanel.Margin = new Thickness(0, -5, 0, -5);
            }
        }
    }
}