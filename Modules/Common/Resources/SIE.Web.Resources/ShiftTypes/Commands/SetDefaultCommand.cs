using SIE.Resources.ShiftTypes;
using SIE.Web.Command;
using System;

namespace SIE.Web.Resources.ShiftTypes.Commands
{
    /// <summary>
    /// 默认命令
    /// </summary>
    [JsCommand("SIE.Web.Resources.ShiftTypes.Commands.SetDefaultCommand")]
    public class SetDefaultCommand : ViewCommand
    {
        /// <summary>
        /// 默认命令设置
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var shiftType = args.Data.ToJsonObject<ShiftType>();
            if (null == shiftType)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(shiftType)));
            }
            RT.Service.Resolve<ShiftTypeController>().SetDefaultShiftType(shiftType);
            return true;
        }
    }
}
