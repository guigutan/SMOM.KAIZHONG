using SIE.Barcodes;
using SIE.Barcodes.Panels;
using SIE.Domain.Validation;
using SIE.Security;
using SIE.Web.Barcodes.Barcodes.Commands;
using SIE.Web.Command;
using System;

namespace SIE.Web.Barcodes.Panels.Commands
{
    /// <summary>
    /// 条码领用
    /// </summary>
    [JsCommand("SIE.Web.Barcodes.Panels.ReceivePanelCommand")]
    public class ReceivePanelCommand : ListViewCommand
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
            var datas = args?.Data.ToJsonObject<ReceiveDataModel>();
            if (datas != null && datas.BarCodeId > 0)
            {
                try
                {
                    RT.Service.Resolve<PanelController>().PanelRangeReceive(datas.BarCodeId, datas.UseName, datas.PassWord);
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

   
}
