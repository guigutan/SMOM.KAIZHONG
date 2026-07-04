using SIE.Domain;
using SIE.MES.TeamManagement.ClockingIns;
using SIE.ObjectModel;
using System;
using System.Text;

namespace SIE.MES.TeamManagement.Win.Job
{
    /// <summary>
    /// 考勤调度控制器
    /// </summary>
    public class JobController : DomainController
    {
        /// <summary>
        /// 考勤机注册实时打卡事件
        /// </summary>
        /// <param name="enrollNumber">工号</param>
        /// <param name="isInValid">是否为有效记录</param>
        /// <param name="attState">考勤状态（没用到默认是checkIn 0）</param>
        /// <param name="verifyMethod">记录的验证方式（没用到）</param>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <param name="hour">时</param>
        /// <param name="minute">分</param>
        /// <param name="second">秒</param>
        /// <param name="workCode">工作号码（没用到）</param>
        public virtual void Zke_OnAttTransactionEx(string enrollNumber, int isInValid, int attState, int verifyMethod, int year, int month, int day, int hour, int minute, int second, int workCode)
        {
            DateTime dt = DateTime.Parse(year.ToString() + "-" + month.ToString() + "-" + day.ToString() + " " + hour.ToString() + ":" + minute.ToString() + ":" + second.ToString());
            SaveCardDetail(enrollNumber, dt);
        }

        /// <summary>
        /// 保存打卡数据
        /// </summary>
        /// <param name="code">工号</param>
        /// <param name="cardTime">打卡时间</param>
        /// <param name="machineName">打卡机名称</param>
        public virtual void SaveCardDetail(string code, DateTime cardTime, string machineName = "")
        {
            var emp = RT.Service.Resolve<ClockInController>().GetEmployeeClockIn(code, cardTime);
            if (emp != null)
            {
                ClockInDetail detail = new ClockInDetail()
                {
                    ClockInId = emp.Id,
                    ClockInDate = cardTime,
                    ClockInAddress = machineName
                };
                emp.ClockInDetail.Add(detail);
                RF.Save(emp);
            }
        }

        /// <summary>
        /// 读取时间范围内的打卡数据
        /// </summary>
        /// <param name="dr">时间范围</param>
        /// <returns>提示信息</returns>
        public virtual string ReadAllMachineCard(DateRange dr)
        {
            try
            {
                zkemkeeper.CZKEMClass zke = new zkemkeeper.CZKEMClass();
                var machineList = RT.Service.Resolve<ClockInController>().GetMachineList();
                const int iMachineNumber = 1;
                if (machineList.Count == 0) return "没有读取到考勤机信息，请配置";
                StringBuilder msg = new StringBuilder();
                foreach (var machine in machineList)
                {
                    string sdwEnrollNumber = string.Empty;
                    int idwVerifyMode = 0;
                    int idwInOutMode = 0;
                    int idwYear = 0;
                    int idwMonth = 0;
                    int idwDay = 0;
                    int idwHour = 0;
                    int idwMinute = 0;
                    int idwSecond = 0;
                    int idwWorkcode = 0;
                    if (zke.Connect_Net(machine.IpAddress, machine.Port))
                    {
                        //使用NUGET包的DLL没有按时间获取记录的方法，但官网的dll有
                        if (zke.ReadAllGLogData(iMachineNumber))
                        {
                            while (zke.SSR_GetGeneralLogData(iMachineNumber, out sdwEnrollNumber, out idwVerifyMode,
                                  out idwInOutMode, out idwYear, out idwMonth, out idwDay, out idwHour, out idwMinute, out idwSecond, ref idwWorkcode))
                            {
                                DateTime dt = DateTime.Parse(idwYear.ToString() + "-" + idwMonth.ToString() + "-" + idwDay.ToString() + " " + idwHour.ToString() + ":" + idwMinute.ToString() + ":" + idwSecond.ToString());
                                if (dt >= dr.BeginValue.Value && dt < dr.EndValue.Value.AddDays(1))
                                    SaveCardDetail(sdwEnrollNumber, dt, machine.Name);
                            }
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(msg.ToString()))
                        {
                            msg.Append(string.Join("", "考勤机连接失败:", machine.Name));
                        }
                        else
                        {
                            msg.Append(",");
                        }
                    }
                }

                return msg.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 获取并执行考勤数据,只今天数据days=0
        /// </summary>
        /// <param name="days">天数</param>
        /// <param name="inculeToday">是否包含今天</param>
        /// <returns>msg</returns>
        public virtual string GetAndExcAttentRecord(int days, bool inculeToday)
        {
            var date = RF.Find<EmployeeClockIn>().GetDbTime().Date;
            DateRange dr = new DateRange() { BeginValue = date.AddDays(-days), EndValue = date };
            if (!inculeToday) dr.EndValue = date.AddDays(-1);
            string msg = ReadAllMachineCard(dr);
            if (!string.IsNullOrEmpty(msg))
                return msg;
            else
            {
                msg = RT.Service.Resolve<ClockInController>().ExeEmployeeClockInState(dr);
                if (!string.IsNullOrEmpty(msg))
                {
                    msg = "已成功获取打卡数据！" + msg;
                }
            }

            return msg;
        }
    }
}
