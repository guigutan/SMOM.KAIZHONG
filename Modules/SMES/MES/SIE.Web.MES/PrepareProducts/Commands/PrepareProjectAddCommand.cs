using SIE.MES.PrepareProducts;
using SIE.MES.PrepareProducts.Services;
using SIE.Web.Command;
using System;

namespace SIE.Web.MES.PrepareProducts.Commands
{
    /// <summary>
    /// 产线准备项目维护添加命令
    /// </summary>
    public class PrepareProjectAddCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected override object Excute(ViewArgs args, string scope)
        {
            var preProject = args.Data.ToJsonObject<PrepareProject>();
            preProject.ProCode = RT.Service.Resolve<PrepareProjectService>().GetPreProjectCodeWithAdd();
            return preProject;
        }
    }
}
