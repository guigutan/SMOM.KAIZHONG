using SIE.Common.Sort;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.BatchWIP.BatchPackings;
using SIE.MES.WIP.Packings;
using SIE.MES.WorkOrders;
using SIE.Packages;
using SIE.Wpf.Command;
using System;
using System.Linq;

namespace SIE.Wpf.MES.BatchWIP.Packings.Commands
{
    /// <summary>
    /// 级联打包命令
    /// </summary>、
    [Command(Label = "级联打包", ToolTip = "级联打包", ImageName = "Package", GroupType = CommandGroupType.Edit)]
    public class PkgBatchPackingCommand : ListViewCommand
    {
        /// <summary>
        /// 条件
        /// </summary>
        /// <param name="view">逻辑视图</param>
        /// <returns>条件结果</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var selectedEntities = view.SelectedEntities.OfType<BatchPackingRelation>();
            var mainView = view.Relations.Find("mainView");
            var model = (mainView?.Current as BatchPackingViewModel);
            if (model == null)
                return false;
            var flag = base.CanExecute(view) && selectedEntities.Any() &&
                selectedEntities.GroupBy(p => p.PackingBatch).Count() == 1 &&
                selectedEntities.GroupBy(p => p.PackageUnitId).Count() == 1;
            if (model.ScanMode == ScanMode.Normal)
            {
                //正常
                return flag;
            }
            else
            {
                //加入
                return flag && model.OuterBatchPackingRelation != null && !selectedEntities.Any(p => p.TreePId != null);
            }
        }

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="view">逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var vm = view.Relations.Find("mainView")?.Current as BatchPackingViewModel;
            if (vm == null)
            {
                throw new ArgumentNullException("批次包装采集视图模型为空".L10N());
            }
            var relaList = new EntityList<BatchPackingRelation>();
            relaList.AddRange(view.SelectedEntities.OfType<BatchPackingRelation>());

            try
            {
                if (vm.ScanMode == ScanMode.Normal) 
                {
                    PackageNormal(vm, relaList);
                }

                if (vm.ScanMode == ScanMode.Join)
                {
                    RT.Service.Resolve<BatchWipPackingController>().JoinWithReletion(relaList, vm.OuterBatchPackingRelation, vm.CurrentPackageRuleLevel, vm.ChildPackageRuleLevel);
                    vm.ShowTips("加入成功！".L10N());
                }
            }
            catch (Exception exc)
            {
                vm?.ShowError(exc);
            }
            finally
            {
                vm.ReloadPackingRelation();
            }
        }

        /// <summary>
        /// 正常包装执行
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="relaList"></param>
        /// <returns></returns>
        private void PackageNormal(BatchPackingViewModel vm, EntityList<BatchPackingRelation> relaList)
        {
            WorkOrderPackageRuleDetail currRuleUnit = vm.PackageRuleDetailList.FirstOrDefault(p => p.PackageUnitId == relaList.FirstOrDefault()?.PackageUnitId);
            if (currRuleUnit == null)
                throw new ValidationException("包装对应的包装规则未在工单维护，请确认后再打包！".L10N());

            WorkOrderPackageRuleDetail parentRuleUnit = vm.PackageRuleDetailList.OrderBy(p => SortExtension.GetIndex(p)).FirstOrDefault(p => SortExtension.GetIndex(p) > SortExtension.GetIndex(currRuleUnit));
            if (parentRuleUnit == null)
                throw new ValidationException("包装对应的包装层级已是最外层包装！".L10N());

            bool isfullBox = true;
            if (relaList.Count < parentRuleUnit.LevelQty)
            {
                if (CRT.MessageService.AskQuestion("是否生成未满批次包装".L10N()))
                    isfullBox = false;
                else
                    return ;
            }

            RT.Service.Resolve<BatchWipPackingController>().DoPkgPackingByPackageUnit(relaList, vm.GetWorkcell(), isfullBox, true);
            vm.ShowTips("级联打包成功！".L10N());
        }
    }
}
