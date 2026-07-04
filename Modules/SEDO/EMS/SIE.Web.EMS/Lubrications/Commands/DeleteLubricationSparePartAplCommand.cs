using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Lubrications;
using SIE.Web.Command;
using System;
using System.Linq;

namespace SIE.Web.EMS.Lubrications.Commands
{
    /// <summary>
    /// 删除点检申请 命令
    /// </summary>
    [JsCommand("SIE.Web.EMS.Lubrications.Commands.DeleteLubricationSparePartAplCommand")]
    public class DeleteLubricationSparePartAplCommand : DeleteCommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        protected override void OnSaving(EntityList data)
        {
            var sparePartList = data as EntityList<LubricationSparePart>;
            var stateIsChange = RT.Service.Resolve<LubricationController>().CheckParentState(sparePartList.Select(p => p.LubricationId).ToList());
            if (stateIsChange)
            {
                throw new ValidationException("润滑记录状态已提交，禁止继续备件更换及备件申请相关操作！".L10N());
            }
            base.OnSaving(data);
        }
    }
   
}
