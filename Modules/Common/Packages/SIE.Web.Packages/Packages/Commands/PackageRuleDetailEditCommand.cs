using SIE.Domain.Validation;
using SIE.Packages;
using SIE.Web.Command;
using System;

namespace SIE.Web.Packages.Packages.Commands
{
    /// <summary>
    /// 获得单位名称
    /// </summary>
    [JsCommand("SIE.Web.Packages.Packages.Commands.PackageRuleDetailEditCommand")]
    public class PackageRuleDetailEditCommand : ViewCommand
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">SIE.MetaModel.View.Block.EntityType</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var unitId = args.Data;
            if (null == unitId)
            {
                throw new ValidationException("数据参数不能为空".L10N());
            }
            var unit = RT.Service.Resolve<PackageController>().GetUnit(double.Parse(unitId));
            if (unit == null)
                throw new ValidationException("单位不存在，请在包装单位功能维护！".L10N());

            return unit.Name;
        }


    }
}