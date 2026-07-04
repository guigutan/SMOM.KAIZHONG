using SIE.AbnormalInfo.AbnormalMonitors;
using SIE.Web.AbnormalInfo.AbnormalMonitors.ViewModels;
using SIE.Web.Command;
using System;

namespace SIE.Web.AbnormalInfo.AbnormalMonitors.Commands
{
    /// <summary>
    /// 取消命令
    /// </summary>
    public class CancelReasonCommand : ViewCommand
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">SIE.MetaModel.View.Block.EntityType</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var data = args.Data.ToJsonObject<CancelReasonViewModel>();
            if (null == data)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(data)));
            }

            string reason = data.Reason;
            if (reason.IsNullOrWhiteSpace())
                throw new SIE.Domain.Validation.ValidationException("取消失败：[取消原因]不可为空或超出字符".L10N());
            return RT.Service.Resolve<AbnormalMonitorTaskService>().Cancel(data.AbnormalMonitorTaskId, reason);
        }
    }
}
