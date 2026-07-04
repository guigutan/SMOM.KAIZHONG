using System.Windows.Controls;

namespace SIE.Wpf.MES.BatchWIP
{
    /// <summary>
    /// BatchControl.xaml 的交互逻辑
    /// </summary>
    public partial class BatchControl : UserControl
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="input">入站批次</param>
        /// <param name="output">出站批次</param>
        public BatchControl(ControlResult input, ControlResult output)
        {
            InitializeComponent();
            inputControl.Content = input.Control;
            outputControl.Content = output.Control;
        }
    }
}