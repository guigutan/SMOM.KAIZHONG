using DevExpress.Xpf.Core;
using SIE.MetaModel.View;
using SIE.Wpf.Common.Editors;
using SIE.Wpf.Editors;
using System;
using System.Windows;

namespace SIE.Wpf.MES.Editors
{
    /// <summary>
    /// 叉板数量编辑器
    /// </summary>
    public class ForkPlateQtyEditor : PropertyEditor<EditorConfig>
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public const string EditorName = "ForkPlateQtyEditor";

        /// <summary>
        /// 绑定属性
        /// </summary>
        /// <returns>内容</returns>
        protected override DependencyProperty BindingProperty()
        {
            return SimpleButton.ContentProperty;
        }

        /// <summary>
        /// 创建编辑元素
        /// </summary>
        /// <returns>控件</returns>
        protected override FrameworkElement CreateEditingElement()
        {
            var editor = new Calculator();
            var button = new SimpleButton();
            button.HorizontalContentAlignment = HorizontalAlignment.Right;
            button.Click += (s, e) =>
            {
                if (button != null && button.Content != null && !string.IsNullOrEmpty(button.Content.ToString()))
                {
                    editor.Value = (int)button.Content;
                }
                else
                {
                    editor.Value = 0;
                }

                CRT.Workbench.ShowDialog(Guid.NewGuid().ToString(), editor, w =>
                {
                    w.Title = "Skip数量录入".L10N();
                    w.Height = 400;
                    w.Width = 400;
                    w.Closing += (a, b) =>
                    {
                        if (w.Result == 0)
                        {
                            if (editor.HasError || editor.Value < 0)
                            {
                                b.Cancel = true;
                                return;
                            }
                            button.Content = (int)editor.Value;
                        }
                    };
                });
            };
            this.ResetBinding(button);
            this.SetAutomationElement(button);
            return button;
        }
    }
}
