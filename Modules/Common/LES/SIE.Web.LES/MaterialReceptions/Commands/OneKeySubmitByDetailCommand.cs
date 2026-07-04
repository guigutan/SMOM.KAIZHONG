using SIE.Domain;
using SIE.LES.MaterialReceptions;
using SIE.LES.MaterialReceptions.Controllers;
using SIE.LES.MaterialReceptions.Services;
using SIE.LES.MaterialReceptions.ViewModels;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Web.LES.MaterialReceptions.Commands
{
    /// <summary>
    /// 按明细接收提交
    /// </summary>
    public class OneKeySubmitByDetailCommand : ViewCommand<ViewArgs>
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
            
            var viewModelList = args.Data.ToJsonObject<EntityList<MaterialReception>>();


            if (!viewModelList.Any())
            {
                throw new SIE.Domain.Validation.ValidationException("请至少选择一条【待接收】数据".L10N());
            }
            RT.Service.Resolve<MaterialReceptionController>().OneKeySubmit(viewModelList);
            return true;
        }
    }
}
