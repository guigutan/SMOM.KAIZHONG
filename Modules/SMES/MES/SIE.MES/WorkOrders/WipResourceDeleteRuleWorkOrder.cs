using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.Resources.WipResources;
using System;
using System.ComponentModel;

namespace SIE.MES.WorkOrders
{

    #region 企业模型与设备删除验证逻辑
    /// <summary>
    /// 生产资源删除验证规则
    /// </summary>
    [DisplayName("生产资源删除验证规则")]
    [Description("工单引用的生产资源不能删除")]
    public class WipResourceDeleteRuleWorkOrder : EntityRule<WipResource>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WipResourceDeleteRuleWorkOrder()
        {
            Scope = EntityStatusScopes.Delete;
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 根据生产资源是否被工单引用，判断是否能被删除
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">验证参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var wipResource = entity as WipResource;
            var flag = RT.Service.Resolve<WorkOrderController>().WorkOrderHasUsedWipResource(wipResource.Id);
            if (flag)
            {
                e.BrokenDescription = "生产资源 [{0}] 被工单引用, 不能删除!".L10nFormat(wipResource.Code);
            }
        }
    }
    #endregion
}