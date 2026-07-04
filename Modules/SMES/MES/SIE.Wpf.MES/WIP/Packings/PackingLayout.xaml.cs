using DevExpress.Xpf.Core;
using DevExpress.Xpf.LayoutControl;
using SIE.Packages;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SIE.Wpf.MES.WIP.Packings
{
    /// <summary>
    /// PackingLayout.xaml 的交互逻辑
    /// </summary>
    public partial class PackingLayout : UserControl, ILayoutControl
    {
        /// <summary>
        /// 构造函数
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
                toolBar.Content = components.CommandsContainer.Control;
            var control = components.Main.Control as LayoutControl;
            mainView.Content = control;
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
    }
}