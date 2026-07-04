using SIE.Core.Barcodes;
using SIE.MES.BatchWIP;
using SIE.MES.BatchWIP.Products;
using SIE.Tech.Processs;
using SIE.Web.Command;

namespace SIE.Web.MES.BatchProductRoutings.Commands
{
    /// <summary>
    /// 暂停产品工艺路线命令
    /// </summary>
    [JsCommand("SIE.Web.MES.BatchProductRoutings.Commands.PauseCommand")]
    public class PauseCommand : ViewCommand
    {
        /// <summary>
        /// 删除工艺路线命令执行方法
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            const string batchNo = "";
            const string oldLayout = "";
            const string newLayout = "";
            var batchRelation = RT.Service.Resolve<BatchManageController>().GetBatchRelation(batchNo, BarcodeType.BatchBarocde);
            RT.Service.Resolve<BatchWipProductRoutingController>().PauseProductRouting(batchRelation.Id, oldLayout, newLayout);
            return true;
        }
    }
}