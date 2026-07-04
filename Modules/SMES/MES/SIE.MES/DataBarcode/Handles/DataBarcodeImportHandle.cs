using SIE.Common.Domain;
using SIE.Common.ImportHelper;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.DataBarcode.Handles
{
    [Services.Service(FallbackType = typeof(DataBarcodeImportHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    /// <inheritdoc/>
    public class DataBarcodeImportHandle : IDisposable, IBusinessImport
    {
        /// <inheritdoc/>
    public List<string> ColumnNameList { get; set; } = new List<string>() {
        "条码化类型".L10N(),
        "条码化工厂".L10N(),
        "条码化参数1".L10N(),
        "条码化参数2".L10N(),
        "条码化参数3".L10N(),
        "条码化参数4".L10N(),
        "条码化参数5".L10N(),
        "条码化参数6".L10N(),
        "条码化参数7".L10N(),
        "条码化参数8".L10N(),
        "条码化参数9".L10N(),
        "条码化参数10".L10N(),
        "条码化参数11".L10N(),
        "条码化参数12".L10N(),
        "条码化参数13".L10N(),
        "条码化参数14".L10N(),
        "条码化参数15".L10N(),
        "条码化参数16".L10N(),
        "条码化参数17".L10N(),
        "条码化参数18".L10N(),
        "条码化参数19".L10N()
    };
        /// <inheritdoc/>
        public Dictionary<string, ValidColumn> ColumnValidList { get; set; }
        /// <inheritdoc/>
        public IBusinessImport CreaetColumnValid()
        {
            ColumnValidList = new Dictionary<string, ValidColumn>() {
            {ColumnNameList[0],new ValidColumn(ImportDataType._String,false ,true) },
            {ColumnNameList[1],new ValidColumn(ImportDataType._String,false ,true) },
            {ColumnNameList[2],new ValidColumn(ImportDataType._String,false ,true) },
            {ColumnNameList[3],new ValidColumn(ImportDataType._String,false ,true) },
            {ColumnNameList[4],new ValidColumn(ImportDataType._String,false ,true) },
            {ColumnNameList[5],new ValidColumn(ImportDataType._String,false ,true) },
            {ColumnNameList[6],new ValidColumn(ImportDataType._String,false ,true) },
            {ColumnNameList[7],new ValidColumn(ImportDataType._String,false ,true) },
            {ColumnNameList[8],new ValidColumn(ImportDataType._String,false ,true) },
            {ColumnNameList[9],new ValidColumn(ImportDataType._String,false ,true) },
            {ColumnNameList[10],new ValidColumn(ImportDataType._String,false ,true) },
            {ColumnNameList[11],new ValidColumn(ImportDataType._String,false ,true) },
            {ColumnNameList[12],new ValidColumn(ImportDataType._String,false ,true) },
            {ColumnNameList[13],new ValidColumn(ImportDataType._String,false ,true) },
            {ColumnNameList[14],new ValidColumn(ImportDataType._String,false ,true) },
            {ColumnNameList[15],new ValidColumn(ImportDataType._String,false ,true) },
            {ColumnNameList[16],new ValidColumn(ImportDataType._String,false ,true) },
            {ColumnNameList[17],new ValidColumn(ImportDataType._String,false ,true) },
            {ColumnNameList[18],new ValidColumn(ImportDataType._String,false ,true) },
            {ColumnNameList[19],new ValidColumn(ImportDataType._String,false ,true) },
            {ColumnNameList[20],new ValidColumn(ImportDataType._String,false ,true) },
        };
            return this;
        }

        /// <summary>
        /// 给保存错误的数据行记录错误数据信息
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="columnName">列名</param>
        /// <param name="errorMsg">错误信息</param>
        private void AppendErrorMsg(DataRow row, string columnName, string errorMsg)
        {
            row[columnName] += errorMsg;
        }

        /// <inheritdoc/>
        public void Dispose()
        {

        }
        /// <inheritdoc/>
        public void ProcessBusinessDataHandle(DataRow[] drs)
        {
            if (drs.Length == 0) return;
            var dataBarcodes = new EntityList<DataBarcode>();

            drs.ForEach(p => {
                try
                {
                    var dataBarcode = new DataBarcode();
                    dataBarcode.BarcodeType = p[ColIndex(ColumnNameList[0])].ToString();
                    dataBarcode.BarcodeSite = p[ColIndex(ColumnNameList[1])].ToString();
                    dataBarcode.BarcodeParam1 = p[ColIndex(ColumnNameList[2])].ToString();
                    dataBarcode.BarcodeParam2 = p[ColIndex(ColumnNameList[3])].ToString();
                    dataBarcode.BarcodeParam3 = p[ColIndex(ColumnNameList[4])].ToString();
                    dataBarcode.BarcodeParam4 = p[ColIndex(ColumnNameList[5])].ToString();
                    dataBarcode.BarcodeParam5 = p[ColIndex(ColumnNameList[6])].ToString();
                    dataBarcode.BarcodeParam6 = p[ColIndex(ColumnNameList[7])].ToString();
                    dataBarcode.BarcodeParam7 = p[ColIndex(ColumnNameList[8])].ToString();
                    dataBarcode.BarcodeParam8 = p[ColIndex(ColumnNameList[9])].ToString();
                    dataBarcode.BarcodeParam9 = p[ColIndex(ColumnNameList[10])].ToString();
                    dataBarcode.BarcodeParam10 = p[ColIndex(ColumnNameList[11])].ToString();
                    dataBarcode.BarcodeParam11 = p[ColIndex(ColumnNameList[12])].ToString();
                    dataBarcode.BarcodeParam12 = p[ColIndex(ColumnNameList[13])].ToString();
                    dataBarcode.BarcodeParam13 = p[ColIndex(ColumnNameList[14])].ToString();
                    dataBarcode.BarcodeParam14 = p[ColIndex(ColumnNameList[15])].ToString();
                    dataBarcode.BarcodeParam15 = p[ColIndex(ColumnNameList[16])].ToString();
                    dataBarcode.BarcodeParam16 = p[ColIndex(ColumnNameList[17])].ToString();
                    dataBarcode.BarcodeParam17 = p[ColIndex(ColumnNameList[18])].ToString();
                    dataBarcode.BarcodeParam18 = p[ColIndex(ColumnNameList[19])].ToString();
                    dataBarcode.BarcodeParam19 = p[ColIndex(ColumnNameList[20])].ToString();
                    dataBarcodes.Add(dataBarcode);
                }
                catch (Exception ex)
                {
                    string strMsg = AppRuntime.Location.ConnectDataDirectly ? ex.Message : ex.InnerException.Message;
                    p[ImportDataHandle.MessageColumnName] = strMsg;                }
            });
            BulkSaver.Save(dataBarcodes);
        }
        /// <inheritdoc/>
        protected virtual int ColIndex(string columnName)
        {
            return ColumnNameList.IndexOf(columnName);
        }
    }
}
