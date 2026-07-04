using SIE.Domain.Validation;
using SIE.MES.BatchWIP;
using SIE.Wpf.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Wpf.MES.BatchWIP.Commands
{
    /// <summary>
    /// 工位批次列表移除命令
    /// </summary>
    [Command(ImageName = "AppRemove", Label = "移除", ToolTip = "移除", GroupType = CommandGroupType.Edit)]
    public class InputBatchRemoveCommand : ListViewCommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var model = view.Current as InputBatch;
            if (model == null)
            {
                return false;
            }
            var count = view.SelectedEntities.Count;
            if (count > 1)
            {
                return false;
            }
            return base.CanExecute(view);
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="view"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void Execute(ListLogicalView view)
        {
            var parentView = view.Relations.Find("mainView")?.Current as BatchDataCollectionViewModel;
            try
            {
                if (parentView == null)
                    throw new ValidationException("未找到批次采集主视图模型，请查看用户权限".L10N());
                parentView.ClearTipsInfos();
                var input = view.Current as InputBatch;
                var workcell = parentView.GetWorkcell();
                if (input == null)
                    throw new ValidationException("转入批次不能为空".L10N());
                parentView.NewRemoveInput(input, workcell);
                parentView.ShowTips("批次{0}移除成功".L10nFormat(input.BatchNo));
                parentView.RefreshInputBatch();
            }
            catch (Exception ex)
            {
                parentView.ShowError(ex);
            }
        }
    }
}
