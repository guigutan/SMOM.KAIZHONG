using SIE.EMS.Purchases.EquipmentReceives;
using SIE.Web.Command;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.Purchases.EquipmentReceives.Commands
{
    /// <summary>
    /// 删除设备接收
    /// </summary>
    [JsCommand("SIE.Web.EMS.Purchases.EquipmentReceives.Commands.DeleteEquipmentReceiveCommand")]
    public class DeleteEquipmentReceiveCommand : DeleteCommand
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
            var ids = new List<double>();
            list.DeletedList.ForEach(p =>
            {
                var model = p as EquipmentReceive;
                if (model != null)
                {
                    ids.Add(model.Id);
                }
            });
            RT.Service.Resolve<EquipmentReceiveController>().DeleteEquipmentReceive(ids);
            return true;
        }
    }
}
