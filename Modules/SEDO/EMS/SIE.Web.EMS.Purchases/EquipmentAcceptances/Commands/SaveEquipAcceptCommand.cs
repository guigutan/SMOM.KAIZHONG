using SIE.EMS.Purchases.EquipmentAcceptances;
using SIE.Web.Command;

namespace SIE.Web.EMS.Purchases.EquipmentAcceptances.Commands
{
    /// <summary>
    /// 保存验收数据
    /// </summary>
    [JsCommand("SIE.Web.EMS.Purchases.EquipmentAcceptances.Commands.SaveEquipAcceptCommand")]
    public class SaveEquipAcceptCommand : FormSaveCommand
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
                var accept = entity as EquipmentAcceptance;
                RT.Service.Resolve<EquipmentAcceptanceController>().SaveEquipAccept(accept);
            }
            return entity;
        }
    }
}
