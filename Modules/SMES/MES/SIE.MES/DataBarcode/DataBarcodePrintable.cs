using SIE.Common.Prints;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.DataBarcode
{
    /// <summary>
    /// 数据条码化打印
    /// </summary>
    [Serializable]
    //[System.ComponentModel.DisplayName("数据条码化打印")]
    [DisplayName("数据条码化打印")]
    public class DataBarcodePrintable : LabelPrintable<DataBarcode>
    {
        public override IEnumerable<String> GetPropertys(Type type = null)
        {
            var propertys = new List<String>();
            propertys.Add("BarcodeType");
            propertys.Add("BarcodeSite");
            propertys.Add("BarcodeParam1");
            propertys.Add("BarcodeParam2");
            propertys.Add("BarcodeParam3");
            propertys.Add("BarcodeParam4");
            propertys.Add("BarcodeParam5");
            propertys.Add("BarcodeParam6");
            propertys.Add("BarcodeParam7");
            propertys.Add("BarcodeParam8");
            propertys.Add("BarcodeParam9");
            propertys.Add("BarcodeParam10");
            propertys.Add("BarcodeParam11");
            propertys.Add("BarcodeParam12");
            propertys.Add("BarcodeParam13");
            propertys.Add("BarcodeParam14");
            propertys.Add("BarcodeParam15");
            propertys.Add("BarcodeParam16");
            propertys.Add("BarcodeParam17");
            propertys.Add("BarcodeParam18");
            propertys.Add("BarcodeParam19");
            propertys.Add("BarcodeParam20");
            return propertys;
        }
        public override string ConverterData(object data)
        {
            var content = string.Empty;
            var dataBarcode = data as DataBarcode;
            if (dataBarcode != null)
            {
                //var catalog = RT.Service.Resolve<CatalogController>().GetCatalog();
                content += dataBarcode.BarcodeType + Separator
                    + dataBarcode.BarcodeSite + Separator
                    + dataBarcode.BarcodeParam1 + Separator
                    + dataBarcode.BarcodeParam2 + Separator
                    + dataBarcode.BarcodeParam3 + Separator
                    + dataBarcode.BarcodeParam4 + Separator
                    + dataBarcode.BarcodeParam5 + Separator
                    + dataBarcode.BarcodeParam6 + Separator
                    + dataBarcode.BarcodeParam7 + Separator
                    + dataBarcode.BarcodeParam8 + Separator
                    + dataBarcode.BarcodeParam9 + Separator
                    + dataBarcode.BarcodeParam10 + Separator
                    + dataBarcode.BarcodeParam11 + Separator
                    + dataBarcode.BarcodeParam12 + Separator
                    + dataBarcode.BarcodeParam13 + Separator
                    + dataBarcode.BarcodeParam14 + Separator
                    + dataBarcode.BarcodeParam15 + Separator
                    + dataBarcode.BarcodeParam16 + Separator
                    + dataBarcode.BarcodeParam17 + Separator
                    + dataBarcode.BarcodeParam18 + Separator
                    + dataBarcode.BarcodeParam19 + Separator
                    ;
                //content = itemLabel.Label + itemLabel.Lot + itemLabel.Qty;
            }
            return content;
        }
    }
}
