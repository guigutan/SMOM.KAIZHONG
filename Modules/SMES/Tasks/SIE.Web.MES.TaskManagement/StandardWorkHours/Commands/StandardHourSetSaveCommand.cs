using SIE.Domain;
using SIE.MES.TaskManagement.StandardWorkHours;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.StandardWorkHours.Commands
{
    /// <summary>
    /// 产品标准工时维护-保存命令
    /// </summary>
    public class StandardHourSetSaveCommand : SaveCommand
    {
        /// <summary>
        /// 保存前校验
        /// </summary>
        /// <param name="data">保存数据</param>
        protected override void OnSaving(EntityList data)
        {
            var list = data as EntityList<StandardHourSet>;
            RT.Service.Resolve<StandardHourSetController>().ValidateBeforeSave(list);
            base.OnSaving(data);
        }
    }
}
