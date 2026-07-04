using SIE.Barcodes;
using SIE.Web.Command;
using System.Collections.Generic;

namespace SIE.Web.Barcodes.Barcodes.Commands
{
    /// <summary>
    /// 条码作废
    /// </summary>
    [JsCommand("SIE.Web.Barcodes.ScarpCommand")]
    public class ScarpCommand : ListViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">s</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            bool rst = false;
            var ctl = RT.Service.Resolve<BarcodeController>();
            var scarpDatas = args.Data.ToJsonObject<DataModel>();
            if (scarpDatas.BarCodeIds.Count > 0)
            {
                var barcodes = ctl.GetBarcodesByIds(scarpDatas.BarCodeIds);
                ctl.BarcodeScrap(barcodes, scarpDatas.Reason);
                rst = true;
            }
            return rst;
        }
    }

    /// <summary>
    /// 保存数据类
    /// </summary>
    public class DataModel
    {
        public List<double> BarCodeIds
        {
            get; set;
        }

        public string Reason
        {
            get; set;
        }
    }
}
