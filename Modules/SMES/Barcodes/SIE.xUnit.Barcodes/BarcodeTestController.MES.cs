using SIE.Barcodes;
using SIE.Domain;
using System;
using System.Collections.Generic;

namespace SIE.xUnit.Barcodes
{
    /// <summary>
    /// 测试条码数据控制器
    /// </summary>
    public partial class BarcodeTestController : DomainController
    {
        /// <summary>
        /// 根据sn列表创建条码列表(无工单)
        /// </summary>
        /// <param name="sn">条码号列表</param>
        /// <returns>条码列表</returns>
        public virtual EntityList<Barcode> CreateBarcodeBySn(List<string> sns)
        {
            var barcodes = new EntityList<Barcode>();
            foreach (var sn in sns)
            {
                var barcode = new Barcode();
                barcode.Sn = sn;
                barcode.Qty = 1;
                barcode.IsScraped = false;
                barcode.BoxesQty = 1;
                barcode.IsMantissa = false;
                barcode.IsPending = false;
                barcode.PrintDate = DateTime.Now;
                barcode.PrintTimes = 1;
                barcode.PrintedState = BarcodeState.Printed;
                barcode.PrintById = RT.IdentityId;
                barcodes.Add(barcode);
            }
            RF.Save(barcodes);
            return barcodes;
        } 
    }
}
