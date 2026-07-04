using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Settings;
using Resources.IconPacks;
using SIE.MES.LoadItems;
using SIE.Wpf.Editors;
using SIE.Wpf.Windows;
using System.Windows;

namespace SIE.Wpf.MES.Editors
{
    /// <summary>
    /// 叫料数量控件
    /// </summary>
    public class CallMateriaQtyEditor : SpinEditor
    {
        /// <summary>
        /// 当前创建的控件
        /// </summary>
        new BaseEdit Control;

        /// <summary>
        /// 编辑器名称
        /// </summary>
        public readonly static string EditorName = "CallMateriaQtyEditor";

        /// <summary>
        /// 当前实体
        /// </summary>
        StationMateriaViewModel Model;

        /// <summary>
        /// 创建编辑器SettingsCore
        /// </summary>
        /// <returns>Settings</returns>
        protected override BaseEditSettings CreateEditSettingsCore()
        {
            var setting = base.CreateEditSettingsCore() as SpinEditSettings;

            PackIcon icon = IconManager.GetPackIcon("Upload", 16, 16);
            icon.BorderThickness = new Thickness(0);
            icon.Margin = new Thickness(0);
            var info = new ButtonInfo() { Content = icon, ButtonKind = ButtonKind.Simple, Margin = new Thickness(0) };
            info.Click += Info_Click;
            setting.Buttons.Add(info);
            return setting;
        }

        /// <summary>
        /// 创建编辑器Settings
        /// </summary>
        /// <returns>Settings</returns>
        public override BaseEditSettings CreateEditSettings()
        {
            var settings = base.CreateEditSettings();
            if (settings is IEditorSettings)
            {
                ((IEditorSettings)settings).EditorCreated += (s, edit) =>
                {
                    if (this.Control != null)
                        this.Control.DataContextChanged -= Edit_DataContextChanged;

                    if (this.Control != edit)
                        this.Control = edit;
                    if (edit != null)
                        edit.DataContextChanged += Edit_DataContextChanged;
                };
            }

            return settings;
        }

        /// <summary>
        /// 编辑器上下文变更事件
        /// </summary>
        /// <param name="sender">事件源对象</param>
        /// <param name="e">事件参数</param>
        private void Edit_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is DevExpress.Xpf.Grid.GridColumn)
            {
                Model = null;
            }
            else if (e.NewValue is DevExpress.Xpf.Grid.EditGridCellData)
            {
                var data = e.NewValue as DevExpress.Xpf.Grid.EditGridCellData;
                var entity = (data?.RowData.Row ?? e.NewValue) as StationMateriaViewModel;
                Model = entity;
            }
        }

        /// <summary>
        /// 叫料数按钮事件
        /// </summary>
        /// <param name="sender">时间对象</param>
        /// <param name="e">事件参数</param>
        private void Info_Click(object sender, RoutedEventArgs e)
        {
            if (Model != null && this.Control != null && !this.Control.IsReadOnly)
            {
                var callQty = Model.Capacity;
                this.Control.EditValue = Model.CallQty < callQty ? callQty : 0;
            }
        }
    }
}
