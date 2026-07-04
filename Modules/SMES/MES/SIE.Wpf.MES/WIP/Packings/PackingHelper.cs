using SIE.Domain;
using SIE.Packages;
using SIE.Packages.Packings;
using SIE.Packages.Packings.Enums;
using System;
using System.Linq;

namespace SIE.Wpf.MES.WIP.Packings
{
    /// <summary>
    /// 包装帮助类
    /// </summary>
    public class PackingHelper
    {
        /// <summary>
        /// 包装视图模型
        /// </summary>
        readonly PackingViewModel _packingViewModel;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="packingViewModel">包装视图模型</param>
        public PackingHelper(PackingViewModel packingViewModel)
        {
            _packingViewModel = packingViewModel;
        }

        /// <summary>
        /// 扫描成功处理
        /// </summary>
        /// <param name="e">包装流程控制参数</param>
        public void SucceedEventHandler(PackingStrategyEvent e)
        {
            RemoveChildPackage();
            RefreshOuterPackage(e.OuterPackingRelation);
            if (e.IsPackageQtyFull || e.IsPackageItemFull)
                SendFullEvent(e);
            _packingViewModel.ShowTips();
        }

        /// <summary>
        /// 发送已包满事件
        /// </summary>
        /// <param name="e">包装策略事件</param> 
        protected void SendFullEvent(PackingStrategyEvent e)
        {
            RT.EventBus.Publish(new DoPackingEvent(DoPackingAction.DoPacking, "MesPacking", e.OuterPackingRelation));

            _packingViewModel.ResetOuterRelation();
        }

        /// <summary>
        /// 移除子包装
        /// </summary>
        private void RemoveChildPackage()
        {
            var childPkg = (_packingViewModel.PackingRelationList.FirstOrDefault(f => f.PackageNo == _packingViewModel.Barcode));
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
            var existsPackingRelation = _packingViewModel.PackingRelationList.FirstOrDefault(f => f.Id == outerPackingRelation.Id);
            if (existsPackingRelation == null)
            {
                var doPackedList = RT.Service.Resolve<PackingRelationController>().GetRelationAllNodesByPtreeId(outerPackingRelation.Id);
                foreach (var node in doPackedList)
                {
                    var existsRelation = _packingViewModel.PackingRelationList.FirstOrDefault(f => f.Id == node.Id);
                    if (existsRelation != null)
                    {
                        _packingViewModel.PackingRelationList.Remove(existsRelation);
                    }
                    _packingViewModel.PackingRelationList.Add(node);
                }
            }
            else
            {
                _packingViewModel.PackingRelationList.Remove(existsPackingRelation);
                _packingViewModel.PackingRelationList.Add(outerPackingRelation);
            }
        }

        /// <summary>
        /// 扫描策略父包装成功处理
        /// </summary>
        /// <param name="e">包装策略事件</param>
        public virtual void ScanParentPackageSucceedEventHandler(PackingStrategyEvent e)
        {
            JoinPackingRelationToTree(_packingViewModel.PackingRelationList, e.OuterPackingRelation, string.Empty);
            _packingViewModel.ShowTips();
        }

        /// <summary>
        /// 把包装关系数据加入到包装关系视图的树结构里
        /// </summary>
        /// <param name="uiPackingRelationList">包装关系列表</param>
        /// <param name="lastOuterPackingRelation">上一次包装关系</param>
        /// <param name="insiderBarcode">内部条码</param>
        private void JoinPackingRelationToTree(EntityList<PackingRelation> uiPackingRelationList, PackingRelation lastOuterPackingRelation, string insiderBarcode)
        {
            PackingRelation lastGrandRelation = RF.GetById<PackingRelation>(lastOuterPackingRelation.TreePId);
            PackingRelation oldGrandRelation;
            PackingRelation oldInnerRelation;
            PackingRelation oldOuterPackingRelation;
            Find_UI_PackingRelation(uiPackingRelationList, lastOuterPackingRelation, insiderBarcode, out oldGrandRelation, out oldInnerRelation, out oldOuterPackingRelation);
            Update_UI_PackingRelation(uiPackingRelationList, lastOuterPackingRelation, oldGrandRelation, oldInnerRelation, oldOuterPackingRelation);
            UpdateParentPackage(oldGrandRelation, lastGrandRelation);
        }

        /// <summary>
        /// 查找包装关系
        /// </summary>
        /// <param name="packingRelationList">包装关系列表</param>
        /// <param name="outerPackingRelation">加入后的外部包装关系</param>
        /// <param name="insiderBarcode">加入的条码</param>
        /// <param name="oldGrandRelation">旧的祖包装关系</param>
        /// <param name="oldInnerRelation">旧的内部包装关系</param>
        /// <param name="oldOuterPackingRelation">旧的外部包装关系</param>
        private void Find_UI_PackingRelation(EntityList<PackingRelation> packingRelationList, PackingRelation outerPackingRelation, string insiderBarcode, out PackingRelation oldGrandRelation, out PackingRelation oldInnerRelation, out PackingRelation oldOuterPackingRelation)
        {
            PackingRelation _oldGrandRelation = null;
            PackingRelation _oldInnerRelation = null;         //旧外包装关系父包装关系
            PackingRelation _oldOuterPackingRelation = null;  //旧外包装关系
            packingRelationList.EachNode(f =>
            {
                var pkg = f as PackingRelation;

                if (pkg.Id == outerPackingRelation.Id)
                {
                    _oldGrandRelation = RF.GetById<PackingRelation>(pkg.TreePId);
                    _oldOuterPackingRelation = pkg;
                }
                else if (insiderBarcode.IsNotEmpty() && pkg.PackageNo == insiderBarcode)
                {
                    _oldInnerRelation = pkg;
                }

                return _oldOuterPackingRelation != null && _oldInnerRelation != null;
            });
            oldGrandRelation = _oldGrandRelation;
            oldInnerRelation = _oldInnerRelation;
            oldOuterPackingRelation = _oldOuterPackingRelation;
        }

        /// <summary>
        /// 更新包装关系界面
        /// </summary>
        /// <param name="uiPackingRelationList">包装关系列表</param>
        /// <param name="lastOuterPackingRelation">加入后的外部包装</param>
        /// <param name="oldGrandRelation">旧的祖包装关系</param>
        /// <param name="oldInnerRelation">旧的内部包装关系</param>
        /// <param name="oldOuterPackingRelation">旧的外部包装关系</param>
        private void Update_UI_PackingRelation(EntityList<PackingRelation> uiPackingRelationList, PackingRelation lastOuterPackingRelation, PackingRelation oldGrandRelation, PackingRelation oldInnerRelation, PackingRelation oldOuterPackingRelation)
        {
            TreeViewList treeViewList = new TreeViewList(uiPackingRelationList);
            var lastOuterPackingRelationNode = treeViewList.GetTreeViewNodeById(lastOuterPackingRelation.Id);
            if (oldOuterPackingRelation != null)
            {
                var oldOuterPackingRelationNode = treeViewList.GetTreeViewNodeById(oldOuterPackingRelation.Id);
                if (oldOuterPackingRelationNode != null)
                {
                    treeViewList.Remove(oldOuterPackingRelationNode);
                }
            }

            if (oldInnerRelation != null)
            {
                var oldInnerRelationNode = treeViewList.GetTreeViewNodeById(oldInnerRelation.Id);
                treeViewList.Remove(oldInnerRelationNode);
            }

            if (oldGrandRelation != null)
            {
                var oldGrandRelationNode = treeViewList.GetTreeViewNodeById(oldGrandRelation.Id);
                oldGrandRelationNode.Children.Add(lastOuterPackingRelationNode);
            }
            else
            {
                treeViewList.Add(lastOuterPackingRelationNode);
            }
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
        public virtual void ScanChildPackageSucceedEventHandler(PackingStrategyEvent e)
        {
            var packingRelation = _packingViewModel.PackingRelationList.FirstOrDefault(p => p.Id == e.OuterPackingRelation.Id);
            if (packingRelation != null)
            {
                packingRelation.ItemQty = e.OuterPackingRelation.ItemQty;
                packingRelation.PackedQty = e.OuterPackingRelation.PackedQty;
            }
            if (e.IsPackageQtyFull || e.IsPackageItemFull)
                SendFullEvent(e);
            _packingViewModel.ShowTips();
        }

        /// <summary>
        /// 扫描失败处理
        /// </summary>
        /// <param name="e">包装流程控制事件参数</param>
        public virtual void FailedEventHandler(PackingStrategyEvent e)
        {
            _packingViewModel.ShowError(e.Error);
            _packingViewModel.ResetStep();
        }
    }
}