using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Helpers;
using DevExpress.Xpf.Editors.Settings;
using SIE.CSM.Common;
using SIE.Domain;
using SIE.ManagedProperty;
using SIE.Reflection;
using SIE.Wpf.Controls;
using SIE.Wpf.Editors;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;

namespace SIE.Wpf.CSM.Edtors
{
    /// <summary>
    /// 区域信息编辑器
    /// </summary>
    public class RegionalInfoEditor : BaseEditor<RegionalInfoEditorConfig>
    {
        /// <summary>
        /// 区域信息编辑器名称
        /// </summary>
        public const string EditorName = "RegionalInfoEditor";

        /// <summary>
        /// 下拉框
        /// </summary>
        private MulComboBoxEdit mulComboBoxEdit;

        /// <summary>
        /// 绑定属性
        /// </summary>
        /// <returns>依赖属性</returns>
        protected override DependencyProperty BindingProperty()
        {
            return MulComboBoxEdit.EditValueProperty;
        }

        /// <summary>
        /// 重写绑定更新方式
        /// </summary>
        /// <returns></returns>
        protected override Binding CreateBinding()
        {
            var binding = base.CreateBinding();
            binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            return binding;
        }

        /// <summary>
        /// 创建编辑器设置
        /// </summary>
        /// <returns>编辑器设置</returns>
        protected override BaseEditSettings CreateEditSettingsCore()
        {
            var settings = new ComboBoxEditorSettings();
            settings.NullText = Config.NullText;
            settings.AssignToEditAction = AssignToEdit;
            settings.IsTextEditable = true;
            var propertyType = Meta.PropertyType;
            settings.AllowNullInput = propertyType.IsNullable();

            return settings;
        }

        /// <summary>
        /// 生成编辑控件后，注入变更事件，此方法被调用后<see cref="MulComboBoxEdit"/>属性才有值
        /// </summary>
        /// <param name="comboBox">MulComboBoxEdit对象</param>
        protected virtual void AssignToEdit(MulComboBoxEdit comboBox)
        {
            if (mulComboBoxEdit != comboBox)
            {
                mulComboBoxEdit = comboBox;
                mulComboBoxEdit.DataContextChanged -= MulComboBoxEdit_DataContextChanged;
                mulComboBoxEdit.DataContextChanged += MulComboBoxEdit_DataContextChanged;
                mulComboBoxEdit.EditValueChanging -= MulComboBoxEdit_EditValueChanging;
                mulComboBoxEdit.EditValueChanging += MulComboBoxEdit_EditValueChanging;
                mulComboBoxEdit.ReloadDataOnPopping = Config.ReloadDataOnPopping;
            }
        }

        /// <summary>
        /// 下拉框值变更触发
        /// </summary>
        /// <param name="sender">当前对象</param>
        /// <param name="e">事件参数</param>
        private void MulComboBoxEdit_EditValueChanging(object sender, DevExpress.Xpf.Editors.EditValueChangingEventArgs e)
        {
            if (mulComboBoxEdit.ItemsSource == null &&
                 !string.IsNullOrEmpty(e.NewValue.ToString()))
            {
                mulComboBoxEdit.LoadData();
                mulComboBoxEdit.EditValue = e.NewValue;
            }
        }

        /// <summary>
        /// 下拉框上下文变更
        /// </summary>
        /// <param name="sender">当前对象</param>
        /// <param name="e">事件参数</param>
        private void MulComboBoxEdit_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var data = e.NewValue as DevExpress.Xpf.Grid.EditGridCellData;
            var entity = (data?.RowData.Row ?? e.NewValue) as Entity;
            if (entity != null)
            {
                mulComboBoxEdit.DataSourceProvider = () =>
                {
                    return GetDataSourceCore(entity);
                };
                if ((Config.ReloadDataOnPopping || mulComboBoxEdit.ItemsSource == null) && mulComboBoxEdit.EditValue != null)
                {
                    mulComboBoxEdit.LoadData();
                }
            }
        }

        /// <summary>
        /// 下拉框获取数据源的核心方法
        /// </summary>
        /// <param name="source">实体源数据</param>
        /// <returns>返回数据源列表</returns>
        protected virtual object GetDataSourceCore(Entity source)
        {
            string upperLevl = string.Empty;
            string upperLvel2 = string.Empty;
            if (Config.UpperLevelProperty != null)
            {
                var upperProperty = source.GetProperty(Config.UpperLevelProperty);
                if (upperProperty == null || upperProperty.ToString().IsNullOrEmpty())
                {
                    return new List<string>();
                }
                else
                {
                    upperLevl = upperProperty.ToString();
                }
            }

            if (Config.UpperLevel2Property != null)
            {
                var upper2Property = source.GetProperty(Config.UpperLevel2Property);
                if (upper2Property == null || upper2Property.ToString().IsNullOrEmpty())
                {
                    return new List<string>();
                }
                else
                {
                    upperLvel2 = upper2Property.ToString();
                }
            }

            List<string> dataSource = RT.Service.Resolve<RegionalInfoController>().GetRegionList(upperLevl, upperLvel2);
            dataSource.Add(string.Empty);

            return dataSource;
        }

        /// <summary>
        /// 下拉框配置类
        /// </summary>
        class ComboBoxEditorSettings : ComboBoxEditSettings
        {
            /// <summary>
            /// 创建控件后触发
            /// </summary>
            public Action<MulComboBoxEdit> AssignToEditAction { get; set; }

            /// <summary>
            /// 静态初始化
            /// </summary>
            static ComboBoxEditorSettings()
            {
                EditorSettingsProvider.Default.RegisterUserEditor2(typeof(MulComboBoxEdit), typeof(ComboBoxEditorSettings),
                    optimized => optimized ? new InplaceBaseEdit() : (IBaseEdit)new MulComboBoxEdit(), () => new ComboBoxEditorSettings());
            }

            /// <summary>
            /// 创建控件后触发
            /// </summary>
            /// <param name="edit">当前编辑器</param>
            protected override void AssignToEditCore(IBaseEdit edit)
            {
                base.AssignToEditCore(edit);
                var mulComboBox = edit as MulComboBoxEdit;
                if (mulComboBox == null)
                {
                    return;
                }
                AssignToEditAction(mulComboBox);
            }
        }
    }

    /// <summary>
    /// 区域信息配置文件帮助类
    /// </summary>
    public class RegionalInfoEditorConfig : BaseEditorConfig
    {
        /// <summary>
        /// 是否每次下拉时重新加载数据
        /// </summary>
        public bool ReloadDataOnPopping
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 上一级对应托管属性
        /// </summary>
        public IManagedProperty UpperLevelProperty
        {
            get { return GetProperty<IManagedProperty>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 上两级对应托管属性
        /// </summary>
        public IManagedProperty UpperLevel2Property
        {
            get { return GetProperty<IManagedProperty>(); }
            set { SetProperty(value); }
        }
    }
}
