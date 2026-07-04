using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;
using SIE.Domain;
using SIE.ManagedProperty;
using SIE.Reflection;
using SIE.Utils;
using SIE.Wpf.Editors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace SIE.Wpf.Resources.Editors
{
    /// <summary>
    /// 自定义枚举编辑器
    /// </summary>
    public class CustomEnumEditor : BaseEditor<CustomEnumEditorConfig>
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public const string EditorName = "CustomEnumEditor";

        /// <summary>
        /// 所有分类Key
        /// </summary>
        private readonly string _allCategory = Guid.NewGuid().ToString("N");

        /// <summary>
        /// 当前ConboBox控件
        /// </summary>
        public new ComboBoxEdit Control { get; set; }

        /// <summary>
        /// 当前数据源
        /// </summary>
        public Entity Source { get; set; }

        /// <summary>
        /// Key分类，数据源
        /// </summary>
        readonly Dictionary<string, object[]> _dics = new Dictionary<string, object[]>();


        /// <summary>
        /// 创建编辑元素
        /// </summary>
        /// <returns>编辑元素</returns>
        protected override BaseEditSettings CreateEditSettingsCore()
        {
            var settings = new CustomEnumEditorSettings();
            settings.AssignToEditAction = AssignToEdit;
            settings.NullText = Config.NullText;
            settings.IsTextEditable = Config.IsTextEditable;
            settings.ValueMember = "EnumValue";
            settings.DisplayMember = "TranslatedLabel";

            if (!_dics.ContainsKey(_allCategory))
                _dics[_allCategory] = GetEnumItems(Meta.PropertyType);
            settings.Items.AddRange(_dics[_allCategory]);

            return settings;
        }

        object[] GetEnumItems(Type propertyType, string category = "")
        {
            List<object> items = new List<object>();
            if (Config.AllowNullInput.HasValue)
            {
                items.Add(new { EnumValue = (Enum)null, TranslatedLabel = string.Empty });
            }

            var enumType = TypeHelper.IgnoreNullable(propertyType);
            foreach (var item in EnumViewModel.GetByEnumType(propertyType))
            {
                if (Config.Category.IsNotEmpty() &&
                    !enumType.GetField(item.EnumValue.ToString()).GetCustomAttributes(typeof(CategoryAttribute), false).Any(a => (a as CategoryAttribute).Category.Equals(Config.Category)))
                    continue;
                if (category.IsNotEmpty() &&
                    !enumType.GetField(item.EnumValue.ToString()).GetCustomAttributes(typeof(CategoryAttribute), false).Any(a => (a as CategoryAttribute).Category.Equals(category)))
                    continue;

                items.Add(item);
            }

            return items.ToArray();
        }

        /// <summary>
        /// 生成编辑控件后，注入变更事件，此方法被调用后ComboBoxEdit属性才有值
        /// </summary>
        /// <param name="comboBox">下拉编辑</param>
        protected virtual void AssignToEdit(ComboBoxEdit comboBox)
        {
            if (Control != comboBox)
            {
                this.Control = comboBox;
                Control.DataContextChanged -= ComboBox_DataContextChanged;
                Control.DataContextChanged += ComboBox_DataContextChanged;
            }
        }

        private void ComboBox_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var combo = sender as ComboBoxEdit;

            if (e.NewValue is GridColumn)
            {
                var settings = (e.NewValue as GridColumn).EditSettings as CustomEnumEditorSettings;
                settings.Items.Clear();
                if (!_dics.ContainsKey(_allCategory))
                    _dics[_allCategory] = GetEnumItems(Meta.PropertyType);
                settings.Items.AddRange(_dics[_allCategory]);
            }
            else if (e.NewValue is EditGridCellData)
            {
                combo.Items.Clear();
                var data = e.NewValue as EditGridCellData;
                var entity = (data?.RowData.Row ?? e.NewValue) as Entity;
                Source = entity;

                string category = string.Empty;
                if (Config.CategoryProperty != null)
                {
                    var value = entity.GetProperty(Config.CategoryProperty);
                    category = value != null ? value.ToString() : string.Empty;
                }

                if (!_dics.ContainsKey(category))
                    _dics[category] = GetEnumItems(Meta.PropertyType, category);
                combo.Items.AddRange(_dics[category]);
            }
        }
    }

    /// <summary>
    /// 自定义编辑器设置
    /// </summary>
    public class CustomEnumEditorSettings : ComboxEditorSettings
    {
        /// <summary>
        /// 控件属性注册方法
        /// </summary>
        public Action<ComboBoxEdit> AssignToEditAction { get; set; }

        /// <summary>
        /// 控件注册核心
        /// </summary>
        /// <param name="edit">编辑器</param>
        protected override void AssignToEditCore(IBaseEdit edit)
        {
            base.AssignToEditCore(edit);
            var comboBox = edit as ComboBoxEdit;
            if (comboBox == null) return;
            AssignToEditAction(comboBox);
        }
    }

    /// <summary>
    /// 自定义枚举编辑器配置
    /// </summary>
    public class CustomEnumEditorConfig : EnumEditorConfig
    {
        /// <summary>
        /// 数据源过滤属性
        /// </summary>
        [DisplayName("分类")]
        public string Category
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 数据源过滤属性
        /// </summary>
        [DisplayName("分类属性")]
        public IManagedProperty CategoryProperty
        {
            get { return GetProperty<IManagedProperty>(); }
            set { SetProperty(value); }
        }
    }
}
