using SIE.Common.Sort;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Packages;
using SIE.Web.Command;
using SIE.Web.Common.Sort.Commands;
using System;
using System.Linq;

namespace SIE.Web.Packages.Packages.Commands
{
    /// <summary>
    /// 添加
    /// </summary>
    public class AddItemPkgRuleDtlCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            return true;
        }
    }

    /// <summary>
    /// 修改
    /// </summary>
    public class EditItemPkgRuleDtlCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            return true;
        }
    }

    /// <summary>
    /// 删除命令
    /// </summary>
    public class DeleteItemPkgRuleDtlCommand : DeleteCommand
    {

    }

    /// <summary>
    /// 置底
    /// </summary>
    public class MoveBottomItemPkgRuleDtlCommand : MoveCommand
    {
        /// <summary>
        /// 保存前
        /// </summary>
        /// <param name="data">实体列表数据</param>
        protected override void OnSaving(EntityList data)
        {
            var itemPackageRuleDetailList = data.ConvertTo<EntityList<ItemPackageRuleDetail>>();
            var curItemPackageRuleDtlIdList = itemPackageRuleDetailList.Select(p => p.Id);
            var itemPackageRuleId = itemPackageRuleDetailList.Select(p => p.ItemPackageRuleId).FirstOrDefault();
            var oldItemPackageRuleDtlList = RT.Service.Resolve<PackageController>().GetItemPackageRuleDetail(itemPackageRuleId, string.Empty, null).Where(p => !curItemPackageRuleDtlIdList.Contains(p.Id)).ToList();

            itemPackageRuleDetailList.AddRange(oldItemPackageRuleDtlList);
            itemPackageRuleDetailList.Where(x => !curItemPackageRuleDtlIdList.Contains(x.Id)).ForEach(x => x.PersistenceStatus = PersistenceStatus.Unchanged);

            var firstDetail = itemPackageRuleDetailList.OrderBy(f => SortExtension.GetIndex(f)).FirstOrDefault();
            if (firstDetail != null && !firstDetail.IsMasterUnit)
            {
                throw new ValidationException("主单位必须是第一个包装单位!".L10nFormat());
            }
            base.OnSaving(data);
        }
    }

    /// <summary>
    /// 下移
    /// </summary>
    public class MoveDownItemPkgRuleDtlCommand : MoveCommand
    {
        /// <summary>
        /// 保存前
        /// </summary>
        /// <param name="data">实体列表数据</param>
        protected override void OnSaving(EntityList data)
        {
            var itemPackageRuleDetailList = data.ConvertTo<EntityList<ItemPackageRuleDetail>>();
            var curItemPackageRuleDtlIdList = itemPackageRuleDetailList.Select(p => p.Id);
            var itemPackageRuleId = itemPackageRuleDetailList.Select(p => p.ItemPackageRuleId).FirstOrDefault();
            var oldItemPackageRuleDtlList = RT.Service.Resolve<PackageController>().GetItemPackageRuleDetail(itemPackageRuleId, string.Empty, null).Where(p => !curItemPackageRuleDtlIdList.Contains(p.Id)).ToList();

            itemPackageRuleDetailList.AddRange(oldItemPackageRuleDtlList);
            itemPackageRuleDetailList.Where(x => !curItemPackageRuleDtlIdList.Contains(x.Id)).ForEach(x => x.PersistenceStatus = PersistenceStatus.Unchanged);

            var firstDetail = itemPackageRuleDetailList.OrderBy(f => SortExtension.GetIndex(f)).FirstOrDefault();
            if (firstDetail != null && !firstDetail.IsMasterUnit)
            {
                throw new ValidationException("主单位必须是第一个包装单位!".L10nFormat());
            }
            base.OnSaving(data);
        }
    }

    /// <summary>
    /// 置顶
    /// </summary>
    public class MoveTopItemPkgRuleDtlCommand : MoveCommand
    {
        /// <summary>
        /// 保存前
        /// </summary>
        /// <param name="data">实体列表数据</param>
        protected override void OnSaving(EntityList data)
        {
            var itemPackageRuleDetailList = data.ConvertTo<EntityList<ItemPackageRuleDetail>>();
            var curItemPackageRuleDtlIdList = itemPackageRuleDetailList.Select(p => p.Id);
            var itemPackageRuleId = itemPackageRuleDetailList.Select(p => p.ItemPackageRuleId).FirstOrDefault();
            var oldItemPackageRuleDtlList = RT.Service.Resolve<PackageController>().GetItemPackageRuleDetail(itemPackageRuleId, string.Empty, null).Where(p => !curItemPackageRuleDtlIdList.Contains(p.Id)).ToList();

            itemPackageRuleDetailList.AddRange(oldItemPackageRuleDtlList);
            itemPackageRuleDetailList.Where(x => !curItemPackageRuleDtlIdList.Contains(x.Id)).ForEach(x => x.PersistenceStatus = PersistenceStatus.Unchanged);

            var firstDetail = itemPackageRuleDetailList.OrderBy(f => SortExtension.GetIndex(f)).FirstOrDefault();
            if (firstDetail != null && !firstDetail.IsMasterUnit)
            {
                throw new ValidationException("主单位必须是第一个包装单位!".L10nFormat());
            }
            base.OnSaving(data);
        }
    }

    /// <summary>
    /// 上移
    /// </summary>
    public class MoveUpItemPkgRuleDtlCommand : MoveCommand
    {
        /// <summary>
        /// 保存前
        /// </summary>
        /// <param name="data">实体列表数据</param>
        protected override void OnSaving(EntityList data)
        {
            var itemPackageRuleDetailList = data.ConvertTo<EntityList<ItemPackageRuleDetail>>();
            var curItemPackageRuleDtlIdList = itemPackageRuleDetailList.Select(p => p.Id);
            var itemPackageRuleId = itemPackageRuleDetailList.Select(p => p.ItemPackageRuleId).FirstOrDefault();
            var oldItemPackageRuleDtlList = RT.Service.Resolve<PackageController>().GetItemPackageRuleDetail(itemPackageRuleId, string.Empty, null).Where(p => !curItemPackageRuleDtlIdList.Contains(p.Id)).ToList();

            itemPackageRuleDetailList.AddRange(oldItemPackageRuleDtlList);
            itemPackageRuleDetailList.Where(x => !curItemPackageRuleDtlIdList.Contains(x.Id)).ForEach(x => x.PersistenceStatus = PersistenceStatus.Unchanged);

            var firstDetail = itemPackageRuleDetailList.OrderBy(f => SortExtension.GetIndex(f)).FirstOrDefault();
            if (firstDetail != null && !firstDetail.IsMasterUnit)
            {
                throw new ValidationException("主单位必须是第一个包装单位!".L10nFormat());
            }
            base.OnSaving(data);
        }
    }

}
