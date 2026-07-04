using SIE.Domain;
using SIE.Domain.Validation;
using SIE.LES.RetreatItemManage;
using SIE.LES.RetreatItemManage.MaterialReturns;
using SIE.Web.Command;
using System;

namespace SIE.Web.LES.RetreatItemManage.MaterialReturns.Commands
{
    /// <summary>
    ///提交命令
    /// </summary>
    [JsCommand("SIE.Web.LES.RetreatItemManage.MaterialReturns.Commands.DetailSumitRrturnCommand")]
    public class DetailSumitRrturnCommand : FormSaveCommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        protected override void DoSave(Entity entity)
        {
            var entityCur = entity as MaterialReturn;
            if (entityCur.Id == 0)
            {
                entityCur.PersistenceStatus = PersistenceStatus.New;
            }
            var isMust = RT.Service.Resolve<MaterialReturnController>().GetReturnMaterialMustReturnReason();
            if (isMust && entityCur.ReturnReason.IsNullOrEmpty())
            {
                throw new ValidationException("退料原因必填！，请填写".L10N());
            }
            RT.Service.Resolve<MaterialReturnController>().DetailSubmit(entityCur);
           
        }
    }
}
