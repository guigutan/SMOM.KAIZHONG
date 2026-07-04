using SIE.Domain;
using SIE.Items;
using SIE.Items.Events;
using SIE.Web.Command;
using System.Linq;

namespace SIE.Web.Items.Items.Commands
{
    /// <summary>
    /// 物料 禁用命令
    /// </summary>
    [JsCommand("SIE.Web.Items.Items.Commands.ItemDisableCommand")]
    public class ItemDisableCommand : ViewCommand<double[]>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>结果</returns>
        protected override object Excute(double[] args, string scope)
        {
            var itemList = RT.Service.Resolve<ItemController>().GetItemList(args.ToList());
            foreach (var item in itemList)
            {
                RT.EventBus.Publish(new HasItemStockEvent() { IetmId = item.Id });
                item.State = State.Disable;
            }
            RF.Save(itemList);
            return "操作成功";
        }
    }
}
