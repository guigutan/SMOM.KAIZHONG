using SIE.Barcodes;
using SIE.Domain.Validation;
using SIE.Security;
using SIE.Web.Command;
using System;

namespace SIE.Web.Barcodes.Barcodes.Commands
{
    /// <summary>
    /// 条码领用
    /// </summary>
    [JsCommand("SIE.Web.Barcodes.ReceiveBarcodeCommand")]
    public class ReceiveBarcodeCommand : ListViewCommand
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
            var datas = args.Data.ToJsonObject<ReceiveDataModel>();
            if (datas.BarCodeId > 0)
            {
                try
                {
                    RT.Service.Resolve<BarcodeController>().BarcodeReceive(datas.BarCodeId, datas.UseName, datas.PassWord);
                    rst = true;
                }
                catch (Exception ex)
                {
                    var baseEx = ex.GetBaseException();
                    if (baseEx is AuthenticationException)
                    {
                        throw new ValidationException((baseEx as AuthenticationException).Message);
                    }
                    else if (baseEx is ValidationException)
                    {
                        throw new ValidationException((baseEx as ValidationException).Message);
                    }
                    else
                    {
                        //
                    }

                    throw;
                }
            }
            return rst;
        }
    }

    /// <summary>
    /// 保存数据类
    /// </summary>
    public class ReceiveDataModel
    {
        /// <summary>
        /// BarCodeId
        /// </summary>
        public double BarCodeId
        {
            get; set;
        }

        /// <summary>
        /// Use Name
        /// </summary>
        public string UseName
        {
            get; set;
        }

        /// <summary>
        /// Password
        /// </summary>
        public string PassWord
        {
            get; set;
        }
    }
}
