using SIE.Domain;
using SIE.MES.ProjectDesigns;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ProjectDesigns.Commands
{
    /// <summary>
    /// 项目号需求设计保存命令
    /// </summary>
    public class ProjectDesignSaveCommand : SaveCommand
    {
        /// <summary>
        /// 保存前校验
        /// </summary>
        /// <param name="data"></param>
        protected override void OnSaving(EntityList data)
        {
            var list = data as EntityList<ProjectDesign>;
            RT.Service.Resolve<ProjectDesignController>().ValidateBeforeSaving(list);
            base.OnSaving(data);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="data"></param>
        protected override void DoSave(EntityList data)
        {
            var list = data as EntityList<ProjectDesign>;
            RT.Service.Resolve<ProjectDesignController>().DesignSave(list);
        }
    }
}
