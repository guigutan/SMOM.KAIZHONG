using SIE.Domain;
using SIE.EMS.SpareParts.Applys;
using SIE.EMS.SpareParts.Applys.Controllers;
using SIE.Web.Command;

namespace SIE.Web.EMS.SpareParts.Applys.Commands
{
    /// <summary>
    /// 取消审核
    /// </summary>
    [JsCommand("SIE.Web.EMS.SpareParts.Applys.Commands.CancelAuditAppCommand")]
    public class CancelAuditAppCommand : FormSaveCommand
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity"></param>
        protected override void DoSave(Entity entity)
        {
            //拿到实体
            var bill = entity as SparePartApp;
            //保存
            var newBill = RT.Service.Resolve<SparePartAppController>().CancelAuditSparePartApp(bill);
            bill.AuditState = newBill.AuditState;
        }
    }
}
