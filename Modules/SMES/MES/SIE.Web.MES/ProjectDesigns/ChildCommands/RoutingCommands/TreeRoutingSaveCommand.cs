using SIE.Domain;
using SIE.MES.ProjectDesigns;
using SIE.MES.ProjectDesigns.ChildInfos;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ProjectDesigns.ChildCommands.RoutingCommands
{
    /// <summary>
    /// 工艺资料-产品工艺路线保存命令
    /// </summary>
    public class TreeRoutingSaveCommand : SaveCommand
    {
        /// <summary>
        /// 保存前
        /// </summary>
        /// <param name="data"></param>
        protected override void OnSaving(EntityList data)
        {
            var list = data as EntityList<DesignTreeRouting>;
            RT.Service.Resolve<ProjectDesignController>().ValidateBeforeSaving(list);
            base.OnSaving(data);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="data"></param>
        protected override void DoSave(EntityList data)
        {
            var list = data as EntityList<DesignTreeRouting>;
            RT.Service.Resolve<ProjectDesignController>().SaveRouting(list);
        }
    }
}
