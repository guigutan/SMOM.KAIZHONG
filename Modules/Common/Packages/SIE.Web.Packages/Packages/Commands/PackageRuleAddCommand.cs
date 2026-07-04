using SIE.Domain.Validation;
using SIE.Packages;
using SIE.Web.Command;
using System;

namespace SIE.Web.Packages.Packages.Commands
{
    /// <summary>
    /// 添加包装规则命令
    /// </summary>
    [JsCommand("SIE.Web.Packages.Packages.Commands.PackageRuleAddCommand")]
    public class PackageRuleAddCommand : ViewCommand
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">SIE.MetaModel.View.Block.EntityType</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var packageRule = args.Data.ToJsonObject<PackageRule>();
            if (null == packageRule)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(packageRule)));
            }
            var masterUnit = RT.Service.Resolve<PackageController>().GetMasterUnit();
            if (masterUnit == null)
                throw new ValidationException("主单位不存在，请在包装单位功能维护！".L10N());
            ////var detail = new PackageRuleDetail() { PackageUnit = masterUnit, Qty = 1, LevelQty = 1 };
            ////detail.GenerateId();
            ////主单位先生成Id，不然排序的Index会比非主单位的大，Bug场景是添加包装明细保存后，主单位会排在下面
            ////packageRule.PackageRuleDetailList.Add(detail);
            ////var list = new EntityList<PackageRuleDetail>();
            ////list.Add(detail);
            return masterUnit;
        }


    }
}