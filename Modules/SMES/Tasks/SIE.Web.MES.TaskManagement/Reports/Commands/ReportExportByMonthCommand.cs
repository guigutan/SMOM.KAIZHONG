using SIE.Web.Command;

namespace SIE.Web.MES.TaskManagement.Reports.Commands
{
    /// <summary>
    /// 报工记录按月导出命令
    /// 注：实际导出逻辑在前端JavaScript中实现，此类仅作为命令注册使用
    /// </summary>
    [JsCommand("SIE.Web.MES.TaskManagement.Reports.Commands.ReportExportByMonthCommand")]
    public class ReportExportByMonthCommand : ViewCommand
    {
        /// <summary>
        /// 执行按月导出（占位方法，实际逻辑由前端处理）
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            // 此方法不会被实际调用，导出逻辑完全在前端实现
            return true;
        }
    }
}