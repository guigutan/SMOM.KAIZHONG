using DevExpress.Xpf.Scheduler.Native;
using DocumentFormat.OpenXml.Bibliography;
using SIE.Core.Barcodes;
using SIE.Domain.Validation;
using SIE.MES.BatchWIP;
using SIE.Wpf.Command;
using SIE.Wpf.Common.Editors;
using SIE.Wpf.MES.BatchWIP.Moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Wpf.MES.BatchWIP.Commands
{
    /// <summary>
    /// 工位批次列表拆分命令
    /// </summary>
    [Command(ImageName = "CallSplit", Label = "批次拆分", ToolTip = "批次拆分", GroupType = CommandGroupType.Edit)]
    public class InputBatchSplitCommand : ListViewCommand
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
                if (input == null)
                    throw new ValidationException("转入批次不能为空".L10N());
                decimal splitQty = 0;
                var workCell = parentView.GetWorkcell();
                var calculator = new Calculator();
                var result = CRT.Workbench.ShowDialog(Guid.NewGuid().ToString(), calculator, w =>
                {
                    w.Title = "请输入拆分批次数量".L10N();
                    w.Width = 400;
                    w.Height = 400;
                    w.Closing += (s, e) =>
                    {
                        if (w.Result == 0)
                        {
                            splitQty = (decimal)calculator.Value;
                        }
                    };
                });
                if (result == 0)
                {
                    var childBarcode = parentView.NewGenerateSplitInput(input, workCell, splitQty, BarcodeType.BatchBarocde);
                    parentView.ShowTips("批次拆分成功，生成批次{0}".L10nFormat(childBarcode));
                    parentView.RefreshInputBatch();
                }
                
            }
            catch(Exception ex)
            {
                parentView.ShowError(ex.Message);
            }
        }
    }
}
