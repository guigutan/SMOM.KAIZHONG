using SIE.Domain.Validation;
using SIE.Wpf.Command;
using System;

namespace SIE.Wpf.MES.BatchWIP.Commands
{
    /// <summary>
    /// 生成子批次命令
    /// </summary>
    [Command(ImageName = "AddEntity", Label = "生成子批次", ToolTip = "生成子批次", GroupType = CommandGroupType.Edit)]
    public class GenerateChildBatchCommand : ListViewCommand
    {
        /// <summary>
        /// 是否可执行的逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        /// <returns>返回是否可执行</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var vm = view.Relations.Find("mainView")?.Current as BatchDataCollectionViewModel;
            return vm != null && view.Data.Count < 1 && vm.Step.IsGenerateBatch && vm.InputBatchList.Count > 0;
        }

        /// <summary>
        /// 执行方法
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
                if (vm?.WorkOrder == null)
                    throw new ValidationException("未找到产线在制工单".L10N());
                vm.GenerateOutputBatch();
                vm.ShowTips("子批次条码生成成功".L10nFormat());
            }
            catch (Exception exc)
            {
                vm.ShowError(exc);
            }
        }
    }
}