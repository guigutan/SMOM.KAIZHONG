using SIE.Tech.Routings.Technologys;
using System.Windows.Controls;

namespace SIE.Wpf.MES.RoutingSettings
{
    /// <summary>
    /// DefaultRoutingDisplay.xaml 的交互逻辑
    /// </summary>
    public partial class DefaultRoutingDisplay : UserControl
    {
        /// <summary>
        /// 默认工艺路线显示构造函数
        /// </summary>
        public DefaultRoutingDisplay()
        {
            InitializeComponent();
            container.ModelChanged += Container_ModelChanged;
        }

        /// <summary>
        /// 设计器模型变更事件
        /// </summary>
        /// <param name="obj">容器接口</param>
        private void Container_ModelChanged(IContainer obj)
        {
            if (obj != null)
            {
                obj.SelectedElementChanged += Model_SelectedElementChanged;
            }
        }

        /// <summary>
        /// 流程元素选中事件
        /// </summary>
        /// <param name="obj">元素</param>
        private void Model_SelectedElementChanged(IElement obj)
        {
            container.svContainer.Focus();
        }
    }
}
