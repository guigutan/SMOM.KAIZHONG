using SIE.Equipments.Abnormal;
using SIE.Web.Command;
using System;

namespace SIE.Web.Equipments.Abnormal.Commands
{
    /// <summary>
    /// 异常停线添加命令
    /// </summary>
    public class AbnormalCauseAddCommand : ViewCommand
    {
        /// <summary>
        /// 初始化日期时间
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>实体</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var abnormalCause = args.Data.ToJsonObject<AbnormalCause>();
            abnormalCause.BeginDate = DateTime.Now;
            abnormalCause.Code = AppRuntime.Service.Resolve<AbnormalCauseController>().GetNewAbnormalCode();
            return abnormalCause;
        }
    }
}
