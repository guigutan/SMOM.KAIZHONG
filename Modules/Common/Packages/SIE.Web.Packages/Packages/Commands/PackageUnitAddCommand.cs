using SIE.Domain.Validation;
using SIE.Packages;
using SIE.Web.Command;
using System;

namespace SIE.Web.Packages.Packages.Commands
{
    /// <summary>
    /// 包装单位主单位添加命令
    /// </summary>
    [JsCommand("SIE.Web.Packages.Packages.Commands.PackageUnitAddCommand")]
    public class PackageUnitAddCommand : ViewCommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="args">js传入数据</param>
        /// <param name="scope">作用域</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            //判断是否已存在主单位
            bool isExist = RT.Service.Resolve<PackageController>().IsExistsMasterUnit();
            if (isExist)
                throw new ValidationException("已存在主单位".L10N());
            //否则自动添加主单位
            var unit = RT.Service.Resolve<PackageController>().AddMasterUnit();
            return unit;
        }
    }
}
