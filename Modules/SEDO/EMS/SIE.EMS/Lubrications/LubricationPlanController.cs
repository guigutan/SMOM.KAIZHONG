using SIE.Domain;
using SIE.EMS.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.Lubrications
{
    /// <summary>
    /// 润滑计划控制器
    /// </summary>
    public class LubricationPlanController : DomainController
    {
        /// <summary>
        /// 根据点检单ID和状态获取备件更换项目列表
        /// </summary>
        /// <param name="lubricationId">润滑计划单ID</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        public virtual EntityList<LubricationSparePart> GetCheckPlanSpareParts(double lubricationId, ChangeSparePartState state)
        {
            var query = Query<LubricationSparePart>();
            query.Where(p => p.LubricationId == lubricationId);
            query.Where(p => p.State == state);
            var elo = new EagerLoadOptions();
            elo.LoadWith(LubricationSparePart.SparePartProperty);
            elo.LoadWith(LubricationSparePart.LubricationProperty);

            return query.ToList(null, elo);
        }

        /// <summary>
        /// 根据点检单ID和申请标记获取备件申请项目列表
        /// </summary>
        /// <param name="lubricationId">点检计划单ID</param>
        /// <param name="isApply">是否申请</param>
        /// <returns></returns>
        public virtual EntityList<LubricationSparePartApply> GetLubricationSparePartApls(double lubricationId, bool isApply)
        {
            var query = Query<LubricationSparePartApply>();
            query.Where(p => p.LubricationId == lubricationId);
            query.Where(p => p.IsApply == isApply);

            var elo = new EagerLoadOptions();
            elo.LoadWith(LubricationSparePartApply.SparePartProperty);
            return query.ToList(null, elo);
        }

    }
}
