using DevExpress.Xpf.Editors;
using SIE.Domain;
using SIE.Items;
using SIE.Items.Units;
using SIE.MetaModel.View;
using SIE.Wpf.Editors;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SIE.Wpf.Items.Editors
{
    /// <summary>
    /// 规格下限编辑器
    /// </summary>
    public class UnitInputEditor : PropertyEditor<UnitInputEditorConfig>
    {
        /// <summary>
        /// 单位输入编辑器
        /// </summary>
        public const string EditorName = "UnitInputEditor";

        /// <summary>
        /// 单位下拉框
        /// </summary>
        ComboBox cbx;

        /// <summary>
        /// 单位前文本
        /// </summary>
        SpinEdit tbk;

        /// <summary>
        /// 绑定实体
        /// </summary>
        /// <returns>属性</returns>
        protected override DependencyProperty BindingProperty()
        {
            return SpinEdit.TextProperty;
        }

        /// <summary>
        /// 创建编辑器样式
        /// </summary>
        /// <returns>FrameworkElement</returns>
        protected override FrameworkElement CreateEditingElement()
        {
            var panel = new Grid();
            panel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(4, GridUnitType.Star) });
            panel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) });
            tbk = new SpinEdit();
            tbk.HorizontalContentAlignment = HorizontalAlignment.Left;
            tbk.AllowNullInput = true;
            tbk.MaskType = MaskType.Numeric;
            var spinEditBinding = new Binding(base.Config.BingTextColumn);
            spinEditBinding.Mode = BindingMode.TwoWay;
            tbk.SetBinding(TextEdit.TextProperty, spinEditBinding);

            cbx = new ComboBox();
            cbx.SelectedIndex = 0;
            cbx.ItemsSource = GetUnitTable();
            cbx.Margin = new Thickness() { Left = 15 };
            var comboBoxBinding = new Binding(base.Config.BingUnitColumn);
            comboBoxBinding.Mode = BindingMode.TwoWay;
            cbx.SetBinding(ComboBox.SelectedValueProperty, comboBoxBinding);
            cbx.DisplayMemberPath = "Display";
            cbx.SelectedValuePath = "Value";

            Grid.SetColumn(tbk, 0);
            Grid.SetColumn(cbx, 1);
            panel.Children.Add(cbx);
            panel.Children.Add(tbk);
            return panel;
        }

        /// <summary>
        /// 获取单位列表
        /// </summary>
        /// <returns>单位列表</returns>
        private List<object> GetUnitTable()
        {
            List<object> unitResult = new List<object>();
            EntityList<Unit> unitList = RT.Service.Resolve<UnitsController>().GetUnitList(base.Config.GetUnitType());
            unitResult.Add(new { Display = string.Empty, Value = 0d });
            foreach (Unit unitItem in unitList)
            {
                unitResult.Add(new { Display = unitItem.Code, Value = unitItem.Id });
            }

            return unitResult;
        }
    }

    /// <summary>
    /// 单位编辑 配置文件
    /// </summary>
    public class UnitInputEditorConfig : EditorConfig
    {
        /// <summary>
        /// 需要绑定的文本字段
        /// </summary>
        public string BingTextColumn { get; set; }

        /// <summary>
        /// 需要绑定的单位字段
        /// </summary>
        public string BingUnitColumn { get; set; }

        /// <summary>
        /// 获取单位编码
        /// </summary>
        public Func<string> GetUnitType { get; set; }
    }
}
