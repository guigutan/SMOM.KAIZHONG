using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.BatchWIP;
using SIE.Wpf.Command;
using System;
using System.Linq;

namespace SIE.Wpf.MES.BatchWIP.Commands
{
    /// <summary>
    /// 移除转出批次命令
    /// </summary>
    [Command(ImageName = "DeleteEntity", Label = "移除", ToolTip = "移除转出批次", GroupType = CommandGroupType.Edit)]
    public class RemoveOutputBatchCommand : ListViewCommand
    {
        /// <summary>
        /// 是否可执行的逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        /// <returns>返回是否可执行</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var mainView = view.Relations.Find("mainView");
            return view.SelectedEntities.OfType<OutputBatch>().Count() == 1 && (mainView?.Current as BatchDataCollectionViewModel) != null;
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
                var output = view.Current as OutputBatch;
                if (output == null)
                    throw new ArgumentNullException("转出批次不能为空".L10N());
                vm.RemoveOutBatch(output); ////先从出站集合中移除对象,再刷新入站集合
                vm.RefreshInputBatch(output);
                vm.ShowTips("转出批次[{0}]移除成功".L10nFormat(output.BatchNo));
            }
            catch (Exception exc)
            {
                vm.ShowError(exc);
            }
        }
    }
}