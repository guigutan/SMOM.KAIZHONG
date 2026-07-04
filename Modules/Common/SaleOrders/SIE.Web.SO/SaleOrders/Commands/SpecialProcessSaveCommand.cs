using SIE.Domain;
using SIE.SO.SaleOrders;
using SIE.Web.Command;

namespace SIE.Web.SO.SaleOrders.Commands
{
    /// <summary>
    /// 特殊工艺保存事件
    /// </summary>
    public class SpecialProcessSaveCommand : SaveCommand
    {
        /// <summary>
        /// 返回消息
        /// </summary>
        private string msg = "";
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            base.Excute(args, scope);
            return msg;
        }
        /// <summary>
        /// 保存后
        /// </summary>
        /// <param name="data"></param>
        protected override void DoSave(EntityList data)
        {
            EntityList<SpecialProcess> List = data as EntityList<SpecialProcess>;
            msg = RT.Service.Resolve<SpecialProcessController>().SaveSpecialProcess(List);
        }
    }
}
