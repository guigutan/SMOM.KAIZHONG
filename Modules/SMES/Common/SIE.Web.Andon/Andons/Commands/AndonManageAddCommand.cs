using SIE.Andon.Andons;
using SIE.Domain;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Andon.Andons.Commands
{
    /// <summary>
    /// 安灯管理触发命令
    /// </summary>
    public class AndonManageAddCommand : ViewCommand
    {
        /// <summary>
        /// 触发
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected override object Excute(ViewArgs args, string scope)
        {
            var andonManage = args.Data.ToJsonObject<AndonManage>();
            var dateTimeNow = RF.Find<AndonManage>().GetDbTime();
            andonManage.AndonManageCode = RT.Service.Resolve<AndonManageController>().GetAndonManageCode();
            andonManage.State = SIE.Andon.Andons.Enum.AndonManageState.Standby;
            andonManage.TriggerId = RT.IdentityId;
            andonManage.TriggerTime = dateTimeNow;
            andonManage.FaultTime = dateTimeNow;
            andonManage.WorkGroup = RT.Service.Resolve<AndonManageController>().GetLoaderWorkGroup();
            return andonManage;
        }
    }
}
