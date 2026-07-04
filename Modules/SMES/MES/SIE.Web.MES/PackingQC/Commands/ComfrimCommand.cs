using Newtonsoft.Json;
using SIE.Domain;
using SIE.MES.PackingQC;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.PackingQC.Commands
{
    /// <summary>
    /// 执行保存命令
    /// </summary>
    //[JsCommand("SIE.Web.MES.PrepareProducts.Commands.ComfrimCommand")]
    public class ComfrimCommand : FormSaveCommand
    {
        protected override object Excute(ViewArgs args, string scope)
        {
            var PackQc = JsonConvert.DeserializeObject<PackingQc>(args.Data);
            if (RT.Service.Resolve<PackingQcController>().SubmitData(null, PackQc.Id))
                return true;
            else
                return false;
        }
    }
}
