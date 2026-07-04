using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Fixtures;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.Web.Command;
using System;

namespace SIE.Web.Fixtures.Accounts.Commands
{
    /// <summary>
    /// 修改保存ID类工治具台账
    /// </summary>
    [JsCommand("SIE.Web.Fixtures.Accounts.Commands.EditSaveIDAccountCommand")]
    public class EditSaveIDAccountCommand : FormSaveCommand
    {
        /// <summary>
        /// 保存验证
        /// </summary>
        /// <param name="entity"></param>
        protected override void DoSave(Entity entity)
        {
            if (entity == null)
            {
                throw new ValidationException("没有数据可以保存。".L10N());
            }
            if (!(entity is FixtureIDAccount))
            {
                throw new ValidationException("该数据不是校验记录数据格式。".L10N());
            }
            var model = entity as FixtureIDAccount;
            RT.Service.Resolve<CoreFixtureController>().EditSaveFixtureIDAccount(model);
        }
    }
}
