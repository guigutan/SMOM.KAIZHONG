using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.PrepareProducts;
using SIE.MES.PrepareProducts.Services;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.MES.PrepareProducts.Commands
{
    /// <summary>
    /// 产前准备项目维护保存命令
    /// </summary>
    public class PrepareProjectSaveCommand : SaveCommand
    {
        /// <summary>
        /// 保存前事件
        /// </summary>
        /// <param name="data"></param>
        protected override void OnSaving(EntityList data)
        {
            RT.Service.Resolve<PrepareProjectService>().PrepareProjectSave(data);
            base.OnSaving(data);
        }
    }
}
