using SIE.Common.ImportHelper;
using SIE.Web.Common.Import.Commands;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.DataBarcode.Commands
{
    internal class DataBarcodeImportCommand : ImportCommandBase
    {
        protected override ImportCompleted GetImportCompleted()
        {
            return (DataRow[] drSuccess, DataRow[] DrFailed) =>
            {
            };
        }

        protected override Type GetImportHandleType()
        {
            return typeof(SIE.MES.DataBarcode.Handles.DataBarcodeImportHandle);
        }
    }
}
