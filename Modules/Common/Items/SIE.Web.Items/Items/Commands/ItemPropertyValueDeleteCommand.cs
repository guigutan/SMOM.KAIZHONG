using SIE.Domain;
using SIE.Items;
using SIE.Web.Command;
using System.Linq;

namespace SIE.Web.Items.Items.Commands
{
    /// <summary>
    /// 删除
    /// </summary>
    [JsCommand("SIE.Web.Items.Items.Commands.ItemPropertyValueDeleteCommand")]
    public class ItemPropertyValueDeleteCommand : ViewCommand<double[]>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>结果</returns>
        protected override object Excute(double[] args, string scope)
        {
            var vals = RT.Service.Resolve<ItemController>().GetItemPropertys(args.ToList());
            var item = vals.FirstOrDefault().Item;
            vals.ForEach(p => p.PersistenceStatus = Domain.PersistenceStatus.Deleted);
            RF.Save(vals);
            var leftVals = RT.Service.Resolve<ItemController>().GetItemPropertys(item.Id);
            if (leftVals.Count == 0)
            {
                item.EnableExtendProperty = false;
                RF.Save(item);
            }
            return "删除成功";
        }
    }
}
