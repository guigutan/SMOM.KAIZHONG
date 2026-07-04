using DevExpress.Xpf.Core;
using SIE.Wpf.MES.WIP;

namespace SIE.Wpf.MES.BatchWIP.Moves
{
    /// <summary>
    /// 批次过站采集模板
    /// </summary>
    public class BatchMoveUITemplate : BatchCollectionUITemplate
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BatchMoveUITemplate() : base(typeof(BatchMoveViewModel))
        {
        }

        /// <summary>
        /// UI创建后
        /// </summary>
        /// <param name="ui">UI控件</param>
        protected override void OnUIGenerated(ControlResult ui)
        {
            var model = new BatchMoveViewModel();
            var tabs = new DXTabControl();
            //tabs.Items.Add(CreateTabItem("工位批次列表", CreateBatchListControl(ui.MainView, model.InputBatchList, model.OutputBatchList, ViewConfig.ListView, ViewConfig.ListView)));
            var inputControl = CreateListControl(ui.MainView, model.InputBatchList, InputBatchViewConfig.BatchMoveViewStr);
            tabs.Items.Add(CreateTabItem("工位批次列表", inputControl.Control));
            tabs.Items.Add(CreateTabItem("采集记录", CreateDetailListControl(ui.MainView, model.CollectDetailList, CollectDetailViewModelViewConfig.BatchViewGroup)));
            CreateReportTaskControl(tabs, ui.MainView, model, "CollectionView");
            //如果不用显示消息列表，则注释下面这句
            tabs.Items.Add(CreateTabItem("消息列表", CreateMessagerControl(model)));
            tabs.Margin = new System.Windows.Thickness(5, 5, 5, 0);
            var layout = ui.Control as DockLayout;
            layout.Children.Add(CreateOperationControl(model.Workstation));
            layout.Children.Add(tabs);
            ui.MainView.Data = model;
            base.OnUIGenerated(ui);
        }
    }
}