using Newtonsoft.Json;
using SIE.AbnormalInfo.AbnormalMonitors;
using SIE.AbnormalInfo.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.AbnormalInfo.AbnormalMonitors.Commands
{
    class GeneralTask : SaveCommand
    {
		protected override object Excute(ViewArgs args, string scope)
		{
			CheckTypeSecurity(args.Type, scope);
			var inventorys = JsonConvert.DeserializeObject<List<AbnormalMonitorInventory>>(args.Data);
			var notTasks = RT.Service.Resolve<AbnormalMonitorTaskService>().InventoryGenerateTask(inventorys);
			if (notTasks.Count > 0)
			{
				throw new ValidationException($"异常清单:[{string.Join(",", notTasks.Select(c => c.Code))}]无法生成新任务,存在相同的异常任务未完成]".L10N());
			}
			return true;
		}
	}
}
 