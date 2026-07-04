using SIE.Andon.Andons;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Andon.Andons.Commands
{
    /// <summary>
    /// 安灯添加命令(同时拉出子表)
    /// </summary>
    public class AndonAddCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var andon = args.Data.ToJsonObject<SIE.Andon.Andons.Andon>();
            return andon;
        }
    }
}
