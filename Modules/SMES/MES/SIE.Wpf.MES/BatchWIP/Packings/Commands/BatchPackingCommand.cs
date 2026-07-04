using SIE.Domain;
using SIE.MES.BatchWIP;
using SIE.MES.BatchWIP.BatchPackings;
using SIE.MES.WIP.Packings;
using SIE.Wpf.Command;
using System;
using System.Linq;

namespace SIE.Wpf.MES.BatchWIP.Packings.Commands
{
    /// <summary>
    /// 打包命令
    /// </summary>
    [Command(Label = "打包", ToolTip = "打包", ImageName = "Package", GroupType = CommandGroupType.Edit)]
    public class BatchPackingCommand : ListViewCommand
    {
        /// <summary>
        /// 执行条件
        /// </summary>
        /// <param name="view">逻辑视图</param>
        /// <returns>条件结果</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var selectedEntities = view.SelectedEntities.OfType<InputBatch>();

            var mainView = view.Relations.Find("mainView");
            var model = (mainView?.Current as BatchPackingViewModel);

            var flag = base.CanExecute(view) && model != null &&
                    selectedEntities.Any() &&
                    selectedEntities.Sum(p => p.SplitQty) > 0 &&
                    selectedEntities.GroupBy(p => p.WipBatchId).Count() == 1;

            if (model != null && model.ScanMode == SIE.MES.WIP.Packings.ScanMode.Normal)
            {
                return flag;
            }
            else
            {
                return flag && model.OuterBatchPackingRelation != null;
            }
        }

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="view"></param>
        public override void Execute(ListLogicalView view)
        {
            var vm = view.Relations.Find("mainView")?.Current as BatchPackingViewModel;
            if (vm == null)
            {
                throw new ArgumentNullException("批次包装采集视图模型为空".L10N());
            }
            var batchList = new EntityList<InputBatch>();
            batchList.AddRange(view.SelectedEntities.OfType<InputBatch>());
            try
            {
                if (vm.ScanMode == ScanMode.Normal) 
                {
                    bool isfullBox = true;
                    //if (batchList.Sum(p => p.SplitQty) < vm.MasterPackageRuleLevel.Qty)
                    //{
                    //    bool isAskTrue = CRT.MessageService.AskQuestion("是否生成未满批次包装".L10N());

                    //    if (isAskTrue)
                    //    {
                            isfullBox = false;
                            RT.Service.Resolve<BatchWipPackingController>().DoBatchPacking(batchList, vm.GetWorkcell(), isfullBox, true);
                            vm.RefreshStatistics();
                            vm.RefrshReportTasks(Core.Items.RetrospectType.Batch, true);
                            vm.ShowTips("打包成功！".L10N());
                    //    }
                    //}
                }

                if (vm.ScanMode == ScanMode.Join)
                {
                    RT.Service.Resolve<BatchWipPackingController>().JoinWithIuputBatch(batchList, vm.OuterBatchPackingRelation, vm.CurrentPackageRuleLevel);
                    vm.RefreshStatistics();
                    vm.RefrshReportTasks(Core.Items.RetrospectType.Batch, true);
                    vm.ShowTips("加入成功！".L10N());
                }
            }
            catch (Exception exc)
            {
                vm?.ShowError(exc);
            }
            finally
            {
                vm.RefreshInputBatch();
                vm.ReloadPackingRelation();
            }
        }
    }
}
