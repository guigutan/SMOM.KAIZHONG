using SIE.LES.MaterialReceptions.Controllers;
using SIE.LES.MaterialReceptions.Services;
using SIE.LES.MaterialReceptions.ViewModels;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.LES.MaterialReceptions.Commands
{
    /// <summary>
    /// 按明细接收提交
    /// </summary>
    public class SubmitByDetailCommand : ViewCommand<ViewArgs>
    {
        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected override object Excute(ViewArgs args, string scope)
        {
            var viewModelList = args.Data.ToJsonObject<List<MaterialReceptionAddViewModel>>();
            RT.Service.Resolve<MaterialReceptionController>().SubByDetail(viewModelList);
            return true;
        }
    }
}
