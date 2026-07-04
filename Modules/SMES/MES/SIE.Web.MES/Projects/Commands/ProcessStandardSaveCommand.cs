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
    /// 工序标准参数保存命令
    /// </summary>
    public class ProcessStandardSaveCommand : SaveCommand
    {
        /// <summary>
        /// 保存前
        /// </summary>
        /// <param name="data"></param>
        protected override void OnSaving(EntityList data)
        {
            var dataList = data as EntityList<ProcessStandardParam>;
            RT.Service.Resolve<ProjectParamController>().ProcessStandardBeforeSaving(dataList);
            base.OnSaving(data);
        }

        /// <summary>
        /// 保存逻辑
        /// </summary>
        /// <param name="data"></param>
        protected override void DoSave(EntityList data)
        {
            var dataList = data as EntityList<ProcessStandardParam>;
            RT.Service.Resolve<ProjectParamController>().ProcessStandardSave(dataList);
        }
    }
}
