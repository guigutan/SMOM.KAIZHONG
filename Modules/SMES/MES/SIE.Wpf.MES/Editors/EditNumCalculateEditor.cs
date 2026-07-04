using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Settings;
using Resources.IconPacks;
using SIE.Wpf.Windows;
using System.Windows;

namespace SIE.Wpf.MES.Editors
{
    /// <summary>
    /// 数量编辑器
    /// </summary>
    public class EditNumCalculateEditor : CalculateEditor
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public new readonly static string EditorName = "EditNumCalculateEditor";
        /// <summary>
        /// 构造函数
        /// </summary>
        protected EditNumCalculateEditor()
        { }

        /// <summary>
        /// 属性绑定
        /// </summary>
        /// <returns>编辑值属性</returns>
        protected override DependencyProperty BindingProperty()
        {
            return PopupCalcEdit.ValueProperty;
        }

        /// <summary>
        /// 创建Settings
        /// </summary>
        /// <returns>settings</returns>
        protected override BaseEditSettings CreateEditSettingsCore()
        {
            base.CreateEditSettingsCore();
            CalcEditSettings settings = base.CreateEditSettingsCore() as CalcEditSettings;
            PackIcon icon = IconManager.GetPackIcon("Upload", 16, 16);
            icon.BorderThickness = new Thickness(0);
            icon.Margin = new Thickness(0);
            var info = new ButtonInfo() { Content = icon, ButtonKind = ButtonKind.Simple, Margin = new Thickness(0) };
            info.Click += ButtonInfo_Click;
            settings.Buttons.Add(info);
            return settings;
        }

        /// <summary>
        /// 最大数量命令
        /// </summary>
        /// <param name="sender">对象</param>
        /// <param name="e">参数</param>
        protected virtual void ButtonInfo_Click(object sender, RoutedEventArgs e)
        {
            ////子类去重写该方法
        }
    }
}
