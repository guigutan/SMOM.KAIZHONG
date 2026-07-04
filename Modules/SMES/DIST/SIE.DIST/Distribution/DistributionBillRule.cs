using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.Resources.WipResources;
using System;
using System.ComponentModel;

namespace SIE.DIST.Distribution
{
    /// <summary>
    /// 生产资源删除验证规则
    /// </summary>
    [DisplayName("生产资源删除验证规则")]
    [Description("配送单引用的生产资源不能删除")]
    public class WipResourceDeleteRuleDistributionBill : EntityRule<WipResource>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WipResourceDeleteRuleDistributionBill()
        {
            Scope = EntityStatusScopes.Delete;
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 根据生产资源是否被配送单引用，判断是否能被删除
        /// </summary>
        /// <param name="entity">生产资源</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var wipResource = entity as WipResource;
            var flag = RT.Service.Resolve<DistributionController>().DistributionBillHasUsedWipResource(wipResource.Id);
            if (flag)
            {
                e.BrokenDescription = "生产资源 [{0}] 被配送单引用, 不能删除!".L10nFormat(wipResource.Code);
            }
        }
    }
}