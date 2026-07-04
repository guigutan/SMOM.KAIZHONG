using DevExpress.Xpf.Editors;
using SIE.MES.RoutingSettings;
using SIE.Tech.Routings;
using SIE.Wpf.Editors;
using System.Windows;
using System.Windows.Media;

namespace SIE.Wpf.MES.RoutingSettings
{
    /// <summary>
    /// 默认工艺路线显示编辑器
    /// </summary>
    public class DefaultRoutingDisplayEditor : PropertyEditor<BaseEditorConfig>
    {
        /// <summary>
        /// 编辑器名字
        /// </summary>
        public const string EditorName = "DefaultRoutingDisplayEditor";

        /// <summary>
        /// 默认工艺路线显示视图
        /// </summary>
        private DefaultRoutingDisplay curDefaultRoute = null;

        /// <summary>
        /// 文本编辑器依赖属性
        /// </summary>
        /// <returns>依赖属性</returns>
        protected override DependencyProperty BindingProperty()
        {
            return TextEditBase.TextProperty;
        }

        /// <summary>
        /// 编辑器创建元素方法
        /// </summary>
        /// <returns>编辑器创建初始化</returns>
        protected override FrameworkElement CreateEditingElement()
        {
            curDefaultRoute = new DefaultRoutingDisplay();
            curDefaultRoute.Name = this.Meta.Name;
            curDefaultRoute.Visibility = Visibility.Visible;
            curDefaultRoute.HorizontalAlignment = HorizontalAlignment.Left;
            curDefaultRoute.VerticalAlignment = VerticalAlignment.Top;
            curDefaultRoute.Background = new SolidColorBrush(Colors.White);
            this.ResetBinding(curDefaultRoute);
            this.SetAutomationElement(curDefaultRoute);
            return curDefaultRoute;
        }

        /// <summary>
        /// 获取工艺路线版本的Layout数据
        /// </summary>
        /// <param name="defaultVersion">工艺路线默认版本</param>
        /// <returns>Layout大文本字符串</returns>
        private string GetLayout(RoutingVersion defaultVersion)
        {
            if (defaultVersion?.Layout != null)
                return defaultVersion.Layout.Layout;
            else
                return string.Empty;
        }

        /// <summary>
        /// 数据变更事件
        /// </summary>
        /// <param name="newValue">当前值</param>
        /// <param name="oldValue">原先值</param>
        protected override void OnDataContextChanged(object newValue, object oldValue)
        {
            if (newValue != null)
            {
                var routingVM = newValue as DefaultRoutingViewModel;
                var routingVersion = routingVM.DefaultVersion;
                curDefaultRoute.container.LoadFromXmlString(GetLayout(routingVersion));
            }
        }
    }
}