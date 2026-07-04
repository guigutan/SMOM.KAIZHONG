using DevExpress.Xpf.Core;
using SIE.Wpf.MES.WIP;
using SIE.Wpf.MES.WIP.Assemblys;

namespace SIE.Wpf.MES.BatchWIP.Assemblys
{
    /// <summary>
    /// 批次上料采集模板
    /// </summary>
    public class BatchAssemblyUITemplate : BatchCollectionUITemplate
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public BatchAssemblyUITemplate() : base(typeof(BatchAssemblyViewModel))
        {
        }

        /// <summary>
        /// UI创建后
        /// </summary>
        /// <param name="ui">UI控件</param>
        protected override void OnUIGenerated(ControlResult ui)
        {
            var model = new BatchAssemblyViewModel() { };
            var tabs = new DXTabControl();
            tabs.Margin = new System.Windows.Thickness(5, 5, 5, 0);
            tabs.Items.Add(CreateTabItem("装配清单", CreateDetailListControl(ui.MainView, model.AssemblyDetailList, AssemblyDetailViewModelViewConfig.BatchAssemblyDetailView)));
            tabs.Items.Add(CreateTabItem("上料采集", CreateDetailListControl(ui.MainView, model.LoadItemList, LoadItemViewModelViewConfig.BatchLoadItemView)));
            tabs.Items.Add(CreateTabItem("下料明细", CreateDetailListControl(ui.MainView, model.UnloadItemList)));
            tabs.Items.Add(CreateTabItem("工位配送", CreateDetailListControl(ui.MainView, model.MoveItemList)));
            var inputControl = CreateListControl(ui.MainView, model.InputBatchList, InputBatchViewConfig.BatchMoveViewStr);
            tabs.Items.Add(CreateTabItem("工位批次列表", inputControl.Control));
            tabs.Items.Add(CreateTabItem("批次采集记录", CreateDetailListControl(ui.MainView, model.CollectDetailList, CollectDetailViewModelViewConfig.BatchViewGroup)));
            CreateReportTaskControl(tabs, ui.MainView, model, "CollectionView");
            //如果不用显示消息列表，则注释下面这句
            tabs.Items.Add(CreateTabItem("消息列表", CreateMessagerControl(model)));
            var layout = ui.Control as DockLayout;
            layout.Children.Add(CreateOperationControl(model.Workstation));
            layout.Children.Add(tabs);
            ui.MainView.Data = model;
            base.OnUIGenerated(ui);
        }

    }
}