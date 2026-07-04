using DevExpress.Xpf.Editors;
using SIE.Defects;
using System.Windows.Controls;
using System.Windows.Data;

namespace SIE.Wpf.MES.Wip.Controls
{
    /// <summary>
    /// DefectLookUpEditor.xaml 的交互逻辑
    /// </summary>
    public partial class DefectLookUpEditor : UserControl
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DefectLookUpEditor()
        {
            InitializeComponent();
            this.DataContextChanged += (s, e) =>
            {
                defectEditor.ItemsSource = RT.Service.Resolve<DefectController>().GetAllDefect();
            };
        }

        /// <summary>
        /// 设置属性绑定
        /// </summary>
        /// <param name="valuePath">值Path</param>
        /// <param name="readOnlyPath">只读属性Path</param>
        public void SetPropertyBinding(string valuePath, string readOnlyPath)
        {
            if (!string.IsNullOrEmpty(valuePath))
                defectEditor.SetBinding(BaseEdit.EditValueProperty, new Binding(valuePath));
            if (!string.IsNullOrEmpty(readOnlyPath))
                defectEditor.SetBinding(BaseEdit.IsEnabledProperty, new Binding(readOnlyPath));
        }
    }
}