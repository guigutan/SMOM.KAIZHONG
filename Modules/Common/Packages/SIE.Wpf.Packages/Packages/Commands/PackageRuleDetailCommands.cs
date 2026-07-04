using SIE.Packages;
using SIE.Wpf.Command;

namespace SIE.Wpf.Packages.Packages.Commands
{
    /// <summary>
    /// 明细删除命令，不能删除主单位
    /// </summary>
    [Command(ImageName = "DeleteEntity", Label = "删除", ToolTip = "删除", GroupType = CommandGroupType.Edit)]
    public class PackageRuleDetailDeleteCommand : ListDeleteCommand
    {
        /// <summary>
        /// 是否可用
        /// </summary>
        /// <param name="view">listView</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var item = view.Current as PackageRuleDetail;
            return item != null && item.PackageUnit != null && !item.PackageUnit.IsMasterUnit;
        }
    }
}
