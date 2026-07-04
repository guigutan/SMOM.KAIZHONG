using DevExpress.Xpf.Grid;
using SIE.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Logging;
using SIE.MES.WIP.Packings;
using SIE.MES.WorkOrders;
using SIE.Packages;
using SIE.Packages.Packings;
using SIE.Packages.Packings.Enums;
using SIE.Wpf.Command;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Wpf.MES.WIP.Packings.Commands
{
    /// <summary>
    /// 打包命令
    /// 1、单个层级打包，未打包的单个层级可以进行打包
    /// 2、选择多个已打包的同一层级，可以打包成上一层级 (包装层级、工单必须一致)
    /// </summary>
    [Command(ImageName = "Package", Label = "打包", ToolTip = "打包", GroupType = CommandGroupType.Edit)]
    public class PackingCommand : ListViewCommand
    {
        readonly ILog logger = Logging.LogManager.GetLogger("wip");
        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var relations = view.SelectedEntities.OfType<PackingRelation>();
            if (relations.Any(p => p.TreePId != null))
                return false;
            if (relations.Count() > 1)
            {
                var relation = relations.FirstOrDefault();
                var result = !relations.Any(p => p.PackageNo.IsNullOrWhiteSpace()) && !relations.Any(p => p.PackageUnitId != relation.PackageUnitId) && !relations.Any(p => p.WorkOrderId != relation.WorkOrderId);
                return result;
            }

            return relations.Count() == 1;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            using (PerformenceWatcher.Start(logger, "PackingCommand"))
            {
                var vm = (view.Parent.Current as PackingViewModel);
                try
                {
                    if (vm == null)
                        throw new ValidationException("包装视图模型为空,请检查".L10N());
                    var relations = view.SelectedEntities.OfType<PackingRelation>().AsEntityList();
                    decimal? packedQty = ValidatePackingRelations(relations);
                    EntityList<PackingRelation> listPack = null;
                    if (relations.Count == 1 && relations[0].PackageNo.IsNullOrEmpty())
                    {
                        using (PerformenceWatcher.Start(logger, "DoPacking"))
                        {
                            var currentPkg = relations.FirstOrDefault();
                            var relation = relations.FirstOrDefault();
                            if (relation.PackageNo.IsNullOrEmpty())
                            {
                                if (packedQty.HasValue && currentPkg.PackedQty < packedQty.Value && !CRT.MessageService.AskQuestion("外包装最大包装数为[{0}]，当前选择包装数为[{1}]，是否生成未满层级包装？".L10nFormat(packedQty.Value, relation.PackedQty)))
                                    return;
                            }
                            else
                            {
                                if (packedQty.HasValue && currentPkg.PackedQty < packedQty.Value && !CRT.MessageService.AskQuestion("外包装最大包装数为[{0}]，当前选择包装数为[{1}]，是否生成未满层级包装？".L10nFormat(packedQty.Value, relations.Count)))
                                    return;
                            }

                            listPack = RT.Service.Resolve<WipPackingController>().DoPacking(currentPkg, vm.GetWorkcell(), vm.AutoDoPackingMode == AutoDoPackingMode.AutoCasePacking);
                        }
                    }
                    else
                    {
                        using (PerformenceWatcher.Start(logger, "DoMultLevelPacking"))
                        {
                            if (packedQty.HasValue && relations.Count < packedQty.Value && !CRT.MessageService.AskQuestion("外包装最大包装数为[{0}]，当前选择包装数为[{1}]，是否生成未满层级包装？".L10nFormat(packedQty.Value, relations.Count)))
                                return;
                            //多个层级打包或者单个已打包层级打包成上一层级
                            listPack = RT.Service.Resolve<WipPackingController>().DoMultLevelPacking(relations, vm.GetWorkcell(), vm.AutoDoPackingMode == AutoDoPackingMode.AutoCasePacking);
                        }
                    }

                    var doPacked = listPack[listPack.Count - 1];
                    AddCurrentPackageToView(view, vm, doPacked);

                    using (PerformenceWatcher.Start(logger, "PackingCommand-1"))
                    {
                        vm.PackageRuleDetailList.MarkSaved();
                        view.Current = doPacked;
                        var treeView = view.Control.View as TreeListView;
                        treeView.ExpandAllNodes();
                        OnDoListPacked(listPack);
                        vm.ResetOuterRelation();
                        view.Control.SelectedItems.Clear();
                        vm.ReloadPackingRelation();
                    }
                }
                catch (Exception exc)
                {
                    vm.ShowError(exc);
                }
            }
        }

        /// <summary>
        /// 验证打包条件
        /// </summary>
        /// <param name="relations">包装关系列表</param>
        decimal? ValidatePackingRelations(EntityList<PackingRelation> relations)
        {
            if (relations.Count <= 0)
                throw new ValidationException("请选择包装".L10N());
            var relation = relations.FirstOrDefault();
            if (relations.Count == 1)
            {
                if (relation.PackageNo.IsNullOrEmpty())
                {
                    var workOrder = RF.GetById<WorkOrder>(relation.WorkOrderId);
                    return workOrder.PackageRuleDetailList.FirstOrDefault(f => f.PackageUnitId == relation.PackageUnitId)?.LevelQty;
                }
                else
                    return ValidateLevelQty(relations.Count, relation);
            }
            else
            {
                if (relations.Any(p => p.PackageNo.IsNullOrEmpty()) && !relations.Any(p => p.PackageUnitId != relation.PackageUnitId) && !relations.Any(p => p.WorkOrderId != relation.WorkOrderId))
                    throw new ValidationException("打包失败，多个包装打包必须都是同一包装层级的已打包包装且工单一致".L10N());
                return ValidateLevelQty(relations.Count, relation);
            }
        }

        /// <summary>
        /// 验证包装层级数量
        /// </summary>
        /// <param name="count">待打包包装数</param>
        /// <param name="relation">包装关系</param>
        /// <returns>包装层级数量</returns>
        private decimal? ValidateLevelQty(int count, PackingRelation relation)
        {
            var workOrder = RF.GetById<WorkOrder>(relation.WorkOrderId);
            var levelQty = RT.Service.Resolve<WipPackingController>().GetOutPackingLevelQty(relation.PackageUnit, workOrder);
            if (count > levelQty)
                throw new ValidationException("打包失败，外包装最大包装数为[{0}]，当前包装数为[{1}]".L10nFormat(levelQty, count));
            return levelQty;
        }

        /// <summary>
        /// 将当前包装添加到视图
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <param name="vm">包装ViewModel</param>
        /// <param name="doPacked">打包</param>
        private void AddCurrentPackageToView(ListLogicalView view, PackingViewModel vm, PackingRelation doPacked)
        {
            using (PerformenceWatcher.Start(logger, "AddCurrentPackageToView"))
            {
                if (!vm.PackingRelationList.Any(e => e.PackageNo == doPacked.PackageNo))
                {
                    var doPackedList = RT.Service.Resolve<PackingRelationController>().GetRelationAllNodesByPtreeId(doPacked.Id);
                    foreach (var p in doPackedList)
                    {
                        var relation = vm.PackingRelationList.FirstOrDefault(r => r.Id == p.Id);
                        if (relation != null)
                            vm.PackingRelationList.Remove(relation);
                    }
                    vm.PackingRelationList.AddRange(doPackedList);
                }
                vm.PackageRuleDetailList.MarkSaved();
            }
        }

        /// <summary>
        /// 打包后
        /// </summary>
        /// <param name="currentPkg">当前包装关系</param>      
        private void OnDoListPacked(EntityList<PackingRelation> currentPkg)
        {
            List<PackingRelation> prints = new List<PackingRelation>();
            prints.AddRange(currentPkg);
            RT.EventBus.Publish(new DoPackingEvent(DoPackingAction.Packed, "MesPacking", prints.ToArray()));
        }
    }
}
