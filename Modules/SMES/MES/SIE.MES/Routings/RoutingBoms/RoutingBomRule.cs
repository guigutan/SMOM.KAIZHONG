using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.Routings.RoutingBoms
{
    /// <summary>
    /// 工艺路线版本（工序bom主表） 参数验证规则
    /// </summary>
    public class RoutingBomRule : EntityRule<RoutingBom>
    {
        /// <summary>
        /// 设置规则作用条件
        /// </summary>
        public RoutingBomRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="entity">工序清单参数</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var RoutingBom = entity as RoutingBom;
            var prv = RT.Service.Resolve<RoutingBomController>()
                .GetRoutingBom(RoutingBom.ProductId,
                RoutingBom.RoutingId,
                RoutingBom.RoutingVersionId,
                RoutingBom.ProcessSegmentId,
                RoutingBom.ProjectMaintainId);
            if (prv != null && prv.Id != RoutingBom.Id)
            {
                e.BrokenDescription = "工序BOM主表已经存在相同数据，无法保存！".L10N();
            }           
        }
    }
}
