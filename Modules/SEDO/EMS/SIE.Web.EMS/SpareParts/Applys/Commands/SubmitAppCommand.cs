using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.SpareParts.Applys;
using SIE.EMS.SpareParts.Applys.Controllers;
using SIE.EMS.SpareParts.Applys.Enums;
using SIE.Web.Command;
using System;
using System.Linq;

namespace SIE.Web.EMS.Repairs.Applys.Commands
{
    /// <summary>
    /// 提交备件申请
    /// </summary>
    [JsCommand("SIE.Web.EMS.SpareParts.Applys.Commands.SubmitAppCommand")]
    public class SubmitAppCommand : FormSaveCommand
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity"></param>
        protected override void DoSave(Entity entity)
        {
            var data = entity as SparePartApp;

            foreach (var item in data.ApplyDetailList)
            {
                if (data.ApplyDetailList.Count(p => p.SparePartId == item.SparePartId && p.WarehouseId == item.WarehouseId) > 1) 
                {
                    throw new ValidationException("请合并相同【备件编码】和【出库仓库】的申请明细".L10N());
                }
            }

            //保存
            RT.Service.Resolve<SparePartAppController>().ChageAuditSatate(data, AuditState.StandAudit);
        }
    }
}
