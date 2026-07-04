using SIE.Common.Configs;
using SIE.Defects;
using SIE.Domain;
using SIE.Domain.Serialization.Json;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.MES.TaskManagement.Configs;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Reports;
using SIE.MES.TaskManagement.Specifications;
using SIE.MES.TaskManagement.TaskConfigs;
using SIE.MES.WorkOrders;
using SIE.Resources.Employees;
using SIE.Resources.ShiftTypes;
using SIE.xUnit.Core;
using SIE.xUnit.Items;
using SIE.xUnit.MES;
using SIE.xUnit.Resources;
using SIE.xUnit.Techs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.xUnit.TaskManagement
{
    /// <summary>
    /// 任务单测试控制器
    /// </summary>
    public partial class TaskManagementTestController : DomainController
    {
        static TechTestController tsTechCt = RT.Service.Resolve<TechTestController>();
        static MesTestController tsMesCt = RT.Service.Resolve<MesTestController>();
        static ItemTestController tsItemCt = RT.Service.Resolve<ItemTestController>();
        static ContextControllerTest tsContextCt = RT.Service.Resolve<ContextControllerTest>();
        static DispatchController taskCt = RT.Service.Resolve<DispatchController>();
        static ResTestController resTestCt = RT.Service.Resolve<ResTestController>();
        static TaskManagementTestController taskTestCt = RT.Service.Resolve<TaskManagementTestController>();

        #region 员工组/班组/员工
        /// <summary>
        /// 创建班组列表
        /// </summary>
        /// <param name="count">笔数</param>
        /// <returns>班组列表</returns>
        public virtual EntityList<WorkGroup> CreateWorkGroups(int count)
        {
            var workGroups = new EntityList<WorkGroup>();
            for (int i = 0; i < count; i++)
            {
                var workGroup = new WorkGroup();
                workGroup.GenerateId();
                var id = workGroup.Id;
                workGroup.Code = $"班组{id}";
                workGroup.Name = $"班组{id}";
                workGroups.Add(workGroup);
            }
            
            RF.Save(workGroups);
            return workGroups;
        }

        /// <summary>
        /// 创建员工组列表
        /// </summary>
        /// <param name="count">笔数</param>
        /// <returns>员工组列表</returns>
        public virtual EntityList<EmployeeGroup> CreateEmployeeGroups(int count)
        {
            var employeeGroups = new EntityList<EmployeeGroup>();
            for (int i = 0; i < count; i++)
            {
                var employeeGroup = new EmployeeGroup();
                employeeGroup.GenerateId();
                var id = employeeGroup.Id;
                employeeGroup.Code = $"员工组{id}";
                employeeGroup.Name = $"员工组{id}";
                employeeGroups.Add(employeeGroup);
            }

            RF.Save(employeeGroups);
            return employeeGroups;
        }

        /// <summary>
        /// 批量创建员工列表
        /// </summary>
        /// <param name="workGroupId">班组Id</param>
        /// <param name="employeeGroupId">员工组Id</param>
        /// <param name="count">笔数</param>
        /// <returns>员工列表</returns>
        public virtual EntityList<Employee> CreateEmployees(double workGroupId, double employeeGroupId, int count)
        {
            var employees = new EntityList<Employee>();
            for (int i = 0; i < count; i++)
            {
                var employee = new Employee();
                employee.GenerateId();
                var id = employee.Id;
                employee.Code = $"员工{id}";
                employee.Name = $"员工{id}";
                employee.WorkGroupId = workGroupId;
                employee.EmployeeGroupId = employeeGroupId;
                employees.Add(employee);
            }

            RF.Save(employees);
            return employees;
        }
        #endregion

        #region 工单管理
        /// <summary>
        /// 工单生成任务单并且派工
        /// </summary>
        /// <param name="isCommonMode">是否共模</param>
        /// <param name="isMainMaterial">是否主料</param>
        /// <param name="planNo">计划单号</param>
        /// <param name="productionOrderCode">制程工艺单</param>
        /// <param name="isGenerate">是否生成工单任务单</param>
        /// <param name="generateMode">任务单生成方式</param>
        /// <param name="isAccordConfig">共模主料工单的辅料工单已存在生成的任务单，如【true】则生成不按共模比的任务单，否【false】则不生成任务单</param>
        /// <param name="isGenerateTask">工艺路线工序是否生成任务单</param>
        /// <param name="isVirtualPart">工单对应产品是否虚拟件</param>
        /// <param name="reportMode">报工方式</param>
        /// <param name="isSyntype">是否共模报工</param>
        /// <param name="isModReport">是否剩余数报工</param>
        /// <param name="reportQty">报工数量</param>
        /// <param name="byProcess">按照工序生成任务单</param>
        /// <param name="bySpecification">按照规格件生成任务单</param>
        /// <param name="byQty">按照固定数量生成任务单</param>
        /// <param name="qty">固定数量</param>
        /// <param name="byVirtualPart">是否生成虚拟件任务</param>
        /// <param name="count">虚拟件任务数量</param>
        public virtual EntityList<DispatchTask> GenerateTaskBillAndDispatchTasks(bool isCommonMode, bool isMainMaterial, string planNo, string productionOrderCode, bool isGenerate, ReportMode generateMode, bool isAccordConfig, bool isGenerateTask, bool isVirtualPart, ReportMode reportMode, bool isSyntype, bool isModReport, decimal reportQty, bool byProcess, bool bySpecification, bool byQty, decimal qty, bool byVirtualPart, int count)
        {
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
                CreateCommonWorkOrderRelateInfos(isCommonMode, planNo, productionOrderCode, isGenerateTask, isVirtualPart, reportMode, isSyntype, isModReport, reportQty, byProcess, bySpecification, byQty, qty, byVirtualPart, count, family, workOrders, products, familys);

                if (isMainMaterial)
                {
                    workOrder = workOrders.FirstOrDefault(p => p.IsMainMaterial);
                    product = products.FirstOrDefault(p => p.Id == workOrder.ProductId);
                    family = familys.FirstOrDefault(p => p.Id == product.ProductFamilyId);
                }
                else
                {
                    workOrder = workOrders.FirstOrDefault(p => !p.IsMainMaterial);
                    product = products.FirstOrDefault(p => p.Id == workOrder.ProductId);
                    family = familys.FirstOrDefault(p => p.Id == product.ProductFamilyId);
                }
            }
            else
            {
                family = tsItemCt.CreateProductFamily(1).FirstOrDefault();
                product = tsItemCt.CreateTaskProduct(false, family.Id);
                if (bySpecification)
                    CreateProductSpecification(product.Id, count);
                //创建车间和产线
                var wipResource = resTestCt.GetOrCreateWipResource();
                var routing = tsTechCt.CreateTaskRouting(isGenerateTask, wipResource);
                workOrder = tsMesCt.CreateWorkOrder(wipResource, product.Id, routing.Id, false, false, 0, 0, "", "", isVirtualPart, count);
                //产品族报工参数配置
                CreateReportRuleConfig(family.Id, reportMode, isSyntype, isModReport, reportQty);
                //产品族任务单生成配置项
                CreateTaskConfig(family.Id, byProcess, bySpecification, byQty, qty, byVirtualPart);
            }

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
            taskTestCt.CreateGlobalConfig("G", typeof(DispatchTaskConfig).GetQualifiedName(), value);
            if (workOrder.IsCommonMode && workOrder.IsMainMaterial)
            {
                var reportRule = RT.Service.Resolve<ReportController>().GetReportRuleConfig(family.Id);
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
                    GenerateDispatchTasks(globalTaskConfig, workOrder, false);
            }
            else
                GenerateDispatchTasks(globalTaskConfig, workOrder, true);

            var tasks = taskTestCt.GetMainDispatchTaskList(workOrder.Id);
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
            RT.Service.Resolve<DispatchController>().DispatchTasks(selectedIds);
            return tasks;
        }

        public virtual void CreateCommonWorkOrderRelateInfos(bool isCommonMode, string planNo, string productionOrderCode, bool isGenerateTask, bool isVirtualPart, ReportMode reportMode, bool isSyntype, bool isModReport, decimal reportQty, bool byProcess, bool bySpecification, bool byQty, decimal qty, bool byVirtualPart, int count, ProductFamily family, List<WorkOrder> workOrders, List<Item> products, List<ProductFamily> familys)
        {
            var mainMaterial = true;
            var tmpProductionOrder = productionOrderCode;
            for (int i = 1; i <= 2; i++)
            {
                if (i != 1)
                {
                    mainMaterial = false;
                    tmpProductionOrder = "{0}{1}".L10nFormat(productionOrderCode, i);
                }

                var tmpFamily = tsItemCt.CreateProductFamily(1).FirstOrDefault();
                familys.Add(tmpFamily);
                var tmpProduct = tsItemCt.CreateTaskProduct(false, tmpFamily.Id);
                products.Add(tmpProduct);
                if (bySpecification)
                    CreateProductSpecification(tmpProduct.Id, count);
                //创建车间和产线
                var wipResource = resTestCt.GetOrCreateWipResource();                
                var routing = tsTechCt.CreateTaskRouting(isGenerateTask, wipResource);
                var workOrder = tsMesCt.CreateWorkOrder(wipResource, tmpProduct.Id, routing.Id, isCommonMode, mainMaterial, i, null, planNo, tmpProductionOrder, isVirtualPart, count);
                workOrders.Add(workOrder);
                //产品族报工参数配置
                CreateReportRuleConfig(tmpFamily.Id, reportMode, isSyntype, isModReport, reportQty);
                //产品族任务单生成配置项
                CreateTaskConfig(tmpFamily.Id, byProcess, bySpecification, byQty, qty, byVirtualPart);
            }
        }

        public virtual void GenerateDispatchTasks(DispatchTaskConfigValue taskConfig, WorkOrder workOrder, bool accordConfig)
        {
            if (workOrder == null) throw new ValidationException("工单有误".L10N());
            if (RT.Service.Resolve<DispatchController>().IsExistsDispatchTask(workOrder.Id))
                throw new ValidationException("工单已经有任务单".L10N());
            if (workOrder.State != SIE.Core.WorkOrders.WorkOrderState.Release || workOrder.IsPause == YesNo.Yes)
                throw new ValidationException("发放状态工单才可生成任务单".L10N());
            if (!taskConfig.IsGenerate) throw new ValidationException("工单没有配置生成任务单".L10N());
            taskCt.GenerateWorkOrderDispatchTasks(workOrder.Id, accordConfig, taskConfig);
        }

        /// <summary>
        /// 创建报工规则配置
        /// </summary>
        /// <param name="familyId">产品族Id</param>
        /// <param name="reportMode">报工方式</param>
        /// <param name="isSyntype">是否共模报工</param>
        /// <param name="isModReport">是否剩余数报工</param>
        /// <param name="reportQty">报工数量</param>
        /// <returns>报工规则配置</returns>
        public virtual ReportRuleConfig CreateReportRuleConfig(double familyId, ReportMode reportMode, bool isSyntype, bool isModReport, decimal reportQty)
        {
            var config = new ReportRuleConfig()
            {
                ProductFamilyId = familyId,
                ReportMode = reportMode,
                IsSyntype = isSyntype,
                IsModReport = isModReport,
                ReportQty = reportQty,
            };

            RF.Save(config);
            return config;
        }

        /// <summary>
        /// 创建任务单生成配置项
        /// </summary>
        /// <param name="familyId">产品族Id</param>
        /// <param name="byProcess">按照工序生成任务单</param>
        /// <param name="bySpecification">按照规格件生成任务单</param>
        /// <param name="byQty">按照固定数量生成任务单</param>
        /// <param name="qty">固定数量</param>
        /// <param name="byVirtualPart">是否生成虚拟件任务</param>
        /// <returns>任务单生成配置项</returns>
        public virtual TaskConfig CreateTaskConfig(double familyId, bool byProcess, bool bySpecification, bool byQty, decimal qty, bool byVirtualPart)
        {
            var config = new TaskConfig()
            {
                Qty = qty,
                ByQty = byQty,
                ByProcess = byProcess,
                ByVirtualPart = byVirtualPart,
                BySpecification = bySpecification,
                ProductFamilyId = familyId,
            };

            RF.Save(config);
            return config;
        }
        #endregion

        #region 派工管理
        /// <summary>
        /// 创建全局配置项
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="typeName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual Config CreateGlobalConfig(string entityType, string typeName, string value)
        {
            var ctl = RT.Service.Resolve<ConfigController>();

            var config = ctl.Get(typeName, entityType);
            if (config == null)
            {
                config = new Config();
                config.EntityType = entityType;
                config.TypeName = typeName;

                var dtl = new ConfigDetail();
                dtl.Category = "";
                dtl.Value = value;
                config.ConfigDetailList.Add(dtl);

                RF.Save(config);
            }
            else
            {
                config.ConfigDetailList.FirstOrDefault().Value = value;
                RF.Save(config);
            }
            return config;
        }

        /// <summary>
        /// 获取主任务单、共模辅料工单非共模比报工的待派工和派工中的任务单列表
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <returns>派工任务列表</returns>
        public virtual EntityList<DispatchTask> GetMainDispatchTaskList(double? workOrderId)
        {
            return Query<DispatchTask>().Where(p => p.IsMainTask && p.WorkOrderId == workOrderId && (p.TaskStatus == DispatchTaskStatus.ToDispatch || p.TaskStatus == DispatchTaskStatus.Dispatching)).ToList();
        }

        /// <summary>
        /// 根据已合并的任务单Id列表获取合并后的任务单Id列表
        /// </summary>
        /// <param name="mergedTaskIds">已合并的任务单Id列表</param>
        /// <returns>合并后的任务单Id列表</returns>
        public virtual List<double> GetMainDispatchTaskId(List<double> mergedTaskIds)
        {
            return Query<AssociatedTask>().Where(p => mergedTaskIds.Contains(p.TaskId)).ToList().Select(p => p.DispatchTaskId).Distinct().ToList();
        }

        /// <summary>
        /// 创建任务单的可选对象
        /// </summary>
        /// <param name="count">笔数</param>
        /// <param name="task">任务单</param>
        /// <param name="employee">员工</param>
        public virtual void CreateDispatchTaskDetails(int count, DispatchTask task, Employee employee)
        {
            var taskDetails = new EntityList<DispatchTaskDetail>();
            for (var i = 0; i < count; i++)
            {
                var taskDetail = new DispatchTaskDetail()
                {
                    AdoId = employee.Id,
                    AdoName = employee.Name,
                    AdoType = AdoType.Employee,
                    AdoGroup = AdoGroup.WorkGroup,
                    DispatchTaskId = task.Id,
                };

                taskDetails.Add(taskDetail);
            }

            RF.Save(taskDetails);
        }
        #endregion

        #region 报工管理      

        public virtual void CreateDefectRecords(Employee employee, Shift shift, ReportRecord record, EntityList<Defect> defects)
        {
            using (var tran = DB.TransactionScope(TaskManagementEntityDataTestProvider.ConnectionStringName))
            {
                foreach (var defect in defects)
                {
                    var reportDefect = new ReportDefect()
                    {
                        DefectId = defect.Id,
                    };

                    record.Defects.Add(reportDefect);
                }

                record.NgQty = 20;
                record.OkQty = 80;
                record.ShiftId = shift.Id;
                record.WorkGroupId = employee.WorkGroupId.Value;
                RF.Save(record);
                tran.Complete();
            }
        }
        #endregion

        #region 规格件管理
        private SpecificationCategory CreateSpecificationCategory()
        {
            var category = new SpecificationCategory();
            category.GenerateId();
            var id = category.Id;
            category.Code = $"SpecificationCategoryCode{id}";
            category.Name = $"SpecificationCategoryName{id}";
            RF.Save(category);
            return category;
        }

        private EntityList<Specification> CreateSpecifications(int count)
        {
            var specifications = new EntityList<Specification>();
            var category = CreateSpecificationCategory();
            for(int i = 0; i < count; i++)
            {
                var specification = new Specification();
                specification.GenerateId();
                var id = specification.Id;
                specification.Code= $"SpecificationCode{id}";
                specification.Name= $"SpecificationName{id}";
                specification.CategoryId = category.Id;
                specifications.Add(specification);
            }

            RF.Save(specifications);
            return specifications;
        }

        public virtual EntityList<ProductSpecification> CreateProductSpecifications(int count)
        {
            var proSpecifications = new EntityList<ProductSpecification>();
            for (int i = 0; i < count; i++)
            {
                var items = tsItemCt.CreateWipProduct(1);
                var proSpecification = new ProductSpecification();
                proSpecification.ProductId = items.FirstOrDefault().Id;
                CreateSpecificationDetails(3, proSpecification);
                proSpecifications.Add(proSpecification);
            }

            return proSpecifications;
        }

        public virtual ProductSpecification CreateProductSpecification(double productId, int count)
        {
            var proSpecification = new ProductSpecification();
            proSpecification.ProductId = productId;
            CreateSpecificationDetails(count, proSpecification);
            return proSpecification;
        }

        private void CreateSpecificationDetails(int count, ProductSpecification proSpecification)
        {
            for (int i = 0; i < count; i++)
            {
                var specifications = CreateSpecifications(1);
                var specDetail = new ProductSpecificationDetail();
                specDetail.GenerateId();
                specDetail.SpecificationId = specifications.FirstOrDefault().Id;
                specDetail.Qty = 1;
                proSpecification.Details.Add(specDetail);
            }

            RF.Save(proSpecification);
        }

        #endregion
    }
}
