using SIE.Domain;
using SIE.ProductIntfc.InspRecords;
using SIE.Threading;
using SIE.Web.Command;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SIE.Web.ProductIntfc.InspRecords.Commands
{
    /// <summary>
    /// 成品报检命令
    /// </summary>
    [JsCommand("SIE.Web.ProductIntfc.InspRecords.Commands.ShippingInspCommand")]
    public class ShippingInspCommand : SaveCommand
    {
        /// <summary>
        /// 成品报检命令执行方法
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>报检结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var entityList = GetDeserializeData(args, scope);
            var barcodes = entityList as EntityList<InspBarcode>;
            var barcodeIds = barcodes.Select(p => p.Id).Distinct().ToList();
            string errMsg = string.Empty;

            //Task task = Task.Run(new Action(() =>
            //{
                errMsg = RT.Service.Resolve<InspRecordController>().ExecuteProductInsp(barcodeIds);
            //}).WithCurrentThreadContext());

            ////等待任务的完成执行过程
            //task.Wait();

            if (errMsg.Length == 0)
                return "报检成功".L10N();
            else
                return errMsg;
        }
    }
}
