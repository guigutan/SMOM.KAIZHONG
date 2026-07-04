using SIE.MES.Validitys;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.Validitys.Commands
{
    /// <summary>
    /// 有效期标准维护添加命令
    /// </summary>
    public class ValidityAddCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var validity = args.Data.ToJsonObject<ValidityStandard>();
            validity.Effective = DateTime.Now;
            return validity;
        }
    }
}
