using System.Windows;
using System.Windows.Controls;

namespace SIE.Wpf.MES.BatchWIP.Packings.Layouts
{
    /// <summary>
    /// BatchAndPackingPanel.xaml 的交互逻辑
    /// 工位批次清单和包装清单面板
    /// </summary>
    public partial class BatchAndPackingPanel : UserControl
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BatchAndPackingPanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 初始化工位批次清单视图
        /// </summary>
        /// <param name="batchUI">批次清单控件</param>
        public void InitBatchListView(ControlResult batchUI/*, object obj*/)
        {
            batchUI.MainView.CommandsContainer.FlowDirection = FlowDirection.RightToLeft;
            batchPanel.Content = batchUI?.Control;
            //batchPanel.DataContext = obj;
        }

        /// <summary>
        /// 初始化批次包装清单视图
        /// </summary>
        /// <param name="packingUI">批次包装清单视图</param>
        public void InitPackingListView(ControlResult packingUI/*, object obj*/)
        {
            packingUI.MainView.CommandsContainer.FlowDirection = FlowDirection.RightToLeft;
            packingPanel.Content = packingUI?.Control;
            //packingPanel.DataContext = obj;
        }
    }
}
