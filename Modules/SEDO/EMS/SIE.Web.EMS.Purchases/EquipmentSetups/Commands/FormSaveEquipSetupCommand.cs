using SIE.EMS.Purchases.EquipmentSetups;
using SIE.Web.Command;

namespace SIE.Web.EMS.Purchases.EquipmentSetups.Commands
{
    /// <summary>
    /// 保存安装调试
    /// </summary>
    [JsCommand("SIE.Web.EMS.Purchases.EquipmentSetups.Commands.FormSaveEquipSetupCommand")]
    public class FormSaveEquipSetupCommand : FormSaveCommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var list = GetDeserializeData(args, scope);
            var entity = list.Count > 0 ? list[0] : null;
            if (entity != null)
            {
                var es = entity as EquipmentSetup;
                RT.Service.Resolve<EquipmentSetupController>().SaveEquipmentSetup(es);
            }
            return entity;
        }
    }
}
