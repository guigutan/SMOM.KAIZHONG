using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.Tech.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.MES.Routings.RoutingBoms
{
    /// <summary>
    /// 工序bom明细 参数验证规则
    /// </summary>
    public class RoutingBomDetailRule : EntityRule<RoutingBomDetail>
    {
        /// <summary>
        /// 设置规则作用条件
        /// </summary>
        public RoutingBomDetailRule()
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
            var prb = entity as RoutingBomDetail;
            if (prb == null || prb.RoutingBomId <= 0)
                return;
            var parent = RF.GetById<RoutingBom>(prb.RoutingBomId);
            if (parent != null)
            {
                // 校验工艺路线工序和工序bom工序是否一致
                var routingProcesses = RT.Service.Resolve<RoutingBomController>()
                    .GetRoutingProcesses(parent.RoutingVersionId);

                if (routingProcesses.Any() && routingProcesses.FirstOrDefault(m => m.Id == prb.RoutingProcessId) == null)
                {
                    e.BrokenDescription = "当前工艺路线中的工序和工序BOM中的工序不一致，无法保存。".L10N();
                    return;
                }
            }
            if (prb.Material == null)
            {
                e.BrokenDescription = "请选择物料。".L10N();
                return;
            }

            #region 单位用量校验
            if (prb.Amount <= 0)
            {
                e.BrokenDescription = "单位用量必须大于0.".L10nFormat(prb.Amount);
                return;
            }

            // 校验物料单位用量总和不能超过产品bom的单位用量。            
            BomDetailViewModel bomDetail = RT.Service.Resolve<RoutingBomController>()
                    .GetRoutingBomDetailViewModel(prb.RoutingBom.ProductId, prb.MaterialId, prb.RoutingBom.ProcessSegmentId);
            if (bomDetail == null)
            {
                e.BrokenDescription = "物料[{0}]在产品BOM中不存在。".L10nFormat(prb.Material.Code);
                return;
            }
            // 取出当前物料在工序bom中单位用量总和
            decimal unitQty = bomDetail.UnitQty;
            decimal amountTotal = RT.Service
                .Resolve<RoutingBomController>()
                .GetRountingBomUnitQty(prb.RoutingBomId, prb.MaterialId, prb.Id); // 如果是修改则不包含当前数量

            if (prb.Id > 0)
            {
                amountTotal += prb.Amount; // 加上当前数量
            }
            if (amountTotal > unitQty + 0.000000001M)
            {
                e.BrokenDescription = "当前物料的单位用量总和[{0}]超过了产品bom的定义[{1}]。".L10nFormat(amountTotal, unitQty);
                return;
            }
            #endregion

        }
    }

    /// <summary>
    /// 子表保存
    /// </summary>
    public class RoutingBomDetailEntityRule : EntityRule<RoutingBomDetail>
    {
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="e"></param>

        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var routingBomDetail =(RoutingBomDetail)entity ;
            if (routingBomDetail.Material != null && routingBomDetail.Material.EnableExtendProperty && routingBomDetail.ItemExtProp.IsNullOrEmpty())
            {
                e.BrokenDescription = "物料[{0}]启用扩展属性，请选择物料扩展属性！".L10nFormat(routingBomDetail.Material.Name);
                return;
            }
        }
    }
}