using SIE.Domain.Validation;
using SIE.MES.BatchWIP;
using SIE.Wpf.MES.BatchWIP.Commands;
using System;
using System.Linq;

namespace SIE.Wpf.MES.BatchWIP.Inspects.Commands
{
    /// <summary>
    /// 移除转入批次命令
    /// </summary> 
    [Command(ImageName = "DeleteEntity", Label = "移除", ToolTip = "移除转入批次", GroupType = CommandGroupType.Edit)]
    public class RemoveInputBatchCommandInspect : RemoveInputBatchCommand
    {
        /// <summary>
        /// 批次检验移除是否可执行的逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        /// <returns>返回是否可执行</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return base.CanExecute(view);
        }

        /// <summary>
        /// 批次检验移除执行方法
        /// </summary>
        /// <param name="view">视图对象</param>
        public override void Execute(ListLogicalView view)
        {
            if (view == null)
            {
                return;
            }
            var mainView = view.Relations.Find("mainView")?.Current;
            var batchInspectVmdl = mainView as BatchInspectViewModel;
            if (batchInspectVmdl == null)
            {
                throw new ArgumentNullException("批次检验采集主视图为空".L10N());
            }
            try
            {
                var curInputBatch = view.Current as InputBatch;
                if (curInputBatch == null)
                {
                    throw new ArgumentNullException("转入批次为空".L10N());
                }
                var batchDefectiveSetVmdl = batchInspectVmdl.BatchDefectSetVmdl;
                var curNgInputBatch = batchDefectiveSetVmdl.BatchDefectiveViewModels.FirstOrDefault(x => x.Barcode == curInputBatch.BatchNo
                                    && x.ChildBarcode == curInputBatch.SubBatchNo);
                if (curNgInputBatch != null)
                {
                    throw new ValidationException("不允许移除，转入批次已经存在不良记录".L10N());
                }

                base.Execute(view);
            }
            catch (Exception exc)
            {
                ((BatchDataCollectionViewModel)mainView).ShowError(exc);
            }
        }
    }
}
