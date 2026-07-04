using SIE.Common.Configs;
using SIE.Domain;
using SIE.Domain.Serialization.Json;
using SIE.EventMessages.Release;
using SIE.Items;
using SIE.MES.Interfaces.TaskManages;
using SIE.MES.TaskManagement.Configs;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Dispatchs.ViewModels;
using SIE.MES.TaskManagement.Interfaces;
using SIE.MES.TaskManagement.Reports;
using SIE.MES.TaskManagement.ShowBoards.ViewModels;
using SIE.MES.TaskManagement.TaskConfigs;
using SIE.MES.WorkOrders;
using SIE.Resources.Employees;
using SIE.xUnit.Core;
using SIE.xUnit.Items;
using SIE.xUnit.MES;
using SIE.xUnit.Resources;
using SIE.xUnit.Techs;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SIE.xUnit.TaskManagement.Dispatchs
{
    /// <summary>
    /// 派工管理测试控制器
    /// </summary>
    public class DispatchControllerTest : IClassFixture<TestStarup>
    {
        static TechTestController tsTechCt = RT.Service.Resolve<TechTestController>();
        static MesTestController tsMesCt = RT.Service.Resolve<MesTestController>();
        static ItemTestController tsItemCt = RT.Service.Resolve<ItemTestController>();
        static ContextControllerTest tsContextCt = RT.Service.Resolve<ContextControllerTest>();
        static DispatchController taskCt = RT.Service.Resolve<DispatchController>();
        static ApsTaskController aspTaskCt = RT.Service.Resolve<ApsTaskController>();
        static ResTestController resTestCt = RT.Service.Resolve<ResTestController>();
        static TaskConfigController taskConfigCt = RT.Service.Resolve<TaskConfigController>();
        static TaskManagementTestController taskTestCt = RT.Service.Resolve<TaskManagementTestController>();

        /// <summary>
        /// 工单生成任务单
        /// </summary>
        /// <param name="isCommonMode">是否共模</param>
        /// <param name="isMainMaterial">是否主料</param>
        /// <param name="planNo">计划单号</param>
        /// <param name="productionOrderCode">制程工艺单</param>
        /// <param name="isGenerate">是否生成工单任务单</param>
        /// <param name="generateMode">任务单生成方式</param>
        /// <param name="isAccordConfig">共模主料工单的辅料工单已存在生成的任务单，如【true】则生成不按共模比的任务单，否【false】则不生成任务单</param>
        /// <param name="isGenerateTask">工艺路线工序是否生成任务单</param>
        /// <param name="isVirtualPart">工单产品Bom对应产品是否虚拟件</param>
        /// <param name="reportMode">报工方式</param>
        /// <param name="isSyntype">是否共模报工</param>
        /// <param name="isModReport">是否剩余数报工</param>
        /// <param name="reportQty">报工数量</param>
        /// <param name="byProcess">按照工序生成任务单</param>
        /// <param name="bySpecification">按照规格件生成任务单</param>
        /// <param name="byQty">按照固定数量生成任务单</param>
        /// <param name="qty">固定数量</param>
        /// <param name="byVirtualPart">是否生成虚拟件任务</param>
        /// <param name="count">虚拟物料的数量/规格件的数量</param>
        /// <returns>工单</returns>
        [Theory]
        [InlineData(false, false, "", "", true, ReportMode.Manual, false, true, false, ReportMode.Manual, false, false, 5, true, false, false, 0, false, 0)]
        [InlineData(false, false, "", "", true, ReportMode.Manual, false, true, false, ReportMode.Manual, false, false, 5, false, false, true, 100, false, 0)]
        public WorkOrder WorkOrderGenerateTaskBillsTest(bool isCommonMode, bool isMainMaterial, string planNo, string productionOrderCode, bool isGenerate, ReportMode generateMode, bool isAccordConfig, bool isGenerateTask, bool isVirtualPart, ReportMode reportMode, bool isSyntype, bool isModReport, decimal reportQty, bool byProcess, bool bySpecification, bool byQty, decimal qty, bool byVirtualPart, int count)
        {
            tsContextCt.InitContext();
            //当前选中工单
            var orgIsSyntype = false;
            var newIsSyntype = false;
            WorkOrder workOrder = null;
            Item product = null;
            ProductFamily family = null;
            if (isCommonMode)
            {
                var workOrders = new List<WorkOrder>();
                var products = new List<Item>();
                var familys = new List<ProductFamily>();
                taskTestCt.CreateCommonWorkOrderRelateInfos(isCommonMode, planNo, productionOrderCode, isGenerateTask, isVirtualPart, reportMode, isSyntype, isModReport, reportQty, byProcess, bySpecification, byQty, qty, byVirtualPart, count, family, workOrders, products, familys);

                if (isMainMaterial)
                {
                    workOrder = workOrders.FirstOrDefault(p => p.IsMainMaterial);
                    Assert.NotNull(workOrder);
                    product = products.FirstOrDefault(p => p.Id == workOrder.ProductId);
                    Assert.NotNull(product);
                    family = familys.FirstOrDefault(p => p.Id == product.ProductFamilyId);
                    Assert.NotNull(family);
                }
                else
                {
                    workOrder = workOrders.FirstOrDefault(p => !p.IsMainMaterial);
                    Assert.NotNull(workOrder);
                    product = products.FirstOrDefault(p => p.Id == workOrder.ProductId);
                    Assert.NotNull(product);
                    family = familys.FirstOrDefault(p => p.Id == product.ProductFamilyId);
                    Assert.NotNull(family);
                }
            }
            else
            {
                family = tsItemCt.CreateProductFamily(1).FirstOrDefault();
                Assert.NotNull(family);
                product = tsItemCt.CreateTaskProduct(false, family.Id);
                Assert.NotNull(product);
                if (bySpecification)
                {
                    var proSpecification = taskTestCt.CreateProductSpecification(product.Id, count);
                    Assert.NotNull(proSpecification);
                }

                //创建车间和产线
                var wipResource = resTestCt.GetOrCreateWipResource();
                Assert.NotNull(wipResource);
                var routing = tsTechCt.CreateTaskRouting(isGenerateTask, wipResource);
                Assert.NotNull(routing);
                workOrder = tsMesCt.CreateWorkOrder(wipResource, product.Id, routing.Id, false, false, 0, 0, "", "", isVirtualPart, count);
                Assert.NotNull(workOrder);
                //产品族报工参数配置
                var reportRuleConfig = taskTestCt.CreateReportRuleConfig(family.Id, reportMode, isSyntype, isModReport, reportQty);
                Assert.NotNull(reportRuleConfig);
                //产品族任务单生成配置项
                var taskConfig = taskTestCt.CreateTaskConfig(family.Id, byProcess, bySpecification, byQty, qty, byVirtualPart);
                Assert.NotNull(taskConfig);
            }

            var taskConfig1 = taskConfigCt.GetTaskConfig(family.Id);
            Assert.NotNull(taskConfig1);

            var familyTaskConfig = new FamilyTaskConfig();
            familyTaskConfig.FamilyId = family.Id;
            familyTaskConfig.Config = new TaskConfigInfo()
            {
                ByProcess = taskConfig1.ByProcess,
                BySpecification = taskConfig1.BySpecification,
                ByQty = taskConfig1.ByQty,
                ByVirtualPart = taskConfig1.ByVirtualPart,
                Qty = taskConfig1.Qty,
                Family = 0
            };

            taskConfigCt.SaveTaskConfigs(new List<FamilyTaskConfig>() { familyTaskConfig });
            taskConfigCt.ValidateTaskConfigs(new List<FamilyTaskConfig>() { familyTaskConfig });

            // 设置任务单全局配置项
            var globalTaskConfig = taskCt.GetDispatchTaskConfig();
            if (globalTaskConfig == null)
            {
                globalTaskConfig = new DispatchTaskConfigValue();
                globalTaskConfig.NumberRule = tsContextCt.CreateNumberRule("任务单号生成规则");
                globalTaskConfig.IsGenerate = isGenerate;
                globalTaskConfig.GenerateMode = generateMode;
            }
            else
            {
                globalTaskConfig.IsGenerate = isGenerate;
                globalTaskConfig.GenerateMode = generateMode;
                if (globalTaskConfig.NumberRule == null)
                    globalTaskConfig.NumberRule = tsContextCt.CreateNumberRule("任务单号生成规则");
            }

            string value = DomainJsonConvert.SerializeObject(globalTaskConfig, ConfigValueSerializerSettings.Settings);
            var config = taskTestCt.CreateGlobalConfig("G", typeof(DispatchTaskConfig).GetQualifiedName(), value);
            Assert.NotNull(config);

            var taskNo = taskCt.GetTaskNo(globalTaskConfig);
            Assert.NotEmpty(taskNo);

            var taskNos = taskCt.GetTaskNo(globalTaskConfig, 3);
            Assert.Equal(3, taskNos.Count);

            var taskNo1 = taskCt.GetDispatchTaskNo();
            Assert.NotEmpty(taskNo1);

            var configValue = taskCt.GetDispatchTaskConfigValue();
            Assert.NotNull(configValue);

            var dispatchConfigValue = taskCt.GetDispatchConfig();
            Assert.NotNull(dispatchConfigValue);

            var templates = taskCt.GetPrintTemplates(globalTaskConfig.NumberRuleId.Value, typeof(DispatchTaskBillPrintable).GetQualifiedName(), "任务单打印", new PagingInfo());
            Assert.NotNull(templates);

            Assert.NotNull(workOrder);
            if (workOrder.IsCommonMode && workOrder.IsMainMaterial)
            {
                var reportRule = RT.Service.Resolve<ReportController>().GetReportRuleConfig(family.Id);
                Assert.NotNull(reportRule);
                if (reportRule.IsSyntype)
                {
                    newIsSyntype = true;
                    orgIsSyntype = true;
                }
                if (RT.Service.Resolve<DispatchController>().IsExistDispatchTask(workOrder.PlanNo))
                    newIsSyntype = false;
            }

            if (orgIsSyntype && !newIsSyntype)
            {
                if (isAccordConfig)
                    taskTestCt.GenerateDispatchTasks(globalTaskConfig, workOrder, false);
            }
            else
                taskTestCt.GenerateDispatchTasks(globalTaskConfig, workOrder, true);
            return workOrder;
        }

        [Theory]
        [InlineData(false, false, "", "", true, ReportMode.Manual, false, false, true, ReportMode.Manual, false, false, 5, false, false, false, 0, true, 3)]
        public void GenerateTasksByVirtualPart(bool isCommonMode, bool isMainMaterial, string planNo, string productionOrderCode, bool isGenerate, ReportMode generateMode, bool isAccordConfig, bool isGenerateTask, bool isVirtualPart, ReportMode reportMode, bool isSyntype, bool isModReport, decimal reportQty, bool byProcess, bool bySpecification, bool byQty, decimal qty, bool byVirtualPart, int count)
        {
            tsContextCt.InitContext();
            var tasks = taskTestCt.GenerateTaskBillAndDispatchTasks(isCommonMode, isMainMaterial, planNo, productionOrderCode, isGenerate, generateMode, isAccordConfig, isGenerateTask, isVirtualPart, reportMode, isSyntype, isModReport, reportQty, byProcess, bySpecification, byQty, qty, byVirtualPart, count);
            Assert.Equal(count + 1, tasks.Count);
        }

        [Theory]
        [InlineData(false, false, "", "", true, ReportMode.Manual, false, true, false, ReportMode.Manual, false, false, 5, true, false, false, 0, false, 3)]
        public void GenerateTasksByProcess(bool isCommonMode, bool isMainMaterial, string planNo, string productionOrderCode, bool isGenerate, ReportMode generateMode, bool isAccordConfig, bool isGenerateTask, bool isVirtualPart, ReportMode reportMode, bool isSyntype, bool isModReport, decimal reportQty, bool byProcess, bool bySpecification, bool byQty, decimal qty, bool byVirtualPart, int count)
        {
            tsContextCt.InitContext();
            var tasks = taskTestCt.GenerateTaskBillAndDispatchTasks(isCommonMode, isMainMaterial, planNo, productionOrderCode, isGenerate, generateMode, isAccordConfig, isGenerateTask, isVirtualPart, reportMode, isSyntype, isModReport, reportQty, byProcess, bySpecification, byQty, qty, byVirtualPart, count);
            Assert.Equal(4, tasks.Count);
        }

        [Theory]
        [InlineData(false, false, "", "", true, ReportMode.Manual, false, false, false, ReportMode.Manual, false, false, 5, false, true, false, 0, false, 3)]
        public void GenerateTasksBySpecification(bool isCommonMode, bool isMainMaterial, string planNo, string productionOrderCode, bool isGenerate, ReportMode generateMode, bool isAccordConfig, bool isGenerateTask, bool isVirtualPart, ReportMode reportMode, bool isSyntype, bool isModReport, decimal reportQty, bool byProcess, bool bySpecification, bool byQty, decimal qty, bool byVirtualPart, int count)
        {
            tsContextCt.InitContext();
            var tasks = taskTestCt.GenerateTaskBillAndDispatchTasks(isCommonMode, isMainMaterial, planNo, productionOrderCode, isGenerate, generateMode, isAccordConfig, isGenerateTask, isVirtualPart, reportMode, isSyntype, isModReport, reportQty, byProcess, bySpecification, byQty, qty, byVirtualPart, count);
            Assert.Equal(count, tasks.Count);
        }

        [Theory]
        [InlineData(false, false, "", "", true, ReportMode.Manual, false, false, false, ReportMode.Manual, false, false, 5, false, false, true, 100, false, 3)]
        public void GenerateTasksByQty(bool isCommonMode, bool isMainMaterial, string planNo, string productionOrderCode, bool isGenerate, ReportMode generateMode, bool isAccordConfig, bool isGenerateTask, bool isVirtualPart, ReportMode reportMode, bool isSyntype, bool isModReport, decimal reportQty, bool byProcess, bool bySpecification, bool byQty, decimal qty, bool byVirtualPart, int count)
        {
            tsContextCt.InitContext();
            var tasks = taskTestCt.GenerateTaskBillAndDispatchTasks(isCommonMode, isMainMaterial, planNo, productionOrderCode, isGenerate, generateMode, isAccordConfig, isGenerateTask, isVirtualPart, reportMode, isSyntype, isModReport, reportQty, byProcess, bySpecification, byQty, qty, byVirtualPart, count);
            Assert.Equal(3, tasks.Count);
        }

        [Fact]
        public void GenerateCommonMainTasks()
        {
            tsContextCt.InitContext();
            var tasks = taskTestCt.GenerateTaskBillAndDispatchTasks(true, true, taskCt.GetDispatchTaskNo(), taskCt.GetDispatchTaskNo() + "001", true, ReportMode.Manual, false, true, false, ReportMode.Manual, true, false, 5, true, false, false, 0, false, 3);
            Assert.Equal(4, tasks.Count);
        }

        [Fact]
        public void GenerateCommonTasks()
        {
            tsContextCt.InitContext();
            var tasks = taskTestCt.GenerateTaskBillAndDispatchTasks(true, false, taskCt.GetDispatchTaskNo(), taskCt.GetDispatchTaskNo() + "001", true, ReportMode.Manual, false, true, false, ReportMode.Manual, false, false, 5, true, false, false, 0, false, 3);
            Assert.Equal(4, tasks.Count);
        }

        [Theory]
        [InlineData(false, false, "", "", true, ReportMode.Manual, false, true, true, ReportMode.Manual, false, false, 5, true, true, true, 100, true, 3)]
        public void GenerateTasks(bool isCommonMode, bool isMainMaterial, string planNo, string productionOrderCode, bool isGenerate, ReportMode generateMode, bool isAccordConfig, bool isGenerateTask, bool isVirtualPart, ReportMode reportMode, bool isSyntype, bool isModReport, decimal reportQty, bool byProcess, bool bySpecification, bool byQty, decimal qty, bool byVirtualPart, int count)
        {
            tsContextCt.InitContext();
            var tasks = taskTestCt.GenerateTaskBillAndDispatchTasks(isCommonMode, isMainMaterial, planNo, productionOrderCode, isGenerate, generateMode, isAccordConfig, isGenerateTask, isVirtualPart, reportMode, isSyntype, isModReport, reportQty, byProcess, bySpecification, byQty, qty, byVirtualPart, count);
            //默认可生成任务的工序有四个，规格件3个，按固定数量是3，最后虚拟件是3
            Assert.Equal(4*3*3+3, tasks.Count);
        }

        /// <summary>
        /// 共模任务单，主料工单生成任务单，且报工方式是共模比报工
        /// </summary>
        [Fact]
        public List<double> GenerateMainTasks()
        {
            tsContextCt.InitContext();
            var tasks = taskTestCt.GenerateTaskBillAndDispatchTasks(true, true, taskCt.GetDispatchTaskNo(), taskCt.GetDispatchTaskNo() + "001", true, ReportMode.Manual, false, true, true, ReportMode.Manual, true, false, 5, true, true, true, 100, true, 3);
            //默认可生成任务的工序有四个，按固定数量是3
            Assert.Equal(4 * 3, tasks.Count);

            var selectedIds = tasks.Select(p => p.Id).Distinct().ToList();
            return selectedIds;
        }

        /// <summary>
        /// 共模任务单，辅料工单生成任务单，且报工方式是非共模比报工
        /// </summary>
        [Fact]
        public void GenerateNotMainTasks()
        {
            tsContextCt.InitContext();
            var tasks = taskTestCt.GenerateTaskBillAndDispatchTasks(true, false, taskCt.GetDispatchTaskNo(), taskCt.GetDispatchTaskNo() + "001", true, ReportMode.Manual, false, true, true, ReportMode.Manual, false, false, 5, true, true, true, 100, true, 3);
            //默认可生成任务的工序有四个，按固定数量是3
            Assert.Equal(4 * 3, tasks.Count);
        }

        /// <summary>
        /// 对所有待派工和派工中的主任务单、共模辅料工单非共模比报工任务单派工
        /// </summary>
        [Fact]
        public List<double> DispatchTasks()
        {
            var selectedIds = new List<double>();
            tsContextCt.InitContext();
            var workOrder = WorkOrderGenerateTaskBillsTest(false, false, "", "", true, ReportMode.Manual, false, true, false, ReportMode.Manual, false, false, 5, true, false, false, 0, false, 0);
            var tasks = taskTestCt.GetMainDispatchTaskList(workOrder.Id);
            Assert.All(tasks, p => Assert.True(p.IsMainTask));
            var employee = RF.GetById<Employee>(RT.IdentityId);
            if (employee == null)
            {
                var workGroups = taskTestCt.CreateWorkGroups(1);
                var employeeGroups = taskTestCt.CreateEmployeeGroups(1);
                var employees = taskTestCt.CreateEmployees(workGroups.FirstOrDefault().Id, employeeGroups.FirstOrDefault().Id, 1);
                employee = employees.FirstOrDefault();
            }
            else
            {
                if (!employee.WorkGroupId.HasValue)
                {
                    var workGroups = taskTestCt.CreateWorkGroups(1);
                    employee.WorkGroupId = workGroups.FirstOrDefault().Id;
                    RF.Save(employee);
                }

                if (!employee.EmployeeGroupId.HasValue)
                {
                    var employeeGroups = taskTestCt.CreateEmployeeGroups(1);
                    employee.EmployeeGroupId = employeeGroups.FirstOrDefault().Id;
                    RF.Save(employee);
                }
            }

            foreach (var task in tasks)
            {
                taskTestCt.CreateDispatchTaskDetails(1, task, employee);
            }

            selectedIds.AddRange(tasks.Select(p => p.Id).Distinct().ToList());
            taskCt.DispatchTasks(selectedIds);
            return selectedIds;
        }

        [Theory]
        [InlineData(false, false, false)]
        [InlineData(false, true, false)]
        [InlineData(false, true, true)]
        [InlineData(true, true, true)]
        public List<double> DispatchMainTasks(bool isCheckEmployeeSkill, bool isCheckMaterialKitting, bool isCheckPersonnelPermission)
        {
            tsContextCt.InitContext();
            var selectedIds = GenerateMainTasks();
            var dispatchConfig = taskCt.GetDispatchConfig();
            if (dispatchConfig == null)
                dispatchConfig = new DispatchConfigValue();
            dispatchConfig.IsCheckEmployeeSkill = isCheckEmployeeSkill;
            dispatchConfig.IsCheckMaterialKitting = isCheckMaterialKitting;
            dispatchConfig.IsCheckPersonnelPermission = isCheckPersonnelPermission;
            string value = DomainJsonConvert.SerializeObject(dispatchConfig, ConfigValueSerializerSettings.Settings);
            tsContextCt.CreateConfig(typeof(DispatchTask).GetQualifiedName(), typeof(DispatchConfig).GetQualifiedName(), value);

            taskCt.DispatchTasks(selectedIds);
            return selectedIds;
        }

        /// <summary>
        /// 对所有待派工和派工中的主任务单、共模辅料工单非共模比报工任务单取消派工
        /// </summary>
        [Fact]
        public void CancelDispatchTasks()
        {
            tsContextCt.InitContext();
            var selectedIds = DispatchTasks();
            taskCt.CancelDispatchTasks(selectedIds);
        }

        [Theory]
        [InlineData(false, false, false)]
        [InlineData(false, true, false)]
        [InlineData(false, true, true)]
        [InlineData(true, true, true)]
        public void CancelMainDispatchTasks(bool isCheckEmployeeSkill, bool isCheckMaterialKitting, bool isCheckPersonnelPermission)
        {
            tsContextCt.InitContext();
            var selectedIds = DispatchMainTasks(isCheckEmployeeSkill, isCheckMaterialKitting, isCheckPersonnelPermission);
            taskCt.CancelDispatchTasks(selectedIds);
        }

        /// <summary>
        /// 合并任务单
        /// </summary>
        [Fact]
        public List<double> MergeTasks()
        {
            var selectedIds = new List<double>();
            tsContextCt.InitContext();
            var workOrder = WorkOrderGenerateTaskBillsTest(false, false, "", "", true, ReportMode.Manual, false, true, false, ReportMode.Manual, false, false, 5, false, false, true, 100, false, 0);
            var tasks = taskTestCt.GetMainDispatchTaskList(workOrder.Id);
            Assert.All(tasks, p => Assert.True(p.IsMainTask));
            Assert.Equal(3, tasks.Count);
            selectedIds.AddRange(tasks.Select(p => p.Id).Distinct().ToList());
            taskCt.MergeDispatchTasks(selectedIds);

            var associatedTasks = taskCt.GetAssociatedTasks(p => selectedIds.Contains(p.TaskId));
            Assert.Equal(selectedIds.Count, associatedTasks.Count);
            return selectedIds;
        }

        /// <summary>
        /// 取消合并任务单
        /// </summary>
        [Fact]
        public void CancelMergeTasks()
        {
            tsContextCt.InitContext();
            var selectedIds = MergeTasks();
            var mainTaskIds = taskTestCt.GetMainDispatchTaskId(selectedIds);
            Assert.Single(mainTaskIds);

            var associatedTasks = taskCt.GetAssociatedTaskList(mainTaskIds);
            Assert.Equal(selectedIds.Count, associatedTasks.Count);

            var associatedTasks1 = taskCt.GetAssociatedTaskListByTaskId(selectedIds.FirstOrDefault());
            Assert.NotNull(associatedTasks1);

            var associatedTasks2 = taskCt.GetAssociatedTaskListByTask(selectedIds);
            Assert.NotNull(associatedTasks2);

            var associatedTask = taskCt.GetAssociatedTaskByMergeTaskId(selectedIds.FirstOrDefault());
            Assert.NotNull(associatedTask);

            var associatedTask3 = taskCt.GetAssociatedDispatchTaskList(mainTaskIds.FirstOrDefault());
            Assert.NotNull(associatedTask3);

            var associatedTask4 = taskCt.GetAssociatedTasksOfMergedTask(mainTaskIds);
            Assert.NotNull(associatedTask4);

            var associatedTask5 = taskCt.GetMergeChildDispatchTask(mainTaskIds.FirstOrDefault());
            Assert.NotNull(associatedTask5);

            taskCt.CancelMergeDispatchTasks(mainTaskIds);
        }

        /// <summary>
        /// 拆分任务单
        /// </summary>
        /// <param name="splitQty">拆分数量</param>
        [Theory]
        [InlineData(20)]
        public void SplitTasks(int splitQty)
        {
            tsContextCt.InitContext();
            var workOrder = WorkOrderGenerateTaskBillsTest(false, false, "", "", true, ReportMode.Manual, false, true, false, ReportMode.Manual, false, false, 5, false, false, true, 100, false, 0);
            var tasks = taskTestCt.GetMainDispatchTaskList(workOrder.Id);
            Assert.All(tasks, p => Assert.True(p.IsMainTask));
            Assert.NotEmpty(tasks);
            var task = tasks.FirstOrDefault();
            var splitTaskVM = new SplitTaskViewModel()
            {
                Qty = splitQty,
                DispatchQty = task.DispatchQty,
                NgQty = 0,
                ReportQty = 0,
                DispatchTaskId = task.Id,
            };

            taskCt.SplitDispatchTask(splitTaskVM);
        }

        /// <summary>
        /// 设置紧急
        /// </summary>
        [Fact]
        public List<double> SetUrgentTasks()
        {
            var selectedIds = new List<double>();
            tsContextCt.InitContext();
            var workOrder = WorkOrderGenerateTaskBillsTest(false, false, "", "", true, ReportMode.Manual, false, true, false, ReportMode.Manual, false, false, 5, false, false, true, 100, false, 0);
            var tasks = taskTestCt.GetMainDispatchTaskList(workOrder.Id);
            Assert.All(tasks, p => Assert.True(p.IsMainTask));
            selectedIds.AddRange(tasks.Select(p => p.Id).Distinct().ToList());
            taskCt.SetUrgentTasks(selectedIds);
            return selectedIds;
        }

        /// <summary>
        /// 设置普通
        /// </summary>
        [Fact]
        public void SetNormalTasks()
        {
            tsContextCt.InitContext();
            var selectedIds = SetUrgentTasks();
            taskCt.SetNormalTasks(selectedIds);
        }

        /// <summary>
        /// 设置暂停
        /// </summary>
        [Fact]
        public List<double> PauseTasks()
        {
            var selectedIds = new List<double>();
            tsContextCt.InitContext();
            var workOrder = WorkOrderGenerateTaskBillsTest(false, false, "", "", true, ReportMode.Manual, false, true, false, ReportMode.Manual, false, false, 5, false, false, true, 100, false, 0);
            var tasks = taskTestCt.GetMainDispatchTaskList(workOrder.Id);
            Assert.All(tasks, p => Assert.True(p.IsMainTask));
            selectedIds.AddRange(tasks.Select(p => p.Id).Distinct().ToList());
            taskCt.SetPauseTasks(selectedIds);
            return selectedIds;
        }

        /// <summary>
        /// 恢复暂停
        /// </summary>
        [Fact]
        public void ResumeTasks()
        {
            tsContextCt.InitContext();
            var selectedIds = PauseTasks();
            taskCt.SetResumeTasks(selectedIds);
        }

        /// <summary>
        /// 强制关闭
        /// </summary>
        [Fact]
        public void CloseTasks()
        {
            tsContextCt.InitContext();
            var selectedIds = PauseTasks();
            taskCt.SetCloseTasks(selectedIds);
        }

        [Fact]
        public void TestGetDispatchTasks()
        {
           tsContextCt.InitContext();
            var workOrder = WorkOrderGenerateTaskBillsTest(false, false, "", "", true, ReportMode.Manual, false, true, false, ReportMode.Manual, false, false, 5, false, false, true, 100, false, 0);
            var tasks = taskTestCt.GetMainDispatchTaskList(workOrder.Id);
            Assert.All(tasks, p => Assert.True(p.IsMainTask));
            var employee = RF.GetById<Employee>(RT.IdentityId);
            if (employee == null)
            {
                var workGroups = taskTestCt.CreateWorkGroups(1);
                var employeeGroups = taskTestCt.CreateEmployeeGroups(1);
                var employees = taskTestCt.CreateEmployees(workGroups.FirstOrDefault().Id, employeeGroups.FirstOrDefault().Id, 1);
                employee = employees.FirstOrDefault();
            }
            else
            {
                if (!employee.WorkGroupId.HasValue)
                {
                    var workGroups = taskTestCt.CreateWorkGroups(1);
                    employee.WorkGroupId = workGroups.FirstOrDefault().Id;
                    RF.Save(employee);
                }

                if (!employee.EmployeeGroupId.HasValue)
                {
                    var employeeGroups = taskTestCt.CreateEmployeeGroups(1);
                    employee.EmployeeGroupId = employeeGroups.FirstOrDefault().Id;
                    RF.Save(employee);
                }
            }

            foreach (var task in tasks)
            {
                taskTestCt.CreateDispatchTaskDetails(1, task, employee);
            }

            var selectedIds = tasks.Select(p => p.Id).Distinct().ToList();
            var dispatchTasks = taskCt.GetDispatchTasksByExpression(p => selectedIds.Contains(p.Id));
            Assert.NotNull(dispatchTasks);
            var taskId = selectedIds.FirstOrDefault();
            var dispatchTask = taskCt.GetDispatchTask(taskId);

            var taskDetails = taskCt.GetDispatchTaskDetails(taskId);
            Assert.NotNull(taskDetails);

            var taskDetails1 = taskCt.GetDispatchTaskDetails(selectedIds);
            Assert.NotNull(taskDetails1);

            var taskDetails2 = taskCt.GetDispatchTaskDetailsByAdoType(new List<double>() { employee.Id }, AdoType.Employee);
            Assert.NotNull(taskDetails2);

            var dispatchTask1 = taskCt.GetDispatchTaskByExpression(p => p.Id == taskId);
            Assert.Equal(dispatchTask.Id, dispatchTask1.Id);
            var mergedTasks = taskCt.GetMergedDispatchTasks(selectedIds);
            Assert.Equal(0, mergedTasks.Count);

            var isExistMerge = taskCt.IsWorkOrderTaskMerge(workOrder.No);
            Assert.False(isExistMerge);

            var dispatchTasks1 = taskCt.GetDispatchTaskList(selectedIds);
            Assert.NotNull(dispatchTasks1);

            var dispatchTasks2 = taskCt.GetDispatchTasks(selectedIds);
            Assert.NotNull(dispatchTasks2);

            var dispatchTasks3 = taskCt.GetDispatchTasks(workOrder.Id);
            Assert.NotNull(dispatchTasks3);

            var dispatchTasks4 = taskCt.GetDispatchTasksByDate(workOrder.WorkShopId.Value, workOrder.ResourceId);
            Assert.NotNull(dispatchTasks4);

            var dispatchTasks44 = taskCt.GetDispatchTasksByDate(workOrder.WorkShopId.Value, null);
            Assert.NotNull(dispatchTasks44);

            var dispatchTasks5 = taskCt.GetDispatchTasksOf3Day(workOrder.WorkShopId.Value, workOrder.ResourceId);
            Assert.NotNull(dispatchTasks5);

            var dispatchTasks55 = taskCt.GetDispatchTasksOf3Day(workOrder.WorkShopId.Value, null);
            Assert.NotNull(dispatchTasks55);

            var dispatchTasks6 = taskCt.GetDispatchTaskOfMonth(workOrder.WorkShopId.Value, workOrder.ResourceId, workOrder.PlanBeginDate, workOrder.PlanEndDate);
            Assert.NotNull(dispatchTasks6);

            var dispatchTasks66 = taskCt.GetDispatchTaskOfMonth(workOrder.WorkShopId.Value, null, workOrder.PlanBeginDate, workOrder.PlanEndDate);
            Assert.NotNull(dispatchTasks66);

            var dispatchTasks7=taskCt.GetMonthlyTaskInfos(workOrder.WorkShopId.Value, workOrder.ResourceId, workOrder.PlanBeginDate, workOrder.PlanEndDate);
            Assert.NotNull(dispatchTasks7);

            var dispatchTasks77 = taskCt.GetMonthlyTaskInfos(workOrder.WorkShopId.Value, null, workOrder.PlanBeginDate, workOrder.PlanEndDate);
            Assert.NotNull(dispatchTasks77);

            var dispatchTasks8 = taskCt.GetAbnormalTasks(workOrder.WorkShopId.Value, workOrder.ResourceId);
            Assert.NotNull(dispatchTasks8);

            var dispatchTasks88 = taskCt.GetAbnormalTasks(workOrder.WorkShopId.Value, null);
            Assert.NotNull(dispatchTasks88);

            var dispatchTasks9 = taskCt.GetAbnormalTasks1(workOrder.WorkShopId.Value, workOrder.ResourceId);
            Assert.NotNull(dispatchTasks9);

            var dispatchTasks99 = taskCt.GetAbnormalTasks1(workOrder.WorkShopId.Value, null);
            Assert.NotNull(dispatchTasks99);

            var dispatchTasks10 = taskCt.GetDayProduceTasks(workOrder.WorkShopId.Value, workOrder.ResourceId);
            Assert.NotNull(dispatchTasks10);

            var dispatchTasks101 = taskCt.GetDayProduceTasks(workOrder.WorkShopId.Value, null);
            Assert.NotNull(dispatchTasks101);

            var dispatchTasks11 = taskCt.GetDispatchTaskList(new DispatchTaskCriteria() { PlanBeginTime = new ObjectModel.DateRange(), PlanEndTime = new ObjectModel.DateRange() });
            Assert.NotNull(dispatchTasks11);

            var planTaskInfos = taskCt.GetDayProduceTaskInfos(workOrder.WorkShopId.Value, workOrder.ResourceId);
            Assert.NotNull(planTaskInfos);

            var planTaskInfos1 = taskCt.GetDayProduceTaskInfos(workOrder.WorkShopId.Value, null);
            Assert.NotNull(planTaskInfos1);

            var taskProcessBoms = taskCt.GetTaskProcessBomList(selectedIds);
            Assert.NotNull(taskProcessBoms);

            var dispatchTask2 = taskCt.GetEmployeeRefDispatchTasks(employee.Id, SIE.Core.Items.RetrospectType.Single);
            Assert.NotNull(dispatchTask2);

            var taskDetailVMs = taskCt.CreateTaskDetailViewModels(employee.Id, AdoType.Employee);
            Assert.NotNull(taskDetailVMs);

            var reportMode = taskCt.GetTaskReportModel(workOrder.Id);
            Assert.Equal(ReportMode.Manual, reportMode);

            var isExist = taskCt.IsExistDispatchTask(p => p.Id == taskId);
            Assert.True(isExist);

            var rstTaskBillInfo = taskCt.IsCanSyntypeReport(workOrder);
            Assert.False(rstTaskBillInfo.IsSyntype);

            var configValue = taskCt.GetDispatchTaskConfigValue();
            Assert.NotNull(configValue);

            taskCt.WorkOrderUpdateDispathTask(workOrder);
            taskCt.GenerateWorkOrderDispatchTasks(workOrder.Id, false, configValue);

            var isExistTask = taskCt.IsExistsDispatchTask(workOrder.Id);
            Assert.True(isExistTask);

            var isExistTaskPerformer = taskCt.IsSelectedTaskPerformer(selectedIds);
            Assert.True(isExistTaskPerformer);

            var adoInfo = new AdoInfo()
            {
                SelectedTaskIds = selectedIds,
                DispatchTaskId = taskId,
                TaskStatus = DispatchTaskStatus.ToDispatch,
                AdoId = employee.Id,
                AdoName = employee.Name,
                AdoGroup = "班组",
                AdoType = "员工",
                SendQty = 0,
                MatchDegree = 0,
                DispatchEquipment = "",
                Status = false,
                Disable = false,
            };

            taskCt.SaveTaskPerformer(adoInfo);

            taskCt.DeleteCancelDispatchTasks(workOrder.Id);
            var workOrderInfo = new WorkOrderInfo()
            {
                WorkOrderId = workOrder.Id,
            };
            aspTaskCt.WorkOrderTask(new List<WorkOrderInfo>() { workOrderInfo });

            var isGenerate = RT.Service.Resolve<ITaskManage>().IsGenerateTask();
            Assert.True(isGenerate);
            var taskSimulation = new TaskSimulation();
            taskSimulation.LoadTaskRelateInfo(dispatchTask.WorkShopId.Value, dispatchTask.ResourceId, planTaskInfos);
        }
    }
}
