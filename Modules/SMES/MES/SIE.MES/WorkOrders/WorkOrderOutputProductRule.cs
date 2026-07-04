using Microsoft.Scripting.Runtime;
using SIE.Domain.Validation;
using SIE.Domain;
using SIE.MetaModel;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SIE.MES.WorkOrders
{
    internal class WorkOrderOutputProductRule
    {
        #region 验证规则
        /// <summary>
        /// 行号非空
        /// </summary>
        [DisplayName("联/副产品验证规则")]
        [Description("联/副产品验证规则")]
        public class WorkOrderOutputRowNumberRule : EntityRule<WorkOrderOutputProduct>
        {
            /// <summary>
            /// 验证方法
            /// </summary>
            /// <param name="entity">IEntity</param>
            /// <param name="e">RuleArgs</param>
            protected override void Validate(IEntity entity, RuleArgs e)
            {
                var workOrderOutputProduct = entity as WorkOrderOutputProduct;
                if (workOrderOutputProduct != null)
                {
                    if (workOrderOutputProduct.RowNumber.IsNullOrEmpty())
                    {
                        e.BrokenDescription = "联/副产品行号必输".L10N();
                    }
                    //if (workOrderOutputProduct.Qty <= 0)
                    //{
                    //    e.BrokenDescription = "联/副产品数量需大于0".L10N();
                    //}
                    if (RT.Service.Resolve<WorkOrderOutputProductController>().IsExsitedRowNumber(workOrderOutputProduct.RowNumber, workOrderOutputProduct.Id,
                        workOrderOutputProduct.WorkOrderId))
                    {
                        e.BrokenDescription = "联/副产品行号系统已存在".L10N();
                    }
                }
               
            }
        }
        #endregion
    }
}