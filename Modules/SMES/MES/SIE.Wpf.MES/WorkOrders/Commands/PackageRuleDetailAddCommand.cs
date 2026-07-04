using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.WorkOrders;
using SIE.Packages;
using SIE.Wpf.Command;
using System;

namespace SIE.Wpf.MES.WorkOrders.Commands
{
    /// <summary>
    /// 工单包装规则添加命令
    /// </summary>
    [Command(ImageName = "AddEntity", Label = "添加", ToolTip = "添加数据", Gestures = "Ctrl+Shift+N", GroupType = 10)]
    public class PackageRuleDetailAddCommand : ListAddCommand
    {
        /// <summary>
        /// 添加工单包装规则
        /// </summary>
        /// <returns>工单包装规则</returns>
        protected override Entity CreateNewItem()
        {
            var entity = base.CreateNewItem() as WorkOrderPackageRuleDetail;
            if (View.Data.Count == 0)
            {
                var masterUnit = RT.Service.Resolve<PackageController>().GetMasterUnit();
                if (masterUnit == null)
                    throw new ValidationException("主单位不存在，请在包装单位功能维护！".L10N());
                entity.PackageUnit = masterUnit;
                entity.Qty = entity.LevelQty = 1;
            }
            return entity;
        }
    }
}