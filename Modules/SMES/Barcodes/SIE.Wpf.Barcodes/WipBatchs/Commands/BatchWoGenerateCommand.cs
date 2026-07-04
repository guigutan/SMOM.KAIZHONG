using SIE.Barcodes.Printables;
using SIE.Barcodes.WipBatchs;
using SIE.Wpf.Barcodes.WipBatchs.ViewModels;
using SIE.Wpf.Command;
using System;

namespace SIE.Wpf.Barcodes.WipBatchs.Commands
{
    /// <summary>
    /// 批次生成命令
    /// </summary>
    [Command(Label = "批次生成", ImageName = "TextRelease", ToolTip = "工单批次生成", GroupType = CommandGroupType.Edit)]
    public class BatchWoGenerateCommand : ListViewCommand
    {
        /// <summary>
        /// 命令执行条件
        /// </summary>
        /// <param name="view">逻辑视图</param>
        /// <returns>执行结果</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var wo = view.Current as BatchWorkOrder;

            return base.CanExecute(view) && wo != null && (wo.GeneratedQty == null || wo.GeneratedQty < wo.PlanQty);
        }

        /// <summary>
        /// 命令执行方法
        /// </summary>
        /// <param name="view">逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            ShowGeneratingView();

            view.QueryView?.TryExecuteQuery();
        }

        /// <summary>
        /// 显示批次生成页面
        /// </summary>
        private void ShowGeneratingView()
        {
            var batchWo = View.Current as BatchWorkOrder;
            string key = CRT.Workbench.CreateKey(ViewConfig.DetailsView, typeof(BatchWorkOrder), batchWo);
            var model = new BatchGeneratingViewModel(batchWo);

            var template = new DetailsUITemplate<BatchGeneratingViewModel>(View.ModuleKey);
            var ui = template.CreateUI();
            ui.MainView.Data = model;
            if (model.Template != null && model.Template.EntityType != typeof(WipBatchPrintable).GetQualifiedName())
            {
                model.Template = null;
                CRT.MessageService.ShowWarning("工单[{0}]打印设置的默认标签模板不是生产批次类型的，请重新选择打印模板".L10nFormat(batchWo.No));
            }

            CRT.Workbench.ShowDialog(key, ui.Control, view =>
            {
                view.Title = this.Label + " - " + batchWo.No;
                view.Width = 1335;
                view.Height = 660;

                view.Commands.Clear();
            });
        }
    }
}
