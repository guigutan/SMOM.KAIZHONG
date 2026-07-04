using SIE.Domain;
using SIE.EMS.Purchases.PurchaseRequisitions;
using SIE.Web.Command;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.Purchases.PurchaseRequisitions.Commands
{
    /// <summary>
    /// 删除采购申请
    /// </summary>
    [JsCommand("SIE.Web.EMS.Purchases.PurchaseRequisitions.Commands.DeletePurRequireCommand")]
    public class DeletePurRequireCommand : DeleteCommand
    {
        /// <summary>
        /// 保存前动作
        /// </summary>
        /// <param name="data"></param>
        protected override void OnSaving(EntityList data)
        {
            base.OnSaving(data);
            var ids = new List<double>();
            data.DeletedList.ForEach(p =>
            {
                var model = p as PurchaseRequisition;
                if (model != null)
                    ids.Add(model.Id);
            });
            RT.Service.Resolve<PurchaseRequisitionController>().DeletePurchaseRequisition(ids);
        }
    }
}
