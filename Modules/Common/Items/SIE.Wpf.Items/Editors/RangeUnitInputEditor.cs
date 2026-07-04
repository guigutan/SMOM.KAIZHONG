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
    /// 范围单位编辑
    /// </summary>
    public class RangeUnitInputEditor : PropertyEditor<RangeUnitInputEditorConfig>
    {
        /// <summary>
        /// 单位输入编辑器
        /// </summary>
        public const string EditorName = "RangeUnitInputEditor";

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
            panel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            panel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            panel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            panel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });

            #region 控件
            SpinEdit tbkStat = new SpinEdit();
            tbkStat.HorizontalContentAlignment = HorizontalAlignment.Left;
            tbkStat.AllowNullInput = true;
            tbkStat.MaskType = MaskType.Numeric;
            var spinEditStartBinding = new Binding(base.Config.BindStartTextColumn);
            spinEditStartBinding.Mode = BindingMode.TwoWay;
            tbkStat.SetBinding(SpinEdit.TextProperty, spinEditStartBinding);

            Label txt = new Label();
            txt.HorizontalContentAlignment = HorizontalAlignment.Center;
            txt.Content = "至".L10N();

            SpinEdit tbkEnd = new SpinEdit();
            tbkEnd.HorizontalContentAlignment = HorizontalAlignment.Left;
            tbkEnd.AllowNullInput = true;
            tbkEnd.MaskType = MaskType.Numeric;
            var spinEditEndBinding = new Binding(base.Config.BindEndTextColumn);
            spinEditEndBinding.Mode = BindingMode.TwoWay;
            tbkEnd.SetBinding(SpinEdit.TextProperty, spinEditEndBinding);

            ComboBoxEdit cbx = new ComboBoxEdit();
            cbx.SelectedIndex = 0;
            cbx.ItemsSource = GetUnitTable();
            cbx.Margin = new Thickness() { Left = 15 };
            var comboBoxBinding = new Binding(base.Config.BindUnitColumn);
            comboBoxBinding.Mode = BindingMode.TwoWay;
            cbx.SetBinding(ComboBoxEdit.EditValueProperty, comboBoxBinding);
            cbx.DisplayMember = "Display";
            cbx.ValueMember = "Value";
            #endregion

            Grid.SetColumn(tbkStat, 0);
            Grid.SetColumn(txt, 1);
            Grid.SetColumn(tbkEnd, 2);
            Grid.SetColumn(cbx, 3);
            panel.Children.Add(cbx);
            panel.Children.Add(txt);
            panel.Children.Add(tbkEnd);
            panel.Children.Add(tbkStat);
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
    /// 范围单位编辑 配置文件
    /// </summary>
    public class RangeUnitInputEditorConfig : EditorConfig
    {
        /// <summary>
        /// 需要绑定的文本字段
        /// </summary>
        public string BindStartTextColumn { get; set; }

        /// <summary>
        /// 需要绑定的文本字段
        /// </summary>
        public string BindEndTextColumn { get; set; }

        /// <summary>
        /// 需要绑定的单位字段
        /// </summary>
        public string BindUnitColumn { get; set; }

        /// <summary>
        /// 获取单位编码
        /// </summary>
        public Func<string> GetUnitType { get; set; }
    }
}
