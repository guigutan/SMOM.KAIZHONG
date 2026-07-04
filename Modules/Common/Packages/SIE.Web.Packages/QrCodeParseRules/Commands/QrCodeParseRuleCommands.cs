using SIE.Domain;
using SIE.Packages.QrCodeParseRules;
using SIE.Web.Command;
using SIE.Web.Common.Commands;
using System;
using System.Linq;

namespace SIE.Web.Packages.QrCodeParseRules.Commands
{
    /// <summary>
    /// 修改命令
    /// </summary>
    public class EditQrCodeParseRuleCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            return true;
        }
    }

    /// <summary>
    /// 删除命令
    /// </summary>
    public class DeleteQrCodeParseRuleCommand : DeleteCommand
    {
    }

    /// <summary>
    /// 启用
    /// </summary>
    public class EnableQrCodeParseRuleCommand : EnableCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var ctl = RT.Service.Resolve<QrCodeParseRuleController>();
            if (null == args.SelectedIds || args.SelectedIds.Length == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(args.SelectedIds)));
            }

            ctl.EnableQrCodeParseRuleData(args.SelectedIds.ToList());

            return true;
        }
    }

    /// <summary>
    /// 保存命令
    /// </summary>
    public class SaveQrCodeParseRuleCommand : SaveCommand
    {
        /// <summary>
        /// 保存前事件
        /// </summary>
        /// <param name="data">实体列表</param>
        protected override void OnSaving(EntityList data)
        {
            var ctl = RT.Service.Resolve<QrCodeParseRuleController>();
            EntityList<QrCodeParseRule> qrCodeRuleList = data.CastTo<EntityList<QrCodeParseRule>>();
            qrCodeRuleList.ForEach(p =>
            {
                if (p.PersistenceStatus != PersistenceStatus.New)
                {
                    var delIdList = p.QrCodeParseRuleDetailList.DeletedList.Select(f => f.GetId()).ToList();
                    var commitDtlIdList = p.QrCodeParseRuleDetailList.Select(c => c.Id).ToList();
                    var oldDtlList = ctl.GetQrCodeParseRuleDetails(p.Id).Where(c => !commitDtlIdList.Contains(c.Id) && !delIdList.Contains(p.Id)).ToList();
                    p.QrCodeParseRuleDetailList.AddRange(oldDtlList);
                    p.QrCodeParseRuleDetailList.Where(x => !commitDtlIdList.Contains(x.Id)).ForEach(x => x.PersistenceStatus = PersistenceStatus.Unchanged);
                }
            });

            base.OnSaving(qrCodeRuleList);
        }
    }
}