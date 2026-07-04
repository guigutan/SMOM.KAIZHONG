using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;


namespace SIE.EMS.FixedAssets.Accounts
{
    #region 资产台账实体验证规则
    /// <summary>
    /// 资产台账实体验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("资产台账实体验证规则")]
    [System.ComponentModel.Description("资产台账实体验证规则")]
    public class FixedAssetsAccountRule : EntityRule<FixedAssetsAccount>
    {
        /// <summary>
        /// 
        /// </summary>
        public FixedAssetsAccountRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 验证方法
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var equipAccount = entity as FixedAssetsAccount;

            if (equipAccount.DepreciationResidualValue > equipAccount.OriginalAssetsValue)
            {
                e.BrokenDescription = "资产残值需小于资产原值，且大于或等于0！".L10N();
            }
        }
    }
    #endregion
}
