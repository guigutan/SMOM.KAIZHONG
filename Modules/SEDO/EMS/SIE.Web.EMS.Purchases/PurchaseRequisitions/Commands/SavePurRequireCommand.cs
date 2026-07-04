using SIE.EMS.Purchases.PurchaseRequisitions;
using SIE.Web.Command;

namespace SIE.Web.EMS.Purchases.PurchaseRequisitions.Commands
{
    /// <summary>
    /// 保存采购申请
    /// </summary>
    [JsCommand("SIE.Web.EMS.Purchases.PurchaseRequisitions.Commands.SavePurRequireCommand")]
    public class SavePurRequireCommand : FormSaveCommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        protected override void OnSaving(Domain.Entity entity)
        {
            //不验证
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        protected override void DoSave(Domain.Entity entity)
        {
            if (entity != null)
            {
                var pur = entity as PurchaseRequisition;
                RT.Service.Resolve<PurchaseRequisitionController>().SavePurchaseRequisition(pur);
            }
        }
    }
}
