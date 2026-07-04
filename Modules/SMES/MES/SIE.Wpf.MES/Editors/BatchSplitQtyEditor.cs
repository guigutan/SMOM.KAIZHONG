using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Settings;
using Resources.IconPacks;
using SIE.MES.BatchWIP;
using SIE.Wpf.Windows;
using System.Windows;

namespace SIE.Wpf.MES.Editors
{
    ///// <summary>
    ///// 拆分数量编辑器
    ///// </summary>
    //public class BatchSplitQtyEditor : PropertyEditor<SpinEditorConfig>
    //{
    //    /// <summary>
    //    /// 编辑器名称
    //    /// </summary>
    //    public const string EditorName = "BatchSplitQtyEditor";

    //    /// <summary>
    //    /// 属性绑定
    //    /// </summary>
    //    /// <returns>编辑值属性</returns>
    //    protected override DependencyProperty BindingProperty()
    //    {
    //        return SpinEdit.EditValueProperty;
    //    }

    //    /// <summary>
    //    /// 创建控件样式
    //    /// </summary>
    //    /// <returns>控件</returns>
    //    protected override FrameworkElement CreateEditingElement()
    //    {
    //        var spinEdit = new SpinEdit()
    //        {
    //            MinValue = Config.MinValue,
    //            MaxValue = Config.MaxValue,
    //            Increment = Config.Increment,
    //            IsFloatValue = Meta.PropertyMeta.PropertyType.IgnoreNullable() != typeof(int),
    //            AllowNullInput = Meta.PropertyMeta.PropertyType.IsNullable(),
    //            ShowEditorButtons = true
    //        };

    //        if (Config.Decimals >= 0 && spinEdit.IsFloatValue)
    //        {
    //            spinEdit.MaskUseAsDisplayFormat = true;
    //            spinEdit.Mask = "n{0}".FormatArgs(Config.Decimals);
    //        }


    //        PackIcon icon = IconManager.GetPackIcon("Upload", 16, 16);
    //        icon.BorderThickness = new Thickness(0);
    //        icon.Margin = new Thickness(0);
    //        var info = new ButtonInfo() { Content = icon, ButtonKind = ButtonKind.Repeat, Margin = new Thickness(0), IsLeft = false };
    //        info.Click += Info_Click; ;
    //        spinEdit.Buttons.Add(info);

    //        this.ResetBinding(spinEdit);
    //        this.SetAutomationElement(spinEdit);
    //        return spinEdit;
    //    }

    //    /// <summary>
    //    /// 最大拆分命令
    //    /// </summary>
    //    /// <param name="sender">对象</param>
    //    /// <param name="e">参数</param>
    //    private void Info_Click(object sender, RoutedEventArgs e)
    //    {
    //        var inputBatch = this.Context.CurrentObject as InputBatch;
    //        inputBatch.SplitQty = inputBatch.RemainQty;
    //    }
    //}

    ///// <summary>
    ///// 拆分数量编辑器
    ///// </summary>
    //public class BatchSplitQtyEditor : SpinEditor
    //{
    //    /// <summary>
    //    /// 编辑器名称
    //    /// </summary>
    //    public const string EditorName = "BatchSplitQtyEditor";

    //    /// <summary>
    //    /// 创建Settings
    //    /// </summary>
    //    /// <returns>settings</returns>
    //    protected override BaseEditSettings CreateEditSettingsCore()
    //    {
    //        SpinEditSettings settings = base.CreateEditSettingsCore() as SpinEditSettings;
    //        PackIcon icon = IconManager.GetPackIcon("Upload", 16, 16);
    //        icon.BorderThickness = new Thickness(0);
    //        icon.Margin = new Thickness(0);
    //        var info = new ButtonInfo() { Content = icon, ButtonKind = ButtonKind.Simple, Margin = new Thickness(0) };
    //        info.Click += Info_Click; ;
    //        settings.Buttons.Add(info);
    //        return settings;
    //    }

    //    /// <summary>
    //    /// 最大拆分命令
    //    /// </summary>
    //    /// <param name="sender">对象</param>
    //    /// <param name="e">参数</param>
    //    private void Info_Click(object sender, RoutedEventArgs e)
    //    {
    //        var inputBatch = this.Context.CurrentObject as InputBatch;
    //        inputBatch.SplitQty = inputBatch.RemainQty;
    //    }
    //}

    /// <summary>
    /// 拆分数量编辑器
    /// </summary>
    public class BatchSplitQtyEditor : CalculateEditor
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public new const string EditorName = "BatchSplitQtyEditor";

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
            CalcEditSettings settings = base.CreateEditSettingsCore() as CalcEditSettings;
            PackIcon icon = IconManager.GetPackIcon("Upload", 16, 16);
            icon.BorderThickness = new Thickness(0);
            icon.Margin = new Thickness(0);
            var info = new ButtonInfo() { Content = icon, ButtonKind = ButtonKind.Simple, Margin = new Thickness(0) };
            info.Click += Info_Click;
            settings.Buttons.Add(info);
            return settings;
        }

        /// <summary>
        /// 最大拆分命令
        /// </summary>
        /// <param name="sender">对象</param>
        /// <param name="e">参数</param>
        private void Info_Click(object sender, RoutedEventArgs e)
        {
            var inputBatch = Source as InputBatch;
            if (inputBatch == null || this.Control.IsReadOnly) return;

            if (this.Control.Value == inputBatch.RemainQty)
                this.Control.Value = 0;
            else
                this.Control.Value = inputBatch.RemainQty;
        }
    }
}
