using SIE.XPCJ.Common.ApiCall;
using SIE.XPCJ.Models;
using SIE.XPCJ.Models.Exceptions;
using SIE.XPCJ.Models.WIP;
using SIE.XPCJ.Models.WIP.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SIE.XPCJ.BussMove
{

    /// <summary>
    /// 过站服务
    /// </summary>
    public static class MoveService
    {
       private static readonly string moveController = "WinFormMoveApiController";


        /// <summary>
        /// 获取条码
        /// </summary>
        /// <param name="sn"></param>
        /// <returns></returns>
        public static Barcode GetBarcode(string sn)
        {
            object[] parameters = new object[1];
            parameters[0] = sn;
            var result = ApiHelper.Post<Barcode>(moveController, "GetBarcode", parameters);
            if (result.Success)
            {
                return result.Result;
            }
            else
            {
                throw new ValidationException(result.Message);
            }
        }

        /// <summary>
        /// 补打
        /// </summary>
        /// <param name="sn"></param>
        public static void Reprint(List<double> barcodeIds, string log = "打印外标签")
        {
            object[] parameters = new object[2];
            parameters[0] = barcodeIds;
            parameters[1] = log;
            var result = ApiHelper.Post<Barcode>(moveController, "Reprint", parameters);
            if (!result.Success)
            {
                throw new ValidationException(result.Message);
            }
        }

        /// <summary>
        /// 获取条码模板打印Id
        /// </summary>
        /// <param name="sn"></param>
        /// <returns></returns>
        public static double GetBarcodeTemplateId(string sn)
        {
            object[] parameters = new object[1];
            parameters[0] = sn;
            var result = ApiHelper.Post<double>(moveController, "GetBarcodeTemplateId", parameters);
            if (!result.Success)
            {
                throw new ValidationException(result.Message);
            }
            return result.Result;
        }

    }
}