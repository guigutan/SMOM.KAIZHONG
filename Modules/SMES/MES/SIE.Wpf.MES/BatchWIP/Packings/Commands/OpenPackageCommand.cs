using SIE.MES.WIP.Packings;
using SIE.Packages;
using SIE.Packages.Packings.Enums;
using SIE.Wpf.Command;
using System;

namespace SIE.Wpf.MES.BatchWIP.Packings.Commands
{
    /// <summary>
    /// 加入选择包装命令
    /// </summary>
    [Command(Label = "选择加入包装", GroupType = CommandGroupType.Edit)]
    public class OpenPackageCommand : ListEditCommand
    {
        /// <summary>
        /// 执行条件
        /// </summary>
        /// <param name="view">逻辑视图</param>
        /// <returns>条件结果</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var mainView = view.Relations.Find("mainView");
            var model = (mainView?.Current as BatchPackingViewModel);
            return base.CanExecute(view) && model != null && model.ScanMode == ScanMode.Join;
        }

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="view">逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var mainView = view.Relations.Find("mainView");
            var model = (mainView?.Current as BatchPackingViewModel);

            if (model == null)
                return;

            BatchPackingRelation rela = view.Current as BatchPackingRelation;
            model.OuterBatchPackingRelation = rela;

            if (rela.TreePId != null)
            {
                model.ShowTips("包装[{0}]识别失败,包装被打包，切换包装后进行加入扫码。".L10nFormat(rela.PackageUnit.Name));
                model.OuterBatchPackingRelation = null;
                return;
            }
            else if (rela.PackageUnit.IsMasterUnit && model.CurrentPackageRuleLevel != null && model.CurrentPackageRuleLevel.Qty <= rela.ItemQty)
            {
                model.ShowTips("包装[{0}]识别失败,包装已满，切换包装后进行加入扫码。".L10nFormat(rela.PackageUnit.Name));
                model.OuterBatchPackingRelation = null;
                return;
            }
            else if (!rela.PackageUnit.IsMasterUnit && model.CurrentPackageRuleLevel != null && model.CurrentPackageRuleLevel.LevelQty <= rela.PackedQty)
            {
                model.ShowTips("包装[{0}]识别失败,包装已满，切换包装后进行加入扫码。".L10nFormat(rela.PackageUnit.Name));
                model.OuterBatchPackingRelation = null;
                return;
            }

            model.JoinAutoPacking(rela, model.GetWorkcell());
        }
    }
}
