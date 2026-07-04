using Microsoft.Scripting.Runtime;
using SIE.Domain.Validation;
using SIE.Domain;
using SIE.MetaModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.MES.Routings.RoutingBoms
{
    /// <summary>
    /// 工艺路线版本（工序bom主表） 删除验证规则
    /// </summary>
    public class RoutingBomDeleteRule : EntityRule<RoutingBom>
    {
        /// <summary>
        /// 设置规则作用条件
        /// </summary>
        public RoutingBomDeleteRule()
        {
            Scope = EntityStatusScopes.Delete;
        }

        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="entity">工序清单参数</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var RoutingBom = entity as RoutingBom;
            bool exists = RT.Service.Resolve<RoutingBomController>().RoutingBomDetailsExists(RoutingBom.Id);
            if (exists)
            {
                e.BrokenDescription = "工序BOM明细表中数据不为空，不允许删除！请先删除工序BOM明细数据。".L10N();
            }
        }
    }

}
