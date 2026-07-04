using SIE.Domain;
using SIE.Domain.Validation;
using SIE.LES.RetreatItemManage;
using SIE.LES.RetreatItemManage.MaterialReturns;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Web.LES.RetreatItemManage.MaterialReturns.Commands
{

    /// <summary>
    ///
    /// </summary>
    [JsCommand("SIE.Web.LES.RetreatItemManage.MaterialReturns.Commands.EditSaveReturnCommand")]
    public class EditSaveReturnCommand : SaveCommand
    {
        /// <summary>
        /// 保存命令
        /// </summary>
        /// <param name="data"></param>
        protected override void OnSaving(EntityList data)
        {
            var lists = data as EntityList<MaterialReturn>;
            if (lists.Any())
            {
                lists.First().PersistenceStatus = PersistenceStatus.Modified;
                var isMust = RT.Service.Resolve<MaterialReturnController>().GetReturnMaterialMustReturnReason();
                if (isMust && lists.First().ReturnReason.IsNullOrEmpty())
                {
                    throw new ValidationException("退料原因必填！，请填写".L10N());
                }
                RT.Service.Resolve<MaterialReturnController>().ValideReturnMaterial(lists.First());
            }
           
            base.OnSaving(data);
        }
    }
}
