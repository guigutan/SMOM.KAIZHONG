using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.BatchWIP;
using SIE.Wpf.Command;
using System;
using System.Linq;
using System.Windows;

namespace SIE.Wpf.MES.BatchWIP.Inspects.Commands
{
    /// <summary>
    /// 不良录入命令
    /// </summary>
    [Command(ImageName = "AddEntity", Label = "不良录入", ToolTip = "不良录入", GroupType = CommandGroupType.Edit)]
    public class BatchDefectiveKeyInCommand : ListViewCommand
    {
        /// <summary>
        /// 是否可以执行的逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        /// <returns>返回是否可执行</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var curInputBatch = view.Current as InputBatch;
            var batchInspectvm = view.Relations.Find("mainView")?.Current as BatchInspectViewModel;
            if (curInputBatch == null || batchInspectvm == null)
                return false;
            if (curInputBatch.NgQty > 0)
            {
                return false;
            }
            var batchDefectVmdlsCount = batchInspectvm.BatchDefectSetVmdl?.BatchDefectiveViewModels?.Count;
            return (batchDefectVmdlsCount > 0) || (curInputBatch.RemainQty - curInputBatch.SplitQty >= 0 && curInputBatch.RemainQty >= 0);
        }

        /// <summary>
        /// 执行不良录入命令按钮
        /// </summary>
        /// <param name="view">视图对象</param>
        public override void Execute(ListLogicalView view)
        {
            var mainView = view.Relations.Find("mainView");
            if (mainView == null)
            {
                throw new ArgumentNullException("批次检验采集主视图为空".L10N());
            }
            var batchInspectvm = mainView.Current as BatchInspectViewModel;
            if (batchInspectvm == null)
            {
                throw new ArgumentNullException("批次检验采集视图模型为空".L10N());
            }
            var curInputBatch = view.Current as InputBatch;
            try
            {
                var batchDefectSetVmdl = GetBatchDftSetVmdl(batchInspectvm, curInputBatch);
                batchDefectSetVmdl.batchInspectvm = batchInspectvm;
                var template = new BatchDefectiveKeyInUITemplate(batchDefectSetVmdl);
                template.ModuleKey = mainView.ModuleKey;
                var ui = template.CreateUI();
                ui.MainView.Data = batchInspectvm;
                CRT.Workbench.ShowDialog(ui, w =>
                {
                    w.Title = "不良录入".L10N();
                    w.Height = 400;
                    w.Width = 840;
                    w.WindowState = WindowState.Normal;
                    w.Commands.Clear();
                });
            }
            catch (Exception exc)
            {
                batchInspectvm.ShowError(exc);
            }
        }

        /// <summary>
        /// 获取批次检验不良集合
        /// </summary>
        /// <param name="batchInspectvm">批次检验采集视图</param>
        /// <param name="curInputBatch">当前转入批次</param>
        /// <returns>批次检验不良集合</returns>
        private BatchDefectiveSetViewModel GetBatchDftSetVmdl(BatchInspectViewModel batchInspectvm, InputBatch curInputBatch)
        {
            var batchDefectSetVmdl = batchInspectvm.BatchDefectSetVmdl;
            if (curInputBatch != null)
            {
                if (batchDefectSetVmdl.BatchDefectiveViewModels.Count > 0)
                {
                    var hasBarcodes = batchDefectSetVmdl.BatchDefectiveViewModels.Select(x => x.Barcode).FirstOrDefault();
                    if (hasBarcodes != null && hasBarcodes != curInputBatch.BatchNo)
                    {
                        throw new ValidationException("已经包含批次 [{0}] 的不良，不能再添加批次 [{1}] ".L10nFormat(hasBarcodes, curInputBatch.BatchNo));
                    }
                }
                batchDefectSetVmdl.Workstation = batchInspectvm.Workstation;
                batchDefectSetVmdl.OutputBatchs = batchInspectvm.OutputBatchList;
                batchDefectSetVmdl.InputBatchs = batchInspectvm.InputBatchList;
                batchDefectSetVmdl.WorkOrder = batchInspectvm.WorkOrder;
                batchDefectSetVmdl.Barcode = curInputBatch.BatchNo;
                batchDefectSetVmdl.ChildBarcode = curInputBatch.SubBatchNo;
                batchDefectSetVmdl.Qty = curInputBatch.Qty;
                batchDefectSetVmdl.NgQty = 0;
                batchDefectSetVmdl.RemainQty = curInputBatch.RemainQty - curInputBatch.SplitQty;
                batchDefectSetVmdl.BarcodeType = batchInspectvm.Step.OutputCollectStep.BarcodeType;
            }
            return batchDefectSetVmdl;
        }
    }
}