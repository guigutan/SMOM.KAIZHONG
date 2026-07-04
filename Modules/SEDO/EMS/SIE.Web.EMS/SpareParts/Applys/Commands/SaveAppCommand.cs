using SIE.Domain;
using SIE.EMS.SpareParts.Applys;
using SIE.EMS.SpareParts.Applys.Controllers;
using SIE.Web.Command;

namespace SIE.Web.EMS.SpareParts.Applys.Commands
{
    /// <summary>
    /// 保存
    /// </summary>
    [JsCommand("SIE.Web.EMS.SpareParts.Applys.Commands.SaveAppCommand")]
    public class SaveAppCommand : FormSaveCommand
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity"></param>
        protected override void DoSave(Entity entity)
        {
            var spa = entity as SparePartApp;
           
            //保存
            RT.Service.Resolve<SparePartAppController>().SparePartAppSave(spa);
        }
    }
}
