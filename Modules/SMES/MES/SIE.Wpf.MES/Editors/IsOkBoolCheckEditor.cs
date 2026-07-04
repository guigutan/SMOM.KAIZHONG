using System;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Helpers;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;
using SIE.Domain;
using System.Windows;
using SIE.Wpf.Editors;
using System.Windows.Input;
using System.Windows.Threading;
using DevExpress.Xpf.Grid.TreeList;
using SIE.Reflection;
using SIE.Wpf.MES.WIP.Inspects;

namespace SIE.Wpf.MES.Editors
{
    /// <summary>
    /// bool值按钮编辑器
    /// </summary>
    public class IsOkBoolCheckEditor : BaseEditor<CheckEditorConfig>
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public const string EditorName = "IsOkBoolCheckEditor";

        /// <summary>
        /// 当前CheckEdit控件
        /// </summary>
        public new CheckEdit Control { get; set; }

        /// <summary>
        /// 当前数据源
        /// </summary>
        public Entity Source { get; set; }

        /// <summary>
        /// 设置绑定属性
        /// </summary>
        /// <returns>属性</returns>
        protected override DependencyProperty BindingProperty()
        {
            return CheckEdit.IsCheckedProperty;
        }

        /// <summary>
        /// 创建Settings
        /// </summary>
        /// <returns>settings</returns>
        protected override BaseEditSettings CreateEditSettingsCore()
        {
            var settings = new BoolCheckEditorSettings()
            {
                NullText = Config.NullText,
                ClickMode = Config.ClickMode,
                IsThreeState = Config.IsThreeState,
            };
            settings.AssignToEditAction = AssignToEdit;
            return settings;
        }

        /// <summary>
        /// 生成编辑控件后，注入变更事件，
        /// </summary>
        /// <param name="checkEdit">下拉编辑</param>
        protected virtual void AssignToEdit(CheckEdit checkEdit)
        {
            if (this.Control != checkEdit)
            {
                this.Control = checkEdit;
                Control.DataContextChanged -= Control_DataContextChanged;
                Control.DataContextChanged += Control_DataContextChanged;
                Control.EditValueChanged -= Control_EditValueChanged;
                Control.EditValueChanged += Control_EditValueChanged;
            }
        }

        /// <summary>
        /// 重写
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Control_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            var inspectionItemViewModel = Source as InspectionItemViewModel;
            if (inspectionItemViewModel != null)
                inspectionItemViewModel.IsOk = (bool)e.NewValue;
        }

        /// <summary>
        /// 上下文变更事件
        /// </summary>
        /// <param name="sender">计算器控件</param>
        /// <param name="e">参数</param>
        private new void Control_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is GridColumn)
            {
                Source = null;
            }
            else if (e.NewValue is EditGridCellData)
            {
                var data = e.NewValue as EditGridCellData;
                var entity = (data?.RowData.Row ?? e.NewValue) as Entity;
                Source = entity;
            }
            else
            {
                //
            }
        }


        /// <summary>
        /// 编辑器设置
        /// </summary>
        private class BoolCheckEditorSettings : CheckEditSettings, IEditorSettings
        {

            /// <summary>
            /// 编辑器创建后事件
            /// </summary>
            public event EventHandler<BaseEdit> EditorCreated;

            /// <summary>
            /// 控件属性注册方法
            /// </summary>
            public Action<CheckEdit> AssignToEditAction { get; set; }

            /// <summary>
            /// 创建编辑器
            /// </summary>
            /// <param name="assignEditorSettings"></param>
            /// <param name="defaultViewInfo"></param>
            /// <param name="optimizationMode"></param>
            /// <returns></returns>
            public override IBaseEdit CreateEditor(bool assignEditorSettings, IDefaultEditorViewInfo defaultViewInfo, EditorOptimizationMode optimizationMode)
            {
                var edit = base.CreateEditor(assignEditorSettings, defaultViewInfo, optimizationMode);
                if (edit is BaseEdit)
                {
                    EditorCreated?.Invoke(this, edit as BaseEdit);
                }
                var column = defaultViewInfo as GridColumn;
                if (column != null && column.EditSettings is BoolCheckEditorSettings)
                {
                    var grid = column.View?.DataControl as GridControl;
                    if (grid != null)
                    {
                        grid.PreviewMouseLeftButtonUp -= Grid_PreviewMouseLeftButtonUp;
                        grid.PreviewMouseLeftButtonUp += Grid_PreviewMouseLeftButtonUp;
                    }
                }
                return edit;
            }

            /// <summary>
            /// 鼠标点击事件
            /// </summary>
            /// <param name="sender">所有者</param>
            /// <param name="e">参数</param>
            protected virtual void Grid_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
            {
                var view = (sender as GridControl).View;
                if (view is TableView)
                {
                    TableView tableView = view as TableView;
                    TableViewHitInfo hitTest = tableView.CalcHitInfo(e.OriginalSource as DependencyObject);
                    if (hitTest.InRowCell && hitTest.Column.FieldType.IgnoreNullable() == typeof(bool) && hitTest.Column.EditSettings is BoolCheckEditorSettings && e.OriginalSource is InplaceBaseEdit)
                    {
                        SetEditValue(view);
                    }
                }
                else if (view is TreeListView)
                {
                    TreeListView treeView = view as TreeListView;
                    TreeListViewHitInfo hitTest = treeView.CalcHitInfo(e.OriginalSource as DependencyObject);
                    if (hitTest.InRowCell && hitTest.Column.FieldType.IgnoreNullable() == typeof(bool) && hitTest.Column.EditSettings is BoolCheckEditorSettings && e.OriginalSource is InplaceBaseEdit)
                    {
                        SetEditValue(view);
                    }
                }
                else
                {
                    //
                }
            }

            /// <summary>
            /// 设置编辑器值
            /// </summary>
            /// <param name="view">视图</param>
            protected virtual void SetEditValue(DataViewBase view)
            {
                view.ShowEditor();
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    var activeEditor = view?.ActiveEditor;
                    if (activeEditor != null)
                    {
                        var flag = false;
                        var editValue = false;
                        //类型转换异常捕获
                        try
                        {
                            editValue = (bool)activeEditor.EditValue;
                            flag = true;
                        }
                        catch (Exception ex)
                        {
                            flag = false;
                        }
                        if (activeEditor != null && !activeEditor.IsReadOnly && flag)
                        {
                            activeEditor.EditValue = activeEditor.EditValue != null && !editValue;
                        }
                    }
                }), DispatcherPriority.Render);
            }

            /// <summary>
            /// 控件注册核心
            /// </summary>
            /// <param name="edit">编辑器</param>
            protected override void AssignToEditCore(IBaseEdit edit)
            {
                base.AssignToEditCore(edit);
                var calcEdit = edit as CheckEdit;
                if (calcEdit == null)
                {
                    return;
                }

                AssignToEditAction(calcEdit);
            }
        }
    }
}
