using SIE.Domain;
using SIE.Packages;
using SIE.Packages.Packings;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SIE.Wpf.MES.WIP.Packings
{

    /// <summary>
    /// 新包装帮助类
    /// </summary>
    public class NewPackingHelper
    {
        /// <summary>
        /// 包装视图模型
        /// </summary>
        readonly NewPackingViewModel _packingViewModel;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="packingViewModel">包装视图模型</param>
        public NewPackingHelper(NewPackingViewModel packingViewModel)
        {
            _packingViewModel = packingViewModel;
        }

        /// <summary>
        /// 扫描成功处理
        /// </summary>
        /// <param name="e">包装流程控制参数</param>
        public async Task SucceedEventHandler(NewPackingStrategyEvent e)
        {
            RemoveChildPackage();

            RefreshOuterPackage(e.OuterPackingRelation);

            if (e.IsPackageQtyFull || e.IsPackageItemFull)
            {
                RT.EventBus.Publish(new DoPackingEvent(DoPackingAction.DoPacking, "MesPacking", e.OuterPackingRelation));
                _packingViewModel.ResetOuterRelation();
                await _packingViewModel.ReloadPackingRelation().ConfigureAwait(false);
            }

            _packingViewModel.ShowTips(e.CollectBarcode, e.OuterPackingRelation);
        }

        /// <summary>
        /// 移除子包装
        /// </summary>
        private void RemoveChildPackage()
        {
            var childPkg = (_packingViewModel.PackingRelations.FirstOrDefault(f => f.PackageNo == _packingViewModel.Barcode));
            if (childPkg != null)
            {
                var lt = RT.Service.Resolve<PackingRelationController>().GetTreeParentList(childPkg);
                lt.Remove(childPkg);
            }
        }

        /// <summary>
        /// 刷新外包装
        /// </summary>
        /// <param name="outerPackingRelation">包装关系</param>
        private void RefreshOuterPackage(PackingRelation outerPackingRelation)
        {
            //获取外包装及下面所有包装
            var doPackedList = RT.Service.Resolve<PackingRelationController>()
                .GetRelationAllNodesByPtreeId(outerPackingRelation.Id);

            //界面移除原来的包装
            foreach (var packingRelation in doPackedList)
            {
                var existsPackingRelation = _packingViewModel.PackingRelations.FirstOrDefault(f => f.Id == packingRelation.Id);
                if (existsPackingRelation != null)
                {
                    _packingViewModel.PackingRelations.Remove(existsPackingRelation);
                }
            }

            //界面加入新生成的包装
            _packingViewModel.PackingRelations.AddRange(doPackedList);
        }

        /// <summary>
        /// 更新父包装
        /// </summary>
        /// <param name="packingRelation">包装关系</param>
        /// <param name="lastPackingRelation">上一条包装关系</param>
        private void UpdateParentPackage(PackingRelation packingRelation, PackingRelation lastPackingRelation)
        {
            if (packingRelation == null) return;
            packingRelation.ItemQty = lastPackingRelation.ItemQty;
            packingRelation.PackageNo = lastPackingRelation.PackageNo;
            packingRelation.PackedQty = lastPackingRelation.PackedQty;
            UpdateParentPackage(RF.GetById<PackingRelation>(packingRelation.TreePId), RF.GetById<PackingRelation>(lastPackingRelation.TreePId));
        }

        /// <summary>
        /// 扫描子包装成功处理
        /// </summary>
        /// <param name="e">包装策略事件</param>
        public virtual void ScanChildPackageSucceedEventHandler(NewPackingStrategyEvent e)
        {
            var packingRelation = _packingViewModel.PackingRelations.FirstOrDefault(p => p.Id == e.OuterPackingRelation.Id);
            if (packingRelation != null)
            {
                packingRelation.ItemQty = e.OuterPackingRelation.ItemQty;
                packingRelation.PackedQty = e.OuterPackingRelation.PackedQty;
            }

            _packingViewModel.ShowJoinTips(packingRelation);
        }

        /// <summary>
        /// 扫描失败处理
        /// </summary>
        /// <param name="e">包装流程控制事件参数</param>
        public virtual void FailedEventHandler(NewPackingStrategyEvent e)
        {
            _packingViewModel.ShowError(e.Error);
            _packingViewModel.ResetStep();
        }
    }
}