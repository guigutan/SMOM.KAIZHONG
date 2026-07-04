using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Resources.Employees;
using SIE.Tech.Processs;
using SIE.Tech.Processs.Event;
using SIE.Tech.Routings;
using SIE.xUnit.Defects;
using SIE.xUnit.Items;
using System;
using System.Collections.Generic;
using SIE.Tech.Routings.ViewModels;
using System.Linq;
using SIE.Tech.Stations;
using SIE.Resources.WipResources;
using SIE.Packages.Packages;
using SIE.xUnit.Resources;
using SIE.xUnit.Tech.Routings;
using Xunit;

namespace SIE.xUnit.Techs
{
    public partial class TechTestController : DomainController
    {
        #region 工序
        public virtual EntityList<Process> CreateRoutingProcesss()
        {
            var types = new List<ProcessType>() { ProcessType.Pqc, ProcessType.Fqc, ProcessType.Fix, ProcessType.Assembly, ProcessType.Packing };
            var processList = CreateProcesss(types);
            return processList;
        }

        public virtual EntityList<Process> CreateProcesss(List<ProcessType> types)
        {
            Check.NotNull(types, "工序类型不能为空");
            if (!types.Any())
                throw new ValidationException("工序类型不能为空");
            var family = RT.Service.Resolve<ItemTestController>().CreateProductFamily(1);
            EntityList<Process> results = new EntityList<Process>();
            types.ForEach(type =>
            {
                var process = new Process();
                process.PropertyChanged += new ProcessPropertyChanged().PropertyChanged;
                process.GenerateId();
                double id = process.Id;
                process.Type = type;
                process.ProductFamily = family.FirstOrDefault();
                process.Name = $"{process.Type.ToLabel()}{id}";
                CreateProcessCollectSteps(process, new List<BarcodeType>() { BarcodeType.SN });
                results.Add(process);
            });
            RF.Save(results);
            return results;
        }

        public virtual EntityList<Process> CreateProcesss(int count, Action<Process> action)
        {
            if (count == 0)
                throw new ValidationException("数据数量必须大于0");
            var family = RT.Service.Resolve<ItemTestController>().CreateProductFamily(1);
            EntityList<Process> results = new EntityList<Process>();
            for (int i = 0; i < count; i++)
            {
                var process = new Process();
                process.PropertyChanged += new ProcessPropertyChanged().PropertyChanged;
                process.GenerateId();
                double id = process.Id;
                process.Type = ProcessType.Pqc;
                process.ProductFamily = family.FirstOrDefault();
                action(process);
                process.Name = $"{process.Type.ToLabel()}{id}";
                CreateProcessCollectSteps(process, new List<BarcodeType>() { BarcodeType.SN });
                results.Add(process);
            }
            RF.Save(results);
            return results;
        }


        public virtual EntityList<Process> CreateProcesss(int count)
        {
            if (count == 0)
                throw new ValidationException("数据数量必须大于0");
            var family = RT.Service.Resolve<ItemTestController>().CreateProductFamily(1);
            EntityList<Process> results = new EntityList<Process>();
            for (int i = 0; i < count; i++)
            {
                var process = new Process();
                process.PropertyChanged += new ProcessPropertyChanged().PropertyChanged;
                process.GenerateId();
                double id = process.Id;
                process.Type = ProcessType.Pqc;
                process.ProductFamily = family.FirstOrDefault();
                process.Name = $"{process.Type.ToLabel()}{id}";
                CreateProcessCollectSteps(process, new List<BarcodeType>() { BarcodeType.SN });
                results.Add(process);
            }
            RF.Save(results);
            return results;
        }

        /// <summary>
        /// 根据产品族ID和工序类型获取一个工序，没有时新建
        /// </summary>
        /// <param name="productFamilyId">产品族ID</param>
        /// <param name="processType">工序类型</param>
        /// <returns>工序</returns>
        public virtual Process GetFirstProcessByFamilyId(double productFamilyId, ProcessType processType)
        {
            var process = Query<Process>().Where(p => p.Type == processType && p.ProductFamilyId == productFamilyId).FirstOrDefault();
            if (process == null)
            {
                process = new Process();
                process.PropertyChanged += new ProcessPropertyChanged().PropertyChanged;
                process.GenerateId();
                double id = process.Id;
                process.Type = processType;
                process.ProductFamilyId = productFamilyId;
                process.Name = $"{process.Type.ToLabel()}{id}";
                CreateProcessCollectSteps(process, new List<BarcodeType>() { BarcodeType.SN });
                RF.Save(process);
                CreateProcessDefects(process, 10);
            }
            return process;
        }
        #endregion

        #region 工序参数
        public virtual EntityList<ProcessParameter> CreateProcessParameters(List<ResultTypeForDesign> resultTypes, Process process)
        {
            Check.NotNull(resultTypes, "结果集合不能为空");
            if (!resultTypes.Any())
                throw new ValidationException("结果集合不能为空");
            Check.NotNull(process, "工序不能为空");
            EntityList<ProcessParameter> parameters = new EntityList<ProcessParameter>();
            resultTypes.ForEach(resultType =>
            {
                var parameter = new ProcessParameter()
                {
                    Type = resultType,
                    Description = resultType.ToLabel(),
                    Process = process
                };
                parameter.GenerateId();
                parameters.Add(parameter);
            });
            //RF.Save(parameters);
            return parameters;
        }

        /// <summary>
        /// 根据工序ID和类型获取一个工序参数
        /// </summary>
        /// <param name="processId">工序ID</param>
        /// <param name="resultType">类型</param>
        /// <returns>工序参数</returns>
        public virtual ProcessParameter GetFirstProcessParameter(double processId, ResultTypeForDesign resultType)
        {
            var processParameter = Query<ProcessParameter>().Where(p => p.ProcessId == processId && p.Type == resultType).FirstOrDefault();
            if (processParameter == null)
                throw new ValidationException("找不到对应的工序参数".L10N());
            return processParameter;
        }
        #endregion

        #region 采集步骤
        public virtual EntityList<ProcessCollectStep> CreateProcessCollectSteps(Process process, List<BarcodeType> types)
        {
            var steps = new EntityList<ProcessCollectStep>();
            for (int i = 0; i < types.Count; i++)
            {
                var step = new ProcessCollectStep()
                {
                    BarcodeType = types[i],
                    Step = i,
                    Process = process
                };
                steps.Add(step);
            }
            process.CollectStepList.AddRange(steps);
            return steps;
        }
        #endregion

        #region 工序缺陷
        public virtual EntityList<ProcessDefect> CreateProcessDefects(Process process, int count)
        {
            var results = new EntityList<ProcessDefect>();
            if (process.Type == ProcessType.Fqc || process.Type == ProcessType.Pqc)
            {
                var defects = RT.Service.Resolve<DefectTestController>().GetOrCreateDefects(count);
                defects.ForEach(defect =>
                {
                    results.Add(new ProcessDefect()
                    {
                        Process = process,
                        Defect = defect
                    });
                });
                RF.Save(results);
            }
            return results;
        }
        #endregion

        #region 工序对应包装
        /// <summary>
        /// 工序对应包装
        /// </summary>
        /// <param name="process">工序</param>
        /// <param name="employeeIds">人员Id列表</param>
        /// <returns>工序与员工</returns>
        public virtual EntityList<ProcessPackingUnit> CreateProcessPackingUnits(Process process, EntityList<PackingUnit> packingUnitList)
        {
            var processPackingUnits = new EntityList<ProcessPackingUnit>();
            for (int i = 0; i < packingUnitList.Count; i++)
            {
                var pm = new ProcessPackingUnit()
                {
                    PackageUnit = packingUnitList[i],
                    Process = process
                };
                processPackingUnits.Add(pm);
            }
            RF.Save(processPackingUnits);
            return processPackingUnits;
        }

        /// <summary>
        /// 创建包装单位
        /// </summary>
        /// <param name="count">个数</param>
        /// <returns></returns>
        public virtual EntityList<PackingUnit> CreatePackingUnit(int count)
        {
            if (count == 0)
                throw new ValidationException("数据数量必须大于0");
            EntityList<PackingUnit> results = new EntityList<PackingUnit>();
            for (int i = 0; i < count; i++)
            {
                var packingUnit = new PackingUnit();
                packingUnit.GenerateId();
                double id = packingUnit.Id;
                packingUnit.Code = $"{"Code"}{id}";
                packingUnit.Name = $"{"Name"}{id}";
                results.Add(packingUnit);
            }
            RF.Save(results);
            return results;
        }

        #endregion

        #region 工序技能
        public virtual ProcessSkill GetProcessSkill(double processId, double skillId)
        {
            var entity = new ProcessSkill();
            entity.GenerateId();
            entity.SkillId = skillId;
            entity.ProcessId = processId;
            entity.IsCheck = true;
            RF.Save(entity);
            return entity;
        }
        #endregion

        #region 员工工序
        /// <summary>
        /// 人员工序
        /// </summary>
        /// <param name="process">工序</param>
        /// <param name="employeeIds">人员Id列表</param>
        /// <returns>工序与员工</returns>
        public virtual EntityList<ProcessEmployee> CreateProcessEmployees(Process process, List<double> employeeIds)
        {
            var processEmployees = new EntityList<ProcessEmployee>();

            for (int i = 0; i < employeeIds.Count; i++)
            {
                var pm = new ProcessEmployee()
                {
                    EmployeeId = employeeIds[i],
                    Process = process
                };
                processEmployees.Add(pm);
            }

            RF.Save(processEmployees);
            return processEmployees;
        }

        public virtual EntityList<ProcessEmployee> CreateProcessEmployees(EntityList<Process> processes, Employee employee)
        {
            var results = new EntityList<ProcessEmployee>();
            processes.ForEach(process =>
            {
                var pm = new ProcessEmployee()
                {
                    Employee = employee,
                    Process = process
                };
                pm.GenerateId();
                results.Add(pm);
            });
            RF.Save(results);
            return results;
        }
        #endregion

        #region 工艺路线
        /// <summary>
        /// 创建一个工艺路线
        /// </summary>
        /// <returns>工艺路线</returns>
        public virtual Routing CreateRouting()
        {
            return CreateAndPublishRouting();
        }

        /// <summary>
        /// 创建工艺路线并发布
        /// </summary>
        /// <returns></returns>
        public virtual Routing CreateAndPublishRouting()
        {
            var newRouting = CreateAndSaveRouting();
            var savedRouting = RF.GetById<Routing>(newRouting.Id);
            var routingVersion = Query<RoutingVersion>().Where(p => p.RoutingId == savedRouting.Id).FirstOrDefault();
            Check.NotNull(routingVersion, "找不到工艺路线版本");
            routingVersion.IsDefault = YesNo.Yes;
            routingVersion.State = RoutingState.Release;
            savedRouting.DefaultVersionId = routingVersion.Id;
            RF.Save(savedRouting);
            RF.Save(routingVersion);
            return savedRouting;
        }

        /// <summary>
        /// 创建工艺路线不发布
        /// </summary>
        /// <returns></returns>
        public virtual Routing CreateAndSaveRouting()
        {
            using (var tran = DB.TransactionScope(TechEntityDataTestProvider.ConnectionStringName))
            {
                var routing = new RoutingImportSaveViewModel();
                routing.RowNum = 0;
                //获取产品族
                var productFamilyCategory = RT.Service.Resolve<ItemTestController>().GetFirstProductFamilyCategory();
                routing.Category = productFamilyCategory.Name;
                routing.CategoryId = productFamilyCategory.Id;
                //新建工艺路线
                var newRouting = new Routing();
                newRouting.GenerateId();
                newRouting.Name = "工艺路线" + newRouting.Id;
                newRouting.Description = "工艺路线" + newRouting.Id;
                newRouting.CategoryId = productFamilyCategory.Id;
                RF.Save(newRouting);
                routing.RoutingName = "工艺路线" + newRouting.Id;
                routing.RoutingDesc = "工艺路线" + newRouting.Id;
                routing.RoutingId = newRouting.Id;
                routing.IsPass = true;
                //生成工序（装配--》检验--》维修--》终检--》包装）
                var productFamily = RT.Service.Resolve<ItemTestController>().GetFirstProductFamily(productFamilyCategory.Id);
                for (var i = 1; i < 8; i++)
                {
                    ProcessViewModel processModel = new ProcessViewModel();
                    ProcessParameter processParameter = null;
                    Process process = null;
                    if (i == 1)
                    {
                        process = GetFirstProcessByFamilyId(productFamily.Id, ProcessType.Assembly);
                        processModel.SortOrder = 1;
                        processModel.SortOrderBack = 2;
                        processParameter = GetFirstProcessParameter(process.Id, ResultTypeForDesign.Any);
                    }
                    else if (i == 2 || i == 3)
                    {
                        process = GetFirstProcessByFamilyId(productFamily.Id, ProcessType.Pqc);
                        processModel.SortOrder = 2;
                        if (i == 2)
                        {
                            processModel.SortOrderBack = 4;
                            processParameter = GetFirstProcessParameter(process.Id, ResultTypeForDesign.Pass);
                        }
                        else
                        {
                            processModel.SortOrderBack = 3;
                            processParameter = GetFirstProcessParameter(process.Id, ResultTypeForDesign.Fail);
                        }
                    }
                    else if (i == 4)
                    {
                        process = GetFirstProcessByFamilyId(productFamily.Id, ProcessType.Fix);
                        processModel.SortOrder = 3;
                        processModel.SortOrderBack = 2;
                        processParameter = GetFirstProcessParameter(process.Id, ResultTypeForDesign.Pass);

                    }
                    else if (i == 5 || i == 6)
                    {
                        process = GetFirstProcessByFamilyId(productFamily.Id, ProcessType.Fqc);
                        processModel.SortOrder = 4;
                        if (i == 5)
                        {
                            processModel.SortOrderBack = 5;
                            processParameter = GetFirstProcessParameter(process.Id, ResultTypeForDesign.Pass);
                        }
                        else
                        {
                            processModel.SortOrderBack = 3;
                            processParameter = GetFirstProcessParameter(process.Id, ResultTypeForDesign.Fail);
                        }
                    }
                    else
                    {
                        process = GetFirstProcessByFamilyId(productFamily.Id, ProcessType.Packing);
                        processModel.SortOrder = 5;
                        processModel.SortOrderBack = 0;
                        processModel.IsCreateSku = true;
                        processParameter = GetFirstProcessParameter(process.Id, ResultTypeForDesign.Pass);
                    }
                    processModel.Result = processParameter.Type;
                    processModel.ParameterId = processParameter.Id;
                    processModel.ResultDesc = processParameter.Description;
                    processModel.IsBatch = false;
                    processModel.ProcessId = process.Id;
                    processModel.ProcessType = process.Type.Value;
                    processModel.ProcessName = process.Name;
                    processModel.CanChoose = null;
                    processModel.IsRepeat = null;
                    routing.ProcessDetailModelList.Add(processModel);
                }
                RT.Service.Resolve<RoutingController>().ImportRouting(routing);
                tran.Complete();
                return newRouting;
            }
        }

        public virtual RoutingVersion GetRoutingVersion(double routingId)
        {
            return Query<RoutingVersion>().Where(p => p.RoutingId == routingId).FirstOrDefault();
        }

        public virtual void SetRoutingVersion(Routing routing, double routingVersionId)
        {
            routing.DefaultVersionId = routingVersionId;
            RF.Save(routing);
        }

        /// <summary>
        /// 创建一个自定义的工艺路线
        /// </summary>
        /// <param name="routingProcessInfoList">工艺路线参数列表</param>
        /// <returns>自定义的工艺路线</returns>
        public virtual Routing CreateCustomRouting(List<RoutingProcessInfo> routingProcessInfoList)
        {
            if (routingProcessInfoList.Count == 0)
                throw new ValidationException("工序列表不能为空！");
            using (var tran = DB.TransactionScope(TechEntityDataTestProvider.ConnectionStringName))
            {
                var routing = new RoutingImportSaveViewModel();
                routing.RowNum = 0;
                var productFamily = routingProcessInfoList.FirstOrDefault().Process.ProductFamily;
                var productFamilyCategory = productFamily.Category;
                routing.Category = productFamilyCategory.Name;
                routing.CategoryId = productFamilyCategory.Id;
                //新建工艺路线
                var newRouting = new Routing();
                newRouting.GenerateId();
                newRouting.Name = "工艺路线" + newRouting.Id;
                newRouting.Description = "工艺路线" + newRouting.Id;
                newRouting.CategoryId = productFamilyCategory.Id;
                RF.Save(newRouting);
                routing.RoutingName = "工艺路线" + newRouting.Id;
                routing.RoutingDesc = "工艺路线" + newRouting.Id;
                routing.RoutingId = newRouting.Id;
                routing.IsPass = true;
                //生成工序
                foreach (var routingProces in routingProcessInfoList)
                {
                    ProcessViewModel processModel = new ProcessViewModel();
                    ProcessParameter processParameter = null;
                    processModel.SortOrder = routingProces.SortOrder;
                    processModel.SortOrderBack = routingProces.SortOrderBack;
                    processParameter = GetFirstProcessParameter(routingProces.Process.Id, routingProces.ResultType);
                    processModel.Result = processParameter.Type;
                    processModel.ParameterId = processParameter.Id;
                    processModel.ResultDesc = processParameter.Description;
                    processModel.IsBatch = false;
                    processModel.ProcessId = routingProces.Process.Id;
                    processModel.ProcessType = routingProces.Process.Type.Value;
                    processModel.ProcessName = routingProces.Process.Name;
                    processModel.CanChoose = routingProces.CanChoose;
                    processModel.IsRepeat = routingProces.IsRepeat;
                    processModel.IsCreateSku = routingProces.CreateSku;
                    routing.ProcessDetailModelList.Add(processModel);
                }
                RT.Service.Resolve<RoutingController>().ImportRouting(routing);
                var savedRouting = RF.GetById<Routing>(newRouting.Id);
                var routingVersion = Query<RoutingVersion>().Where(p => p.RoutingId == savedRouting.Id).FirstOrDefault();
                Check.NotNull(routingVersion, "找不到工艺路线版本");
                routingVersion.IsDefault = YesNo.Yes;
                routingVersion.State = RoutingState.Release;
                savedRouting.DefaultVersionId = routingVersion.Id;
                RF.Save(savedRouting);
                RF.Save(routingVersion);
                tran.Complete();
                return savedRouting;
            }
        }

        public virtual Routing CreateMoveRouting(Process process, Action<RoutingImportSaveViewModel> saving)
        {
            using (var tran = DB.TransactionScope(TechEntityDataTestProvider.ConnectionStringName))
            {
                Check.NotNull(process, "工序不能为空");
                var category = process.ProductFamily.Category;
                var newRouting = new Routing();
                newRouting.GenerateId();
                newRouting.Name = "工艺路线" + newRouting.Id;
                newRouting.Description = "工艺路线" + newRouting.Id;
                newRouting.CategoryId = category.Id;
                RF.Save(newRouting);
                var routing = new RoutingImportSaveViewModel()
                {
                    RowNum = 0,
                    Category = category.Name,
                    CategoryId = category.Id,
                    RoutingName = "工艺路线" + newRouting.Id,
                    RoutingDesc = "工艺路线" + newRouting.Id,
                    RoutingId = newRouting.Id,
                    IsPass = true
                };
                ProcessParameter processParameter = GetFirstProcessParameter(process.Id, ResultTypeForDesign.Any);
                ProcessViewModel processModel = new ProcessViewModel()
                {
                    SortOrder = 1,
                    SortOrderBack = 0,
                    Result = processParameter.Type,
                    ParameterId = processParameter.Id,
                    ResultDesc = processParameter.Description,
                    IsBatch = false,
                    ProcessId = process.Id,
                    ProcessType = process.Type.Value,
                    ProcessName = process.Name,
                    CanChoose = null,
                    IsRepeat = null,
                    IsCreateSku = null,
                };
                routing.ProcessDetailModelList.Add(processModel);
                saving(routing);
                RT.Service.Resolve<RoutingController>().ImportRouting(routing);
                var savedRouting = RF.GetById<Routing>(newRouting.Id);
                var routingVersion = Query<RoutingVersion>().Where(p => p.RoutingId == savedRouting.Id).FirstOrDefault();
                Check.NotNull(routingVersion, "找不到工艺路线版本");
                routingVersion.IsDefault = YesNo.Yes;
                routingVersion.State = RoutingState.Release;
                savedRouting.DefaultVersionId = routingVersion.Id;
                RF.Save(savedRouting);
                RF.Save(routingVersion);
                tran.Complete();
                return savedRouting;
            }
        }

        public virtual Routing CreatePackingRouting(Process process, Action<RoutingImportSaveViewModel> saving)
        {
            using (var tran = DB.TransactionScope(TechEntityDataTestProvider.ConnectionStringName))
            {
                Check.NotNull(process, "工序不能为空");
                var category = process.ProductFamily.Category;
                var newRouting = new Routing();
                newRouting.GenerateId();
                newRouting.Name = "工艺路线" + newRouting.Id;
                newRouting.Description = "工艺路线" + newRouting.Id;
                newRouting.CategoryId = category.Id;
                RF.Save(newRouting);
                var routing = new RoutingImportSaveViewModel()
                {
                    RowNum = 0,
                    Category = category.Name,
                    CategoryId = category.Id,
                    RoutingName = "工艺路线" + newRouting.Id,
                    RoutingDesc = "工艺路线" + newRouting.Id,
                    RoutingId = newRouting.Id,
                    IsPass = true
                };
                ProcessParameter processParameter = GetFirstProcessParameter(process.Id, ResultTypeForDesign.Pass);
                ProcessViewModel processModel = new ProcessViewModel()
                {
                    SortOrder = 1,
                    SortOrderBack = 0,
                    Result = processParameter.Type,
                    ParameterId = processParameter.Id,
                    ResultDesc = processParameter.Description,
                    IsBatch = false,
                    ProcessId = process.Id,
                    ProcessType = process.Type.Value,
                    ProcessName = process.Name,
                    CanChoose = null,
                    IsRepeat = null,
                    IsCreateSku = null,
                };
                routing.ProcessDetailModelList.Add(processModel);
                saving(routing);
                RT.Service.Resolve<RoutingController>().ImportRouting(routing);
                var savedRouting = RF.GetById<Routing>(newRouting.Id);
                var routingVersion = Query<RoutingVersion>().Where(p => p.RoutingId == savedRouting.Id).FirstOrDefault();
                Check.NotNull(routingVersion, "找不到工艺路线版本");
                routingVersion.IsDefault = YesNo.Yes;
                routingVersion.State = RoutingState.Release;
                savedRouting.DefaultVersionId = routingVersion.Id;
                RF.Save(savedRouting);
                RF.Save(routingVersion);
                tran.Complete();
                return savedRouting;
            }
        }

        /// <summary>
        /// 
        /// 开始--->过站--->装配--->检验--pass->包装--->结束
        ///                           --fail->维修--->检验--->包装--->结束
        /// </summary>
        /// <param name="processList"></param>
        /// <param name="saving"></param>
        /// <returns></returns>
        public virtual Routing CreateWipRouting(Process[] processList)
        {
            Check.NotNull(processList, "工序不能为空");
            var info = new List<RoutingProcessInfo>();
            var assemblyList = processList.Where(p => p.Type == ProcessType.Assembly).ToArray();
            for (int i = 0; i < assemblyList.Length; i++)
            {
                var process = assemblyList[i];
                info.Add(new RoutingProcessInfo() { Process = process, SortOrder = i + 1, SortOrderBack = i + 2, ResultType = ResultTypeForDesign.Any });
            }
            var pqc = processList.FirstOrDefault(p => p.Type == ProcessType.Pqc);
            info.Add(new RoutingProcessInfo() { Process = pqc, SortOrder = 3, SortOrderBack = 4, ResultType = ResultTypeForDesign.Pass });
            info.Add(new RoutingProcessInfo() { Process = pqc, SortOrder = 3, SortOrderBack = 5, ResultType = ResultTypeForDesign.Fail });
            var fix = processList.FirstOrDefault(p => p.Type == ProcessType.Fix);
            info.Add(new RoutingProcessInfo() { Process = fix, SortOrder = 5, SortOrderBack = 3, ResultType = ResultTypeForDesign.Pass });
            var packing = processList.FirstOrDefault(p => p.Type == ProcessType.Packing);
            info.Add(new RoutingProcessInfo() { Process = packing, SortOrder = 4, SortOrderBack = 0, ResultType = ResultTypeForDesign.Pass, CreateSku = true });
            return CreateCustomRouting(info);
        }

        private ProcessViewModel CreateProcessViewModel(Process process, ProcessParameter processParameter, int sortOrder, int sortOrderBack, bool isCreateSku = false)
        {
            return new ProcessViewModel()
            {
                SortOrder = sortOrder,
                SortOrderBack = sortOrderBack,
                Result = processParameter.Type,
                ParameterId = processParameter.Id,
                ResultDesc = processParameter.Description,
                IsBatch = false,
                ProcessId = process.Id,
                ProcessType = process.Type.Value,
                ProcessName = process.Name,
                CanChoose = null,
                IsRepeat = null,
                IsCreateSku = isCreateSku,
            };
        }

        /// <summary>
        /// 创建工艺路线
        /// </summary>
        /// <param name="isGenerateTask">是否生成任务单</param>
        /// <returns>工艺路线</returns>
        public virtual Routing CreateTaskRouting(bool isGenerateTask, WipResource wipResource)
        {
            using (var tran = DB.TransactionScope(TechEntityDataTestProvider.ConnectionStringName))
            {
                var routing = new RoutingImportSaveViewModel();
                routing.RowNum = 0;
                //获取产品族
                var productFamilyCategory = RT.Service.Resolve<ItemTestController>().GetFirstProductFamilyCategory();
                routing.Category = productFamilyCategory.Name;
                routing.CategoryId = productFamilyCategory.Id;
                //新建工艺路线
                var newRouting = new Routing();
                newRouting.GenerateId();
                newRouting.Name = "工艺路线" + newRouting.Id;
                newRouting.Description = "工艺路线" + newRouting.Id;
                newRouting.CategoryId = productFamilyCategory.Id;
                RF.Save(newRouting);
                routing.RoutingName = "工艺路线" + newRouting.Id;
                routing.RoutingDesc = "工艺路线" + newRouting.Id;
                routing.RoutingId = newRouting.Id;
                routing.IsPass = true;
                //生成工序（装配--》检验--》维修--》终检--》包装）
                var productFamily = RT.Service.Resolve<ItemTestController>().GetFirstProductFamily(productFamilyCategory.Id);
                for (var i = 1; i < 8; i++)
                {
                    ProcessViewModel processModel = new ProcessViewModel();
                    ProcessParameter processParameter = null;
                    Process process = null;
                    if (i == 1)
                    {
                        process = GetFirstProcessByFamilyId(productFamily.Id, ProcessType.Assembly);
                        processModel.SortOrder = 1;
                        processModel.SortOrderBack = 2;
                        if (isGenerateTask)
                            processModel.IsGenerateTask = true;
                        processParameter = GetFirstProcessParameter(process.Id, ResultTypeForDesign.Any);
                    }
                    else if (i == 2 || i == 3)
                    {
                        process = GetFirstProcessByFamilyId(productFamily.Id, ProcessType.Pqc);
                        processModel.SortOrder = 2;
                        if (i == 2)
                        {
                            processModel.SortOrderBack = 4;
                            if (isGenerateTask)
                                processModel.IsGenerateTask = true;
                            processParameter = GetFirstProcessParameter(process.Id, ResultTypeForDesign.Pass);
                        }
                        else
                        {
                            processModel.SortOrderBack = 3;
                            if (isGenerateTask)
                                processModel.IsGenerateTask = true;
                            processParameter = GetFirstProcessParameter(process.Id, ResultTypeForDesign.Fail);
                        }
                    }
                    else if (i == 4)
                    {
                        process = GetFirstProcessByFamilyId(productFamily.Id, ProcessType.Fix);
                        processModel.SortOrder = 3;
                        processModel.SortOrderBack = 2;
                        processParameter = GetFirstProcessParameter(process.Id, ResultTypeForDesign.Pass);

                    }
                    else if (i == 5 || i == 6)
                    {
                        process = GetFirstProcessByFamilyId(productFamily.Id, ProcessType.Fqc);
                        processModel.SortOrder = 4;
                        if (isGenerateTask)
                            processModel.IsGenerateTask = true;
                        if (i == 5)
                        {
                            processModel.SortOrderBack = 5;
                            processParameter = GetFirstProcessParameter(process.Id, ResultTypeForDesign.Pass);
                        }
                        else
                        {
                            processModel.SortOrderBack = 3;
                            processParameter = GetFirstProcessParameter(process.Id, ResultTypeForDesign.Fail);
                        }
                    }
                    else
                    {
                        process = GetFirstProcessByFamilyId(productFamily.Id, ProcessType.Packing);
                        processModel.SortOrder = 5;
                        processModel.SortOrderBack = 0;
                        processModel.IsCreateSku = true;
                        if (isGenerateTask)
                            processModel.IsGenerateTask = true;
                        processParameter = GetFirstProcessParameter(process.Id, ResultTypeForDesign.Pass);
                    }
                    processModel.Result = processParameter.Type;
                    processModel.ParameterId = processParameter.Id;
                    processModel.ResultDesc = processParameter.Description;
                    processModel.IsBatch = false;
                    processModel.ProcessId = process.Id;
                    processModel.ProcessType = process.Type.Value;
                    processModel.ProcessName = process.Name;
                    processModel.CanChoose = null;
                    processModel.IsRepeat = null;
                    routing.ProcessDetailModelList.Add(processModel);

                    //创建工位
                    GetOrCreateStation(wipResource, process);
                }
                RT.Service.Resolve<RoutingController>().ImportRouting(routing);
                var savedRouting = RF.GetById<Routing>(newRouting.Id);
                var routingVersion = Query<RoutingVersion>().Where(p => p.RoutingId == savedRouting.Id).FirstOrDefault();
                Check.NotNull(routingVersion, "找不到工艺路线版本");
                routingVersion.IsDefault = YesNo.Yes;
                routingVersion.State = RoutingState.Release;
                savedRouting.DefaultVersionId = routingVersion.Id;
                RF.Save(savedRouting);
                RF.Save(routingVersion);
                tran.Complete();
                return savedRouting;
            }
        }
        #endregion

        #region 工位
        public virtual Station GetOrCreateStation(WipResource wipResouce, Process process)
        {
            var stations = RT.Service.Resolve<StationController>().GetStations(wipResouce.Id, process.Id, null, null);
            if (stations.Any())
                return stations.FirstOrDefault();
            var station = new Station()
            {
                Process = process,
                Resource = wipResouce
            };
            station.GenerateId();
            station.Code = $"{process.Type.ToLabel()}工位{station.Id}";
            station.Name = $"{process.Type.ToLabel()}工位{station.Id}";
            RF.Save(station);
            return station;
        }

        /// <summary>
        /// 创建工位
        /// </summary>
        /// <param name="count">个数</param>
        /// <returns>工位列表</returns>
        public virtual EntityList<Station> CreateStation(int count)
        {
            if (count == 0)
                throw new ValidationException("数据数量必须大于0");
            var processs = CreateProcesss(1);
            //生产资源
            var wipResource = RT.Service.Resolve<ResTestController>().GetOrCreateWipResource();
            EntityList<Station> results = new EntityList<Station>();
            for (int i = 0; i < count; i++)
            {
                var station = new Station();
                station.GenerateId();
                station.Process = processs.FirstOrDefault();
                station.Resource = wipResource;
                station.Code = $"StationCode{station.Id}";
                station.Name = $"StationName{station.Id}";
                results.Add(station);
            }
            RF.Save(results);
            return results;
        }
        #endregion
    }
}