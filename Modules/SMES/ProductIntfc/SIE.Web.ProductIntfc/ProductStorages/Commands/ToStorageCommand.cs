using SIE.Domain;
using SIE.ProductIntfc.ProductStorages;
using SIE.Web.Command;
using System.Linq;
using System.Collections.Generic;
using System;

namespace SIE.Web.ProductIntfc.ProductStorages.Commands
{
    /// <summary>
    /// 入库命令
    /// </summary>
    [JsCommand("SIE.Web.ProductIntfc.ProductStorages.Commands.ToStorageCommand")]
    public class ToStorageCommand : SaveCommand
    {
        /// <summary>
        /// 执行入库
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>入库结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            List<double> selectedIds = new List<double>(args.SelectedIds);
            var barcodes = RT.Service.Resolve<ProductStorageController>().GetStorageBarcode(selectedIds);
            double storageWorkOrderId = barcodes.Select(p => p.StorageWorkOrderId).Distinct().FirstOrDefault();
            RT.Service.Resolve<ProductStorageController>().ToStorageIn(storageWorkOrderId, barcodes);
            return "入库成功".L10N();
        }
    }
}
