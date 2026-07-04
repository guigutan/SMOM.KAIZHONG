using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.MES.Routings.RoutingBoms.Commands
{
    /// <summary>
    /// 保存工序BOM主表命令
    /// </summary>
    [JsCommand("SIE.Web.MES.Routings.RoutingBoms.Commands.RoutingBomSaveCommand")]
    public class RoutingBomSaveCommand : FormSaveCommand
    {       
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>命令执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            return base.Excute(args, scope);
        }
    }

}
