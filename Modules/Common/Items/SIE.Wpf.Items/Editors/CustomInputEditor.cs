using DevExpress.Xpf.Editors;
using SIE.ManagedProperty;
using SIE.MetaModel.View;
using SIE.Wpf.Editors;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SIE.Wpf.Items.Editors
{
    /// <summary>
    /// 自定义多个控件组合
    /// </summary>
    public abstract class CustomInputEditor : PropertyEditor<CustomInputEditorConfig>
    {
        /// <summary>
        /// 按顺序存放控件
        /// </summary>
        protected List<BaseEdit> editorList = new List<BaseEdit>();

        /// <summary>
        /// 绑定实体
        /// </summary>
        /// <returns>属性</returns>
        protected override DependencyProperty BindingProperty()
        {
            return BaseEdit.EditValueProperty;
        }

        /// <summary>
        /// 创建编辑器样式
        /// </summary>
        /// <returns>FrameworkElement</returns>
        protected override FrameworkElement CreateEditingElement()
        {
            var panel = new Grid();
            if (Config.BindFieldList.Count > Config.ColumnRateList.Count)
            {
                int additional = Config.BindFieldList.Count - Config.ColumnRateList.Count;
                for (int i = 0; i < additional; i++) Config.ColumnRateList.Add(1);
            }

            for (int i = 0; i < Config.BindFieldList.Count; i++)
            {
                panel.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition { Width = new GridLength(Config.ColumnRateList[i], GridUnitType.Star) });
            }

            for (int i = 0; i < Config.BindFieldList.Count; i++)
            {
                bool isBindReadOnly = false;
                Control ctl = CreateBaseEdit(Config.BindFieldList[i], i, out isBindReadOnly);
                BaseEdit control = ctl as BaseEdit;
                if (control != null)
                {
                    var fieldBinding = new Binding(Config.BindFieldList[i]);
                    fieldBinding.Mode = BindingMode.TwoWay;
                    control.SetBinding(BaseEdit.EditValueProperty, fieldBinding);
                    if (isBindReadOnly && base.Config.ReadOnlyProperty != null)
                    {
                        Binding readOnlyBinding = new Binding(base.Config.ReadOnlyProperty.Name);
                        readOnlyBinding.Mode = BindingMode.OneWay;
                        control.SetBinding(BaseEdit.IsReadOnlyProperty, readOnlyBinding);
                    }

                    editorList.Add(control);
                }

                Grid.SetColumn(ctl, i);
                panel.Children.Add(ctl);
            }

            return panel;
        }

        /// <summary>
        /// 创建控件
        /// </summary>
        /// <param name="fieldName">创建控件对应的字段</param>
        /// <param name="colIndex">创建控件对应字段的索引</param>
        /// <param name="isBindReadOnly">是否绑定只读字段</param>
        /// <returns>返回新建的控件</returns>
        protected abstract Control CreateBaseEdit(string fieldName, int colIndex, out bool isBindReadOnly);
    }

    /// <summary>
    /// 自定义多个控件组合 配置文件
    /// </summary>
    public class CustomInputEditorConfig : EditorConfig
    {
        /// <summary>
        /// 需要绑定的字段
        /// </summary>
        public List<string> BindFieldList { get; set; }

        /// <summary>
        /// 列的比例（对应绑定字段）
        /// </summary>
        public List<int> ColumnRateList { get; set; }

        /// <summary>
        /// 只读属性
        /// </summary>
        public IManagedProperty ReadOnlyProperty { get; set; }

        /// <summary>
        /// 是否小数
        /// </summary>
        public bool IsFloatValue { get; set; }
    }
}
