using SIE.Web.Data;
using SIE.Web.MES.DataBarcode.ViewModels;
using SIE.Web.Packages.ItemLabels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.DataBarcode.DataQuerys
{
    public class DataBarcodeDataQuery : DataQueryer
    {
        /// <summary>
        /// 数据条码打印
        /// </summary>
        /// <param name="huId">数据条码ID</param>
        /// <returns>补打信息</returns>
        public DataBarcodeViewModels GetReprintInfo(double huId)
        {
            return null;
        }
    }
    }
