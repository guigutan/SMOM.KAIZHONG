using SIE.MES.TeamManagement.ClockingIns;
using SIE.MES.TeamManagement.Vacancies;
using SIE.Web.Command;
using System;

namespace SIE.Web.MES.TeamManagement.ClockingIns
{
    /// <summary>
    /// 获取打卡记录
    /// </summary>
    [JsCommand("SIE.Web.MES.TeamManagement.ClockingIns.Commands.GetAttRecordCommand")]
    public class GetAttRecordCommand : ViewCommand
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var msg = RT.Service.Resolve<ClockInController>().GetEffectEmployee();
            if (!msg.IsNullOrEmpty()) msg = "<span style='color:blue'>员工同步：</span>" + msg + "；\r\n";
            else
            {
                msg = "<span style='color:blue'>员工同步：</span>执行成功；\r\n";
            }

            var msg2 = RT.Service.Resolve<VacancyController>().SyncWorkGroupVacancy();
            msg += "<span style='color:blue'>缺编初始化数据：</span>" + msg2 + "；\r\n";
            var msg4 = RT.Service.Resolve<VacancyController>().ExeWorkGroupVacancy(0, true);
            msg += "<span style='color:blue'>缺编统计计算：</span>" + msg4;
            return msg;
        }
    }
}