using SIE.Domain;
using SIE.EMS.SpareParts.Applys;
using SIE.EMS.SpareParts.Applys.Controllers;
using SIE.EMS.SpareParts.Applys.Enums;
using SIE.Web.Command;

namespace SIE.Web.EMS.SpareParts.Applys.Commands
{
    /// <summary>
    /// 撤销
    /// </summary>
    [JsCommand("SIE.Web.EMS.SpareParts.Applys.Commands.UndoAppCommand")]
    public class UndoAppCommand : FormSaveCommand
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity"></param>
        protected override void DoSave(Entity entity)
        {
            //得到实体
            var spa = entity as SparePartApp;
            
            //保存
            RT.Service.Resolve<SparePartAppController>().ChageAuditSatate(spa, AuditState.Create);
        }
    }
}
