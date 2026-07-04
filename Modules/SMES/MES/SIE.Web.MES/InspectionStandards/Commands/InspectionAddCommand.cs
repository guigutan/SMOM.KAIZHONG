using SIE.MES.InspectionStandards;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.MES.InspectionStandards.Commands
{
    /// <summary>
    /// 检验项目添加命令
    /// </summary>
    public class InspectionAddCommand : ViewCommand
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected override object Excute(ViewArgs args, string scope)
        {
            var inspection = args.Data.ToJsonObject<ModelInspectionItem>();
            return inspection;
        }
    }
}
