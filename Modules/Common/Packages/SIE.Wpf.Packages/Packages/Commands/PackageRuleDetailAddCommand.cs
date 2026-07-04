using SIE.Common.Sort;
using SIE.Domain;
using SIE.Packages;
using SIE.Wpf.Command;
using System.Linq;

namespace SIE.Wpf.Packages.Packages.Commands
{
    /// <summary>
    /// 包装规则明细添加命令
    /// </summary>
    [Command(ImageName = "AddEntity", Label = "添加", ToolTip = "添加", GroupType = CommandGroupType.Edit)]
    public class PackageRuleDetailAddCommand : ListAddCommand
    {
        /// <summary>
        /// 添加包装规则明细
        /// </summary>
        /// <param name="entity"></param>
        protected override void OnItemCreated(Entity entity)
        {
            base.OnItemCreated(entity);
            var pkgRuleDtl = entity as PackageRuleDetail;
            var pkgRule = View.Parent.Current as PackageRule;

            pkgRuleDtl.LevelQty = 1;
            var topLevel = pkgRule.PackageRuleDetailList.OrderByDescending(f => SortExtension.GetIndex(f)).FirstOrDefault();
            if (topLevel != null)
                pkgRuleDtl.Qty = pkgRuleDtl.LevelQty * topLevel.Qty;
        }
    }
}
