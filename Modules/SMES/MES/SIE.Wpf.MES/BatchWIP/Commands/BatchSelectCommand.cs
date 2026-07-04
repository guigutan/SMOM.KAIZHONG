using SIE.Barcodes.WipBatchs;
using SIE.Domain.Validation;
using SIE.MES.BatchWIP;
using SIE.MetaModel.View;
using SIE.Wpf.Command;
using System;

namespace SIE.Wpf.MES.BatchWIP.Commands
{
    /// <summary>
    /// 选择批次命令
    /// </summary>
    [Command(ImageName = "FileFind", Label = "选择批次", ToolTip = "选择批次", GroupType = CommandGroupType.Edit)]
    public class BatchSelectCommand : DetailViewCommand
    {
        /// <summary>
        /// 是否可执行的逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        /// <returns>返回是否可执行</returns>
        public override bool CanExecute(DetailLogicalView view)
        {
            return base.CanExecute(view) && (view.Current as BatchDataCollectionViewModel) != null;
        }

        /// <summary>
        /// 执行具体的逻辑。
        /// </summary>
        /// <param name="view">视图对象</param>
        public override void Execute(DetailLogicalView view)
        {
            var vm = view.Current as BatchDataCollectionViewModel;
            ValidateWorkCell(vm);
            // 资源——》工单——》工单工序((是否首工序)《——工序
            var template = new ListUITemplate(typeof(WipBatch), WipBatchExtViewConfig.BatchSelectView, view.ModuleKey);
            template.BlocksDefined += Template_BlocksDefined;
            var ui = template.CreateUI();
            var mainView = ui.MainView as ListLogicalView;
            var queryView = mainView.QueryView;
            queryView.Querying += QueryView_Querying;
            var result = CRT.Workbench.ShowDialog(ui, w =>
              {
                  w.Title = "生产批次选择".L10N();
                  w.Closing += (s, e) =>
                  {
                      if (w.Result == 0 && mainView.SelectedEntities.Count != 1)
                      {
                          CRT.MessageService.ShowError("请选择批次".L10N());
                          e.Cancel = true;
                      }
                  };
              });
            if (result == 0)
            {
                var wipBatch = mainView.SelectedEntities[0] as WipBatch;
                vm.Barcode = wipBatch.BatchNo;
            }
        }

        /// <summary>
        /// 验证工作单元
        /// </summary>
        /// <param name="vm">批次采集视图模型</param>
        void ValidateWorkCell(BatchDataCollectionViewModel vm)
        {
            try
            {
                vm.GetWorkcell();
            }
            catch
            {
                throw new ValidationException("工作单元信息不能为空，请维护".L10N());
            }
        }

        /// <summary>
        /// 查询前事件
        /// </summary>
        /// <param name="sender">查询视图</param>
        /// <param name="e">参数</param>
        private void QueryView_Querying(object sender, QueryEventArgs e)
        {
            var queryView = sender as QueryLogicalView;
            if (queryView == null || queryView.Current as WipBatchCriteria == null)
                return;
            var criteria = queryView.Current as WipBatchCriteria;
            var vm = View.Current as BatchDataCollectionViewModel;
            var workcell = vm.GetWorkcell();
            criteria.ResourceId = workcell.ResourceId;
            criteria.ProcessId = workcell.ProcessId;
        }

        /// <summary>
        /// 块定义
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void Template_BlocksDefined(object sender, CodeBlocksDefinedEventArgs e)
        {
            e.Blocks.Surrounders.Clear();
            var conditionBlock = new ConditionBlock(typeof(WipBatchCriteria), ViewConfig.QueryView);
            e.Blocks.Surrounders.Add(conditionBlock);
        }
    }
}