using SIE.Domain;
using SIE.MES.InspectionStandards;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.MES.InspectionStandards.Commands
{
    /// <summary>
    /// 检验项目保存命令
    /// </summary>
    public class InspectionSaveCommand : SaveCommand
    {
        /// <summary>
        /// 保存事件
        /// </summary>
        protected override void DoSave(EntityList data)
        {
            var inspectionList = data.CastTo<EntityList<ModelInspectionItem>>();
            RT.Service.Resolve<ModelInspectionItemController>().InspectionSave(inspectionList);
            base.DoSave(data);
        }
    }
}
