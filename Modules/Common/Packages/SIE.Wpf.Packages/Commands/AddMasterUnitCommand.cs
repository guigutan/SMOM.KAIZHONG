using SIE.Domain.Validation;
using SIE.Packages;
using SIE.Wpf.Command;
using System;

namespace SIE.Wpf.Packages.Commands
{
    /// <summary>
    /// 包装规则界面默认
    /// </summary>
    [Command(Label = "新增主单位", ImageName = "AddUnit", GroupType = CommandGroupType.Edit)]
    public class AddMasterUnitCommand : ListAddCommand
    {
        /// <summary>
        /// 按钮执行事件
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            bool isExist = RT.Service.Resolve<PackageController>().IsExistsMasterUnit();
            if (isExist) throw new ValidationException("已存在主单位".L10N());
            var unit = RT.Service.Resolve<PackageController>().AddMasterUnit();
            view.Data.Add(unit);
            view.RefreshControl();
        }
    }
}
