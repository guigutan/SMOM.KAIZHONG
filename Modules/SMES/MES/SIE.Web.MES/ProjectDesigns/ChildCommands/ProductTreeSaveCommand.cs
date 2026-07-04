using SIE.Domain;
using SIE.MES.ProjectDesigns;
using SIE.MES.ProjectDesigns.ChildInfos;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ProjectDesigns.ChildCommands
{
    /// <summary>
    /// 工艺资料保存命令
    /// </summary>
    public class ProductTreeSaveCommand : SaveCommand
    {
        /// <summary>
        /// 保存前校验
        /// </summary>
        /// <param name="data"></param>
        protected override void OnSaving(EntityList data)
        {
            var list = data as EntityList<DesignProductTree>;
            RT.Service.Resolve<ProjectDesignController>().ValidateBeforeSaving(list);
            base.OnSaving(data);
        }
    }
}
