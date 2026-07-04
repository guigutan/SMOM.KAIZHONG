using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Purchases.FixtureAcceptances;
using SIE.Web.Command;
using System;

namespace SIE.Web.EMS.Purchases.FixtureAcceptances.Commands
{
    /// <summary>
    /// 保存命令
    /// </summary>
    public class SaveAcceptanceAttachmentCommand : FormSaveCommand
    {
        /// <summary>
        /// 保存前动作
        /// </summary>
        protected override void OnSaving(Entity entity)
        {
            base.OnSaving(entity);
        }
        /// <summary>
        /// 执行上传和复传
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var data = args.Data.ToJsonObject<FixtureAcceptanceAttachment>();
            data.PersistenceStatus = PersistenceStatus.New;

            RF.Save(data);

            return true;
        }

    }
}
