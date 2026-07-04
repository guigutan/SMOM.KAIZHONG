using SIE.ProductIntfc.OutputProducts;
using SIE.Web.Command;
using System.Collections.Generic;

namespace SIE.Web.ProductIntfc.OutputProducts.Commands
{
    /// <summary>
    /// 副产品收货
    /// </summary>
    public class OutputProductReceiveCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var datas = args.Data.ToJsonObject<List<OutputProductRecordViewModel>>();
            
            RT.Service.Resolve<OutputProductController>().SubmitOutputProductDatas(datas);
            return true;
        }
    }
}
