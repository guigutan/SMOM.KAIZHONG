using SIE.Fixtures;
using SIE.Web.Command;
using System;
using System.Diagnostics;

namespace SIE.Web.Fixtures.Abnormals.Commands
{
    /// <summary>
    /// 添加工治具异常类型
    /// </summary>
    [JsCommand("SIE.Web.Fixtures.Abnormals.Commands.AddAbnormalCommand")]
    public class AddAbnormalCommand : ViewCommand<ViewArgs>
    {
        /// <summary>
        /// 执行工治具异常类型
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>工治具异常类型</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            try
            {
                return RT.Service.Resolve<CoreFixtureController>().GetAbnormalCode();
            }
            catch (Exception exc)
            {
                Debug.Write(exc);
                return "";
            }
        }
    }
}
