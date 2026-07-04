using SIE.Packages;
using SIE.Wpf.Command;
using System.Linq;

namespace SIE.Wpf.Packages.Commands
{
    /// <summary>
    /// 包装规则界面默认
    /// </summary>
    [Command(ImageName = "FlagTriangle", Label = "设为缺省", GroupType = CommandGroupType.Edit)]
    public class PackageRuleDefaultCommand : ListViewCommand
    {
        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var rule = view.Current as ItemPackageRule;
            return rule != null && !rule.IsDefault;
        }

        /// <summary>
        /// 命令执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            ItemPackageRule rule = view.Current as ItemPackageRule;
            if (rule != null)
            {
                view.Data.OfType<ItemPackageRule>().ForEach(p =>
                {
                    if (p == rule) p.IsDefault = true;
                    else if (p.IsDefault) p.IsDefault = false;
                });
            }
        }
    }
}
