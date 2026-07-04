using DevExpress.Xpf.Core;
using SIE.Wpf.MES.WIP;

namespace SIE.Wpf.MES.BatchWIP.Inspects
{
    /// <summary>
    /// 批次检验采集模板类
    /// </summary>
    public class BatchInspectUITemplate : BatchCollectionUITemplate
    {
        /// <summary>
        /// 批次检验采集构造函数
        /// </summary>
        public BatchInspectUITemplate() : base(typeof(BatchInspectViewModel))
        {
        }

        /// <summary>
        /// 批次检验采集UI创建后的方法
        /// </summary>
        /// <param name="ui">控件结果</param>
        protected override void OnUIGenerated(ControlResult ui)
        {
            ////base.OnUIGenerated(ui);
            var model = new BatchInspectViewModel();
            model.ShowTips("批次检验采集");

            var tabs = new DXTabControl();
            //tabs.Items.Add(CreateTabItem("工位批次列表", CreateBatchListControl(ui.MainView, model.InputBatchList,
            //    model.OutputBatchList, InputBatchViewConfig.BatchInspectView, OutputBatchViewConfig.BatchInspectView)));
            var inputControl = CreateListControl(ui.MainView, model.InputBatchList, InputBatchViewConfig.BatchInspectView);
            tabs.Items.Add(CreateTabItem("工位批次列表", inputControl.Control));
            tabs.Items.Add(CreateTabItem("采集记录", CreateDetailListControl(ui.MainView, model.CollectDetailList,
                CollectDetailViewModelViewConfig.BatchViewGroup)));
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
