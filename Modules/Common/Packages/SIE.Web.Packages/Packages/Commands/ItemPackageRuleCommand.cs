using SIE.Common.Sort;
using SIE.Domain;
using SIE.Packages;
using SIE.Web.Command;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.Packages.Packages.Commands
{
    /// <summary>
    /// 物料包装规则默认命令
    /// </summary>
    public class ItemPackageDefautlRuleCommand : ViewCommand<double[]>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(double[] args, string scope)
        {
            List<double> idList = args.ToList();
            double itemPkgRuleId = idList.FirstOrDefault();
            ItemPackageRule itemPackageRule = RF.GetById<ItemPackageRule>(itemPkgRuleId);
            if (itemPackageRule != null)
            {
                double itemId = itemPackageRule.ItemId;
                EntityList<ItemPackageRule> isDefaultItemPkgRules = RT.Service.Resolve<PackageController>().GetItemPackageRule(itemId, string.Empty, null);
                isDefaultItemPkgRules.ForEach(p =>
                {
                    if (p.Id == itemPackageRule.Id && !p.IsDefault)
                    {
                        p.IsDefault = true;
                    }
                    else
                    {
                        p.IsDefault = false;
                    }
                });
                RF.Save(isDefaultItemPkgRules);
            }

            return true;
        }
    }

    /// <summary>
    /// 删除命令
    /// </summary>
    public class DeleteItemPackageRuleCommand : ViewCommand<double[]>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(double[] args, string scope)
        {
            List<double> idList = args.ToList();
            var itemPackageRules = RT.Service.Resolve<PackageController>().GetItemPackageRules(idList);
            double itemId = itemPackageRules.Select(p => p.ItemId).FirstOrDefault();
            itemPackageRules.ForEach(p => p.PersistenceStatus = PersistenceStatus.Deleted);
            RF.Save(itemPackageRules);

            ItemPackageRule isDefaultItemPkgRule = RT.Service.Resolve<PackageController>().GetItemPackageRule(itemId, string.Empty, null).FirstOrDefault(p => !idList.Contains(p.Id));
            if (isDefaultItemPkgRule != null)
            {
                isDefaultItemPkgRule.IsDefault = true;
                RF.Save(isDefaultItemPkgRule);
            }

            return true;
        }
    }

    /// <summary>
    /// 物料包装规则明细添加命令
    /// </summary>
    public class AddItemPackageRuleDetailCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var data = args.Data.ToJsonObject<ItemPackageRuleDetail>();
            data.LevelQty = 1;
            var itemPackageRule = RF.GetById<ItemPackageRule>(data.ItemPackageRuleId, new EagerLoadOptions().LoadWithViewProperty());
            var details = itemPackageRule.ItemPackageRuleDetailList.OrderBy(f => SortExtension.GetIndex(f));
            foreach (var detail in details)
            {
                var topLevel = itemPackageRule.ItemPackageRuleDetailList.Where(p => SortExtension.GetIndex(p) < SortExtension.GetIndex(detail)).OrderByDescending(f => SortExtension.GetIndex(f)).FirstOrDefault();
                if (topLevel == null) continue;
                detail.Qty = detail.LevelQty * topLevel.Qty;
                data.Qty = detail.Qty;
            }
            return data;
        }
    }
}