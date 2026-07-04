using SIE.Core.Barcodes;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.BatchWIP;
using SIE.Wpf.Command;
using SIE.Wpf.Common.Editors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Wpf.MES.BatchWIP.Commands
{
    /// <summary>
    /// 工位批次列表合并命令
    /// </summary>
    [Command(ImageName = "CallMerge", Label = "批次合并", ToolTip = "批次合并", GroupType = CommandGroupType.Edit)]
    public class InputBatchMergeCommand : ListViewCommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var model = view.SelectedEntities;
            if (model == null)
            {
                return false;
            }
            if (model.Count <= 1)
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
                EntityList<InputBatch> inputs = new EntityList<InputBatch>();
                foreach(var item in view.SelectedEntities)
                {
                    var input = item as InputBatch;
                    inputs.Add(input);
                }
                if (inputs == null)
                {
                    throw new ValidationException("转入批次不能为空".L10N());
                }
                if (inputs.Count <= 1)
                {
                    throw new ValidationException("请选择至少2个转入批次进行批次合并操作".L10N());
                }
                var workCell = parentView.GetWorkcell();
                var childBarcode = parentView.NewGenerateMergeInput(inputs, workCell, BarcodeType.BatchBarocde);
                parentView.ShowTips("批次合并成功，生成批次{0}".L10nFormat(childBarcode));
                parentView.RefreshInputBatch();

            }
            catch (Exception ex)
            {
                parentView.ShowError(ex);
            }
        }
    }
}
