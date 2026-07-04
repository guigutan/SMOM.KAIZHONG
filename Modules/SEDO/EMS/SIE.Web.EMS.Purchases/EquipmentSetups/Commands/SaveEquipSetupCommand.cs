using SIE.Domain;
using SIE.EMS.Purchases.EquipmentSetups;
using SIE.Web.Command;

namespace SIE.Web.EMS.Purchases.EquipmentSetups.Commands
{
    /// <summary>
    /// 保存安装调试
    /// </summary>
    [JsCommand("SIE.Web.EMS.Purchases.EquipmentSetups.Commands.SaveEquipSetupCommand")]
    public class SaveEquipSetupCommand : SaveCommand
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
            EntityList<EquipmentSetup> setupList = list as EntityList<EquipmentSetup>;
            RT.Service.Resolve<EquipmentSetupController>().SaveEquipSetupList(setupList);
            return list;
        }
    }
}
