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
    /// 项目号需求设计-基础属性保存命令
    /// </summary>
    public class BasicPropertySaveCommand : SaveCommand
    {
        /// <summary>
        /// 保存前校验
        /// </summary>
        /// <param name="data"></param>
        protected override void OnSaving(EntityList data)
        {
            var list = data as EntityList<DesignBasicProperty>;
            RT.Service.Resolve<ProjectDesignController>().ValidateBeforeSaving(list);
            base.OnSaving(data);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="data">保存命令</param>
        protected override void DoSave(EntityList data)
        {
            var list = data as EntityList<DesignBasicProperty>;
            RT.Service.Resolve<ProjectDesignController>().DesignBasicPropertySave(list);
        }
    }
}
