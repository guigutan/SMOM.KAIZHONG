using SIE.Wpf.Common.Diagram;
using System.ComponentModel;
using System.Windows;

namespace SIE.Wpf.WorkBenchCommon.Workbench
{
    /// <summary>
    /// ThemeBorder.xaml 的交互逻辑
    /// </summary>
    public partial class ThemeBorder : ComponentItem
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ThemeBorder()
        {
            InitializeComponent();
            UseProperty<ThemeBorderProperty>();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ThemeBorderProperty : ComponentProperty<ThemeBorder>
    {
        /// <summary>
        /// 边框线宽
        /// </summary>
        [DisplayName()]
        public Thickness BorderThickness
        {
            get { return Item.border.BorderThickness; }
            set { Item.border.BorderThickness = value; }
        }
    }
}
