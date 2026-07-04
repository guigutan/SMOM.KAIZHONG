using DevExpress.Xpf.Editors;
using SIE.Utils;
using SIE.Wpf.Items.Editors;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace SIE.Wpf.Warehouses.Editors
{
    /// <summary>
    /// 仓储编辑器
    /// </summary>
    public abstract class StorageEditor : CustomInputEditor
    {
        /// <summary>
        /// 绑定实体
        /// </summary>
        /// <returns>属性</returns>
        protected override DependencyProperty BindingProperty()
        {
            return ComboBoxEdit.EditValueProperty;
        }

        /// <summary>
        /// 创建控件
        /// </summary>
        /// <param name="fieldName">创建控件对应的字段</param>
        /// <param name="colIndex">创建控件对应字段的索引</param>
        /// <param name="isBindReadOnly">是否绑定只读字段</param>
        /// <returns>返回新建的控件</returns>
        protected override Control CreateBaseEdit(string fieldName, int colIndex, out bool isBindReadOnly)
        {
            Control baseEdit = null;
            isBindReadOnly = false;
            if (colIndex == 0)
            {
                baseEdit = CreateComboBoxEdit();
            }
            else if (fieldName == "至")
            {
                baseEdit = CreateLabel();
            }
            else
            {
                baseEdit = CreateSpinEdit();
            }

            return baseEdit;
        }

        /// <summary>
        /// 创建数值控件
        /// </summary>
        /// <returns>返回数值控件</returns>
        private BaseEdit CreateSpinEdit()
        {
            SpinEdit editor = new SpinEdit();
            editor.AllowRoundOutOfRangeValue = true;
            editor.IsFloatValue = true;
            editor.AllowNullInput = true;
            editor.Margin = new Thickness() { Left = 5 };

            return editor;
        }

        /// <summary>
        /// 创建"至" Label控件
        /// </summary>
        /// <returns>返回Label控件</returns>
        private Control CreateLabel()
        {
            Label label = new Label();
            label.Content = "至";
            label.Margin = new Thickness() { Left = 5 };

            return label;
        }

        /// <summary>
        /// 创建枚举下拉框控件
        /// </summary>
        /// <returns>返回枚举下拉框控件</returns>
        private BaseEdit CreateComboBoxEdit()
        {
            ComboBoxEdit editor = new ComboBoxEdit();
            editor.SelectedIndex = 0;
            editor.ItemsSource = GetDataSource();
            editor.DisplayMember = "Label";
            editor.ValueMember = "EnumValue";

            return editor;
        }

        /// <summary>
        /// 创建下拉框的数据源
        /// </summary>
        /// <returns>返回下拉框的数据源</returns>
        protected abstract List<EnumViewModel> GetDataSource();
    }
}
