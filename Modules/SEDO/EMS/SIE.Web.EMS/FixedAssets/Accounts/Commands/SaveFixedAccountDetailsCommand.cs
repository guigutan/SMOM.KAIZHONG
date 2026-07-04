using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.FixedAssets.Accounts;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.FixedAssets.Accounts.Commands
{
    /// <summary>
    /// 保存命令
    /// </summary>
    public class SaveFixedAccountDetailsCommand : FormSaveCommand
    {
        /// <summary>
        /// 保存操作
        /// </summary>
        /// <param name="entity">固定资产台账实体</param>
        protected override void DoSave(Entity entity)
        {
            var account = entity as FixedAssetsAccount;

            if (account == null)
            {
                throw new ValidationException("保存数据异常，请重新尝试！".L10N());
            }

            RT.Service.Resolve<FixedAssetsAccountController>().SaveFixedAssetsAccount(account);
        }
    }
}
