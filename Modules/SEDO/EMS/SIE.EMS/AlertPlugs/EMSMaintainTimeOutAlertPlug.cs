using SIE.Common.Alert;
using SIE.EMS.Maintains.Controller;
using SIE.EMS.Senders;
using System;
using System.Linq;
using System.Text;

namespace SIE.EMS.AlertPlugs
{
    /// <summary>
    /// 设备保养任务超时提醒
    /// </summary>
    [RootEntity, Serializable]
    [Alert("设备保养任务超时提醒", typeof(EmsMaintainTimeOutAlertPlugConfig), typeof(StringAlertResult), "设备保养任务超时提醒")]
    public class EmsMaintainTimeOutAlertPlug : AlertBase
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <returns>预警参数</returns>
        public override AlertResultBase Run()
        {
            var config = this.Context.Config as EmsMaintainTimeOutAlertPlugConfig;

            var maintainPlanList = RT.Service.Resolve<MaintainController>().GetTimeOutMaintainPlanList(config.EnterpriseId, config.ProcessId, config.TimeOut);

            if (maintainPlanList.Any())
            {
                var result = new StringAlertResult();
                var message = new StringBuilder();
                message.Append("[" + config.Enterprise.Name + "]车间[" + config.Process.Name + "]工序:\\n\r");
                var i = 1;
                foreach (var item in maintainPlanList)
                {
                    message.Append("(" + i + ")" + item.MachineNo + "\\n\r");
                    i++;
                }
                message.Append("以上设备保养任务快超期！\\n\r");
                message.Append("请车间做好停机计划，请机修人员按计划进行保养");
                result.Message = message.ToString();
                result.Value = JudgeEarlyOrNight(config.EarlyStartDate, config.NightStartDate, DateTime.Now) ? 1 : 2;
                return result;
            }

            return null;
        }

        /// <summary>
        /// 判断是否早班
        /// </summary>
        /// <param name="earlyBeginDate">最早开始时间</param>
        /// <param name="nightBeginDate">最晚开始时间</param>
        /// <param name="nowDate">当前时间</param>
        /// <returns>true/false</returns>
        public bool JudgeEarlyOrNight(DateTime earlyBeginDate, DateTime nightBeginDate, DateTime nowDate)
        {
            bool result = false;
            if (nowDate >= DateTime.Now.Date.AddHours(earlyBeginDate.Hour).AddMinutes(earlyBeginDate.Minute).AddMilliseconds(earlyBeginDate.Millisecond) && nowDate < DateTime.Now.Date.AddHours(nightBeginDate.Hour).AddMinutes(nightBeginDate.Minute).AddMilliseconds(nightBeginDate.Millisecond))
                //早班返回true
                result = true;
            return result;
        }
    }
}
