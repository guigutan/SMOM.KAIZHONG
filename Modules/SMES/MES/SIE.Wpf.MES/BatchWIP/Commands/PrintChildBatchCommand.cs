using SIE.Domain.Validation;
using SIE.MES.BatchWIP;
using SIE.Wpf.Command;
using System;

namespace SIE.Wpf.MES.BatchWIP.Commands
{
    /// <summary>
    /// 子批次条码打印命令
    /// </summary>
    [Command(ImageName = "Printer", Label = "打印", ToolTip = "打印", GroupType = CommandGroupType.Edit)]
    public class PrintChildBatchCommand : ListViewCommand
    {
        /// <summary>
        /// 是否可执行的逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        /// <returns>返回是否可执行</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var mainView = view.Relations.Find("mainView");
            if (mainView?.Current as BatchDataCollectionViewModel == null)
            {
                return false;
            }
            var batch = view.Current as InputBatch;
            if (batch == null)
            {
                return false;
            }
            var count = view.SelectedEntities.Count;
            if (count > 1)
            {
                return false;
            }
            //return batch != null && batch.IsGenerateBatch && !batch.SubBatchNo.IsNullOrEmpty() && (mainView?.Current as BatchDataCollectionViewModel) != null;
            return base.CanExecute(view);
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
                var input = view.Current as InputBatch;
                if (input == null)
                    throw new ArgumentNullException("转入批次不能为空".L10N());
                //if (input.SubBatchNo.IsNullOrEmpty())
                //    throw new ValidationException("不允许打印，未生成子批次".L10N());
                vm.PrintBarcode(input.BatchNo);
                vm.ShowTips("批次条码[{0}]打印成功".L10nFormat(input.BatchNo));
            }
            catch (Exception exc)
            {
                vm.ShowError(exc);
            }
        }
    }
}