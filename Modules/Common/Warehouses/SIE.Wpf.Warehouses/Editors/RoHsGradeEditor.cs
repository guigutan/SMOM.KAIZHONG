using DevExpress.Xpf.Editors;
using SIE.Common.Catalogs;
using SIE.Domain;
using SIE.Warehouses;
using SIE.Wpf.Items.Editors;
using System.Windows;
using System.Windows.Controls;

namespace SIE.Wpf.Warehouses.Editors
{
    /// <summary>
    /// RoHS等级编辑器
    /// </summary>
    public class RoHsGradeEditor : CustomInputEditor
    {
        /// <summary>
        /// RoHS等级编辑器
        /// </summary>
        public const string EditorName = "RoHsGradeEditor";

        /// <summary>
        /// 绑定实体
        /// </summary>
        /// <returns>属性</returns>
        protected override DependencyProperty BindingProperty()
        {
            return CheckEdit.EditValueProperty;
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
            BaseEdit baseEdit = null;
            if (colIndex == 0)
            {
                isBindReadOnly = false;
                baseEdit = CreateCheckEdit();
            }
            else
            {
                isBindReadOnly = true;
                baseEdit = CreateComboBoxEdit();
            }

            return baseEdit;
        }

        /// <summary>
        /// 创建数值控件
        /// </summary>
        /// <returns>返回数值控件</returns>
        private BaseEdit CreateCheckEdit()
        {
            CheckEdit editor = new CheckEdit();
            editor.AllowNullInput = true;

            return editor;
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
            editor.Margin = new Thickness() { Left = 15 };
            editor.DisplayMember = "Name";
            editor.ValueMember = "Code";

            return editor;
        }

        /// <summary>
        /// 创建下拉框的数据源
        /// </summary>
        /// <returns>返回下拉框的数据源</returns>
        private EntityList<Catalog> GetDataSource()
        {
            return RT.Service.Resolve<CatalogController>().GetCatalogList(StorageLocationLayinInfo.ROHSLEVEL);
        }
    }
}
