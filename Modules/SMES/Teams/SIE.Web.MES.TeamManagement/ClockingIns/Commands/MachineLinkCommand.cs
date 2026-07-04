using SIE.Domain.Validation;
using SIE.MES.TeamManagement.ClockingIns;
using SIE.Web.Command;
using SIE.Web.Common;
using System;

namespace SIE.Web.MES.TeamManagement.ClockingIns
{
    /// <summary>
    /// 考勤机连接命令
    /// </summary>
    [JsCommand("SIE.Web.MES.TeamManagement.ClockingIns.Commands.MachineLinkCommand")]
    public class MachineLinkCommand : ViewCommand
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>考勤机信息</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var entity = args.Data.ToJsonObject<ClockInMachine>();
            if (string.IsNullOrEmpty(entity.IpAddress))
            {
                throw new ValidationException("请输入正确的IP地址和端口号".L10N());
            }
            if (!UserAgent.IsIP(entity.IpAddress))
            {
                throw new ValidationException("IP地址格式有误，请输入正确的IP地址".L10N());
            }
            var item = RT.Service.Resolve<ClockInController>().ReadMachineInfo(entity.IpAddress, entity.Port);
            return item;// new ClockInMachine();
        }
    }
}