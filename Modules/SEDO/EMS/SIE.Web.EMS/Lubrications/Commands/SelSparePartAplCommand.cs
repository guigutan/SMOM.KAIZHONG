using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Lubrications;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.Lubrications.Commands
{
    /// <summary>
    /// 申请选择备件
    /// </summary>
    [JsCommand("SIE.Web.EMS.Lubrications.Commands.SelSparePartAplCommand")]
    public class SelSparePartAplCommand : ViewCommand
    {
        /// <summary>
        /// 执行选择
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var sparePartList = args.Data.ToJsonObject<List<LubricationSparePartApply>>();
            Check.NotNullOrEmpty(sparePartList, nameof(sparePartList));
            if (sparePartList == null || sparePartList.Count == 0)
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(sparePartList)));
            var stateIsChange = RT.Service.Resolve<LubricationController>().CheckParentState(sparePartList.Select(p => p.LubricationId).ToList());
            if (stateIsChange)
            {
                throw new ValidationException("润滑记录状态已提交，禁止继续备件更换及备件申请相关操作！".L10N());
            }
            EntityList<LubricationSparePartApply> checkPlanSpareParts = new EntityList<LubricationSparePartApply>();
            sparePartList.ForEach(p =>
            {
                p.PersistenceStatus = PersistenceStatus.New;
                checkPlanSpareParts.Add(p);
            });
            RF.Save(checkPlanSpareParts);
            return true;
        }
    }
}
