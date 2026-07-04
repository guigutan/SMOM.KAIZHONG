using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.BatchWIP;
using SIE.Wpf.Command;
using System;
using System.Linq;

namespace SIE.Wpf.MES.BatchWIP.Commands
{
    /// <summary>
    /// 移除转入批次命令
    /// </summary> 
    [Command(ImageName = "DeleteEntity", Label = "移除", ToolTip = "移除转入批次", GroupType = CommandGroupType.Edit)]
    public class RemoveInputBatchCommand : ListViewCommand
    {
        /// <summary>
        /// 是否可执行的逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        /// <returns>返回是否可执行</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var mainView = view.Relations.Find("mainView");
            var vm = mainView?.Current as BatchDataCollectionViewModel;
            var input = view.Current as InputBatch;
            return input != null && view.SelectedEntities.OfType<InputBatch>().Count() == 1 && vm != null && !vm.OutputBatchList.SelectMany(p => p.RelationBatchList).Any(p => p.InputBatchId == input.Id);
        }

        /// <summary>
        /// 移除执行方法
        /// </summary>
        /// <param name="view">视图对象</param>
        public override void Execute(ListLogicalView view)
        {
            var vm = view.Relations.Find("mainView")?.Current as BatchDataCollectionViewModel;
            try
            {
                if (vm == null)
                    throw new ValidationException("未找到批次采集主视图模型，请查看用户权限".L10N());
                vm.ClearTipsInfos();
                var intput = view.Current as InputBatch;
                if (intput == null)
                    throw new ArgumentNullException("转入批次不能为空".L10N());
                vm.InputBatch = intput;
                ValidateRemoveInputBatch(intput, vm.OutputBatchList);
                vm.RemoveInputBatch(intput);
            }
            catch (Exception exc)
            {
                vm.ShowError(exc);
            }
        }

        /// <summary>
        /// 验证转入批次是否可以移除
        /// </summary>
        /// <param name="intput">转入批次</param>
        /// <param name="outputBatches">转出批次列表</param>
        private void ValidateRemoveInputBatch(InputBatch intput, EntityList<OutputBatch> outputBatches)
        {
            if (outputBatches.SelectMany(p => p.RelationBatchList).Any(p => p.InputBatchId == intput.Id))
                throw new ValidationException("不允许移除，转入批次已经关联转出批次".L10N());
        }
    }
}