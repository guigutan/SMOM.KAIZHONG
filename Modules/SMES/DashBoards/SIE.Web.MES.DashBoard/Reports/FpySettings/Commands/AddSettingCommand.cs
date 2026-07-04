using SIE.MES.DashBoard.Reports.FpySettings;
using SIE.Web.Command;
using System;

namespace SIE.Web.MES.DashBoard.Reports.FpySettings.Commands
{
    /// <summary>
    /// 产品直通率设置命令
    /// </summary>
    [JsCommand("SIE.Web.MES.DashBoard.Reports.FpySettings.Commands.AddSettingCommand")]
    public class AddSettingCommand : ViewCommand
    {
        /// <summary>
        /// 命令执行
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>返回实体</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var current = new ProductFpySetting();
            current.UpdateDate = DateTime.Now;
            current.UpdateBy = RT.IdentityId;
            return current;
        }
    }
}
