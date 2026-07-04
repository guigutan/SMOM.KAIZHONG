using SIE.Domain;
using SIE.Items;
using SIE.Web.Command;
using System.Linq;

namespace SIE.Web.Items.Items.Commands
{
    /// <summary>
    /// 物料 启用命令
    /// </summary>
    [JsCommand("SIE.Web.Items.Items.Commands.ItemEnableCommand")]
    public class ItemEnableCommand : ViewCommand<double[]>
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
                item.State = State.Enable;
            }
            RF.Save(itemList);
            return "操作成功";
        }
    }
}
