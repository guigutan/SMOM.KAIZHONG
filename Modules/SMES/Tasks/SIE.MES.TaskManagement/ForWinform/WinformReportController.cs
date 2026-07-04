using SIE.Api;
using SIE.Barcodes;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Models;
using SIE.MES.TaskManagement.Reports;
using SIE.MES.WorkReportPlans;
using SIE.ProductIntfc.FirstInsps;
using SIE.Resources.Employees;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.TaskManagement.ForWinform
{
    /// <summary>
    /// 任务单报工 WINform接口
    /// </summary>
    public class WinformReportController : ReportController
    {
        /// <summary>
        /// 获取工位
        /// </summary>
        /// <returns></returns>

        [ApiService("获取工位集合")]
        [return: ApiReturn("工位集合")]
        public virtual List<Station> GetStations([ApiParameter("查询关键字")] string keyword)
        {
            return RT.Service.Resolve<StationController>().GetStations(null, keyword).ToList();
        }

        /// <summary>
        /// 根据扫描信息获取员工
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        [ApiService("根据扫描信息获取员工")]
        [return: ApiReturn("员工对象")]
        public virtual Employee GetEmployeeByCode([ApiParameter("报工任务查询信息")] string barcode)
        {
            return RT.Service.Resolve<EmployeeController>().GetEmployeeByCode(barcode);
        }

        /// <summary>
        /// 加载任务单信息
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="employeeCode"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        [ApiService("获取任务单信息")]
        [return: ApiReturn("任务单信息")]
        public virtual DispatchTask GetDispatchTaskByBarcode([ApiParameter("报工任务查询信息")] string barcode, [ApiParameter("报工员工编码")] string employeeCode)
        {

            var reportEmployee = RT.Service.Resolve<EmployeeController>().GetEmployeeByCode(employeeCode);
            if (reportEmployee == null)
            {
                throw new ValidationException("员工编码【{0}】不存在".L10nFormat(barcode));
            }
            return RT.Service.Resolve<DispatchController>().GetDispatchTaskByBarcode(barcode, reportEmployee);//加载任务单信息
        }

        /// <summary>
        /// 获取主报工信息
        /// </summary>
        /// <param name="dispatchTaskId"></param>
        /// <returns></returns>
        [ApiService("获取主报工信息")]
        [return: ApiReturn("任务单Id信息")]

        public virtual ReportRecord GetMainReportRecord([ApiParameter("任务单Id信息")] double dispatchTaskId)
        {
            return RT.Service.Resolve<ReportController>().GetOrCreateMainReportRecord(dispatchTaskId);
        }

        /// <summary>
        /// 提交报工信息
        /// </summary>
        /// <param name="info"></param>
        /// <param name="isReport">是否报工，true报工，false保存</param>
        /// <param name="isGetSyntypeTask">是否重新获取工模任务单信息</param>
        /// <returns></returns>
        [ApiService("提交报工信息")]
        [return: ApiReturn("返回报工记录")]
        public virtual Tuple<ReportRecord, List<ReportRecord>> SubimtTaskReport([ApiParameter("报工信息")] ReportTaskInfo info, [ApiParameter("是否报工")] bool isReport, [ApiParameter("是否重新获取工模任务单信息")] bool isGetSyntypeTask = false)
        {
            var mainRecored = RT.Service.Resolve<ReportController>().TaskReport(info, isReport, isGetSyntypeTask);
            if (mainRecored != null)
            {
                var allRecoreds = RT.Service.Resolve<ReportController>().GetReportRecords(info.TaskId, null).ToList();
                return new Tuple<ReportRecord, List<ReportRecord>>(mainRecored, allRecoreds);
            }
            return null;
        }

        /// <summary>
        /// 开工
        /// </summary>
        /// <param name="curDispatchTaskId">当前派工Id</param>
        /// <exception cref="ValidationException"></exception>
        [ApiService("开工")]
        [return: ApiReturn("返回报工记录")]
        public virtual bool StartWork([ApiParameter("当前报工任务Id")] double curDispatchTaskId)
        {
            var employee = RT.Service.Resolve<EmployeeController>().GetEmployeeById(RT.IdentityId);
            var curDispatchTask = RF.GetById<DispatchTask>(curDispatchTaskId);
            if (curDispatchTask == null)
            {
                throw new ValidationException("当前任务单系统不存在，请检查扫描的任务单号".L10N());
            }
            return RT.Service.Resolve<ReportController>().StartWork(employee, curDispatchTask);
        }


        /// <summary>
        /// 首件报检
        /// </summary>
        /// <param name="curDispatchTaskId"></param>
        /// <param name="inspQty">报检数量</param>
        [ApiService("首件报检")]
        [return: ApiReturn("无")]

        public virtual void DispatchTaskReportFirstInsp([ApiParameter("当前报工任务Id")] double curDispatchTaskId, [ApiParameter("报检数量")] double inspQty)
        {
            if (inspQty <= 0)
            {
                throw new ValidationException("请至少输入报检数量".L10N());
            }

            var curDispatchTask = RF.GetById<DispatchTask>(curDispatchTaskId);

            if (curDispatchTask == null)
            {
                throw new ValidationException("无法找到当前报工任务，请检查".L10N());
            }
            curDispatchTask.InspQty = (int)inspQty;

            RT.Service.Resolve<ReportController>().ReportFirstInsp(curDispatchTask);
        }

        /// <summary>
        /// 获取报工记录打印模板
        /// </summary>
        /// <returns></returns>
        [ApiService("获取报工记录打印模板")]
        [return: ApiReturn("返回报工记录打印模板集合 不分页")]
        public virtual List<PrintTemplate> GetPrintTemplates()
        {
            var labelPrintName = typeof(ReportRecordPrintable).GetQualifiedName();
            return RT.Service.Resolve<BarcodeController>().GetPrintTemplatesByType(labelPrintName, null, "").ToList();
        }

        /// <summary>
        /// 获取员工有权限的工位集合
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [ApiService("获取员工有权限的工位集合")]
        [return: ApiReturn("员工有权限的工位集合 不分页")]
        public virtual List<Station> GetStationsByEmployee([ApiParameter("员工Id")] double employeeId)
        {
            return Query<Station>()
                  .Join<StationProcess>((x, y) => x.Id == y.StationId)
                  .Join<StationProcess, ProcessEmployee>((n, m) => m.ProcessId == m.ProcessId && m.EmployeeId == employeeId)
                  .Distinct().OrderBy(p => p.Code).ToList().ToList();
        }

        /// <summary>
        /// 获取员工有权限且有派工任务的工位
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>

        [ApiService("获取员工有权限的,且工位存在有派工的工位集合")]
        [return: ApiReturn("员工有权限的工位集合 不分页")]
        public virtual List<Station> GetHasTaskStationsByEmployee([ApiParameter("员工Id")] double employeeId)
        {
            List<Station> stations = GetStationsByEmployee(employeeId);
            if (stations.Count > 0)
            {
                var ids = stations.Select(x => x.Id).ToList();
                var result = Query<Station>().Join<DispatchTaskDetail>((x, y) => x.Id == y.AdoId 
                     && y.AdoType == AdoType.Station
                     &&ids.Contains(x.Id)).Join<DispatchTaskDetail, DispatchTask>((n, k) => n.DispatchTaskId == k.Id
                    && k.TaskStatus != DispatchTaskStatus.Closed
                    && k.TaskStatus != DispatchTaskStatus.Finished).Distinct().OrderBy(p => p.Code).ToList().ToList();
                return result;
            }
            return new List<Station>();
        }


        /// <summary>
        /// 根据员工ID、员工组、班组获取派工中、已派工 派工任务
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [ApiService("根据员工ID、员工组、班组获取派工中、已派工、暂停的 派工任务")]
        [return: ApiReturn("返回派工任务 不分页")]

        public virtual List<DispatchTask> GetDispatchTasksByEmpId([ApiParameter("员工Id")] double employeeId)
        {
            var employee = RF.GetById<Employee>(employeeId);
            if (employee == null)
                throw new EntityNotFoundException(typeof(Employee), employeeId);
            var query = Query<DispatchTask>()
                .Exists<DispatchTaskDetail>((d, p) => p.Where(f => f.DispatchTaskId == d.Id
                && ((f.AdoType == AdoType.Employee && f.AdoId == employeeId)
                || (f.AdoType == AdoType.WorkGroup && f.AdoId == employee.WorkGroupId)
                || (f.AdoType == AdoType.EmployeeGroup && f.AdoId == employee.EmployeeGroupId))))
                .Where(p => (p.TaskStatus != DispatchTaskStatus.Finished && p.TaskStatus != DispatchTaskStatus.Closed && p.TaskStatus != DispatchTaskStatus.ToDispatch));
            var tasks = query.OrderByDescending(m=>m.PlanEndTime).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            return tasks.ToList();
        }

        /// <summary>
        /// 按工位获取获取派工中、已派工、暂停的 派工任务
        /// </summary>
        /// <param name="stationId">工位Id</param>
        /// <returns></returns>
        [ApiService("按工位获取获取派工中、已派工、暂停的 派工任务")]
        [return: ApiReturn("返回派工任务 不分页")]
        public virtual List<DispatchTask> GetDispatchTaskByStationId([ApiParameter("工位Id")]  double stationId)
        {
            var results = Query<DispatchTask>()
              .Exists<DispatchTaskDetail>((d, p) => p.Where(f => f.DispatchTaskId == d.Id
              && ((f.AdoType == AdoType.Station && f.AdoId == stationId))))
              .Where(p => (p.TaskStatus != DispatchTaskStatus.Finished && p.TaskStatus != DispatchTaskStatus.Closed && p.TaskStatus != DispatchTaskStatus.ToDispatch))
              .OrderByDescending(m=>m.PlanEndTime).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            return results.ToList();
        }

        /// <summary>
        /// 根据工序获取报工方案
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        [ApiService("根据工序获取报工方案模板")]
        [return: ApiReturn("返回报工方案模板")]
        public virtual WorkReportPlan GetWorkReportPlanByProcessId([ApiParameter("工序Id")]  double processId)
        {
            var result = Query<WorkReportPlan>()
              .Exists<ProcessInfo>((d, p) => p.Where(f => f.WorkReportPlanId == d.Id && f.ProcessId == processId))
              .Where(m => m.EnableStatus == true)
              .FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            return result;
        }

        /// <summary>
        /// 暂停派工任务
        /// </summary>
        /// <param name="dispatchTaskId"></param>
        /// <exception cref="ValidationException"></exception>
        [ApiService("暂停派工任务")]
        [return: ApiReturn("")]
        public virtual void PauseDispatchTask([ApiParameter("任务单Id")] double dispatchTaskId)
        {

            var dispatchTask = RF.GetById<DispatchTask>(dispatchTaskId);
            if (dispatchTask == null)
            {
                throw new ValidationException("系统不存在当前派工单，请检查".L10N());
            }
            if (dispatchTask.TaskStatus != DispatchTaskStatus.Executing)
            {
                throw new ValidationException("只有状态为执行中的派工单才能暂停，请检查".L10N());
            }
            dispatchTask.TaskStatus = DispatchTaskStatus.Pause;
            RF.Save(dispatchTask);
        }
        /// <summary>
        /// 执行派工任务
        /// </summary>
        /// <param name="dispatchTaskId"></param>
        /// <exception cref="ValidationException"></exception>
        [ApiService("执行派工任务")]
        [return: ApiReturn("")]
        public virtual void StartDispatchTask([ApiParameter("任务单Id")] double dispatchTaskId)
        {

            var dispatchTask = RF.GetById<DispatchTask>(dispatchTaskId);
            if (dispatchTask == null)
            {
                throw new ValidationException("系统不存在当前派工单，请检查".L10N());
            }
            if (dispatchTask.TaskStatus != DispatchTaskStatus.Pause)
            {
                throw new ValidationException("只有状态为暂停的派工单才能执行，请检查".L10N());
            }
            dispatchTask.TaskStatus = DispatchTaskStatus.Executing;
            RF.Save(dispatchTask);
        }


        /// <summary>
        /// 派工任务完工确认
        /// </summary>
        /// <param name="dispatchTaskId"></param>
        /// <exception cref="ValidationException"></exception>
        [ApiService("派工任务完工确认")]
        [return: ApiReturn("")]
        public virtual void FinishDispatchTask([ApiParameter("任务单Id")]  double dispatchTaskId)
        {
            //当前报工数量大于或等于派工任务数量，这个时候才能点完工确认；
            var dispatchTask = RF.GetById<DispatchTask>(dispatchTaskId);
            if (dispatchTask == null)
            {
                throw new ValidationException("系统不存在当前派工单，请检查".L10N());
            }
            ReportFinish(new List<double>() { dispatchTaskId });
        }


        /// <summary>
        /// 开工
        /// </summary>
        /// <param name="dispatchTaskId">派工Id</param>
        /// <param name="employeeId">操作员Id</param>
        [ApiService("开工")]
        [return: ApiReturn("")]
        public virtual void WinXpStartWork([ApiParameter("任务单Id")] double dispatchTaskId, [ApiParameter("员工Id")] double employeeId)
        {
            var newdispatchTask = RF.GetById<DispatchTask>(dispatchTaskId);
            if (newdispatchTask == null)
            {
                throw new ValidationException("当前派工单据不存在，请检查".L10N());
            }

            var employee = RF.GetById<Employee>(employeeId);//当前员工
            if (employee == null)
            {
                throw new ValidationException("当前报工员工系统不存在，请检查".L10N());
            }
            //开始开工
            StartWork(employee, newdispatchTask);

        }
        [ApiService("获取任务单信息")]
        [return: ApiReturn("任务单信息")]
        public virtual DispatchTask GetDispatchTaskById([ApiParameter("报工任务查询信息")] double Id)
        {

            var dispatchTask = RF.GetById<DispatchTask>(Id, new EagerLoadOptions().LoadWithViewProperty());
            if (dispatchTask == null)
            {
                throw new ValidationException("报工信息不存在".L10N());
            }
            return dispatchTask;
        }

        /// <summary>
        /// 获取任务单首件报检状态
        /// </summary>
        /// <param name="woId"></param>
        /// <param name="dispatchTaskNo"></param>
        /// <returns></returns>
        [ApiService("获取任务单首件报检状态")]
        [return: ApiReturn("状态信息")]
        public virtual string GetDispatchTaskFirstInsp([ApiParameter("工单Id")] double woId, [ApiParameter("任务单号")] string dispatchTaskNo)
        {

            var firstInsp = Query<FirstInsp>().Where(m => m.WorkOrderId == woId && m.DispatchTaskNo == dispatchTaskNo).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            if (firstInsp != null && firstInsp.InspectionResult.HasValue)
            {
                return firstInsp.InspectionResult.ToLabel();
            }
            return "未进行".L10N();
        }

        /// <summary>
        /// 获取工位主设备名称
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        [ApiService("获取工位主设备名称")]
        [return: ApiReturn("工位主设备名称")]
        public virtual string GetStationMainEquipment([ApiParameter("工位Id")] double stationId)
        {
            var equipment = Query<StationEquipment>().Where(m => m.StationId == stationId && m.IsMaster).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            return equipment?.EquipAccountName;
        }

    }
}
