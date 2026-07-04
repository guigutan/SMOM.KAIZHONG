using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Packages;
using SIE.Wpf.Command;
using System.Linq;

namespace SIE.Wpf.Packages.Commands
{
    /// <summary>
    /// 物料包装规则选择包装规则按钮
    /// </summary>
    [Command(ImageName = "PlaylistCheck", Label = "选择", ToolTip = "选择", GroupType = 10, DisplayMode = CommandDisplayMode.LabelAndIcon)]
    class SelectPackageRuleCommand : LookupCommand
    {
        /// <summary>
        /// 选择确定后
        /// </summary>
        protected override void OnAccept()
        {
            var addDataList = SelectedView.Data.OfType<ItemPackageRule>().Where(f => f.PersistenceStatus == PersistenceStatus.New).Select(p => p.PackageRule).ToList();
            double itemId = 0;
            double.TryParse(View.Parent.Current.GetId().ToString(), out itemId);
            EntityList<ItemPackageRule> itemPackageRuleList = RT.Service.Resolve<PackageController>().CreateItemPackageRule(addDataList, itemId);
            View.Data.AddRange(itemPackageRuleList);
        }
    }
}