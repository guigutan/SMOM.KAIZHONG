using SIE.Domain;
using SIE.MES.ProjectDesigns;
using SIE.MES.ProjectDesigns.ChildInfos;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ProjectDesigns.ChildCommands.DocumentCommands
{
    /// <summary>
    /// 工艺资料-文档上传保存命令
    /// </summary>
    public class DocumentSaveCommand : SaveCommand
    {
        /// <summary>
        /// 保存前校验
        /// </summary>
        /// <param name="data"></param>
        protected override void OnSaving(EntityList data)
        {
            var list = data as EntityList<DesignTreeDocument>;
            RT.Service.Resolve<ProjectDesignController>().ValidateBeforeSaving(list);
            base.OnSaving(data);
        }

        /// <summary>
        /// 保存逻辑
        /// </summary>
        /// <param name="data"></param>
        protected override void DoSave(EntityList data)
        {
            var list = data as EntityList<DesignTreeDocument>;
            RT.Service.Resolve<ProjectDesignController>().DesignTreeSave(list);
        }
    }
}
