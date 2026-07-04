using SIE.AbnormalInfo.AbnormalMonitors;
using SIE.Common.Schdules;
using SIE.Domain;
using SIE.Schedule;
using SIE.Threading;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Web.AbnormalInfo.AbnormalMonitors.AbnormalDefinitions.Commands
{
    /// <summary>
    /// 触发 触发当前任务，进入调度的队列中
    /// </summary>
    public class JobRunCommand : ListViewCommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="SIE.Domain.Validation.ValidationException"></exception>
        protected override object Excute(ViewArgs args, string scope)
        {

            var meta = ClientEntities.Find(args.Type);
            if (scope != meta.EntityType.GetQualifiedName())
                throw new System.Security.SecurityException("参数type[{0}]与令牌不一致".L10nFormat(args.Type));
            double? defineid = null;
            if (args.SelectedIds.ToList().IsNotEmpty())
            {
                defineid = args.SelectedIds.FirstOrDefault();
            }
            if (defineid == null)
                throw new SIE.Domain.Validation.ValidationException("请选择一个异常定义".L10N());
            return RT.Service.Resolve<AbnormalDefineService>().Run(defineid.Value);
        }
    }
}
