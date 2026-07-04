using SIE.Domain;
using SIE.MES.Projects;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.Projects.Commands
{
    /// <summary>
    /// 项目参数保存命令
    /// </summary>
    public class ProjectParamSaveCommand : SaveCommand
    {
        /// <summary>
        /// 保存前校验
        /// </summary>
        /// <param name="data"></param>
        protected override void OnSaving(EntityList data)
        {
            var dataList = data as EntityList<ProjectParam>;
            RT.Service.Resolve<ProjectParamController>().ParamBeforeSaving(dataList);
            base.OnSaving(data);
        }
    }
}
