using DevExpress.Xpf.Editors;
using Newtonsoft.Json;
using SIE.Core.RedisUtil;
using SIE.Domain;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.IOT;
using SIE.MES.TaskManagement.Reports.Datas;
using SIE.ObjectModel;
using SIE.Wpf.Controls.WaitProgress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;

namespace SIE.Wpf.MES.TaskManagement.KZReports
{
    /// <summary>
    /// 生产报工(押出) 视图模型
    /// </summary>
    [RootEntity, Serializable]
    [Label("生产报工(押出)")]
    public class KZTaskReportExtrusionViewModel : KZTaskReportViewModel
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public KZTaskReportExtrusionViewModel()
        {
            IotMode = IotMode.Extrusion;
        }

        /// <summary>
        /// 定时报工事件
        /// </summary>
        public override void ReportTimerElapsed()
        {
            try
            {
                DispatchTask = LoadTask(DispatchTaskId);

                if (DispatchTask.TaskStatus == DispatchTaskStatus.Finished)
                {
                    StartNextTask();
                    return;
                }
                var iotRecords = RT.Service.Resolve<DispatchController>().GetAxisChangeRecords(DispatchTask.No, IotEntity, false);
                if (iotRecords.Count == 0)
                    return;
                var last = iotRecords.LastOrDefault();
                AxisQty = last.AxisQty;
                MeterCount = last.MeterCount;

                if (Zcode <= 0 || DispatchTask.RemainQty == 0)
                    return;

                if (iotRecords.All(p => !p.ChangeFlag))
                    return;

                CRT.MainThread.InvokeIfRequired(() =>
                {
                    var redisKey = "KZTaskReportExtrusionViewModel_iotRecords" + string.Join('-', iotRecords.Select(p => p.Id)) + RT.InvOrg;
                    //增加redis锁
                    var tuple = RT.Service.Resolve<RedisUtilController>().Lock(redisKey);
                    if (tuple.Item1)
                    {
                        AutoTaskReport(iotRecords);
                        //解锁
                        RT.Service.Resolve<RedisUtilController>().UnLock(redisKey, tuple.Item2);
                    }
                });

            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        /// <summary>
        /// 自动报工
        /// </summary>
        /// <param name="iotRecords"></param>
        public virtual void AutoTaskReport(EntityList<AxisChangeRecord> iotRecords)
        {
            List<PdaPrintInfo> printInfos = new List<PdaPrintInfo>();
            Exception exception = null;
            var win = new WaitDialog();
            win.Width = 300;
            win.WindowStyle = WindowStyle.None;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Topmost = true;
            win.GetLogicalChild<ProgressBarEdit>().StyleSettings = new ProgressBarMarqueeStyleSettings();
            win.ShowInTaskbar = false;
            win.Text = "正在报工...".L10N();
            ThreadPool.QueueUserWorkItem(oo =>
            {
                try
                {
                    StopIOTReportTimer();

                    foreach (var record in iotRecords)
                    {
                        AutoTaskReport(record, printInfos);
                    }
                }
                catch (Exception exc)
                {
                    exception = exc;
                }
                finally
                {
                    DispatchTask = LoadTask(DispatchTaskId);

                }

                Action ac = () =>
                {
                    win.DialogResult = true;
                    //打印标签
                    if (printInfos.Count > 0)
                        PrintLabels(printInfos, true);
                };
                win.Dispatcher.BeginInvoke(ac);
            });

            win.ShowDialog();
            if (exception != null)
            {
                exception.Alert();
                StartIOTReportTimer();
            }
            else
            {
                CRT.MessageService.ShowInstantMessage("报工成功".L10N(), "提示".L10N(), 3);

                StartNextTask();
            }
        }

        /// <summary>
        /// 对象锁
        /// </summary>
        private readonly object lockObj = new object();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        /// <param name="printInfos"></param>
        public virtual void AutoTaskReport(AxisChangeRecord record, List<PdaPrintInfo> printInfos)
        {
            if (record == null)
                return;
            lock (lockObj)
            {
                var record1 = RF.GetById<AxisChangeRecord>(record.Id, new EagerLoadOptions().LoadWithViewProperty());
                if (record1.IsReport)
                    return;
                var reportThreshold = Zcode;    //报工阀值
                if (DispatchTask.ZcodeThreshold > 0)
                {
                    reportThreshold = Math.Ceiling(Zcode * DispatchTask.ZcodeThreshold / 100);
                }

                var maxReportQty = (decimal)RT.Service.Resolve<DispatchController>().MaxReportQtyAndMaxRemainQty(DispatchTask).Item2;

                //var maxReportQty = (decimal)DispatchTask.MaxRemainQty;

                AxisQty = record.AxisQty;
                MeterCount = record.MeterCount;

                var goodQty = 0m;
                var suspectQty = 0m;
                if (!record.ChangeFlag)
                    return;
                record.IsReport = true;


                if (maxReportQty <= 0)
                {
                    record.Remark = "任务单无剩余可报工数";
                    RF.Save(record);
                    return;
                }

                if (record.AxisQty > reportThreshold)
                {
                    //换轴米数>报工阀值,按分单数报工良品
                    var tempQty = maxReportQty > Zcode ? Zcode : maxReportQty;
                    maxReportQty -= tempQty;
                    goodQty = tempQty;
                    record.ReportQty = tempQty;
                    record.Remark = "良品";
                }
                else
                {
                    //换轴米数>报工阀值,按实际数报工可疑品
                    var tempQty = maxReportQty > record.AxisQty ? (record.AxisQty ?? 0) : maxReportQty;
                    maxReportQty -= tempQty;
                    suspectQty = tempQty;
                    record.ReportQty = tempQty;
                    record.Remark = "可疑品";
                }
                var ret = TaskReport(goodQty, suspectQty, true);
                printInfos.AddRange(ret);

                RF.Save(record);
                RT.Service.Resolve<DispatchController>().AddTaskIotQty(DispatchTask.Id, record.ReportQty);


            }
        }
    }
}
