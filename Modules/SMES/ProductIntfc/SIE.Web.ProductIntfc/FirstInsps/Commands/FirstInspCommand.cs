using SIE.Domain;
using SIE.ProductIntfc.FirstInsps;
using SIE.ProductIntfc.InspLogs;
using SIE.Threading;
using SIE.Web.Command;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SIE.Web.ProductIntfc.FirstInsps.Commands
{
    /// <summary>
    /// 首件报检命令
    /// </summary>
    [JsCommand("SIE.Web.ProductIntfc.FirstInsps.Commands.FirstInspCommand")]
    public class FirstInspCommand : SaveCommand
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
            if (entityList.Count == 0)
            {
                return "请先选中报检单据".L10N();
            }
            var firstInsp = entityList[0] as FirstInsp;
            var barcodeLogIds = firstInsp.InspBarcodeLogList.Select(p => p.Id).Distinct().ToList();
            string errMsg = string.Empty;
            errMsg = RT.Service.Resolve<InspLogController>().GenerateFirstInsp(barcodeLogIds);
            return errMsg;
        }
    }
}
