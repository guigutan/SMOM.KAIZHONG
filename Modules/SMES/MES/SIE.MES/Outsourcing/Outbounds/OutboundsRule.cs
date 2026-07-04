using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.MES.Outsourcing;
using SIE.MetaModel;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace SIE.MES.WorkOrders
{
    #region 验证规则
    /// <summary>
    /// 单体追溯时候必须是正整数
    /// </summary>
    [DisplayName("委外需求单验证规则")]
    [Description("委外需求单验证规则")]
    public class OutboundsRule : EntityRule<OutsourcingRequest>
    {
        /// <summary>
        /// 验证方法
        /// </summary>
        /// <param name="entity">IEntity</param>
        /// <param name="e">RuleArgs</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var outsourcingRequest = entity as OutsourcingRequest;
            if (outsourcingRequest != null)
            {
                CheckQty(outsourcingRequest);
            }

        }

        /// <summary>
        /// 检查数量
        /// </summary>
        /// <param name="entity"></param>
        /// <exception cref="ValidationException"></exception>
        private void CheckQty(OutsourcingRequest entity)
        {
            var wo = RF.GetById<WorkOrder>(entity.WorkOrderId);
            if (wo != null)
            {
                var item = RF.GetById<Item>(wo.ProductId);
                if (item != null)
                {
                    var retrospectType = RT.Service.Resolve<ItemController>().GetRetrospectType(item.Id);
                    if (retrospectType == Core.Items.RetrospectType.Single && !IsIntergerNonZero(entity.RequestQty.ToString()))//单体追溯只能输入正整数
                    {
                        throw new ValidationException("工单产品为单体追溯只能输入正整数！".L10N());
                    }

                }
            }
        }
        /// <summary>
        /// 是否是正整数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsIntergerNonZero(string str)
        {
            return Regex.IsMatch(str, @"^[1-9]\d*$");
        }

    }
    #endregion
}