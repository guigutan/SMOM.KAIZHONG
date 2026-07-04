using SIE.AbnormalInfo.AbnormalMonitors;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.AbnormalInfo.AbnormalMonitors.Commands
{
    /// <summary>
    /// 添加异常任务
    /// </summary>
  public  class AddTaskCommand : ViewCommand<ViewArgs>
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">SIE.MetaModel.View.Block.EntityType</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var bill = args.Data.ToJsonObject<AbnormalMonitorTask>();
            if (null == bill)
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(bill)));
            bill.PersistenceStatus = Domain.PersistenceStatus.New;
            bill.Code = RT.Service.Resolve<AbnormalMonitorTaskService>().GenerateCode();
            bill.TaskType = SIE.AbnormalInfo.Common.TaskType.Manual;
            bill.TaskState = SIE.AbnormalInfo.Common.TaskStateEnum.ToDo;
            return bill;
        }
    }
}
