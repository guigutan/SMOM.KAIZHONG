using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIE.MES.DataBarcode;

namespace SIE.MES.DataBarcode
{
    public class DataBarcodeController : DomainController
    {
        /// <summary>
        /// 查询数据条码化数据
        /// </summary>
        /// <param name="itemIds"></param>
        /// <returns></returns>
        public virtual List<SIE.MES.DataBarcode.DataBarcode> GetDataBarcodePrintDatas(List<double> DataBarcodeIds)
        {
            List<SIE.MES.DataBarcode.DataBarcode> dataBarcodeDatas = new List<SIE.MES.DataBarcode.DataBarcode>();
            DataBarcodeIds.SplitDataExecute(DataBarcodeIds =>
            {
                var list = Query<SIE.MES.DataBarcode.DataBarcode>()
                    .Where(x => DataBarcodeIds.Contains(x.Id))
                    .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                dataBarcodeDatas.AddRange(list);
            });
            return dataBarcodeDatas;
        }
    }
}
