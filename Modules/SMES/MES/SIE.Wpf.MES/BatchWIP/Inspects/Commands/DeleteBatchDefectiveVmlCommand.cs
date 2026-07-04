using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Wpf.Command;
using System;
using System.Linq;

namespace SIE.Wpf.MES.BatchWIP.Inspects.Commands
{
    /// <summary>
    /// 删除批次检验不良记录
    /// </summary>
    [Command(ImageName = "DeleteEntity", Label = "删除", ToolTip = "删除数据", Gestures = "Delete", Location = CommandLocation.All, GroupType = CommandGroupType.Edit)]
    public class DeleteBatchDefectiveVmlCommand : ListDeleteCommand
    {
        /// <summary>
        /// 删除命令是否可以使用
        /// </summary>
        /// <param name="view">视图对象</param>
        /// <returns>返回是否可执行</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            ////return base.CanExecute(view);
            var currentMdl = view.Current;
            var flag = currentMdl != null;
            return flag;
        }

        /// <summary>
        /// 删除命令处理方法
        /// </summary>
        /// <param name="view">视图对象</param>
        public override void Execute(ListLogicalView view)
        {
            var current = view.Current as BatchDefectiveViewModel;

            var mainView = view.Relations.Find("mainView");
            if (mainView == null)
            {
                throw new ArgumentNullException("批次检验采集主视图为空".L10N());
            }
            var batchInspectVmdl = mainView?.Current as BatchInspectViewModel;
            if (batchInspectVmdl == null)
            {
                throw new ArgumentNullException("批次检验采集视图模型为空".L10N());
            }
            var batchDefectiveSetVmdl = batchInspectVmdl.BatchDefectSetVmdl;
            if (batchDefectiveSetVmdl == null)
            {
                throw new ArgumentNullException("批次检验不良集合为空".L10N());
            }
            CommandUpdateInputBatch(batchDefectiveSetVmdl, current);
            ClearOutputBatch(batchInspectVmdl);
            ////base.Execute(view);
            var batchDefectiveVmdls = view.Data as EntityList<BatchDefectiveViewModel>;
            batchDefectiveVmdls.Remove(current);
        }

        /// <summary>
        /// 清除转出批次
        /// 删除不良时，如果存在非子批次的转出批次时，清除转出批次
        /// </summary>
        /// <param name="batchInspectVmdl">检验采集视图模型</param>
        private void ClearOutputBatch(BatchInspectViewModel batchInspectVmdl)
        {
            var outputBatch = batchInspectVmdl.OutputBatchList.FirstOrDefault();
            if (outputBatch != null && !outputBatch.IsGenerateBatch)
                batchInspectVmdl.OutputBatchList.Remove(outputBatch);
        }

        /// <summary>
        /// 不良记录删除更新对应InputBatch的数量
        /// </summary>
        /// <param name="batchDefectiveSetVmdl">批次检验不良集合</param>
        /// <param name="batchDefectiveVmdl">批次检验不良记录</param>
        private void CommandUpdateInputBatch(BatchDefectiveSetViewModel batchDefectiveSetVmdl, BatchDefectiveViewModel batchDefectiveVmdl)
        {
            var inputBatchItem = batchDefectiveSetVmdl.InputBatchs.FirstOrDefault(x => x.BatchNo == batchDefectiveVmdl.Barcode
            && x.SubBatchNo == batchDefectiveVmdl.ChildBarcode);
            inputBatchItem.NgQty -= batchDefectiveVmdl.NgQty;
            inputBatchItem.RemainQty += batchDefectiveVmdl.NgQty;
            batchDefectiveSetVmdl.RemainQty += batchDefectiveVmdl.NgQty;
        }
    }
}
