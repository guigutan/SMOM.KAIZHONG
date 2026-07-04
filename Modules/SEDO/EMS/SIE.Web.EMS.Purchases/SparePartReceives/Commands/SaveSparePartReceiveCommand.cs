using SIE.EMS.Purchases.SparePartReceives;
using SIE.Web.Command;

namespace SIE.Web.EMS.Purchases.SparePartReceives.Commands
{
    /// <summary>
    /// 保存备件接收
    /// </summary>
    [JsCommand("SIE.Web.EMS.Purchases.SparePartReceives.Commands.SaveSparePartReceiveCommand")]
    public class SaveSparePartReceiveCommand : FormSaveCommand
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
                var eq = entity as SparePartReceive;
                RT.Service.Resolve<SparePartReceiveController>().SaveSparePartReceive(eq);
            }
            return entity;
        }
    }
}
