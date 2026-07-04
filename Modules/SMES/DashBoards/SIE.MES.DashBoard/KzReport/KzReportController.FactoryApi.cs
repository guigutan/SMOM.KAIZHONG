using Castle.Core.Internal;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Math;
using DotLiquid.Util;
using IronPython.Runtime.Operations;
using NPOI.SS.Formula.Functions;
using SIE.Andon.Andons;
using SIE.Andon.Andons.Enum;
using SIE.Api;
using SIE.Common.InvOrg;
using SIE.Core.ApiModels;
using SIE.Core.Common;
using SIE.Core.Enums;
using SIE.Data;
using SIE.Defects;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.EquipStatus;
using SIE.Items;
using SIE.KZ.Base.Interfaces;
using SIE.MES.Andon;
using SIE.MES.DashBoard.DashBoards.WorkShop;
using SIE.MES.DashBoard.KzBoard.Datas;
using SIE.MES.DashBoard.KzBoard.RegionBoards;
using SIE.MES.DashBoard.KzReport.Datas;
using SIE.MES.DashBoard.KzReport.OrganizeCodes;
using SIE.MES.DashBoard.KzReport.ProductionProcesss;
using SIE.MES.LineAndon;
using SIE.MES.ProcessProperty;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.FeedingRecords;
using SIE.MES.TaskManagement.Reports;
using SIE.MES.TaskManagement.SuspectProductLabels;
using SIE.MES.WorkOrders;
using SIE.ObjectModel;
using SIE.Packages;
using SIE.Rbac.InvOrgs;
using SIE.Resources.Enterprises;
using SIE.Resources.ShiftTypes;
using SIE.Resources.WipResources;
using SIE.Security;
using SIE.Tech.OEE;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IronPython.Modules._ast;
using static IronPython.Modules.PythonSocket;

namespace SIE.MES.DashBoard.KzReport
{
    public partial class KzReportController
    {
        /// <summary>
        /// 获取下拉工序-工厂
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        [ApiService("获取下拉工序-工厂")]
        [AllowAnonymous]
        public virtual List<string> GetProcessesFactory(Dictionary<string, List<string>> dic)
        {
            List<string> processes = new List<string>();
            foreach (var d in dic)
            {
                try
                {
                    //登录
                    RT.Service.Resolve<KzLoginController>().Login(d.Key);
                    var list = Query<ReportRecordExamine>().Where(p => d.Value.Contains(p.WorkShop.Code)).Select(p => p.Process.Code).Distinct().ToList<string>();
                    processes.AddRange(list);
                }
                catch (Exception ex)
                {

                }
            }

            return processes.Distinct().ToList();
        }

        #region 投入产出日报表

        /// <summary>
        /// 投入产出日报表明细-工厂
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="process"></param>
        /// <param name="year"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        [ApiService("投入产出日报表明细-工厂")]
        [AllowAnonymous]
        public virtual List<InputOutputDailyReportDtlData> GetInputOutputDailyReportDtlDatasFactory(Dictionary<string, List<string>> dic, List<string> process, int year, string date)
        {
            var split = date.Split("月");
            var month = Convert.ToInt32(split[0]);
            var day = Convert.ToInt32(split[1].Split("日")[0]);

            var beginTime = new DateTime(year, month, day, 8, 0, 0);
            var endTime = beginTime.AddDays(1);

            List<InputOutputDailyReportDtlData> datas = new List<InputOutputDailyReportDtlData>();
            foreach (var d in dic)
            {
                try
                {
                    var factory = d.Key;
                    //登录
                    RT.Service.Resolve<KzLoginController>().Login(factory);

                    //获取任务单
                    var tasks = Query<DispatchTask>()
                    .Where(p => p.PlanBeginTime >= beginTime && p.PlanBeginTime < endTime && d.Value.Contains(p.WorkShop.Code) && process.Contains(p.Process.Code))
                    .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                    //获取报工记录
                    var reportRecords = Query<ReportRecordExamine>()
                        .Where(p => p.ReportTime >= beginTime && p.ReportTime < endTime && d.Value.Contains(p.WorkShop.Code) && process.Contains(p.Process.Code))
                        .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                    //这里工序只会有一条
                    foreach (var pro in process)
                    {
                        var ts = tasks.Where(p => p.ProcessCode == pro).ToList();
                        var rrs = reportRecords.Where(p => p.ProcessCode == pro).ToList();
                        var productCodes = new List<string>();
                        productCodes.AddRange(ts.Select(p => p.ProductCode).Distinct().ToList());
                        productCodes.AddRange(rrs.Select(p => p.ProductCode).Distinct().ToList());
                        productCodes = productCodes.Distinct().ToList();
                        //找出产品，此处工序只会有一条，所以不会造成循环查询的情况
                        var products = RT.Service.Resolve<ItemController>().GetItemDataList(productCodes, new EagerLoadOptions().LoadWithViewProperty());

                        foreach (var product in products)
                        {
                            InputOutputDailyReportDtlData data = new InputOutputDailyReportDtlData();

                            data.ProductCode = product.Code;
                            data.ProductName = product.Name;
                            data.Date = date;
                            //派工管理中排程开始时间为某一天的任务数量的合计
                            data.PlanQty = ts.Where(p => p.ProductCode == product.Code).Sum(p => p.DispatchQty);
                            //报工记录中报工时间为某一天的报工数+可疑品数量的合计
                            data.ActualQty = rrs.Where(p => p.ProductCode == product.Code).Sum(p => p.ReportQty + p.SuspectQty);
                            //实际投入数-计划投入数
                            data.DiffQty = data.ActualQty - data.PlanQty;
                            datas.Add(data);
                        }
                    }
                }
                catch (Exception ex)
                { }
            }

            return datas;
        }

        /// <summary>
        /// 投入产出日报表-工厂
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="process"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        [ApiService("投入产出日报表-工厂")]
        [AllowAnonymous]
        public virtual List<InputOutputDailyReportData> GetInputOutputDailyReportDatasFactory(Dictionary<string, List<string>> dic, List<string> process, int year, string month)
        {
            var mon = GetInputOutputDailyReportDatasFactoryMonth(month);
            //如果传入的月份不正常，那么就直接结束计算
            if (mon == 0)
                return new List<InputOutputDailyReportData>();

            //这个月的1号8点之后为开始时间
            var beginTime = new DateTime(year, mon, 1, 8, 0, 0);
            //下个月的1号8点之前为结束时间
            var endTime = beginTime.AddMonths(1);

            //<开始时间，结束时间>
            Dictionary<DateTime, DateTime> totoTime = GetDicTime(beginTime, endTime);

            List<InputOutputDailyReportData> datas = new List<InputOutputDailyReportData>();

            foreach (var d in dic)
            {
                try
                {
                    var factory = d.Key;
                    //登录
                    RT.Service.Resolve<KzLoginController>().Login(factory);
                    //取值各工厂工序属性中勾上了排程点的工序
                    var processPtys = Query<ProcessPty>()
                        .WhereIf(process != null && process.Count > 0, p => process.Contains(p.Process.Code))
                        .Where(p => p.Scheduling == true)
                        .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                    //找出要显示的工序
                    var processIds = processPtys.Select(p => p.ProcessId).Distinct().ToList();
                    //获取任务单
                    var tasks = Query<DispatchTask>()
                        .Where(p => p.PlanBeginTime >= beginTime && p.PlanBeginTime < endTime && d.Value.Contains(p.WorkShop.Code) && processIds.Contains((double)p.ProcessId))
                        .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                    //获取报工记录
                    var reportRecords = Query<ReportRecordExamine>()
                        .Where(p => p.ReportTime >= beginTime && p.ReportTime < endTime && d.Value.Contains(p.WorkShop.Code) && processIds.Contains((double)p.ProcessId))
                        .ToList(null, new EagerLoadOptions().LoadWithViewProperty());

                    foreach (var processId in processIds)
                    {
                        var processCode = processPtys.FirstOrDefault(p => p.ProcessId == processId).ProcessCode;

                        foreach (var ttd in totoTime)
                        {
                            InputOutputDailyReportData data = new InputOutputDailyReportData();

                            data.Process = processCode;
                            data.Date = ttd.Key.Month + "月" + ttd.Key.Day + "日";
                            //派工管理中排程开始时间为某一天的任务数量的合计
                            data.PlanQty = tasks.Where(p => p.PlanBeginTime >= ttd.Key && p.PlanBeginTime < ttd.Value && p.ProcessId == processId).Sum(p => p.DispatchQty);
                            //报工记录中报工时间为某一天的报工数+可疑品数量的合计
                            data.ActualQty = reportRecords.Where(p => p.ReportTime >= ttd.Key && p.ReportTime < ttd.Value && p.ProcessId == processId).Sum(p => p.ReportQty + p.SuspectQty);
                            //实际投入数-计划投入数
                            data.DiffQty = data.ActualQty - data.PlanQty;
                            datas.Add(data);
                        }
                    }
                }
                catch (Exception ex)
                { }
            }

            return datas;
        }

        /// <summary>
        /// 获取时间段
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public virtual Dictionary<DateTime, DateTime> GetDicTime(DateTime beginTime, DateTime endTime)
        {
            Dictionary<DateTime, DateTime> totoTime = new Dictionary<DateTime, DateTime>();

            while (beginTime < endTime)
            {
                //<开始时间，结束时间>
                totoTime.Add(beginTime, beginTime.AddDays(1));
                beginTime = beginTime.AddDays(1);
            }

            return totoTime;
        }

        /// <summary>
        /// 获取月份
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public virtual int GetInputOutputDailyReportDatasFactoryMonth(string month)
        {
            int mon = 0;
            switch (month)
            {
                case "1月":
                    mon = 1;
                    break;
                case "2月":
                    mon = 2;
                    break;
                case "3月":
                    mon = 3;
                    break;
                case "4月":
                    mon = 4;
                    break;
                case "5月":
                    mon = 5;
                    break;
                case "6月":
                    mon = 6;
                    break;
                case "7月":
                    mon = 7;
                    break;
                case "8月":
                    mon = 8;
                    break;
                case "9月":
                    mon = 9;
                    break;
                case "10月":
                    mon = 10;
                    break;
                case "11月":
                    mon = 11;
                    break;
                case "12月":
                    mon = 12;
                    break;
            }
            return mon;
        }

        #endregion

        #region OEE

        /// <summary>
        /// OEE
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="process"></param>
        /// <param name="resource"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        [ApiService("OEE-工厂")]
        [AllowAnonymous]
        public virtual List<OeeData> GetOeeDatasFactory(Dictionary<string, List<string>> dic, List<string> process, string resource, DateTime? beginTime, DateTime? endTime)
        {
            List<OeeData> oees = new List<OeeData>();
            List<WoReportQty> WoReportQtys = new List<WoReportQty>();
            foreach (var d in dic)
            {
                try
                {
                    var factory = d.Key;
                    //登录
                    RT.Service.Resolve<KzLoginController>().Login(factory);

                    var oeeProcesses = Query<OeeProcess>().WhereIf(process != null && process.Count > 0, p => process.Contains(p.Process.Code)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());

                    //获取此次要计算的工序
                    var processIds = oeeProcesses.Select(p => p.ProcessId).Distinct().ToList();

                    ShiftType shiftType = Query<ShiftType>().Where(p => p.IsDefault == YesNo.Yes).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
                    double workTime = 0;
                    double restTime = 0;
                    if (shiftType != null)
                    {
                        foreach (var shift in shiftType.ShiftList)
                        {
                            if (shift.IsOverDay == true)
                            {
                                //跨天的，直接当前减去0点的+(隔天0点-开始时间)
                                workTime += (shift.EndTime - shift.EndTime.Date).TotalMinutes + (shift.BeginTime.AddDays(1).Date.AddSeconds(-1) - shift.BeginTime).TotalMinutes;
                            }
                            else
                            {
                                workTime += (shift.EndTime - shift.BeginTime).TotalMinutes;
                            }
                            //休息时间
                            foreach (var shiftRest in shift.ShiftRestList)
                            {
                                restTime += (shiftRest.EndTime - shiftRest.BeginTime).TotalMinutes;
                            }
                        }
                    }

                    //找出都有哪些资源、工序、设备，然后进行分组
                    var groupAndonLine = DB.Query<ReportRecordExamine>("rre")
                        .Join<AndonLine>((x, y) => x.Resource.Code == y.MachineCode)
                        .Join<AndonLine, EquipStatus>((x, y) => x.EquipmentId == y.EquipAccountId)
                        .WhereIf<EquipStatus>(!resource.IsNullOrEmpty(), (x, y) => y.EquipAccount.Code.Contains(resource))
                        .WhereIf(beginTime != null, p => p.ReportTime >= beginTime)
                        .WhereIf(endTime != null, p => p.ReportTime <= endTime)
                        .Where(p => d.Value.Contains(p.WorkShop.Code))
                        .Where(p => processIds.Contains((double)p.ProcessId))
                        .Select<AndonLine, EquipStatus>((x, y, z) => new { x.ProcessId, y.EquipmentId, x.ResourceId, Resource = x.Resource.Code, Process = x.Process.Code })
                        .Distinct().ToList<EquipResource>();

                    foreach (var g in groupAndonLine)
                    {
                        oees.Add(new OeeData()
                        {
                            Process = g.Process,
                            Resource = g.Resource,
                            PlanTime = (decimal)(workTime - restTime)
                        });
                    }

                    //参与计算有哪些资源
                    var resourceIds = groupAndonLine.Select(p => p.ResourceId).Distinct().ToList();

                    //取值设备状态明细表中当前设备编码状态为运行的且开始时间在日期查询条件范围内的时长合计
                    var equipmentIds = groupAndonLine.Select(p => p.EquipmentId).Distinct().ToList();
                    var runningQuery = DB.Query<EquipStatusDetail>("esd").Where(p => equipmentIds.Contains(p.EquipStatus.EquipAccountId) && p.Status == Equipments.EquipStatus.Enums.EquipStatusDetailStatus.Running);
                    if (beginTime != null)
                    {
                        runningQuery.Where(p => p.BeginTime >= beginTime);
                    }
                    if (endTime != null)
                    {
                        runningQuery.Where(p => p.BeginTime <= endTime);
                    }
                    //对运行中设备进行分组求和
                    var runnungDetails = runningQuery.GroupBy(p => p.EquipStatus.EquipAccountId).GroupBy(p => p.EquipStatus.EquipAccount.Code).Select(p => new { EquipAccountId = p.EquipStatus.EquipAccountId, EquipAccountCode = p.EquipStatus.EquipAccount.Code, Minute = p.SQL<decimal>("round(SUM(nvl(esd.Minute,0)),4) Minute") }).ToList<EquipStatusDetail>();

                    //取值设备状态明细表中当前设备编码状态为离线的且开始时间在日期查询条件范围内的时长合计
                    var offLineQuery = DB.Query<EquipStatusDetail>("esd").Where(p => equipmentIds.Contains(p.EquipStatus.EquipAccountId) && p.Status == Equipments.EquipStatus.Enums.EquipStatusDetailStatus.OffLine);
                    if (beginTime != null)
                    {
                        offLineQuery.Where(p => p.BeginTime >= beginTime);
                    }
                    if (endTime != null)
                    {
                        offLineQuery.Where(p => p.BeginTime <= endTime);
                    }
                    //对离线设备进行分组求和
                    var offLineDetails = offLineQuery.GroupBy(p => p.EquipStatus.EquipAccountId).GroupBy(p => p.EquipStatus.EquipAccount.Code).Select(p => new { EquipAccountId = p.EquipStatus.EquipAccountId, EquipAccountCode = p.EquipStatus.EquipAccount.Code, Minute = p.SQL<decimal>("round(SUM(nvl(esd.Minute,0)),4) Minute") }).ToList<EquipStatusDetail>();

                    //对查询范围内的工单按照工序、资源、工单进行分组，合计他们的报工数
                    var woReportQtys = Query<ReportRecordExamine>()
                        .Where(p => d.Value.Contains(p.WorkShop.Code) && processIds.Contains((double)p.ProcessId) && resourceIds.Contains((double)p.ResourceId))
                        .WhereIf(beginTime != null, p => p.ReportTime >= beginTime)
                        .WhereIf(endTime != null, p => p.ReportTime <= endTime)
                        .GroupBy(p => p.Wo)
                        .GroupBy(p => p.ProcessId)
                        .GroupBy(p => p.ResourceId)
                        .GroupBy(p => p.Process.Code)
                        .GroupBy(p => p.Resource.Code)
                        .Select(p => new { p.Wo, p.ProcessId, Process = p.Process.Code, p.ResourceId, Resource = p.Resource.Code, ReportQty = p.ReportQty.SUM(), GoodQty = p.OkQty.SUM() }).ToList<WoReportQty>();

                    var wos = woReportQtys.Select(p => p.Wo).Distinct().ToList();
                    //找出工艺路线信息
                    var layoutInfos = Query<LayoutInfo>().Join<Process>((x, y) => x.ProcessCode == y.Code && processIds.Contains(y.Id)).Where(p => wos.Contains(p.WorkOrder.No)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());

                    foreach (var oee in oees)
                    {
                        oee.NormalTime = runnungDetails.Where(p => p.EquipAccountCode == oee.Resource).Sum(p => (p.Minute ?? 0));
                        oee.AbnormalityTime = offLineDetails.Where(p => p.EquipAccountCode == oee.Resource).Sum(p => (p.Minute ?? 0));
                        var wrqs = woReportQtys.Where(p => p.Process == oee.Process && p.Resource == oee.Resource).ToList();

                        oee.TheoryTime = 0;
                        foreach (var wrq in wrqs)
                        {
                            //把各个库存组织的工单保存下来，因为可能存在跨库存组织报工的情况
                            if (WoReportQtys.Any(p => p.Wo == wrq.Wo && p.Process == wrq.Process && p.Resource == wrq.Resource))
                            {
                                WoReportQtys.Where(p => p.Wo == wrq.Wo && p.Process == wrq.Process && p.Resource == wrq.Resource).ForEach(p =>
                                {
                                    p.ReportQty += wrq.ReportQty;
                                    p.GoodQty += wrq.GoodQty;
                                });
                            }
                            else
                            {
                                WoReportQtys.Add(new WoReportQty
                                {
                                    Wo = wrq.Wo,
                                    ReportQty = wrq.ReportQty,
                                    Process = wrq.Process,
                                    ProcessId = wrq.ProcessId,
                                    GoodQty = wrq.GoodQty,
                                    Resource = wrq.Resource,
                                    ResourceId = wrq.ResourceId,
                                    ProcessQty = layoutInfos.Where(p => p.WorkOrderNo == wrq.Wo).Sum(p => p.ProcessQty == 0 ? 0 : ((p.Vgw03 ?? 0) / p.ProcessQty))
                                });
                            }

                            //oee.TheoryTime += layoutInfos.Where(p => p.WorkOrderNo == wrq.Wo).Sum(p => p.ProcessQty == 0 ? 0 : ((p.Vgw03 ?? 0) / p.ProcessQty)) * wrq.ReportQty;
                        }

                        oee.GoodQty = wrqs.Sum(p => p.GoodQty);
                        oee.TotalQty = wrqs.Sum(p => p.ReportQty);
                        oee.A = oee.PlanTime == 0 ? 0 : oee.NormalTime / oee.PlanTime;
                        oee.P = oee.NormalTime == 0 ? 0 : oee.TotalQty * oee.TheoryTime / oee.NormalTime;
                        oee.Q = oee.TotalQty == 0 ? 0 : oee.GoodQty / oee.TotalQty;
                        oee.Oee = oee.A * oee.P * oee.Q;
                    }
                }
                catch (Exception ex)
                { }
            }
            List<OeeData> datas = new List<OeeData>();

            foreach (var g in oees.GroupBy(p => new { p.Process, p.Resource }))
            {
                OeeData data = new OeeData();

                data.TheoryTime = 0;
                WoReportQtys.Where(p => p.Process == g.Key.Process && p.Resource == g.Key.Resource).ForEach(p =>
                {
                    data.TheoryTime += p.ProcessQty * p.ReportQty;
                });
                data.Process = g.Key.Process;
                data.Resource = g.Key.Resource;
                data.PlanTime = g.FirstOrDefault().PlanTime;
                data.TheoryTime = g.Sum(p => p.TheoryTime);
                data.AbnormalityTime = g.FirstOrDefault().AbnormalityTime;
                data.NormalTime = g.FirstOrDefault().NormalTime;
                data.GoodQty = g.Sum(p => p.GoodQty);
                data.TotalQty = g.Sum(p => p.TotalQty);
                data.A = data.PlanTime == 0 ? 0 : data.NormalTime / data.PlanTime;
                data.P = data.NormalTime == 0 ? 0 : data.TotalQty * data.TheoryTime / data.NormalTime;
                data.Q = data.TotalQty == 0 ? 0 : data.GoodQty / data.TotalQty;
                data.Oee = data.A * data.P * data.Q;

                datas.Add(data);
            }

            return datas;
        }

        #endregion

        #region 生产效率报表

        /// <summary>
        /// 生产效率报表-工厂
        /// </summary>
        /// <param name="factoryCodes"></param>
        /// <param name="mrpControllers"></param>
        /// <param name="process"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        [ApiService("生产效率报表-工厂")]
        [AllowAnonymous]
        public virtual List<ProductionEfficiencyData> GetProductionEfficiencyDatasFactory(List<string> factoryCodes, List<string> mrpControllers, List<string> process, DateTime? beginTime, DateTime? endTime)
        {
            var sqlQuery1 = " 1 = 1";
            var sqlQuery2 = " 1 = 1";
            if (mrpControllers != null && mrpControllers.Count > 0)
            {
                var escapedItems = mrpControllers.Select(s => $"'{s}'");
                sqlQuery1 += $" and re.Code in ({string.Join(",", escapedItems)})";
            }
            if (factoryCodes != null && factoryCodes.Count > 0)
            {
                var escapedItems = factoryCodes.Select(s => $"'{s}'");
                sqlQuery1 += $" and sio.EXTERNAL_ID in ({string.Join(",", escapedItems)})";
                sqlQuery2 += $" and sio.EXTERNAL_ID in ({string.Join(",", escapedItems)})";
            }
            if (process != null && process.Count > 0)
            {
                var processes = process.Select(p => $"'{p}'").ToList();
                sqlQuery1 += $" and tp.code in ({string.Join(",", processes)})";
            }
            if (beginTime != null)
            {
                sqlQuery1 += $" and trr.report_time >= to_date('{beginTime}','yyyy-mm-dd hh24:mi:ss')";
                sqlQuery2 += $" and (oodb.Off_Duty_Time >= to_date('{beginTime}','yyyy-mm-dd hh24:mi:ss') or oodb.On_Duty_Time >= to_date('{beginTime}','yyyy-mm-dd hh24:mi:ss'))";
            }
            if (endTime != null)
            {
                sqlQuery1 += $" and trr.report_time <= to_date('{endTime}','yyyy-mm-dd hh24:mi:ss')";
                sqlQuery2 += $"and (oodb.Off_Duty_Time <= to_date('{endTime}','yyyy-mm-dd hh24:mi:ss') or oodb.On_Duty_Time <= to_date('{endTime}','yyyy-mm-dd hh24:mi:ss'))";
            }

            var sql = $@"with 
cte1 as
(    --先找出要参与计算的报工记录
     SELECT trr.id,nvl(trr.report_qty,0) report_qty,trr.dispatch_task_id,trr.process_id,trr.work_order_id,tp.code process_code
     FROM TM_REPORT_RECORD trr          --报工记录
     inner join tech_process tp on tp.id = trr.process_id and tp.is_phantom = 0             --工序
     inner join tm_disp_task tdt on tdt.id = trr.dispatch_task_id and tdt.is_phantom = 0       --任务单
     inner join RES_ENTERPRISE re on re.id = tdt.work_shop_id and re.is_phantom = 0     --企业模型(车间)
     inner join SYS_INV_ORG sio on sio.code = trr.inv_org_id and sio.is_phantom = 0  --库存组织
     where trr.is_phantom = 0 and {sqlQuery1}
),
cte2 as
(
     --按照工单和工序分组
     select cte1.work_order_id,cte1.process_id,sum(cte1.report_qty) report_qty
     from cte1
     group by cte1.work_order_id,cte1.process_id
),
cte3 as 
(
     --按照任务单和工序去分组
     select cte1.dispatch_task_id,cte1.process_id
     from cte1
     group by cte1.dispatch_task_id,cte1.process_id
),
cte4 as
(    --计算车间产出总人员工时（h）
select sum(nvl(li.Vgw01*cte2.report_qty/li.Process_Qty,0)) qty,li.process_code
from layout_info li                     --工单工艺路线
inner join tech_process tp on tp.code = li.process_code and li.inv_org_id = tp.inv_org_id and tp.is_phantom = 0     --工序
inner join cte2 on cte2.process_id = tp.id and cte2.work_order_id = li.work_order_id               
where li.is_phantom = 0 
group by li.process_code
),
cte5 as
(
    --先按照找出B在岗信息与资源关系，因为生产资源名称可重复，先找出来，否则在下面用派工单的任务执行对象去找的时候会出现重复
    select oodb.id,rws.code,rws.name,
    --当下岗时间为空的时候，就是上岗类型，还没下岗，所以用当前时间-去上岗时间就是在岗时间
    case when oodb.off_duty_time is null then (sysdate - oodb.on_duty_time)*1440 else oodb.on_duty_duration end on_duty_duration 
    from ON_OFF_DUTY_B oodb                 --B在岗信息
    inner join RES_WIP_SCHE rws on rws.id = oodb.resource_id and rws.is_phantom = 0         --资源
    inner join SYS_INV_ORG sio on sio.code = oodb.inv_org_id and sio.is_phantom = 0  --库存组织
    where oodb.is_phantom = 0 and {sqlQuery2}
),
cte6 as
(
     --计算车间出勤人员总工时（h）
    select tp.code process_code,sum(nvl(cte5.on_duty_duration,0))/60 on_duty_duration
    from TM_DISP_TASK tdt                       --任务单
    JOIN JSON_TABLE(
    '[""' || REPLACE(tdt.TASK_PERFORMER, ';', '"",""') || '""]',
    '$[*]' COLUMNS (Code VARCHAR2(100) PATH '$')
    ) jt ON 1=1 and jt.code is not null                         --拆分任务执行对象
    inner join cte3 on cte3.dispatch_task_id = tdt.id           --参与计算的派工单
    inner join cte5 on cte5.name = jt.code                      --和B在岗信息通过资源进行匹配
    inner join tech_process tp on tp.id = tdt.process_id        --工序
    where tdt.is_phantom = 0 and tdt.inv_org_id = 7
    group by tp.code
),
cte7 as
(
    --显示此次计算的工序
     select process_code
     from cte1
     group by process_code
)
select cte7.process_code,round(nvl(cte4.qty,0),4) Putput,round(nvl(cte6.on_duty_duration,0),4) Attendance
from cte7
left join cte4 on cte4.process_code = cte7.process_code
left join cte6 on cte6.process_code = cte7.process_code
";
            List<ProductionEfficiencyData> datas = new List<ProductionEfficiencyData>();

            using (var db = DB.Create("MES"))
            {
                try
                {
                    var dt = db.ExecuteDataTable(sql, CommandType.Text);
                    foreach (DataRow row in dt.Rows)
                    {
                        var processCode = row["process_code"].ToString();
                        var putput = row["Putput"].ToString();
                        var attendance = row["Attendance"].ToString();
                        ProductionEfficiencyData data = new ProductionEfficiencyData();
                        data.Process = processCode;
                        data.Putput = 0;
                        if (!putput.IsNullOrEmpty())
                            data.Putput = Math.Round(Convert.ToDecimal(putput) / 60, 4);
                        data.Attendance = 0;
                        if (!attendance.IsNullOrEmpty())
                            data.Attendance = Convert.ToDecimal(attendance);
                        data.Rate = Math.Round(data.Attendance == 0 ? 0 : data.Putput / data.Attendance * 100, 4);
                        datas.Add(data);
                    }
                }
                catch (Exception e)
                {
                    throw new ValidationException(e.GetBaseException().Message);
                }
            }

            return datas;
        }

        #endregion

        #region 安灯异常统计报表

        /// <summary>
        /// 安灯异常统计报表-工厂
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="resource"></param>
        /// <param name="andonName"></param>
        /// <param name="equipAccountCode"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        [ApiService("安灯异常统计报表-工厂")]
        [AllowAnonymous]
        public virtual List<AndonReportData> GetAndonReportDatasFactory(List<string> factoryCodes, string resource, string andonName, string equipAccountCode, int? state, DateTime? beginTime, DateTime? endTime)
        {
            List<AndonReportData> datas = new List<AndonReportData>();

            var querySql = "1 = 1";
            if (factoryCodes.Count > 0)
            {
                var escapedItems = factoryCodes.Select(s => $"'{s}'");
                querySql += $" and sio.External_Id in ({string.Join(",", escapedItems)})";
            }
            if (!resource.IsNullOrEmpty())
            {
                if (resource.Contains("%"))
                    querySql += $" and (rws.code like '{resource}' or rws.name like '{resource}')";
                else
                    querySql += $" and (rws.code = '{resource}' or rws.name = '{resource}')";
            }
            if (!andonName.IsNullOrEmpty())
            {
                if (andonName.Contains("%"))
                    querySql += $" and ma.andon_Name like '{andonName}'";
                else
                    querySql += $" and ma.andon_Name = '{andonName}'";
            }
            if (!equipAccountCode.IsNullOrEmpty())
            {
                if (equipAccountCode.Contains("%"))
                    querySql += $" and eea.code like '{equipAccountCode}'";
                else
                    querySql += $" and eea.code = '{equipAccountCode}'";
            }
            if (state != null)
            {
                querySql += $" and am.state = {state}";
            }
            if (beginTime != null)
            {
                querySql += $" and am.Fault_Time >= to_date('{beginTime}','yyyy-mm-dd hh24:mi:ss')";
            }
            if (endTime != null)
            {
                querySql += $" and am.Fault_Time <= to_date('{endTime}','yyyy-mm-dd hh24:mi:ss')";
            }
            var sql = $@"
                with
--先将安灯责任维护基础表中的人名用/拼接好，方便下面可以直接匹配编码
andonGroup as
(
  select ag.code,ag.inv_org_id,TRIM(TRAILING '/' FROM TO_CHAR((select xmlagg(xmlparse(content re.name || '/' wellformed) order by agd.is_responser).getclobval() from ANDON_GROUP_DTL agd 
  inner join SYS_USER su on su.id = agd.user_id and su.is_phantom = 0 
  inner join RES_EMP re on re.id = su.employee_id and re.is_phantom = 0
  where agd.is_phantom = 0 and agd.andon_group_id = ag.id))) levelname
  from ANDON_GROUP ag
    --判断必须有明细才能拼接人名
  where exists(select 1 from ANDON_GROUP_DTL agd inner join SYS_USER su on su.id = agd.user_id and su.is_phantom = 0 inner join RES_EMP re on re.id = su.employee_id and re.is_phantom = 0 where agd.is_phantom = 0 and agd.andon_group_id = ag.id) and ag.is_phantom = 0
),
--获取安灯管理的操作明细，对库存组织、安灯管理Id、操作类型进行分组，然后去每组第一条，方便后面找出他的每一组的操作时间去计算
mao as (
select *
from (select mao.inv_org_id,mao.andon_manage_id,mao.operate_type,mao.Operate_Time,row_number() over (partition by mao.inv_org_id,mao.andon_manage_id,mao.operate_type order by mao.create_date desc) as row_num
from MES_ANDONMANAGEOPERATELOG mao)
where row_num = 1
)
SELECT sio.name Factory,rws.code ResourceCode,eea.code EquipAccountCode,ma.andon_name AndonName,am.problem_desc ProblemDesc,am.Fault_Time FaultTime
,am.Last_Time LastTime,nvl(lv4.levelname,'') lv4,nvl(lv3.levelname,'') lv3,nvl(lv2.levelname,'') lv2,nvl(lv1.levelname,'') lv1,ROUND((mao1.Operate_Time - am.Fault_Time) * 24, 2) AS ResponseTime,
ROUND((mao3.Operate_Time - am.Fault_Time) * 24, 2) AS HandleTime,am.state
FROM MES_ANDONMANAGE am         --安灯管理
inner join SYS_INV_ORG sio on sio.code = am.inv_org_id and sio.is_phantom = 0               --库存组织
inner join RES_WIP_SCHE rws on rws.id = am.wip_resource_id and rws.is_phantom = 0 and rws.inv_org_id = am.inv_org_id          --生产资源
left join EMS_EQUIP_ACCOUNT eea on eea.id = am.equip_account_id and eea.is_phantom = 0 and eea.inv_org_id = am.inv_org_id     --设备台账
inner join MES_ANDON ma on ma.id = am.andon_id and ma.is_phantom = 0 and ma.inv_org_id = am.inv_org_id                       --安灯维护
left join ANDON_LINE al on al.machine_code = rws.code and al.is_phantom = 0 and al.inv_org_id = am.inv_org_id                --产线与安灯区域
left join andonGroup lv4 on lv4.code = al.andon_code||ma.andon_name||'LV4' and lv4.inv_org_id = am.inv_org_id   --获取LV4的人名
left join andonGroup lv3 on lv3.code = al.andon_code||ma.andon_name||'LV3' and lv3.inv_org_id = am.inv_org_id   --获取LV3的人名
left join andonGroup lv2 on lv2.code = al.andon_code||ma.andon_name||'LV2' and lv2.inv_org_id = am.inv_org_id   --获取LV2的人名
left join andonGroup lv1 on lv1.code = al.andon_code||ma.andon_name||'LV1' and lv1.inv_org_id = am.inv_org_id   --获取LV1的人名
left join mao mao1 on mao1.andon_manage_id = am.id and mao1.inv_org_id = am.inv_org_id and mao1.operate_type = 1    --获取操作时间
left join mao mao3 on mao3.andon_manage_id = am.id and mao3.inv_org_id = am.inv_org_id and mao3.operate_type = 3    --获取操作时间
where {querySql}
";
            using (var db = DB.Create("MES"))
            {
                try
                {
                    var dt = db.ExecuteDataTable(sql, CommandType.Text);
                    foreach (DataRow row in dt.Rows)
                    {
                        var Factory = row["Factory"].ToString();
                        var Resource = row["ResourceCode"].ToString();
                        var EquipAccountCode = row["EquipAccountCode"].ToString();
                        var AndonName = row["AndonName"].ToString();
                        var ProblemDesc = row["ProblemDesc"].ToString();
                        var FaultTime = row["FaultTime"].ToString();
                        var LastTime = row["LastTime"].ToString();
                        var lv4 = row["lv4"].ToString();
                        var lv3 = row["lv3"].ToString();
                        var lv2 = row["lv2"].ToString();
                        var lv1 = row["lv1"].ToString();
                        var ResponseTime = row["ResponseTime"].ToString();
                        var HandleTime = row["HandleTime"].ToString();
                        var State = row["state"].ToString();

                        AndonReportData data = new AndonReportData();
                        data.Factory = Factory;
                        data.Resource = Resource;
                        data.EquipAccountCode = EquipAccountCode;
                        data.AndonName = AndonName;
                        data.ProblemDesc = ProblemDesc;
                        data.FaultTime = FaultTime;
                        data.LastTime = LastTime;
                        data.Level4 = lv4.TrimEnd('/');
                        data.Level3 = lv3.TrimEnd('/');
                        data.Level2 = lv2.TrimEnd('/');
                        data.Level1 = lv1.TrimEnd('/');
                        data.ResponseTime = ResponseTime;
                        data.HandleTime = HandleTime;
                        if (!State.IsNullOrEmpty())
                        {
                            data.State = ((AndonManageState)Convert.ToInt32(State)).ToLabel();
                        }
                        datas.Add(data);
                    }
                }
                catch (Exception e)
                {
                    throw new ValidationException(e.GetBaseException().Message);
                }
            }
            return datas;
        }

        /// <summary>
        /// 安灯异常统计报表柱形图-工厂
        /// </summary>
        /// <param name="factoryCodes"></param>
        /// <param name="resource"></param>
        /// <param name="andonName"></param>
        /// <param name="equipAccountCode"></param>
        /// <param name="state"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        [ApiService("安灯异常统计报表柱形图-工厂")]
        [AllowAnonymous]
        public virtual List<AndonReportBarChartData> GetAndonReportBarChartDatasFactory(List<string> factoryCodes, string resource, string andonName, string equipAccountCode, int? state, DateTime? beginTime, DateTime? endTime)
        {
            List<AndonReportBarChartData> datas = new List<AndonReportBarChartData>();

            using (InvOrgs.WithAll())
            {
                var q = DB.Query<AndonManage>("am")
                    .Join<Rbac.InvOrgs.InvOrg>("org", (x, y) => x.SQL<int>("am.Inv_Org_Id") == y.Code)
                    //只找待响应、待验收、处理中数据
                    .Where(p => p.State == AndonManageState.Standby || p.State == AndonManageState.Processing || p.State == AndonManageState.ToAccepted);
                if (state != null)
                    q.Where(p => p.State == (AndonManageState)(int)state);
                if (!resource.IsNullOrEmpty())
                    q.Where(p => p.WipResource.Code.Contains(resource) || p.WipResource.Name.Contains(resource));
                if (!andonName.IsNullOrEmpty())
                    q.Where(p => p.Andon.AndonName.Contains(andonName));
                if (!equipAccountCode.IsNullOrEmpty())
                    q.Where(p => p.EquipAccount.Code.Contains(equipAccountCode));
                if (beginTime != null)
                    q.Where(p => p.FaultTime >= beginTime);
                if (endTime != null)
                    q.Where(p => p.FaultTime <= endTime);
                if (factoryCodes.Count > 0)
                    q.Where(p => factoryCodes.Contains(p.SQL<string>("org.External_Id")));

                var factoryDatas = q.GroupBy(p => p.State).Select(p => new { State = p.State, Qty = p.SQL<decimal>("count(1) Qty") }).ToList<AndonReportBarChartDataFactory>().ToList();

                if (factoryDatas.Count > 0)
                {
                    AndonReportBarChartData data = new AndonReportBarChartData();

                    data.Standby = factoryDatas.Where(p => p.State == AndonManageState.Standby).FirstOrDefault()?.Qty ?? 0;
                    data.Processing = factoryDatas.Where(p => p.State == AndonManageState.Processing).FirstOrDefault()?.Qty ?? 0;
                    data.ToAccepted = factoryDatas.Where(p => p.State == AndonManageState.ToAccepted).FirstOrDefault()?.Qty ?? 0;

                    datas.Add(data);
                }
            }

            return datas;
        }

        #endregion

        #region 可疑品处理报表

        /// <summary>
        /// 可疑品处理报表-工厂
        /// </summary>
        /// <param name="factoryCodes"></param>
        /// <param name="mrpControllers"></param>
        /// <param name="process"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        [ApiService("可疑品处理报表-工厂")]
        [AllowAnonymous]
        public virtual List<SuspectReportData> GetSuspectReportDatasFactory(List<string> factoryCodes, List<string> mrpControllers, List<string> process, DateTime? beginTime, DateTime? endTime)
        {
            List<SuspectReportData> datas = new List<SuspectReportData>();

            using (InvOrgs.WithAll())
            {
                var q = DB.Query<ReportRecord>("rr")
                    .Join<Rbac.InvOrgs.InvOrg>("org", (x, y) => x.SQL<int>("rr.Inv_Org_Id") == y.Code)
                    .Join<DispatchTask>((x, y) => x.DispatchTaskId == y.Id);
                q.WhereIf(beginTime != null, p => p.ReportTime >= beginTime);
                q.WhereIf(endTime != null, p => p.ReportTime <= endTime);
                if (factoryCodes.Count > 0)
                {
                    q.Where(p => factoryCodes.Contains(p.SQL<string>("org.External_Id")));
                }
                q.WhereIf(mrpControllers.Count > 0, p => mrpControllers.Contains(p.WorkOrder.WorkShop.Code));
                q.WhereIf(process != null && process.Count > 0, p => process.Contains(p.Process.Code));
                //先在报工记录中按照工序分组，计算总量、报废总量
                datas = q.GroupBy(p => p.Process.Code).Select(p => new { Process = p.Process.Code, TotalQty = p.ReportQty.SUM(), TotalNgQty = p.NgQty.SUM() }).ToList<SuspectReportData>().ToList();

                //再单独计算可疑品数
                //同样按照工序分组
                var qSuppect = DB.Query<SuspectProductLabel>("spl")
                               .Join<Rbac.InvOrgs.InvOrg>("org", (x, y) => x.SQL<int>("spl.Inv_Org_Id") == y.Code);
                qSuppect.WhereIf(beginTime != null, p => p.CreateDate >= beginTime);
                qSuppect.WhereIf(endTime != null, p => p.CreateDate <= endTime);
                if (factoryCodes.Count > 0)
                {
                    q.Where(p => factoryCodes.Contains(p.SQL<string>("org.External_Id")));
                }
                qSuppect.WhereIf(mrpControllers.Count > 0, p => mrpControllers.Contains(p.WorkOrder.WorkShop.Code));
                qSuppect.WhereIf(process != null && process.Count > 0, p => process.Contains(p.Process.Code));
                var rSuppect = qSuppect.GroupBy(p => p.Process.Code).Select(p => new { Process = p.Process.Code, TotalSuspectQty = p.Qty.SUM() }).ToList<SuspectReportData>().ToList();

                //然后再以result为主，将两个集合的数量合在一起
                foreach (var r in datas)
                {
                    r.TotalSuspectQty = rSuppect.Where(p => p.Process == r.Process).Sum(p => p.TotalSuspectQty);
                }

            }

            return datas;
        }

        /// <summary>
        /// 缺陷报表-工厂
        /// </summary>
        /// <param name="factoryCodes"></param>
        /// <param name="mrpControllers"></param>
        /// <param name="process"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        [ApiService("缺陷报表-工厂")]
        [AllowAnonymous]
        public virtual List<SuspectDefectData> GetSuspectDefectDatasFactory(List<string> factoryCodes, List<string> mrpControllers, List<string> process, DateTime? beginTime, DateTime? endTime)
        {
            List<SuspectDefectData> datas = new List<SuspectDefectData>();

            using (InvOrgs.WithAll())
            {
                var q = DB.Query<Defect>("d")
                .Join<SuspectProductLabelDetail>("spld", (x, y) => x.Id == y.DefectId)
                .Join<SuspectProductLabelDetail, SuspectProductLabel>((x, y) => x.SuspectProductLabelId == y.Id)
                .Join<Rbac.InvOrgs.InvOrg>("org", (x, y) => x.SQL<int>("d.Inv_Org_Id") == y.Code);

                if (beginTime != null)
                {
                    q.Where<SuspectProductLabel>((d, spl) => spl.CreateDate >= beginTime);
                }
                if (endTime != null)
                {
                    q.Where<SuspectProductLabel>((d, spl) => spl.CreateDate <= endTime);
                }
                if (factoryCodes.Count > 0)
                {
                    q.Where(p => factoryCodes.Contains(p.SQL<string>("org.External_Id")));
                }
                if (mrpControllers.Count > 0)
                {
                    q.Where<SuspectProductLabel>((d, spl) => mrpControllers.Contains(spl.WorkOrder.WorkShop.Code));
                }
                if (process != null && process.Count > 0)
                {
                    q.Where<SuspectProductLabel>((d, spl) => process.Contains(spl.Process.Code));
                }
                datas = q.GroupBy(p => p.Code).GroupBy(p => p.Description).Select(p => new { DefectCode = p.Code, DefectName = p.Description, Qty = p.SQL<decimal>("Sum(nvl(spld.Qty,0)) Qty") }).ToList<SuspectDefectData>().ToList();
            }

            return datas;
        }

        #endregion

        #region 产品直通率报表

        /// <summary>
        /// 物料平衡报表-工厂
        /// </summary>
        /// <param name="factoryCodes"></param>
        /// <param name="mrpControllers"></param>
        /// <param name="itemType"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        [ApiService("产品直通率报表-工厂")]
        [AllowAnonymous]
        public virtual List<ProductFirstPassYieldFactoryData> GetProductFirstPassYieldDatasFactory(List<string> factoryCodes, List<string> mrpControllers, string product, DateTime? beginTime, DateTime? endTime)
        {
            List<ProductFirstPassYieldFactoryData> datas = new List<ProductFirstPassYieldFactoryData>();

            using (InvOrgs.WithAll())
            {
                var q = DB.Query<WorkOrder>("wo")
                    .Join<Enterprise>("e", (x, y) => x.WorkShopId == y.Id)
                    .Join<Item>("i", (x, y) => x.ProductId == y.Id)
                    .Join<Rbac.InvOrgs.InvOrg>("org", (x, y) => x.SQL<int>("wo.Inv_Org_Id") == y.Code);
                if (factoryCodes.Count > 0)
                {
                    q.Where(p => factoryCodes.Contains(p.SQL<string>("org.External_Id")));
                }
                if (mrpControllers.Count > 0)
                {
                    q.Where(p => mrpControllers.Contains(p.WorkShop.Code));
                }
                if (!product.IsNullOrEmpty())
                {
                    q.Where(p => p.Product.Code.Contains(product));
                }
                q.Exists<ReportRecord>((x, y) => y.Where(p => p.WorkOrderId == x.Id).WhereIf(beginTime != null, p => p.ReportTime >= beginTime).WhereIf(endTime != null, p => p.ReportTime <= endTime));
                //要对车间和物料分组,后面需要用这个进行查询直通率
                var result = q.GroupBy(p => p.Product.Code).GroupBy(p => p.SQL<string>("org.External_Id")).GroupBy(p => p.WorkShop.Code).Select(p => new { Product = p.Product.Code, Inv_Org_Id = p.SQL<string>("org.External_Id Inv_Org_Id"), WorkShopCode = p.WorkShop.Code }).ToList<ProductFirstPassYieldFactoryData>().ToList();

                //将查出来的按照库存组织去分，这样就只会查询3次
                foreach (var g in result.GroupBy(p => p.Inv_Org_Id))
                {
                    //调用明细的接口，获取他们的投料量和可疑品数
                    var dtlDatas = GetProductFirstPassYieldDtlDatasFactory(new List<string>() { g.Key }, g.Select(p => p.WorkShopCode).ToList(), g.Select(p => p.Product).ToList(), beginTime, endTime);
                    //根据车间和物料去区分
                    foreach (var g1 in g.GroupBy(p => p.Product))
                    {
                        ProductFirstPassYieldFactoryData r = new ProductFirstPassYieldFactoryData();
                        r.Inv_Org_Id = g.Key;
                        r.Product = g1.Key;
                        r.datas = dtlDatas.Where(p => p.ProductCode == g1.Key).ToList();
                        datas.Add(r);
                    }
                }
            }

            return datas;
        }

        /// <summary>
        /// 物料平衡报表明细-工厂
        /// </summary>
        /// <param name="factoryCodes"></param>
        /// <param name="mrpControllers"></param>
        /// <param name="product"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTim"></param>
        /// <returns></returns>
        [ApiService("物料平衡报表明细-工厂")]
        [AllowAnonymous]
        public virtual List<ProductFirstPassYieldDtlData> GetProductFirstPassYieldDtlDatasFactory(List<string> factoryCodes, List<string> mrpControllers, List<string> products, DateTime? beginTime, DateTime? endTime)
        {
            List<ProductFirstPassYieldDtlData> datas = new List<ProductFirstPassYieldDtlData>();

            using (InvOrgs.WithAll())
            {
                datas = DB.Query<ReportRecord>("rr")
                    .Join<Rbac.InvOrgs.InvOrg>("org", (x, y) => x.SQL<int>("rr.Inv_Org_Id") == y.Code && factoryCodes.Contains(y.ExternalId))
                    .Where(p => mrpControllers.Contains(p.DispatchTask.WorkOrder.WorkShop.Code) && products.Contains(p.DispatchTask.WorkOrder.Product.Code))
                    .WhereIf(beginTime != null, p => p.ReportTime >= beginTime)
                    .WhereIf(endTime != null, p => p.ReportTime <= endTime)
                    .GroupBy(p => p.Process.Code)
                    .GroupBy(p => p.DispatchTask.WorkOrder.Product.Code)
                    .Select(p => new { Process = p.Process.Code, FeedingQty = p.SQL<decimal>("sum(nvl(rr.Report_Qty,0) + nvl(rr.Suspect_Qty,0)) FeedingQty"), SuspectQty = p.SQL<decimal>("sum(nvl(rr.Suspect_Qty,0)) SuspectQty"), ProductCode = p.DispatchTask.WorkOrder.Product.Code })
                    .ToList<ProductFirstPassYieldDtlData>().ToList();
            }
            return datas;
        }

        #endregion

        #region 物料平衡报表

        [ApiService("物料平衡报表-工厂")]
        [AllowAnonymous]
        public virtual List<ItemBalanceData> GetItemBalanceDatasFactory(List<string> factoryCodes, List<string> mrpControllers, string itemType, DateTime? beginTime, DateTime? endTime)
        {

            List<ItemBalanceData> datas = new List<ItemBalanceData>();

            string query = " 1 = 1";
            var can4Query = " 1 = 1";

            if (mrpControllers != null && mrpControllers.Count > 0)
            {
                var escapedItems = mrpControllers.Select(s => $"'{s}'");
                query += $" and t1.Work_Shop_Code in ({string.Join(",", escapedItems)})";
                can4Query += $" and t5.Code in ({string.Join(",", escapedItems)})";
            }
            if (beginTime != null)
            {
                query += $" and t1.report_time >= to_date('{beginTime}','yyyy-mm-dd hh24:mi:ss')";
                can4Query += $" and DEDUCTION_RECORD.Create_Date >= to_date('{beginTime}','yyyy-mm-dd hh24:mi:ss')";
            }
            if (endTime != null)
            {
                query += $" and t1.report_time <= to_date('{endTime}','yyyy-mm-dd hh24:mi:ss')";
                can4Query += $" and DEDUCTION_RECORD.Create_Date <= to_date('{endTime}','yyyy-mm-dd hh24:mi:ss')";
            }
            if (!itemType.IsNullOrEmpty() && itemType != "全部")
                query += $" and t1.Product_Name like '%{itemType}%'";
            if (factoryCodes != null && factoryCodes.Count > 0)
            {
                var escapedItems = factoryCodes.Select(s => $"'{s}'");
                query += $" and t1.Factory in ({string.Join(",", escapedItems)})";
                can4Query += $" and t3.external_id in ({string.Join(",", escapedItems)})";
            }

            //以下每个字段再SQL中都是独立计算的，一块计算一个字段内容
            string sql = $@"
                                    with 
            can1 as ( SELECT t1.Wo,t1.Factory,--t1.Work_Shop_Code,
                CASE
                    WHEN t1.Product_Name LIKE '%铜%' THEN '铜'
                    WHEN t1.Product_Name LIKE '%铝%' THEN '铝'
                END as item_type
            FROM FACTORY_REPORT_RECORD_V t1
            where {query} 
            GROUP BY --t1.Work_Shop_Code,
                t1.Wo,t1.Factory,
                CASE
                    WHEN t1.Product_Name LIKE '%铜%' THEN '铜'
                    WHEN t1.Product_Name LIKE '%铝%' THEN '铝'
                END
                ), 
            --通过扣料记录的创建时间，获取到对应的标签，然后再通过这些标签再去上料记录查找
            w1 as (
    select DEDUCTION_RECORD.Feeding_Item_Label,DEDUCTION_RECORD.Inv_Org_Id
    from DEDUCTION_RECORD
    inner join TM_REPORT_RECORD t1 on t1.Is_Phantom = 0 and t1.id = DEDUCTION_RECORD.Report_Record_Id
    inner join wo on wo.is_phantom = 0 and wo.id =  t1.Work_Order_Id
    inner join RES_ENTERPRISE t5 on t5.id = wo.work_shop_id and t5.is_phantom = 0  --车间
    inner join SYS_INV_ORG t3 on t3.code = DEDUCTION_RECORD.inv_org_id and t3.is_phantom = 0
    where DEDUCTION_RECORD.Is_Phantom = 0 and {can4Query}
    group by DEDUCTION_RECORD.Feeding_Item_Label,DEDUCTION_RECORD.Inv_Org_Id
    ),
            can2 as (                               --获取取样净重详情,计算产出用量
            select 
            (select t1.Finish_Qty * t1.Weight from FAC_WEIGHT_OF_SAMP_REPORT_V t1 where t1.Wo_No = t0.Wo and t1.Factory = t0.Factory and rownum = 1) ProductQty,t0.wo
            ,t0.Factory,--t0.Work_Shop_Code,
                CASE
                    WHEN t1.Product_Name LIKE '%铜%' THEN '铜'
                    WHEN t1.Product_Name LIKE '%铝%' THEN '铝'
                END as item_type
            from can1 t0
            inner join FAC_WEIGHT_OF_SAMP_REPORT_V t1 on  t1.Wo_No = t0.Wo and t1.Factory = t0.Factory
            where (t1.Product_Name LIKE '%铜%' or t1.Product_Name LIKE '%铝%')
            group by t0.wo,t0.factory,--t0.Work_Shop_Code,
                CASE
                    WHEN t1.Product_Name LIKE '%铜%' THEN '铜'
                    WHEN t1.Product_Name LIKE '%铝%' THEN '铝'
                END
            ),
            can3 as                      --工单BOM,计算产出用量
            (
            select
                     SUM(
                    CASE 
                        WHEN t1.Bwart = '261' THEN t1.Single_Qty * t1.Finish_Qty  -- 261的A2正常累加
                        WHEN t1.Bwart = '531' THEN -t1.Single_Qty * t1.Finish_Qty -- 531的A2按负数累加（等价于相减）
                        ELSE 0                   -- 其他A1值不参与计算
                    END
                ) AS ProductQty,
                    CASE
                    WHEN t1.item_Name LIKE '%铜%' THEN '铜'
                    WHEN t1.item_Name LIKE '%铝%' THEN '铝'
                END as item_type
            from can1 t0
            inner join FACTORY_WO_BOM_V t1 on t1.Wo_no = t0.wo and t1.Factory = t0.Factory
            where not exists(select 1 from FAC_WEIGHT_OF_SAMP_REPORT_V t1 where t1.Wo_No = t0.Wo and t1.Factory = t0.Factory) and (t1.item_Name LIKE '%铜%' or t1.item_Name LIKE '%铝%')
            group by 
                CASE
                    WHEN t1.item_Name LIKE '%铜%' THEN '铜'
                    WHEN t1.item_Name LIKE '%铝%' THEN '铝'
                END
            ),
            can4 as      --计算出投料量,计算余料量
            (
                   select sum(nvl(FEEDING_RECORD.Feeding_Qty,0)) feedingQty,sum(nvl(FEEDING_RECORD.Remaining_Qty,0)) Remaining_Qty
                    ,CASE
        WHEN t1.name LIKE '%铜%' THEN '铜'
        WHEN t1.name LIKE '%铝%' THEN '铝'
    END as item_type
    from FEEDING_RECORD
    inner join w1 on w1.Feeding_Item_Label = FEEDING_RECORD.Feeding_Item_Label and w1.Inv_Org_Id = FEEDING_RECORD.Inv_Org_Id
    inner join item t1 on t1.id = FEEDING_RECORD.item_id and t1.is_phantom = 0
    inner join SYS_INV_ORG t3 on t3.code = FEEDING_RECORD.inv_org_id and t3.is_phantom = 0
    where FEEDING_RECORD.Is_Phantom = 0 and (t1.name LIKE '%铜%' or t1.name LIKE '%铝%')
    group by 
    CASE
        WHEN t1.name LIKE '%铜%' THEN '铜'
        WHEN t1.name LIKE '%铝%' THEN '铝'
    END
            ),
            can5 as                 --联/副产品入库
            (
                 select sum(nvl(v1.qty,0)) Output_Product_Qty,
                          CASE
                    WHEN v1.item_Name LIKE '%铜%' THEN '铜'
                    WHEN v1.item_Name LIKE '%铝%' THEN '铝'
                END as item_type
                 from FACTORY_OUTPUT_PRO_REC_V v1
                 inner join can1 t1 on v1.Work_Order_No = t1.Wo and t1.Factory = v1.factory
                 where (v1.item_Name LIKE '%铜%' or v1.item_Name LIKE '%铝%')
                 group by 
                 CASE
                    WHEN v1.item_Name LIKE '%铜%' THEN '铜'
                    WHEN v1.item_Name LIKE '%铝%' THEN '铝'
                END
            )
            select t1.item_type,t2.ProductQty ProductQty2,t3.ProductQty ProductQty3,t4.feedingQty,t4.Remaining_Qty,t5.Output_Product_Qty
            --构建一个只有铜和铝两行数据，后面可以根据这两行显示数据
            from (SELECT CASE LEVEL WHEN 1 THEN '铜' WHEN 2 THEN '铝' END AS item_type FROM dual CONNECT BY LEVEL <= 2) t1
            left join (select sum(ProductQty) ProductQty,item_type from can2 group by item_type) t2 on t2.item_type = t1.item_type --can2 t2 on t2.item_type = t1.item_type 
            left join can3 t3 on t3.item_type = t1.item_type 
            left join can4 t4 on t4.item_type = t1.item_type 
            left join can5 t5 on t5.item_type = t1.item_type 
                                    ";

            //            string sql = $@"
            //                        with 
            //can1 as ( SELECT t1.Wo,t1.Factory,--t1.Work_Shop_Code,
            //    CASE
            //        WHEN t1.Product_Name LIKE '%铜%' THEN '铜'
            //        WHEN t1.Product_Name LIKE '%铝%' THEN '铝'
            //    END as item_type
            //FROM FACTORY_REPORT_RECORD_V t1
            //where (t1.Product_Name LIKE '%铜%' or t1.Product_Name LIKE '%铝%') and {query} 
            //GROUP BY --t1.Work_Shop_Code,
            //    t1.Wo,t1.Factory,
            //    CASE
            //        WHEN t1.Product_Name LIKE '%铜%' THEN '铜'
            //        WHEN t1.Product_Name LIKE '%铝%' THEN '铝'
            //    END
            //    ), 
            //can2 as (                               --获取取样净重详情,计算产出用量
            //select 
            //(select t1.Finish_Qty * t1.Weight from FAC_WEIGHT_OF_SAMP_REPORT_V t1 where t1.Wo_No = t0.Wo and t1.Factory = t0.Factory and rownum = 1) ProductQty,t0.wo
            //,t0.Factory,--t0.Work_Shop_Code,
            //    CASE
            //        WHEN t1.Product_Name LIKE '%铜%' THEN '铜'
            //        WHEN t1.Product_Name LIKE '%铝%' THEN '铝'
            //    END as item_type
            //from can1 t0
            //inner join FAC_WEIGHT_OF_SAMP_REPORT_V t1 on  t1.Wo_No = t0.Wo and t1.Factory = t0.Factory
            //where (t1.Product_Name LIKE '%铜%' or t1.Product_Name LIKE '%铝%')
            //group by t0.wo,t0.factory,--t0.Work_Shop_Code,
            //    CASE
            //        WHEN t1.Product_Name LIKE '%铜%' THEN '铜'
            //        WHEN t1.Product_Name LIKE '%铝%' THEN '铝'
            //    END
            //),
            //can3 as                      --工单BOM,计算产出用量
            //(
            //select
            //         SUM(
            //        CASE 
            //            WHEN t1.Bwart = '261' THEN t1.Single_Qty * t1.Finish_Qty  -- 261的A2正常累加
            //            WHEN t1.Bwart = '531' THEN -t1.Single_Qty * t1.Finish_Qty -- 531的A2按负数累加（等价于相减）
            //            ELSE 0                   -- 其他A1值不参与计算
            //        END
            //    ) AS ProductQty,t0.wo,t0.Factory,
            //        CASE
            //        WHEN t1.item_Name LIKE '%铜%' THEN '铜'
            //        WHEN t1.item_Name LIKE '%铝%' THEN '铝'
            //    END as item_type
            //    --,t0.Work_Shop_Code
            //from can1 t0
            //inner join FACTORY_WO_BOM_V t1 on t1.Wo_no = t0.wo and t1.Factory = t0.Factory
            //where not exists(select 1 from FAC_WEIGHT_OF_SAMP_REPORT_V t1 where t1.Wo_No = t0.Wo and t1.Factory = t0.Factory) and (t1.item_Name LIKE '%铜%' or t1.item_Name LIKE '%铝%')
            //group by t0.wo,t0.Factory,--t0.Work_Shop_Code,
            //    CASE
            //        WHEN t1.item_Name LIKE '%铜%' THEN '铜'
            //        WHEN t1.item_Name LIKE '%铝%' THEN '铝'
            //    END
            //),
            //can4 as      --计算出投料量,计算余料量
            //(
            //     select sum(nvl(v1.Feeding_Qty,0)) feedingQty,sum(nvl(v1.Remaining_Qty,0)) Remaining_Qty,t1.wo,t1.Factory,--t1.Work_Shop_Code,
            //         CASE
            //        WHEN v1.item_Name LIKE '%铜%' THEN '铜'
            //        WHEN v1.item_Name LIKE '%铝%' THEN '铝'
            //    END as item_type
            //     from Factory_Wo_FEEDINGQty_V v1
            //     inner join can1 t1 on v1.WoNo = t1.wo and v1.factory = t1.Factory
            //where (v1.item_Name LIKE '%铜%' or v1.item_Name LIKE '%铝%')
            //     group by t1.wo,t1.Factory,--t1.Work_Shop_Code,
            //         -- GROUP BY需和SELECT的CASE判断完全一致
            //    CASE
            //        WHEN v1.item_Name LIKE '%铜%' THEN '铜'
            //        WHEN v1.item_Name LIKE '%铝%' THEN '铝'
            //    END
            //),
            //can5 as                 --联/副产品入库
            //(
            //     select sum(nvl(v1.qty,0)) Output_Product_Qty,t1.wo,t1.Factory,--t1.Work_Shop_Code,
            //              CASE
            //        WHEN v1.item_Name LIKE '%铜%' THEN '铜'
            //        WHEN v1.item_Name LIKE '%铝%' THEN '铝'
            //    END as item_type
            //     from FACTORY_OUTPUT_PRO_REC_V v1
            //     inner join can1 t1 on v1.Work_Order_No = t1.Wo and t1.Factory = v1.factory
            //     where (v1.item_Name LIKE '%铜%' or v1.item_Name LIKE '%铝%')
            //     group by          t1.wo,t1.Factory,--t1.Work_Shop_Code,
            //     CASE
            //        WHEN v1.item_Name LIKE '%铜%' THEN '铜'
            //        WHEN v1.item_Name LIKE '%铝%' THEN '铝'
            //    END
            //)
            //select t1.item_type,t1.wo,t2.ProductQty ProductQty2,t3.ProductQty ProductQty3,t4.feedingQty,t4.Remaining_Qty,t5.Output_Product_Qty,t1.factory--,t1.Work_Shop_Code
            //from can1 t1
            //left join can2 t2 on t2.wo = t1.wo and t2.item_type = t1.item_type --and t2.Work_Shop_Code = t1.Work_Shop_Code
            //left join can3 t3 on t3.wo = t1.wo and t3.item_type = t1.item_type --and t3.Work_Shop_Code = t1.Work_Shop_Code
            //left join can4 t4 on t4.wo = t1.wo and t4.item_type = t1.item_type --and t4.Work_Shop_Code = t1.Work_Shop_Code
            //left join can5 t5 on t5.wo = t1.wo and t5.item_type = t1.item_type --and t5.Work_Shop_Code = t1.Work_Shop_Code
            //                        ";
            List<ItemBalanceData> list = new List<ItemBalanceData>();
            using (var db = DB.Create("MES"))
            {
                try
                {
                    var dt = db.ExecuteDataTable(sql, CommandType.Text);
                    foreach (DataRow row in dt.Rows)
                    {
                        //var productLine = row["Product_Line"].ToString();
                        //var plantName = row["Plant_Name"].ToString();
                        //var workShopCode = row["Work_Shop_Code"].ToString();
                        var iType = row["item_type"].ToString();
                        var productQty2 = row["ProductQty2"].ToString();
                        var productQty3 = row["ProductQty3"].ToString();
                        var feedingQty = row["feedingQty"].ToString();
                        var remainingQty = row["Remaining_Qty"].ToString();
                        var outputProductQty = row["Output_Product_Qty"].ToString();

                        //var factory = row["factory"].ToString();
                        ItemBalanceData data = new ItemBalanceData();

                        //data.ProductLine = productLine;
                        //data.Department = plantName;
                        //data.FactoryCode = factory;
                        //data.WorkShopCode = workShopCode;
                        data.ItemType = iType;
                        decimal productQty = 0;
                        if (!productQty2.IsNullOrEmpty())
                            productQty = Convert.ToDecimal(productQty2);
                        else if (!productQty3.IsNullOrEmpty())
                            productQty = Convert.ToDecimal(productQty3);
                        data.ProductQty = productQty;
                        data.FeedingQty = feedingQty.IsNullOrEmpty() ? 0 : Convert.ToDecimal(feedingQty);
                        data.RemainingQty = remainingQty.IsNullOrEmpty() ? 0 : Convert.ToDecimal(remainingQty);
                        data.OutputProductQty = outputProductQty.IsNullOrEmpty() ? 0 : Convert.ToDecimal(outputProductQty);
                        data.DiffQty = data.FeedingQty - data.ProductQty - data.OutputProductQty - data.RemainingQty;
                        data.Rate = data.FeedingQty == 0 ? 0 : Math.Round(data.DiffQty / data.FeedingQty * 100, 2);
                        list.Add(data);
                    }
                }
                catch (Exception e)
                {
                    throw new ValidationException(e.GetBaseException().Message);
                }
            }

            var dic = list.GroupBy(p => new { p.ItemType }).ToDictionary(p => p.Key, p => p.ToList());
            foreach (var d in dic)
            {
                ItemBalanceData data = new ItemBalanceData();

                //data.ProductLine = d.Key.ProductLine;
                //data.Department = d.Key.Department;
                //data.FactoryCode = d.Key.FactoryCode;
                //data.WorkShopCode = d.Key.WorkShopCode;
                data.ItemType = d.Key.ItemType;
                data.ProductQty = d.Value.Sum(p => p.ProductQty);
                data.FeedingQty = d.Value.Sum(p => p.FeedingQty);
                data.RemainingQty = d.Value.Sum(p => p.RemainingQty);
                data.OutputProductQty = d.Value.Sum(p => p.OutputProductQty);
                data.DiffQty = data.FeedingQty - data.ProductQty - data.OutputProductQty - data.RemainingQty;
                data.Rate = data.FeedingQty == 0 ? 0 : Math.Round(data.DiffQty / data.FeedingQty * 100, 2);

                datas.Add(data);
            }

            return datas;
        }

        #endregion

        /// <summary>
        /// 生产达成率报表 - 工厂
        /// </summary>
        /// <param name="rateData"></param>
        /// <param name="mrpDics"></param>
        /// <param name="dicInvCodeProcessCode"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        [ApiService("生产达成率报表- 工厂")]
        [AllowAnonymous]
        public virtual List<ProductionAchievementRateData> ProductionAchievementRateFactory
            (RequestProductionAchievementRateData rateData, List<DictionaryData> mrpDics,
            List<DictionaryData> dicInvCodeProcessCode, EntityList<OrganizeCode> list)
        {
            var entityList = RT.Service.Resolve<DispatchTaskController>().GetDispatchTaskList(mrpDics, dicInvCodeProcessCode, rateData.DateRange?.BeginValue ?? null, rateData.DateRange?.EndValue ?? null);


            List<ProductionAchievementRateData> datas = new List<ProductionAchievementRateData>();


            Dictionary<string, List<DispatchTask>> dicDispatchTasks = new Dictionary<string, List<DispatchTask>>();
            foreach (var item in mrpDics)
            {
                foreach (var mrp in item.DicValue)
                {
                    var entity = list.Where(p => p.MrpController == mrp && p.FactoryCode == item.DicKey).FirstOrDefault();
                    if (!entityList.ContainsKey(item.DicKey) || entityList[item.DicKey] == null || entityList[item.DicKey].Count == 0)
                        continue;

                    var dispatchTaskList = entityList[item.DicKey].Where(p => p.WorkShopCode == mrp).ToList();
                    if (dispatchTaskList == null || dispatchTaskList.Count == 0) continue;

                    //修改：对产品线和厂部进行分组，将任务单存在来，放到下面去计算
                    var key = entity.ProductLine + ";" + entity.PlantName;
                    if (dicDispatchTasks.ContainsKey(key))
                    {
                        dicDispatchTasks[key].AddRange(dispatchTaskList);
                    }
                    else
                    {
                        dicDispatchTasks.Add(key, dispatchTaskList);
                    }
                }
            }


            foreach (var dic in dicDispatchTasks)
            {
                var dts = dic.Value.ToList();


                //此时datas为产品级别的最细数据,无任何聚合
                foreach (var dt in dts)
                {
                    datas.Add(new ProductionAchievementRateData()
                    {
                        ProductLine = dic.Key.Split(";")[0],
                        PlantName = dic.Key.Split(";")[1],
                        ProcessName = dt.ProcessName,
                        ProductCode = dt.ProductCode,
                        ProductName = dt.ProductName,
                        PlanBeginTime = dt.PlanBeginTime,
                        PlanQty = dt.DispatchQty,
                        ActualQty = dt.ReportQty,
                        UnitName = dt?.UnitName ?? "",
                        ProductionAchievement = (dt.DispatchQty == 0 || dt.ReportQty == 0) ? 0 : (dt.ReportQty / dt.DispatchQty)
                    });
                }
            }


            if (datas.Count == 0)
                datas.Add(new ProductionAchievementRateData());
            return datas;


        }






        /// <summary>
        /// 派工任务表
        /// </summary>
        /// <param name="mrpDics"></param>
        /// <param name="processeList"></param>
        /// <param name="dateRange"></param>
        /// <returns></returns>
        [ApiService("派工任务表")]
        [AllowAnonymous]
        public virtual List<DispatchTaskData> GetDispatchTaskData(List<DictionaryData> mrpDics, List<string> processeList, DateRange dateRange)
        {
            List<DispatchTaskData> result = new List<DispatchTaskData>();

            ////用于调试的条件                     
            //mrpDics =ByPlantData.debugMrpDics;
            //dateRange = new DateRange();
            //dateRange.BeginValue = new DateTime(2025, DateTime.Now.Month, 1);
            //dateRange.EndValue = DateTime.Now;
            //processeList = ByPlantData.onlyProcesses.ToList();

            string startTime = dateRange?.BeginValue?.ToString("yyyy-MM-dd") ?? "";
            string endTime = dateRange?.EndValue?.ToString("yyyy-MM-dd") ?? "";
            string processeListStr = processeList?.Any() == true ? string.Join(",", processeList.Select(x => $"'{x}'")) : "";
            foreach (var mrpDic in mrpDics)
            {
                if (string.IsNullOrEmpty(mrpDic.DicKey) || mrpDic.DicValue.Count <= 0) { break; }
                string mrpDicValueStr = mrpDic?.DicValue?.Any() == true ? string.Join(",", mrpDic.DicValue.Select(x => $"'{x}'")) : string.Empty;

                //提高效率不使用LEFT JOIN
                string sql = $@"SELECT 
                                INV_ORG_ID,
                                (SELECT CODE FROM ITEM WHERE INV_ORG_ID=TM_DISP_TASK.INV_ORG_ID AND ID=TM_DISP_TASK.PRODUCT_ID) AS ITEM_CODE,
                                (SELECT NAME FROM ITEM WHERE INV_ORG_ID=TM_DISP_TASK.INV_ORG_ID AND ID=TM_DISP_TASK.PRODUCT_ID) AS ITEM_NAME,
                                (SELECT MRP_CONTROLLER FROM ITEM WHERE INV_ORG_ID=TM_DISP_TASK.INV_ORG_ID AND ID=TM_DISP_TASK.PRODUCT_ID) AS MRP_CONTROLLER,
                                (SELECT CODE FROM TECH_PROCESS WHERE  INV_ORG_ID=TM_DISP_TASK.INV_ORG_ID AND ID=TM_DISP_TASK.PROCESS_ID) AS PROCESS_CODE,
                                DISPATCH_QTY,
                                REPORT_QTY,
                                PLAN_BEGIN_TIME
                                FROM TM_DISP_TASK   ";
                //条件 
                string whereStr = $@" WHERE  1=1 AND INV_ORG_ID={mrpDic.DicKey}  ";
                if (!string.IsNullOrEmpty(startTime)) { whereStr += $@" AND TM_DISP_TASK.PLAN_BEGIN_TIME>=TO_DATE('{startTime}','YYYY-MM-DD')   "; }
                if (!string.IsNullOrEmpty(endTime)) { whereStr += $@" AND TM_DISP_TASK.PLAN_BEGIN_TIME<TO_DATE('{endTime}','YYYY-MM-DD')   "; }
                if (!string.IsNullOrEmpty(processeListStr)) { whereStr += $@" AND PROCESS_ID IN (SELECT DISTINCT ID FROM TECH_PROCESS WHERE  INV_ORG_ID={mrpDic.DicKey}  AND CODE IN ({processeListStr})) "; }
                if (!string.IsNullOrEmpty(mrpDicValueStr)) { whereStr += $@" AND PRODUCT_ID IN (SELECT ID FROM ITEM WHERE INV_ORG_ID={mrpDic.DicKey}  AND MRP_CONTROLLER IN ({mrpDicValueStr}))"; }
                sql += $@"{whereStr}";


                //执行并把结果追加到result
                using (var db = DbAccesserFactory.Create("master"))
                {
                    using (System.Data.IDataReader dr = db.ExecuteReader(sql))

                        while (dr.Read())
                        {
                            DispatchTaskData dispatchTaskData = new DispatchTaskData();
                            dispatchTaskData.InvOrgId = dr.GetDecimal(0);
                            dispatchTaskData.ItemCode = dr.GetString(1);
                            dispatchTaskData.ItemName = dr.GetString(2);
                            dispatchTaskData.MrpController = dr.GetString(3);
                            dispatchTaskData.ProcessCode = dr.GetString(4);
                            dispatchTaskData.DispatchQty = dr.GetDecimal(5);
                            dispatchTaskData.ReportQty = dr.GetDecimal(6);
                            dispatchTaskData.PlanBeginTime = dr.GetDateTime(7);
                            result.Add(dispatchTaskData);
                        }
                }
            }
            if (result.Count <= 0) { result.Add(new DispatchTaskData()); }
            return result;
        }






        #region 厂部看板相关接口

        /// <summary>
        /// 计划产量表
        /// </summary>
        /// <param name="mrpDics"></param>
        /// <param name="processeList"></param>
        /// <param name="dateRange"></param>
        /// <returns></returns>
        [ApiService("计划产量表")]
        [AllowAnonymous]
        public virtual List<ResponseDispatchQtyData> GetDispatchQtyData(List<DictionaryData> mrpDics, List<string> processeList, DateRange dateRange)
        {
            List<ResponseDispatchQtyData> result = new List<ResponseDispatchQtyData>();

            ////用于调试的条件
            //mrpDics = RT.Service.Resolve<OrganizeCodeController>().GetMrps(null, null);
            //dateRange = new DateRange();
            //dateRange.BeginValue = new DateTime(2025, DateTime.Now.Month, 1);
            //dateRange.EndValue = DateTime.Now;
            //processeList = ByPlantData.onlyProcesses.ToList();


            string startTime = dateRange?.BeginValue?.ToString("yyyy-MM-dd") ?? "";
            string endTime = dateRange?.EndValue?.ToString("yyyy-MM-dd") ?? "";
            string processeListStr = processeList?.Any() == true ? string.Join(",", processeList.Select(x => $"'{x}'")) : "";
            foreach (var mrpDic in mrpDics)
            {
                if (string.IsNullOrEmpty(mrpDic.DicKey) || mrpDic.DicValue.Count <= 0) { break; }
                string mrpDicValueStr = mrpDic?.DicValue?.Any() == true ? string.Join(",", mrpDic.DicValue.Select(x => $"'{x}'")) : string.Empty;

                string sql = $@"SELECT 
                                TRUNC(TM_DISP_TASK.PLAN_BEGIN_TIME) AS PLAN_BEGIN_DATE,
                                SUM(TM_DISP_TASK.DISPATCH_QTY) AS DISPATCH_QTY 
                                FROM TM_DISP_TASK  ";

                //条件（提高效率不使用LEFT JOIN） 
                string whereStr = $@" WHERE  1=1 AND INV_ORG_ID={mrpDic.DicKey}  ";
                if (!string.IsNullOrEmpty(startTime)) { whereStr += $@"AND TM_DISP_TASK.PLAN_BEGIN_TIME>=TO_DATE('{startTime}','YYYY-MM-DD')   "; }
                if (!string.IsNullOrEmpty(endTime)) { whereStr += $@"AND TM_DISP_TASK.PLAN_BEGIN_TIME<TO_DATE('{endTime}','YYYY-MM-DD')   "; }
                if (!string.IsNullOrEmpty(processeListStr)) { whereStr += $@" AND PROCESS_ID IN (SELECT DISTINCT ID FROM TECH_PROCESS WHERE  INV_ORG_ID={mrpDic.DicKey} AND CODE IN ({processeListStr})) "; }
                if (!string.IsNullOrEmpty(mrpDicValueStr)) { whereStr += $@"AND PRODUCT_ID IN (SELECT ID FROM ITEM WHERE INV_ORG_ID={mrpDic.DicKey} AND MRP_CONTROLLER  IN ({mrpDicValueStr}))"; }

                sql += $@"{whereStr} GROUP BY TRUNC(TM_DISP_TASK.PLAN_BEGIN_TIME) ";

                //执行并把结果追加到result
                using (var db = DbAccesserFactory.Create("master"))
                {
                    using (System.Data.IDataReader dr = db.ExecuteReader(sql))

                        while (dr.Read())
                        {
                            ResponseDispatchQtyData responseDispatchQtyData = new ResponseDispatchQtyData();
                            responseDispatchQtyData.PlanBeginDate = dr.GetDateTime(0);
                            responseDispatchQtyData.DispatchQTY = dr.GetDecimal(1);
                            result.Add(responseDispatchQtyData);
                        }
                }
            }
            //if (result.Count <= 0) { result.Add(new ResponseDispatchQtyData()); }
            return result;
        }


        /// <summary>
        /// 报工记录表-(大于等于BeginValue，小于EndValue)
        /// </summary>
        /// <param name="mrpDics"></param>
        /// <param name="processeList"></param>
        /// <param name="dateRange"></param>
        /// <returns></returns>
        [ApiService("报工记录表-(大于等于BeginValue，小于EndValue)")]
        [AllowAnonymous]
        public virtual List<ReportRecordData> GetReportRecordData(List<DictionaryData> mrpDics, List<string> processeList, DateRange dateRange)
        {
            List<ReportRecordData> result = new List<ReportRecordData>();

            ////用于调试的条件
            //mrpDics = RT.Service.Resolve<OrganizeCodeController>().GetMrps(null, null);
            //mrpDics = ByPlantData.debugMrpDics;
            //dateRange = new DateRange();
            //dateRange.BeginValue = new DateTime(2025, DateTime.Now.Month, 1);
            //dateRange.EndValue = DateTime.Now;
            //processeList = ByPlantData.onlyProcesses.ToList();


            string startTime = dateRange?.BeginValue?.ToString("yyyy-MM-dd") ?? "";
            string endTime = dateRange?.EndValue?.ToString("yyyy-MM-dd") ?? "";
            string processeListStr = processeList?.Any() == true ? string.Join(",", processeList.Select(x => $"'{x}'")) : "";
            foreach (var mrpDic in mrpDics)
            {
                if (string.IsNullOrEmpty(mrpDic.DicKey) || mrpDic.DicValue.Count <= 0) { break; }
                string mrpDicValueStr = mrpDic?.DicValue?.Any() == true ? string.Join(",", mrpDic.DicValue.Select(x => $"'{x}'")) : string.Empty;
                string sql = $@"SELECT                              
                            TRUNC(TM_REPORT_RECORD.REPORT_TIME) AS REPORT_DATE,
                            SUM(REPORT_QTY) AS REPORT_QTY,
                            SUM(OK_QTY) AS OK_QTY,
                            SUM(NG_QTY) AS NG_QTY,
                            SUM(REWORK_QTY) AS REWORK_QTY
                            FROM TM_REPORT_RECORD ";

                //条件（提高效率不使用LEFT JOIN） 
                string whereStr = $@" WHERE  1=1 AND INV_ORG_ID={mrpDic.DicKey}  ";
                if (!string.IsNullOrEmpty(startTime)) { whereStr += $@"AND REPORT_TIME>=TO_DATE('{startTime}','YYYY-MM-DD')   "; }
                if (!string.IsNullOrEmpty(endTime)) { whereStr += $@"AND REPORT_TIME<TO_DATE('{endTime}','YYYY-MM-DD')   "; }
                if (!string.IsNullOrEmpty(processeListStr)) { whereStr += $@" AND PROCESS_ID IN (SELECT DISTINCT ID FROM TECH_PROCESS WHERE INV_ORG_ID={mrpDic.DicKey} AND CODE IN ({processeListStr})) "; }
                if (!string.IsNullOrEmpty(mrpDicValueStr)) { whereStr += $@"AND WORK_ORDER_ID IN (SELECT DISTINCT ID FROM WO WHERE INV_ORG_ID={mrpDic.DicKey} AND PRODUCT_ID IN  (SELECT ID FROM ITEM WHERE INV_ORG_ID={mrpDic.DicKey} AND MRP_CONTROLLER IN ({mrpDicValueStr}))  ) "; }

                sql += $@"{whereStr} GROUP BY TRUNC(TM_REPORT_RECORD.REPORT_TIME) ";

                //执行并把结果追加到result
                using (var db = DbAccesserFactory.Create("master"))
                {
                    using (System.Data.IDataReader dr = db.ExecuteReader(sql))

                        while (dr.Read())
                        {
                            ReportRecordData reportRecordData = new ReportRecordData();
                            reportRecordData.ReportTime = dr.GetDateTime(0);
                            reportRecordData.ReportQty = dr.GetDecimal(1);
                            reportRecordData.OkQty = dr.GetDecimal(2);
                            reportRecordData.NgQty = dr.GetDecimal(3);
                            reportRecordData.ReworkQty = dr.GetDecimal(4);
                            result.Add(reportRecordData);
                        }
                }
            }
            //if (result.Count <= 0) { result.Add(new ReportRecordData()); }
            return result;
        }


        /// <summary>
        /// 可疑品标签明细-(大于等于BeginValue，小于EndValue)
        /// </summary>
        /// <param name="mrpDics"></param>
        /// <param name="dateRange"></param>
        /// <returns></returns>
        [ApiService("可疑品标签明细-(大于等于BeginValue，小于EndValue)")]
        [AllowAnonymous]
        public virtual List<SuspectProdData> GetSuspectProd(List<DictionaryData> mrpDics, DateRange dateRange)
        {

            ////用于调试的条件
            //mrpDics = RT.Service.Resolve<OrganizeCodeController>().GetMrps(null, null);
            //mrpDics = ByPlantData.debugMrpDics;
            //dateRange = new DateRange();
            //dateRange.BeginValue = new DateTime(2025, DateTime.Now.Month, 1);
            //dateRange.EndValue = DateTime.Now;


            List<SuspectProdData> result = new List<SuspectProdData>();

            string whereStr = " WHERE 1=1 ";

            //日期条件
            if (dateRange.BeginValue.HasValue) { whereStr += $" AND WIP_SUSPECT_PROD_LABEL_DTL.UPDATE_DATE >= TO_DATE('{dateRange.BeginValue.Value:yyyy-MM-dd}', 'YYYY-MM-DD') "; }
            if (dateRange.EndValue.HasValue) { whereStr += $" AND WIP_SUSPECT_PROD_LABEL_DTL.UPDATE_DATE < TO_DATE('{dateRange.EndValue.Value.AddDays(1):yyyy-MM-dd}', 'YYYY-MM-DD') "; }

            //库存组织条件
            var keyList = (mrpDics ?? Enumerable.Empty<DictionaryData>()).Where(d => !string.IsNullOrEmpty(d.DicKey)).Select(d => $"'{d.DicKey}'").Distinct().ToList();
            if (keyList.Any())
            {
                string inStr = string.Join(",", keyList);
                whereStr += $" AND WIP_SUSPECT_PROD_LABEL_DTL.INV_ORG_ID IN ({inStr})";
            }



            string sql = $@"SELECT 
                  TRUNC(WIP_SUSPECT_PROD_LABEL_DTL.UPDATE_DATE) AS UPDATE_DATE_DAY,
                  DEF_DEFECT.CODE,
                  DEF_DEFECT.DESCRIPTION,     
                  SUM(WIP_SUSPECT_PROD_LABEL_DTL.QTY) AS TOTAL_QTY
              FROM WIP_SUSPECT_PROD_LABEL_DTL 
              LEFT JOIN DEF_DEFECT 
                  ON WIP_SUSPECT_PROD_LABEL_DTL.DEFECT_ID = DEF_DEFECT.ID 
             {whereStr}                          
              GROUP BY 
                  TRUNC(WIP_SUSPECT_PROD_LABEL_DTL.UPDATE_DATE),
                  DEF_DEFECT.CODE,
                  DEF_DEFECT.DESCRIPTION 
           ";

            using (var db = DbAccesserFactory.Create("master"))
            using (System.Data.IDataReader dr = db.ExecuteReader(sql))
            {
                while (dr.Read())
                {
                    SuspectProdData suspectProdData = new SuspectProdData
                    {
                        UpdateDateDay = dr.GetDateTime(0),
                        Code = dr.GetString(1),
                        Description = dr.GetString(2),
                        DefectQTY = dr.GetDecimal(3)
                    };
                    result.Add(suspectProdData);
                }
            }

            return result;
        }


        /// <summary>
        /// 获取所有的安灯类型
        /// </summary>
        /// <returns></returns>
        [ApiService("获取所有的安灯类型")]
        [AllowAnonymous]
        public virtual List<AllAndonStype> GetAllAndonStype(string notParameter)
        {
            List<AllAndonStype> result = new List<AllAndonStype>();
            string sql = "SELECT DISTINCT ANDON_TYPE_CODE, ANDON_TYPE_NAME FROM MES_ANDONTYPE ";

            using (var db = DbAccesserFactory.Create("master"))
            {
                using (System.Data.IDataReader dr = db.ExecuteReader(sql))
                {
                    while (dr.Read())
                    {
                        AllAndonStype allAndonStype = new AllAndonStype();
                        allAndonStype.AndonTypeCode = dr.GetString(0);
                        allAndonStype.AndonTypeName = dr.GetString(1);
                        result.Add(allAndonStype);
                    }
                }
            }

            // if (result.Count <= 0) { result.Add(new AllAndonStype()); }
            return result;
        }

        /// <summary>
        /// 获取所有未关闭的安灯
        /// </summary>
        /// <returns></returns>
        [ApiService("获取所有未关闭的安灯")]
        [AllowAnonymous]
        public virtual List<BaseAdonAndonManage> GetMesAndonManage(string notParameter)
        {
            List<BaseAdonAndonManage> result = new List<BaseAdonAndonManage>();
            //提高效率不用联表查询
            string sql = $@"SELECT 
                            (SELECT ANDON_TYPE_CODE FROM MES_ANDONTYPE WHERE INV_ORG_ID=MES_ANDONMANAGE.INV_ORG_ID AND ID=MES_ANDONMANAGE.ANDON_TYPE_ID ) AS ANDON_TYPE_CODE,
                            (SELECT ANDON_TYPE_NAME FROM MES_ANDONTYPE WHERE INV_ORG_ID=MES_ANDONMANAGE.INV_ORG_ID AND ID=MES_ANDONMANAGE.ANDON_TYPE_ID ) AS ANDON_TYPE_NAME,
                            ANDON_MANAGE_CODE,
                            (SELECT NAME FROM RES_WIP_SCHE WHERE INV_ORG_ID=MES_ANDONMANAGE.INV_ORG_ID AND ID=MES_ANDONMANAGE.WIP_RESOURCE_ID) AS WIP_RESOURCE_NAME,
                            (SELECT ANDON_DESC FROM ANDON_UPHOLD WHERE INV_ORG_ID=MES_ANDONMANAGE.INV_ORG_ID AND ID=(SELECT ANDON_UPHOLD_ID FROM RES_WIP_SCHE WHERE INV_ORG_ID=MES_ANDONMANAGE.INV_ORG_ID AND ID=MES_ANDONMANAGE.WIP_RESOURCE_ID) ) AS ANDON_DESC,                           
                            (SELECT CODE FROM EMS_EQUIP_ACCOUNT WHERE INV_ORG_ID=MES_ANDONMANAGE.INV_ORG_ID AND ID=MES_ANDONMANAGE.EQUIP_ACCOUNT_ID) AS EQUIP_ACCOUNT_CODE,
                            PROBLEM_DESC,
                            TRIGGER_TIME,
                            (SELECT PLANT_CODE FROM ANDON_UPHOLD WHERE INV_ORG_ID=MES_ANDONMANAGE.INV_ORG_ID AND ID=(SELECT ANDON_UPHOLD_ID FROM RES_WIP_SCHE WHERE INV_ORG_ID=MES_ANDONMANAGE.INV_ORG_ID AND ID=MES_ANDONMANAGE.WIP_RESOURCE_ID) ) AS PLANT_NAME,
                            (SELECT PLANT_NAME FROM ANDON_UPHOLD WHERE INV_ORG_ID=MES_ANDONMANAGE.INV_ORG_ID AND ID=(SELECT ANDON_UPHOLD_ID FROM RES_WIP_SCHE WHERE INV_ORG_ID=MES_ANDONMANAGE.INV_ORG_ID AND ID=MES_ANDONMANAGE.WIP_RESOURCE_ID) ) AS PLANT_NAME
                            FROM MES_ANDONMANAGE 
                            WHERE 1 =1 
                            AND (STATE=10 OR STATE=20) ";
            using (var db = DbAccesserFactory.Create("master"))
            {
                using (System.Data.IDataReader dr = db.ExecuteReader(sql))
                {
                    while (dr.Read())
                    {
                        BaseAdonAndonManage baseAdonAndonManage = new BaseAdonAndonManage();
                        baseAdonAndonManage.AndonTypeCode = dr.GetString(0);
                        baseAdonAndonManage.AndonTypeName = dr.GetString(1);
                        baseAdonAndonManage.AdonManageCode = dr.GetString(2);
                        baseAdonAndonManage.WipResourceName = dr.GetString(3);
                        baseAdonAndonManage.AndonArea = dr.GetString(4);
                        baseAdonAndonManage.EquipAccountCode = dr.GetString(5);
                        baseAdonAndonManage.ProblemDesc = dr.GetString(6);
                        baseAdonAndonManage.TriggerTime = dr.GetDateTime(7);
                        baseAdonAndonManage.PlantCode = dr.GetString(8);
                        baseAdonAndonManage.PlantName = dr.GetString(9);
                        result.Add(baseAdonAndonManage);
                    }
                }
            }

            //if (result.Count <= 0) { result.Add(new BaseAdonAndonManage()); }
            return result;
        }

        /// <summary>
        /// 产线/设备 状态
        /// </summary>
        /// <param name="mrpDics"></param>
        /// <returns></returns>
        [ApiService("获取产线或设备状态")]
        [AllowAnonymous]
        public virtual List<LineEquipmentStatusData> GetLineMachineStatus(List<DictionaryData> mrpDics)
        {
            List<LineEquipmentStatusData> result = new List<LineEquipmentStatusData>();

            var dicWids = mrpDics;
            var invCurr = RT.InvOrg;
            foreach (var dicWid in dicWids)
            {
                var invOrg = Query<SIE.Rbac.InvOrgs.InvOrg>().Where(p => p.ExternalId == dicWid.DicKey).FirstOrDefault();
                if (invOrg == null)
                    continue;
                RT.InvOrg = invOrg.Code;

                ////登录
                //try { RT.Service.Resolve<KzLoginController>().Login(invOrg.ToString()); } catch { continue; }


                var curTime = RF.Find<DispatchTask>().GetDbTime();
                //获取 看板区域和类型
                var RegionList = Query<SIE.MES.DashBoard.KzBoard.RegionBoards.RegionBoard>().ToList();
                foreach (var region in RegionList)
                {
                    string Region = region.Region;
                    RegionBoardType regionBoardType = region.RegionBoardType;

                    //获取产线明细(看板区域)
                    var entityList = RT.Service.Resolve<RegionBoardsController>().GetRegionBoardDetailsByRegion(Region, regionBoardType);
                    var wipResourceIds = entityList.Select(p => p.WipResourceId).ToList();


                    //两小时以内有报工记录的，也算正常排产
                    var twoHourResourceIds = new List<double>();

                    #region twoHourResourceIds取值逻辑修改：20260528
                    // 构建资源ID -> ReportHours 映射（默认2小时）
                    var resourceHoursMap = entityList.ToDictionary(item => item.WipResourceId, item => item.ReportHours > 0 ? (double)item.ReportHours : 2.0);
                    // 遍历所有资源ID，逐个查询
                    foreach (var rid in wipResourceIds)
                    {
                        double hours = resourceHoursMap[rid];
                        var count = Query<ReportRecordExamine>().Where(p => p.ReportTime >= curTime.AddHours(-hours) && (double)p.ResourceId == rid).Count();
                        if (count > 0) { twoHourResourceIds.Add(rid); }
                    }
                    #endregion

                    //wipResourceIds.SplitDataExecute(temp =>
                    //{      
                    //    twoHourResourceIds.AddRange(Query<ReportRecordExamine>().Where(p => p.ReportTime >= curTime.AddHours(-2) && temp.Contains((double)p.ResourceId)).Select(p => (double)p.ResourceId).Distinct().ToList<double>().ToList());
                    //});




                    //获取安灯异常产线
                    var andonWipResourceIds = RT.Service.Resolve<AndonManageController>().GetAndonManageIds(wipResourceIds);
                    var taskWipResourceIds = RT.Service.Resolve<DispatchTaskController>().GetSchedulingWipResourceIds(wipResourceIds);


                    foreach (var item in entityList.OrderBy(p => p.Sort))
                    {
                        var entity = new LineEquipmentStatusData()
                        {
                            LineName = item.ResourceName,
                            StatuType = StatuType.Scheduling
                        };
                        //绿色：该产线两个小时内有报工记录
                        //黄色：该产线当前班次无排程任务单，且两个小时内没有报工记录
                        //红色：存在安灯事件未关闭，或当前产线有排程任务但是两个小时内没有报工
                        if (twoHourResourceIds.Any(p => p == item.WipResourceId))
                            entity.StatuType = StatuType.Scheduling;
                        if (taskWipResourceIds.All(p => p != item.WipResourceId) && twoHourResourceIds.All(p => p != item.WipResourceId))
                            entity.StatuType = StatuType.Unscheduled;
                        if (andonWipResourceIds.Any(p => p == item.WipResourceId) || (taskWipResourceIds.Any(p => p == item.WipResourceId) && twoHourResourceIds.All(p => p != item.WipResourceId)))
                            entity.StatuType = StatuType.Abnormal;

                        result.Add(entity);
                    }
                    //if (result.Count <= 0) { result.Add(new LineEquipmentStatusData()); }

                }

            }
            RT.InvOrg = invCurr;

            return result;
        }

        /// <summary>
        /// 获取安全生产天数
        /// </summary>
        /// <returns></returns>
        [ApiService("获取安全生产天数")]
        [AllowAnonymous]
        public virtual WorkSafetyDayData GetWorkSafetyDay(List<DictionaryData> dicWids)
        {
            WorkSafetyDayData workSafetyDayData = new WorkSafetyDayData();
            workSafetyDayData.WorkSafetyDay = 0;

            //SQL语句          
            List<string> listInvOrg = dicWids
                .Select(d => d.DicKey)
                .Where(key => !string.IsNullOrEmpty(key)) //过滤空值
                .Distinct()
                .Select(key => key.Replace("'", "''"))   // 单引号转义，用于 SQL
                .ToList();
            string whereStr = " WHERE 1=1 AND SAFETY_DATE IS NOT NULL ";
            if (listInvOrg.Any())
            {
                string resultForSql = "(" + string.Join(",", listInvOrg.Select(s => $"'{s}'")) + ")";
                whereStr += $" AND INV_ORG_ID IN {resultForSql} ";
            }
            string sql = $@"SELECT SAFETY_DATE FROM WORK_SAFETY {whereStr} AND ROWNUM = 1";


            //执行SQL查询
            using (var db = DbAccesserFactory.Create("master"))
            {
                using (System.Data.IDataReader dr = db.ExecuteReader(sql))
                {
                    while (dr.Read()) // ROWNUM = 1 只有一行
                    {
                        try { workSafetyDayData.WorkSafetyDay = (DateTime.Now - dr.GetDateTime(0)).Days; }
                        catch { }
                    }
                }
            }

            //返回
            return workSafetyDayData;
        }






        #endregion















        /// <summary>
        /// 产能利用率报表 - 工厂
        /// </summary>
        /// <param name="productionProcesses"></param>
        /// <param name="dataType"></param>
        /// <param name="dicpProcessCodes"></param>
        /// <returns></returns>
        [ApiService("产能利用率报表- 工厂")]
        [AllowAnonymous]
        public virtual List<CapacityUtilizationRateData> CapacityUtilizationRateFactory(EntityList<ProductionProcess> productionProcesses, RequestCapacityUtilizationRateData model, List<DictionaryData> dicpProcessCodes)
        {
            List<CapacityUtilizationRateData> datas = new List<CapacityUtilizationRateData>();
            DateRange dateRange = new DateRange();
            int Year = model.Year.IsNullOrEmpty() ? 0 : int.Parse(model.Year);
            int Month = model.Month.IsNullOrEmpty() ? 0 : int.Parse(model.Month);
            int Num = model.Num.IsNullOrEmpty() ? 0 : int.Parse(model.Num);
            switch (model.CapacityDataType)
            {
                case ProductionProcesss.Enums.CapacityDataType.Moon:
                    var dateTime = new DateTime(Year, Month, 1, 0, 0, 0);
                    dateRange.BeginValue = dateTime;
                    dateRange.EndValue = dateTime.AddMonths(1);
                    break;
                case ProductionProcesss.Enums.CapacityDataType.Week:
                    var weekDay = GetWeekDateRange(Year, Month, Num);
                    dateRange.BeginValue = weekDay;
                    dateRange.EndValue = weekDay.AddDays(7);
                    break;
                case ProductionProcesss.Enums.CapacityDataType.Day:
                    var day = new DateTime(Year, Month, Num, 0, 0, 0);
                    dateRange.BeginValue = day;
                    dateRange.EndValue = day.AddDays(1);
                    break;
            }

            var entityList = RT.Service.Resolve<DispatchTaskController>().GetDispatchTaskList(dateRange.BeginValue, dateRange.EndValue, dicpProcessCodes);
            var uphList = RT.Service.Resolve<CapacityResourceController>().GetStandardCapacity(model.CapacityDataType, dicpProcessCodes);

            foreach (var item in productionProcesses.OrderBy(p => p.ProductLine))
            {
                if (entityList[item.InventoryCode] == null)
                    continue;
                var dispatchTaskList = entityList[item.InventoryCode].Where(p => p.ProcessCode == item.ProcessCode).ToList();
                var actualQty = dispatchTaskList.Sum(p => p.ReportQty);
                var standardCapacity = uphList[item.InventoryCode + item.ProcessCode];
                var entity = new CapacityUtilizationRateData();
                entity.ProductLine = item.ProductLine;
                entity.PlantName = item.PlantName;
                entity.ProcessName = item.ProcessCode;
                entity.ActualQty = actualQty;
                entity.StandardCapacity = standardCapacity;
                entity.CapacityUtilization = entity.ActualQty == 0 || entity.StandardCapacity == 0 ? 0 : entity.ActualQty / entity.StandardCapacity;
                datas.Add(entity);
            }
            if (datas.Count == 0)
                datas.Add(new CapacityUtilizationRateData());
            return datas;
        }

        private DateTime GetWeekDateRange(
          int year,
          int month,
          int weekNumber,
          DayOfWeek startOfWeek = DayOfWeek.Monday)
        {
            // 获取当月第一天
            DateTime firstDayOfMonth = new DateTime(year, month, 1);

            // 计算当月第一天与一周起始日的差值，确定第一周的起始日期
            int daysToAdd = ((int)startOfWeek - (int)firstDayOfMonth.DayOfWeek + 7) % 7;
            DateTime firstDayOfFirstWeek = firstDayOfMonth.AddDays(daysToAdd);

            // 如果当月第一天就是一周的起始日，第一周起始日就是当月第一天
            if (firstDayOfFirstWeek > firstDayOfMonth)
            {
                firstDayOfFirstWeek = firstDayOfFirstWeek.AddDays(-7);
            }

            // 计算目标周的起始日期
            DateTime startDate = firstDayOfFirstWeek.AddDays((weekNumber - 1) * 7);

            return startDate;
        }

        /// <summary>
        ///  安灯异常统计报表- 工厂
        /// </summary>
        /// <param name="model"></param>
        /// <param name="dicOrganizeCode"></param>
        /// <param name="dicWids"></param>
        /// <returns></returns>
        [ApiService("安灯异常统计报表- 工厂")]
        [AllowAnonymous]
        public virtual List<AndonAnomalyData> AndonAnomalyFactory(RequestAndonAnomalyData model,
            List<DictionaryObjData> dicOrganizeCode, List<DictionaryData> dicWids)
        {
            var entityList = RT.Service.Resolve<AndonManageController>().GetAndonAnomaly(dicWids, model.DateRange?.BeginValue ?? null, model.DateRange?.EndValue ?? null);
            List<AndonAnomalyData> datas = new List<AndonAnomalyData>();

            foreach (var item in entityList)
            {
                var organizeCodeList = dicOrganizeCode.Where(p => p.DicKey == item.FactoryCode).Select(p => p.DicValue).ToList<object>();
                OrganizeCode organizeCode = null;
                organizeCodeList.ForEach(p =>
                {
                    var entity = p as OrganizeCode;
                    if (entity.WorkshopCode == item.WorkShopCode)
                    {
                        organizeCode = entity;
                        return;
                    }
                });
                datas.Add(new AndonAnomalyData()
                {
                    ProductLine = organizeCode?.ProductLine ?? "",
                    PlantName = organizeCode?.PlantName ?? "",
                    AnDengCount = item.AndonNum,
                    AndonClass = item.AndonBigType,
                    AnDengType = item.AndonType,
                    OnTimeProcessCount = item.OnTimeProcessCount,
                    OnTimeResponseCount = item.OnTimeResponseCount,
                    OnTimeResponseRate = Math.Round(item.OnTimeResponseRate, 2),
                    OnTimeProcessRate = Math.Round(item.OnTimeProcessRate, 2),
                    ExceptionProcessTime = item.ExceptionProcessTime,
                    ExceptionResponseTime = item.ExceptionResponseTime,
                });
            }
            if (datas.Count == 0)
                datas.Add(new AndonAnomalyData());
            return datas;
        }


        #region 质量不良统计报表
        /// <summary>
        /// 质量不良统计报表-工厂
        /// </summary>
        /// <param name="factoryCodes">工厂编码列表</param>
        /// <param name="mrpControllers">MRP控制者列表</param>
        /// <param name="process">工序</param>
        /// <param name="productCode">产品编码</param>
        /// <param name="oldPartNumber">旧料号</param>
        /// <param name="isReWork">是否返工单</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        [ApiService("质量不良统计报表-工厂")]
        [AllowAnonymous]
        public virtual List<QualityDefectReportFactoryData> GetQualityDefectReportDatasFactory(
            List<string> factoryCodes, List<string> mrpControllers, List<string> process,
            string productCode, string oldPartNumber, string isReWork, DateTime? beginTime, DateTime? endTime)
        {
            List<QualityDefectReportFactoryData> datas = new List<QualityDefectReportFactoryData>();

            using (InvOrgs.WithAll())
            {
                // 查询报工记录：报工时间在查询时间范围内
                var q = DB.Query<ReportRecord>("rr")
                    .Join<Rbac.InvOrgs.InvOrg>("org", (x, y) => x.SQL<int>("rr.Inv_Org_Id") == y.Code)
                    .Join<DispatchTask>((x, y) => x.DispatchTaskId == y.Id);

                q.WhereIf(beginTime != null, p => p.ReportTime >= beginTime);
                q.WhereIf(endTime != null, p => p.ReportTime <= endTime);
                if (factoryCodes.Count > 0)
                {
                    q.Where(p => factoryCodes.Contains(p.SQL<string>("org.External_Id")));
                }
                q.WhereIf(mrpControllers.Count > 0, p => mrpControllers.Contains(p.WorkOrder.WorkShop.Code));
                q.WhereIf(process != null && process.Count > 0, p => process.Contains(p.Process.Code));
                if (!productCode.IsNullOrEmpty())
                    q.Where(p => p.WorkOrder.Product.Code == productCode);
                if (!oldPartNumber.IsNullOrEmpty())
                    q.Where(p => p.WorkOrder.Product.ShortDescription == oldPartNumber);
                if (!isReWork.IsNullOrEmpty())
                {
                    if (isReWork == "是")
                        q.Where(p => p.IsRework == true);
                    else if (isReWork == "否")
                        q.Where(p => p.IsRework == false);
                }

                // 按工序+产品分组，计算报工总量、不合格数、返工数
                var reportList = q.ToList(new PagingInfo() { PageNumber = 1, PageSize = 100000000 },
                    new EagerLoadOptions().LoadWithViewProperty());
                var reportGroups = reportList
                    .GroupBy(p => new
                    {
                        ProcessId = p.ProcessId,
                        ProcessCode = p.ProcessCode,
                        ProcessName = p.ProcessName,
                        ProductCode = p.ProductCode,
                        ProductName = p.ProductName,
                        OldPartNumber = p.ShortDescription
                    })
                    .Select(g => new QualityDefectReportFactoryData
                    {
                        ProductLine = "",
                        PlantName = "",
                        ProcessId = g.Key.ProcessId,
                        Process = g.Key.ProcessName,
                        ProductCode = g.Key.ProductCode,
                        ProductName = g.Key.ProductName,
                        OldPartNumber = g.Key.OldPartNumber,
                        ReportQty = g.Sum(x => x.ReportQty),
                        RecordNgQty = g.Sum(x => x.NgQty),
                        ReworkQty = g.Sum(x => x.ReworkQty),
                        SuspectQty = 0
                    })
                    .ToList();
                datas.AddRange(reportGroups);

                // 再单独查询可疑品标签的可疑品数量
                var qSuspect = DB.Query<SuspectProductLabel>("spl")
                    .Join<Rbac.InvOrgs.InvOrg>("org", (x, y) => x.SQL<int>("spl.Inv_Org_Id") == y.Code);

                qSuspect.WhereIf(beginTime != null, p => p.CreateDate >= beginTime);
                qSuspect.WhereIf(endTime != null, p => p.CreateDate <= endTime);
                if (factoryCodes.Count > 0)
                {
                    qSuspect.Where(p => factoryCodes.Contains(p.SQL<string>("org.External_Id")));
                }
                qSuspect.WhereIf(mrpControllers.Count > 0, p => mrpControllers.Contains(p.WorkOrder.WorkShop.Code));
                qSuspect.WhereIf(process != null && process.Count > 0, p => process.Contains(p.Process.Code));
                if (!productCode.IsNullOrEmpty())
                    qSuspect.Where(p => p.WorkOrder.Product.Code == productCode);
                if (!oldPartNumber.IsNullOrEmpty())
                    qSuspect.Where(p => p.WorkOrder.Product.ShortDescription == oldPartNumber);

                var suspectList = qSuspect.ToList(new PagingInfo() { PageNumber = 1, PageSize = 100000000 },
                    new EagerLoadOptions().LoadWithViewProperty());
                var suspectGroups = suspectList
                    .GroupBy(p => new
                    {
                        ProcessId = p.ProcessId,
                        ProductCode = p.ProductCode,
                        ProductName = p.ProductName,
                        OldPartNumber = p.ProductShortDescription
                    })
                    .Select(g => new QualityDefectReportFactoryData
                    {
                        ProductLine = "",
                        PlantName = "",
                        ProcessId = g.Key.ProcessId,
                        ProductCode = g.Key.ProductCode,
                        ProductName = g.Key.ProductName,
                        OldPartNumber = g.Key.OldPartNumber,
                        ReportQty = 0,
                        RecordNgQty = 0,
                        ReworkQty = 0,
                        SuspectQty = g.Sum(x => x.Qty)
                    })
                    .ToList();

                // 以报工记录为主，将可疑品数量合并进去
                foreach (var d in datas)
                {
                    d.SuspectQty = suspectGroups
                        .Where(s => s.ProcessId == d.ProcessId && s.ProductCode == d.ProductCode)
                        .Sum(s => s.SuspectQty);
                }
            }

            if (datas.Count == 0)
                datas.Add(new QualityDefectReportFactoryData());

            return datas;
        }

        #endregion

        #region 质量帕累托图
        /// <summary>
        /// 质量帕累托图-工厂
        /// </summary>
        /// <param name="factoryCodes">工厂编码列表</param>
        /// <param name="mrpControllers">MRP控制者列表</param>
        /// <param name="process">工序</param>
        /// <param name="productCode">产品编码</param>
        /// <param name="oldPartNumber">旧料号</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        [ApiService("质量帕累托图-工厂")]
        [AllowAnonymous]
        public virtual List<QualityParetoChartFactoryData> GetQualityParetoChartDataFactory(
            List<string> factoryCodes, List<string> mrpControllers, List<string> process,
            string productCode, string oldPartNumber, DateTime? beginTime, DateTime? endTime)
        {
            List<QualityParetoChartFactoryData> datas = new List<QualityParetoChartFactoryData>();

            string whereStr = " WHERE 1=1 ";

            // 库存组织(工厂)条件
            if (factoryCodes != null && factoryCodes.Count > 0)
            {
                var escapedItems = factoryCodes.Select(s => $"'{s}'");
                whereStr += $" AND SYS_INV_ORG.EXTERNAL_ID IN ({string.Join(",", escapedItems)})";
            }
            // 日期条件：按可疑品标签明细修改时间
            if (beginTime != null)
            {
                whereStr += $" AND WIP_SUSPECT_PROD_LABEL_DTL.UPDATE_DATE >= TO_DATE('{beginTime:yyyy-MM-dd HH:mm:ss}','YYYY-MM-DD HH24:MI:SS')";
            }
            if (endTime != null)
            {
                whereStr += $" AND WIP_SUSPECT_PROD_LABEL_DTL.UPDATE_DATE <= TO_DATE('{endTime:yyyy-MM-dd HH:mm:ss}','YYYY-MM-DD HH24:MI:SS')";
            }
            // 车间(MRP控制者)条件
            if (mrpControllers != null && mrpControllers.Count > 0)
            {
                var escapedItems = mrpControllers.Select(s => $"'{s}'");
                whereStr += $" AND RES_ENTERPRISE.CODE IN ({string.Join(",", escapedItems)})";
            }
            // 工序条件
            if (process != null && process.Count > 0)
            {
                var escapedItems = process.Select(s => $"'{s}'");
                whereStr += $" AND TECH_PROCESS.CODE IN ({string.Join(",", escapedItems)})";
            }
            // 产品编码条件
            if (!productCode.IsNullOrEmpty())
            {
                whereStr += $" AND ITEM.CODE = '{productCode}'";
            }
            // 旧料号条件
            if (!oldPartNumber.IsNullOrEmpty())
            {
                whereStr += $" AND ITEM.SHORT_DESCRIPTION = '{oldPartNumber}'";
            }

            string sql = $@"
SELECT
    ITEM.CODE AS ProductCode,
    ITEM.NAME AS ProductName,
    ITEM.SHORT_DESCRIPTION AS OldPartNumber,
    DEF_DEFECT.CODE AS DefectCode,
    DEF_DEFECT.DESCRIPTION AS DefectName,
    SUM(NVL(WIP_SUSPECT_PROD_LABEL_DTL.QTY, 0)) AS Qty,
    WIP_SUSPECT_PROD_LABEL_DTL.SUSPECT_JUDGE_RESULT AS JudgmentResult
FROM WIP_SUSPECT_PROD_LABEL
INNER JOIN WIP_SUSPECT_PROD_LABEL_DTL
    ON WIP_SUSPECT_PROD_LABEL.ID = WIP_SUSPECT_PROD_LABEL_DTL.SUSPECT_PRODUCT_LABEL_ID AND WIP_SUSPECT_PROD_LABEL_DTL.IS_PHANTOM = 0
LEFT JOIN DEF_DEFECT ON WIP_SUSPECT_PROD_LABEL_DTL.DEFECT_ID = DEF_DEFECT.ID AND DEF_DEFECT.IS_PHANTOM = 0
INNER JOIN WO ON WIP_SUSPECT_PROD_LABEL.WORK_ORDER_ID = WO.ID AND WO.IS_PHANTOM = 0
INNER JOIN ITEM ON WO.PRODUCT_ID = ITEM.ID AND ITEM.IS_PHANTOM = 0
INNER JOIN RES_ENTERPRISE ON WO.WORK_SHOP_ID = RES_ENTERPRISE.ID AND RES_ENTERPRISE.IS_PHANTOM = 0
INNER JOIN TECH_PROCESS ON WIP_SUSPECT_PROD_LABEL.PROCESS_ID = TECH_PROCESS.ID AND TECH_PROCESS.IS_PHANTOM = 0
INNER JOIN SYS_INV_ORG ON WIP_SUSPECT_PROD_LABEL.INV_ORG_ID = SYS_INV_ORG.CODE AND SYS_INV_ORG.IS_PHANTOM = 0
{whereStr}
AND WIP_SUSPECT_PROD_LABEL.IS_PHANTOM = 0
GROUP BY
    ITEM.CODE,
    ITEM.NAME,
    ITEM.SHORT_DESCRIPTION,
    DEF_DEFECT.CODE,
    DEF_DEFECT.DESCRIPTION,
    WIP_SUSPECT_PROD_LABEL_DTL.SUSPECT_JUDGE_RESULT
";

            using (var db = DbAccesserFactory.Create("master"))
            {
                using (System.Data.IDataReader dr = db.ExecuteReader(sql))
                {
                    while (dr.Read())
                    {
                        QualityParetoChartFactoryData data = new QualityParetoChartFactoryData
                        {
                            ProductCode = dr.GetString(0),
                            ProductName = dr.GetString(1),
                            OldPartNumber = dr.GetString(2),
                            DefectCode = dr.GetString(3),
                            DefectName = dr.IsDBNull(4) ? "" : dr.GetString(4),
                            Qty = dr.GetDecimal(5),
                            JudgmentResult = dr.IsDBNull(6) ? "" : dr.GetDecimal(6).ToString()
                        };
                        datas.Add(data);
                    }
                }
            }

            // 将判定结果枚举值转换为可读字符串
            foreach (var item in datas)
            {
                switch (item.JudgmentResult)
                {
                    case "0":
                        item.JudgmentResult = "Good";
                        break;
                    case "1":
                        item.JudgmentResult = "Scrap";
                        break;
                    case "2":
                        item.JudgmentResult = "Repair";
                        break;
                    default:
                        item.JudgmentResult = "";
                        break;
                }
            }

            if (datas.Count == 0)
                datas.Add(new QualityParetoChartFactoryData());

            return datas;
        }
        #endregion

        #region 安灯统计报表
        /// <summary>
        /// 安灯统计报表-工厂
        /// </summary>
        /// <param name="factoryCodes">工厂编码列表</param>
        /// <param name="mrpControllers">MRP控制者(车间编码)列表</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        [ApiService("安灯统计报表-工厂")]
        [AllowAnonymous]
        public virtual List<AndonStatisticsReportData> GetAndonStatisticsReportDatasFactory(List<string> factoryCodes, DateTime? beginTime, DateTime? endTime)
        {
            var datas = new List<AndonStatisticsReportData>();

            using (InvOrgs.WithAll())
            {
                // 查询日期范围内的安灯明细
                var andonQuery = DB.Query<AndonManage>("ad")
                    .Join<Rbac.InvOrgs.InvOrg>("org", (x, y) => x.SQL<int>("ad.Inv_Org_Id") == y.Code)
                    .Where(m => m.State != AndonManageState.Cancel); ;
                if (factoryCodes.Count > 0)
                {
                    andonQuery.Where(p => factoryCodes.Contains(p.SQL<string>("org.External_Id")));
                }
                if (beginTime.HasValue)
                    andonQuery.Where(p => p.CreateDate >= beginTime.Value);
                if (endTime.HasValue)
                    andonQuery.Where(p => p.CreateDate <= endTime.Value);

                EagerLoadOptions elo = new EagerLoadOptions();
                elo.LoadWith(AndonManage.AndonTypeProperty);
                var andonList = andonQuery.ToList(null, elo);
                if (!andonList.Any())
                    return datas;

                // 批量查询操作日志，用于计算响应/处理/验收时长
                var allAndonIds = andonList.Select(p => p.Id).Distinct().ToList();
                var logs = allAndonIds.SplitContains(ids =>
                {
                    return Query<AndonManageOperateLog>().Where(p => ids.Contains(p.AndonManageId)).ToList();
                });
                var now = RF.Find<AndonManageOperateLog>().GetDbTime();
                foreach (var l in andonList)
                {
                    //这里是按分钟统计，后面再转换成小时
                    l.HandleTime = logs.Where(p => p.OperateType == AndonManageOperateType.Handle && p.AndonManageId == l.Id)
                        .OrderByDescending(p => p.CreateDate).FirstOrDefault()?.OperateTime;
                    l.ResponseTime = logs.Where(p => p.OperateType == AndonManageOperateType.Response && p.AndonManageId == l.Id)
                        .OrderByDescending(p => p.CreateDate).FirstOrDefault()?.OperateTime;
                    l.CheckTime = logs.Where(p => p.OperateType == AndonManageOperateType.Check && p.AndonManageId == l.Id)
                        .OrderByDescending(p => p.CreateDate).FirstOrDefault()?.OperateTime;
                    l.HandleHour = l.HandleTime == null || l.ResponseTime == null ? 0 :
                        Math.Round((decimal)(l.HandleTime.Value - l.ResponseTime.Value).TotalMinutes, 2);
                    l.ResponseHour = l.ResponseTime == null ? 0 :
                        Math.Round((decimal)(l.ResponseTime.Value - l.TriggerTime).TotalMinutes, 2);
                    l.CheckHour = l.CheckTime == null || l.HandleTime == null ? 0 :
                        Math.Round((decimal)(l.CheckTime.Value - l.HandleTime.Value).TotalMinutes, 2);

                    var data = new AndonStatisticsReportData();
                    data.AndonType = l.AndonType.AndonTypeName;
                    //（验收时间－创建时间），保留2位小数
                    data.AbnormalDowntime = l.CheckTime == null ? 0 :
                        Math.Round((decimal)(l.CheckTime.Value - l.CreateDate).TotalMinutes, 2);
                    data.AvgResponseTime = l.ResponseHour ?? 0;
                    data.AvgHandleTime = l.HandleHour ?? 0;
                    data.AvgCheckTime = l.CheckHour ?? 0;
                    //未关闭的
                    if (l.State != AndonManageState.Closed)
                    {
                        //时长（当前时间-创建时间）
                        data.OverTime = Math.Round((decimal)(now - l.CreateDate).TotalMinutes, 2);
                    }
                    datas.Add(data);
                }
            }
            return datas;
        }

        #endregion

    }
}
