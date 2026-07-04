using SIE.Domain;
using SIE.Packages;
using SIE.Web.Command;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.Packages.Packages.Commands
{
    /// <summary>
    /// 包装规则保存命令
    /// </summary>
    public class PackageRuleSaveCommand : SaveCommand
    {
        /// <summary>
        /// 保存前事件
        /// </summary>
        /// <param name="data">实体列表</param>
        protected override void OnSaving(EntityList data)
        {
            var ctl = RT.Service.Resolve<PackageController>();
            var list = data as EntityList<PackageRule>;
            list.ForEach(p =>
            {
                if (p.PersistenceStatus != PersistenceStatus.New)
                {
                    var delIdList = p.PackageRuleDetailList.DeletedList.Select(c => c.GetId()).ToList();
                    var commitDtlIdList = p.PackageRuleDetailList.Select(c => c.Id).ToList();
                    var oldDtlList = ctl.GetPackageRuleDetails(p.Id).Where(c => !commitDtlIdList.Contains(c.Id) && !delIdList.Contains(c.Id)).ToList();
                    p.PackageRuleDetailList.AddRange(oldDtlList);
                    p.PackageRuleDetailList.Where(c => !commitDtlIdList.Contains(c.Id)).ForEach(c => p.PersistenceStatus = PersistenceStatus.Unchanged);
                }
            });
            base.OnSaving(list);
        }
    }
}