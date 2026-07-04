using DevExpress.Xpf.Core;
using SIE.Domain;
using SIE.Wpf.MES.WIP;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SIE.Wpf.MES.BatchWIP.Repairs
{
    /// <summary>
    /// 维修采集模板
    /// </summary>
    public class BatchRepairUITemplate : BatchCollectionUITemplate
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BatchRepairUITemplate() : base(typeof(BatchRepairViewModel))
        {
        }

        /// <summary>
        /// 创建UI
        /// </summary>
        /// <param name="ui">控件结果</param>
        protected override void OnUIGenerated(ControlResult ui)
        {
            var model = new BatchRepairViewModel();
            var tabs = new DXTabControl();
            tabs.Margin = new Thickness(5, 5, 5, 0);
            var batchControl = CreateBatchListControl(ui.MainView, model.BatchRepairDefectList, model.InputBatchList, BatchRepairDefectViewModelViewConfig.BatchRepairView, InputBatchViewConfig.BatchRepairingView);
            tabs.Items.Add(CreateTabItem("工位批次列表", batchControl));
            tabs.Items.Add(CreateTabItem("采集记录", CreateDetailListControl(ui.MainView, model.CollectDetailList, CollectDetailViewModelViewConfig.BatchRepairGroup)));
            //tabs.Items.Add(CreateTabItem("采集记录", CreateDetailListControl(ui.MainView, model.CollectDetailList, CollectDetailViewModelViewConfig.BatchRepairGroup)));
            //如果不用显示消息列表，则注释下面这句
            tabs.Items.Add(CreateTabItem("消息列表", CreateMessagerControl(model)));
            var layout = ui.Control as DockLayout;
            layout.Children.Add(CreateOperationControl(model.Workstation));
            layout.Children.Add(tabs);
            ui.MainView.Data = model;
            base.OnUIGenerated(ui);
        }

        ///// <summary>
        ///// 创建缺陷控件
        ///// </summary>
        ///// <param name="mainView">主视图</param>
        ///// <param name="defectList">缺陷列表</param>
        ///// <param name="details">装配明细</param>
        ///// <returns>缺陷控件</returns>
        //FrameworkElement CreateDefectControl(LogicalView mainView, EntityList<BatchRepairDefectViewModel> defectList, EntityList<BatchProductAssemblyDetailViewModel> details)
        //{
        //    var panel = new Grid();
        //    panel.RowDefinitions.Add(new RowDefinition() { });
        //    panel.ColumnDefinitions.Add(new ColumnDefinition() { });
        //    var defectContol = CreateDetailListControl(mainView, defectList, BatchRepairDefectViewModelViewConfig.BatchRepairView);
        //    panel.Children.Add(defectContol);
        //    Grid.SetColumn(defectContol, 0);

        //    return panel;
        //}

        /// <summary>
        /// 创建明细列表控件
        /// </summary>
        /// <param name="mainView">视图对象</param>
        /// <param name="input">维修信息</param>
        /// <param name="output">维修批次</param>
        /// <param name="inputViewGroup">入站视图组名称</param>
        /// <param name="outputViewGroup">出站视图组名称</param>
        /// <returns>返回UI</returns>
        protected override FrameworkElement CreateBatchListControl(LogicalView mainView, EntityList input, EntityList output, string inputViewGroup = null, string outputViewGroup = null)
        {
            var defectUI = CreateListControl(mainView, input, inputViewGroup);
            var RepairBatchUI = CreateListControl(mainView, output, outputViewGroup);

            var batchControl = new BatchControl(defectUI, RepairBatchUI);
            (batchControl.FindName("inputControl") as DevExpress.Xpf.LayoutControl.GroupBox).Header = "维修信息".L10N();
            (batchControl.FindName("outputControl") as DevExpress.Xpf.LayoutControl.GroupBox).Header = "维修批次".L10N();
            return batchControl;
        }
    }
}
