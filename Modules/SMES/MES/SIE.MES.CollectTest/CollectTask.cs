using SIE.Barcodes;
using SIE.Common.Employees;
using SIE.Common.NumberRules;
using SIE.Common.Prints;
using SIE.DataPortal;
using SIE.Domain;
using SIE.Logging;
using SIE.MES.WIP;
using SIE.MES.WIP.Assemblys;
using SIE.MES.WIP.Inspects;
using SIE.MES.WIP.Packings;
using SIE.MES.WorkOrders;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SIE.MES.CollectTest
{
    /// <summary>
    /// 采集任务
    /// </summary>
    public class CollectTask
    {
        List<CollectWorkCell> _workcells = new List<CollectWorkCell>();
        int _invorg;
        DataPortalPrincipal _principal;
        ILog logger = Logging.LogManager.GetLogger("wip");

        /// <summary>
        /// 初始化,返回条码列表
        /// </summary>
        public List<string> Init(int tasks)
        {
            var info = RT.Config.Get<TestContext>("TestContext");
            RT.InvOrg = _invorg = info.InvOrg;
            var emp = GetEmployee(info.EmployeeName);
            RT.Principal = _principal = new DataPortalPrincipal(emp.Id, emp.UserId.Value, info.EmployeeName);
            var tpl = GetPrintTemplate(info.PrintTemplate);
            var wo = GetWorkOrder(info.Workorder);
            var rule = GetNumberRule(info.BarcodeRule);
            var qty = Math.Min(tasks, wo.PlanQty);
            //打印条码
            var barcodes = RT.Service.Resolve<BarcodeController>().Print(new PrinterInfo
            {
                WorkOrderId = wo.Id,
                NumberRuleId = rule.Id,
                PrintTemplateId = tpl.Id,
                SingleQty = 1,
                PrintedQty = 0,
                PrintQty = qty.ConvertTo<int>()
            });
            //读取工序
            foreach (var wc in info.Workcells)
            {
                var process = GetProcess(wc.ProcessName);
                var resource = GetWipResource(wc.ResourceName);
                var station = GetStation(wc.StationName);
                _workcells.Add(new CollectWorkCell
                {
                    ProcessType = process.Type.Value,
                    Workcell = new Workcell
                    {
                        EmployeeId = emp.Id,
                        ProcessId = process.Id,
                        ResourceId = resource.Id,
                        StationId = station.Id
                    }
                });
            }
            return barcodes.Select(p => p.Sn).ToList();
        }

        PrintTemplate GetPrintTemplate(string name)
        {
            Check.NotNullOrEmpty(name, nameof(name));
            CriteriaQuery criteria = new CriteriaQuery()
            {
                EntityType = typeof(PrintTemplate),
                PagingInfo = new PagingInfo { PageNumber = 1, PageSize = 1 }
            };
            criteria.CriteraType = ECriteriaType.ShortCutQuery;
            criteria.SetCriteriaFromString("FileName like '{0}'".FormatArgs(name));
            var template = criteria.GetList().OfType<PrintTemplate>().FirstOrDefault();
            if (template == null)
                throw new EntityNotFoundException("找不到名称为[{0}]的打印模板".FormatArgs(name));
            return template;
        }

        WorkOrder GetWorkOrder(string wo)
        {
            Check.NotNullOrEmpty(wo, nameof(wo));
            var w = RT.Service.Resolve<WorkOrderController>().GetWorkOrder(wo);
            if (w == null)
                throw new EntityNotFoundException("找不到工单[{0}]".FormatArgs(wo));
            return w;
        }

        NumberRule GetNumberRule(string name)
        {
            Check.NotNullOrEmpty(name, nameof(name));
            var rule = RT.Service.Resolve<NumberRuleController>().GetNumberRule(name);
            if (rule == null)
                throw new EntityNotFoundException("找不到名称为[{0}]的编码规则".FormatArgs(name));
            return rule;
        }

        Station GetStation(string name)
        {
            Check.NotNullOrEmpty(name, nameof(name));
            var station = RT.Service.Resolve<StationController>().GetStationByName(name);
            if (station == null)
                throw new EntityNotFoundException("找不到名称为[{0}]的工位".FormatArgs(name));
            return station;
        }

        WipResource GetWipResource(string name)
        {
            Check.NotNullOrEmpty(name, nameof(name));
            var resource = RT.Service.Resolve<WipResourceController>().GetWipResourceByName(name);
            if (resource == null)
                throw new EntityNotFoundException("找不到名称为[{0}]的生产资源".FormatArgs(name));
            return resource;
        }

        Process GetProcess(string name)
        {
            Check.NotNullOrEmpty(name, nameof(name));
            var process = RT.Service.Resolve<ProcessController>().GetProcess(name);
            if (process == null)
                throw new EntityNotFoundException("找不到名称为[{0}]的工序".FormatArgs(name));
            return process;
        }

        Employee GetEmployee(string name)
        {
            Check.NotNullOrEmpty(name, nameof(name));
            CriteriaQuery criteria = new CriteriaQuery()
            {
                EntityType = typeof(Employee),
                PagingInfo = new PagingInfo { PageNumber = 1, PageSize = 1 }
            };
            criteria.CriteraType = ECriteriaType.ShortCutQuery;
            criteria.SetCriteriaFromString("Name like '{0}'".FormatArgs(name));
            var employee = criteria.GetList().OfType<Employee>().FirstOrDefault();
            if (employee == null)
                throw new EntityNotFoundException("找不到名称为[{0}]的员工".FormatArgs(name));
            return employee;
        }

        /// <summary>
        /// 运行要测试的业务
        /// </summary>
        /// <param name="context"></param>
        public void Run(TaskContext context)
        {
            RT.InvOrg = _invorg;
            RT.Principal = _principal;
            context.Stopwatch.Start();
            foreach (var wc in _workcells)
            {
                if (wc.ProcessType == ProcessType.Assembly)
                    RT.Service.Resolve<AssemblyController>().Collect(new[] { context.BarCode }, CollectData.Empty, wc.Workcell);
                else if (wc.ProcessType == ProcessType.Pqc || wc.ProcessType == ProcessType.Fqc)
                    RT.Service.Resolve<InspectController>().Collect(new[] { context.BarCode }, CollectData.Empty, wc.Workcell);//TODO检验需要检验项目和缺陷
                else if (wc.ProcessType == ProcessType.Packing)
                    RT.Service.Resolve<WipPackingController>().Collect(new[] { context.BarCode }, CollectData.Empty, wc.Workcell);//TODO包装还需要生成包装关系
                else
                    RT.Service.Resolve<WipController>().Collect(new[] { context.BarCode }, CollectData.Empty, wc.Workcell);
            }
            context.Stopwatch.Stop();
            logger.Info($"#{Thread.CurrentThread.ManagedThreadId} 完成任务:{context.Id} Collect({context.BarCode}) 耗时{context.Stopwatch.ElapsedMilliseconds}ms");
            Console.WriteLine($"#{Thread.CurrentThread.ManagedThreadId} 完成任务:{context.Id} Collect({context.BarCode}) 耗时{context.Stopwatch.ElapsedMilliseconds}ms");
        }
        class CollectWorkCell
        {
            public ProcessType ProcessType { get; set; }
            public Workcell Workcell { get; set; }
        }
    }

    /// <summary>
    /// 测试上下文，从配置文件读取
    /// </summary>
    public class TestContext
    {
        public string Workorder { get; set; }
        public string EmployeeName { get; set; }
        public string BarcodeRule { get; set; }
        public string PrintTemplate { get; set; }
        public int InvOrg { get; set; }
        public WorkCell[] Workcells { get; set; }
    }

    /// <summary>
    /// 工位信息
    /// </summary>
    public class WorkCell
    {
        public string ProcessName { get; set; }
        public string ResourceName { get; set; }
        public string StationName { get; set; }
    }
}
