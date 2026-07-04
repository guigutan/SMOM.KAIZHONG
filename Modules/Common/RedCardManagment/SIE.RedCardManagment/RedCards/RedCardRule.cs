using NPOI.HSSF.Record;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.RedCardManagment.RedCards
{
    #region  规格不能为空
    [System.ComponentModel.DisplayName("规格不能为空")]
    [System.ComponentModel.Description("规格不能为空")]
    public class ProductDateRule : EntityRule<RedCard>
    {

        /// <summary>
        /// 规格的验证规则
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="e"></param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var redCard = entity as RedCard;
            if (redCard.ProductDateStart == null && redCard.ProductDateEnd == null) return;
            if (redCard.ProductDateStart != null && redCard.ProductDateEnd != null)
            {
                int result = DateTime.Compare((DateTime)redCard.ProductDateStart, (DateTime)redCard.ProductDateEnd);
                if (result > 0)
                {
                    e.BrokenDescription = "[生产周期结束时间]不能早于[生产周期开始时间]。".L10nFormat();
                }
            } 
            if (redCard.ProductDateStart == null && redCard.ProductDateEnd != null || redCard.ProductDateStart != null && redCard.ProductDateEnd == null)
            {
                e.BrokenDescription = "不能只填写一个生产周期时间。".L10nFormat();
            }

        }

    }
    
    #endregion
}
