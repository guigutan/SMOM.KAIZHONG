using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Helpers;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;
using SIE.Domain;
using SIE.MetaModel;
using SIE.Reflection;
using SIE.Wpf.Editors;
using System;
using System.ComponentModel;
using System.Windows;

namespace SIE.Wpf.MES.Editors
{
    /// <summary>
    /// 计算器编辑器
    /// </summary>
    public class CalculateEditor : BaseEditor<CalcurateEditorConfig>
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public const string EditorName = "BatchQtyEditor";

        /// <summary>
        /// 当前PopupCalcEdit控件
        /// </summary>
        public new PopupCalcEdit Control { get; set; }

        /// <summary>
        /// 当前数据源
        /// </summary>
        public Entity Source { get; set; }

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
            var settings = new CalcurateEditorSettings();
            settings.AssignToEditAction = AssignToEdit;

            settings.IsTextEditable = false;
            settings.NullText = Config.NullText;
            if (Config.Width.HasValue)
                settings.PopupWidth = Config.Width.Value;
            if (Config.Height.HasValue)
                settings.PopupHeight = Config.Height.Value;
            if (Config.MinWidth.HasValue)
                settings.PopupMinWidth = Config.MinWidth.Value;
            if (Config.MinHeight.HasValue)
                settings.PopupMinHeight = Config.MinHeight.Value;
            if (Config.MaxWidth.HasValue)
                settings.PopupMaxWidth = Config.MaxWidth.Value;
            if (Config.MaxHeight.HasValue)
                settings.PopupMaxHeight = Config.MaxHeight.Value;
            if (Config.AllowNullInput.HasValue)
                settings.AllowNullInput = Config.AllowNullInput.Value;
            else
                settings.AllowNullInput = Meta.PropertyType.IsNullable();

            //settings.DataContext = this.CreateBinding();
            return settings;
        }

        /// <summary>
        /// 生成编辑控件后，注入变更事件，此方法被调用后PopupCalcEdit属性才有值
        /// </summary>
        /// <param name="popupCalcEdit">下拉编辑</param>
        protected virtual void AssignToEdit(PopupCalcEdit popupCalcEdit)
        {
            if (this.Control != popupCalcEdit)
            {
                this.Control = popupCalcEdit;
                Control.DataContextChanged -= Control_DataContextChanged;
                Control.DataContextChanged += Control_DataContextChanged;
            }
        }

        /// <summary>
        /// 上下文变更事件
        /// </summary>
        /// <param name="sender">计算器控件</param>
        /// <param name="e">参数</param>
        protected override void Control_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
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
    }

    /// <summary>
    /// 编辑器设置
    /// </summary>
    class CalcurateEditorSettings : CalcEditSettings, IEditorSettings
    {
        static CalcurateEditorSettings()
        {
            EditorSettingsProvider.Default.RegisterUserEditor2(typeof(PopupCalcEdit), typeof(CalcurateEditorSettings),
                    optimized => optimized ? new InplaceBaseEdit() : (IBaseEdit)new PopupCalcEdit(), () => new CalcurateEditorSettings());
        }

        /// <summary>
        /// 编辑器创建后事件
        /// </summary>
        public event EventHandler<BaseEdit> EditorCreated;

        /// <summary>
        /// 控件属性注册方法
        /// </summary>
        public Action<PopupCalcEdit> AssignToEditAction { get; set; }

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

            return edit;
        }

        /// <summary>
        /// 控件注册核心
        /// </summary>
        /// <param name="edit">编辑器</param>
        protected override void AssignToEditCore(IBaseEdit edit)
        {
            base.AssignToEditCore(edit);
            var calcEdit = edit as PopupCalcEdit;
            if (calcEdit == null) return;
            AssignToEditAction(calcEdit);
        }
    }

    /// <summary>
    /// 编辑器数量配置
    /// </summary>
    public class CalcurateEditorConfig : BaseEditorConfig
    {
        /// <summary>
        /// 是否允许空输入
        /// </summary>
        [DisplayName("是否允许空输入")]
        public bool? AllowNullInput
        {
            get { return GetProperty<bool?>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 最小高度
        /// </summary>
        [DisplayName("最小高度")]
        public int? MinHeight
        {
            get { return GetProperty<int?>(); }
            set { SetProperty(value); }
        }


        /// <summary>
        /// 最小宽度
        /// </summary>
        [DisplayName("最小宽度")]
        public int? MinWidth
        {
            get { return GetProperty<int?>(); }
            set { SetProperty(value); }
        }


        /// <summary>
        /// 最大高度
        /// </summary>
        [DisplayName("最大高度")]
        public int? MaxHeight
        {
            get { return GetProperty<int?>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 最大宽度
        /// </summary>
        [DisplayName("最大宽度")]
        public int? MaxWidth
        {
            get { return GetProperty<int?>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 高度
        /// </summary>
        [DisplayName("高度")]
        public int? Height
        {
            get { return GetProperty<int?>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 最大宽度
        /// </summary>
        [DisplayName("宽度")]
        public int? Width
        {
            get { return GetProperty<int?>(); }
            set { SetProperty(value); }
        }
    }
}
