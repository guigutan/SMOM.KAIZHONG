using SIE.Domain.Validation;
using SIE.EMS.Purchases.EquipmentSetups;
using SIE.Web.Command;
using System;

namespace SIE.Web.EMS.Purchases.EquipmentSetups.Commands
{
    /// <summary>
    /// 保存命令
    /// </summary>
    [JsCommand("SIE.Web.EMS.Purchases.EquipmentSetups.Commands.SaveSetupAttachmentCommand")]
    public class SaveSetupAttachmentCommand : FormSaveCommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            if (args == null)
            {
                throw new ValidationException("数据异常".L10N());
            }
            var data = args.Data.ToJsonObject<SetupAttachment>();
            RT.Service.Resolve<EquipmentSetupController>().SaveSetupAttachment(data);
            return true;
        }
    }
}
