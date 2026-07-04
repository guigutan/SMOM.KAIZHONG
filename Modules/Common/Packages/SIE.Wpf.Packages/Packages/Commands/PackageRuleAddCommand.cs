using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Packages;
using SIE.Wpf.Command;
using System;

namespace SIE.Wpf.Packages.Packages.Commands
{
    /// <summary>
    /// 添加包装规则命令
    /// </summary>
    [Command(ImageName = "AddEntity", Label = "添加", ToolTip = "添加包装规则", Gestures = "Ctrl+Shift+N", GroupType = 10)]
    public class PackageRuleAddCommand : ListAddCommand
    {
        /// <summary>
        /// 创建包装规则
        /// </summary>
        /// <param name="entity">包装规则</param>
        protected override void OnItemCreated(Entity entity)
        {
            base.OnItemCreated(entity);
            var rule = entity as PackageRule;
            var masterUnit = RT.Service.Resolve<PackageController>().GetMasterUnit();
            if (masterUnit == null)
                throw new ValidationException("主单位不存在，请在包装单位功能维护！".L10N());
            var detail = new PackageRuleDetail() { PackageUnit = masterUnit, Qty = 1, LevelQty = 1 };
            detail.GenerateId();
            //主单位先生成Id，不然排序的Index会比非主单位的大，Bug场景是添加包装明细保存后，主单位会排在下面
            rule.PackageRuleDetailList.Add(detail);
        }
    }
}