using SIE.Domain;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using SIE.xUnit.Core;
using SIE.xUnit.Items;
using SIE.xUnit.Resources;
using SIE.xUnit.Techs;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SIE.xUnit.Tech.Processs
{
    public class ProcessControllerTest : IClassFixture<TestStarup>   
    {
        static ContextControllerTest _contextControllerTest = RT.Service.Resolve<ContextControllerTest>();
        static TechTestController _techTestController = RT.Service.Resolve<TechTestController>();
        static ProcessController _processController = RT.Service.Resolve<ProcessController>();
        static ResTestController _resTestController = RT.Service.Resolve<ResTestController>();
        static StationController _stationController = RT.Service.Resolve<StationController>();
        static ItemTestController _itemTestController = RT.Service.Resolve<ItemTestController>(); 

        [Fact]
        public void TestGetProcess()
        {
            _contextControllerTest.InitContext();
            //测试根据名称查询
            var processList = _techTestController.CreateProcesss(1);
            Assert.Single(processList);
            var result = _processController.GetProcess(processList[0].Name);
            Assert.NotNull(result);
            Assert.Equal(processList[0].Name, result.Name);

            //测试工序参数
            var ppNum = _processController.GetProcessParameterList(processList[0].Id);
            Assert.Equal(2, ppNum);

            //测试工序全部查询
            var allList = _processController.GetProcess();
            Assert.NotNull(allList);
            var addProcess = allList.FirstOrDefault(w => w.Id == processList[0].Id);
            Assert.NotNull(addProcess);
        }

        [Fact]
        public void TestGetProcessList()
        {
            _contextControllerTest.InitContext();
            var processList = _techTestController.CreateProcesss(2);
            Assert.Equal(2, processList.Count());

            //测试根据ID列表查询
            var ids = processList.Select(s => s.Id).ToList();
            var criProcessList = _processController.GetProcessByIds(ids);
            Assert.Equal(processList.Count(), criProcessList.Count());

            //测试根据产品族ID查询工序
            PagingInfo pageInfo = new PagingInfo();
            pageInfo.PageNumber = 1;
            pageInfo.PageSize = 25;
            var criProcess = _processController.GetProcessListByCategory(processList[0].ProductFamilyId, "", pageInfo);
            Assert.Equal(2, criProcess.Count());
        }

        [Fact]
        public void TestGetProcessEmployees() 
        {
            _contextControllerTest.InitContext();
            double employee = RT.IdentityId;
            var types = new List<ProcessType>() { ProcessType.Packing };
            var processList = _techTestController.CreateProcesss(types);
            Assert.Single(processList);

            //测试根据工序类型查询工序
            var criProcessList = _processController.GetProcessByType(types.Cast<int>().ToList(), "", null);
            Assert.NotNull(criProcessList);
            var criProcess = criProcessList.FirstOrDefault(w => w.Id == processList[0].Id);
            Assert.NotNull(criProcess);

            //创建员工工序
            var processEmployees = _techTestController.CreateProcessEmployees(processList[0], new List<double>() { employee });
            Assert.Single(processEmployees);
            //根据单个员工查询员工工序
            var processEmployeeList = _processController.GetProcessEmployees(employee);
            Assert.NotNull(processEmployeeList);
            var processEmployee = processEmployeeList.FirstOrDefault(w => w.ProcessId == processList[0].Id);
            Assert.NotNull(processEmployee);

            //根据员工列表查询员工工序
            processEmployeeList = _processController.GetProcessEmployees(new List<double>() { employee });
            Assert.NotNull(processEmployeeList);
            processEmployee = processEmployeeList.FirstOrDefault(w => w.ProcessId == processList[0].Id);
            Assert.NotNull(processEmployee);

            //根据员工查询工序
            var newProcessList = _processController.GetProcessByEmployeeId(employee);
            Assert.NotNull(newProcessList);
            var newProcess = newProcessList.FirstOrDefault(w => w.Id == processList[0].Id);
            Assert.NotNull(newProcess);

            //根据员工、工序查权限
            var hasResult = _processController.EmployeeHasProcess(employee, processList[0].Id);
            Assert.True(hasResult);

            //员工与工序
            var userProcessList = _processController.GetProcesssByUserId(employee, processList[0].Name, types);
            Assert.Single(userProcessList);
        }

        [Fact]
        public void TestGetProcessPackingUnit()
        {
            _contextControllerTest.InitContext();
            //创建工序
            var types = new List<ProcessType>() { ProcessType.Packing };
            var processList = _techTestController.CreateProcesss(types);
            Assert.Single(processList);
            //创建包装单位
            var packUnitList = _techTestController.CreatePackingUnit(1);
            Assert.Single(packUnitList);
            //创建工序与包装单位
            var processPackingUnitList = _techTestController.CreateProcessPackingUnits(processList[0], packUnitList);
            Assert.Single(processPackingUnitList);
            //根据包装单位ID查询工序与包装单位关系
            var ppuList = _processController.GetProcessPackingUnitsByUnitId(packUnitList[0].Id);
            Assert.Single(ppuList);
            //根据工序ID查义工序与包装单位关系
            ppuList = _processController.GetProcessPackingUnitsByProcessId(processList[0].Id);
            Assert.NotNull(ppuList);
            var ppu = ppuList.FirstOrDefault(w => w.PackageUnitId == packUnitList[0].Id);
            Assert.NotNull(ppu);
            //根据包装单位、工序查权限
            var hasResult = _processController.PackingUnitHasProcess(packUnitList[0].Id, processList[0].Id);
            Assert.True(hasResult);
            var nameProcess = _processController.GetProcess(processList[0].Name);
            Assert.NotNull(nameProcess);
            hasResult = _processController.PackingUnitHasProcess(packUnitList[0].Id, nameProcess);
            Assert.True(hasResult);
        }

        [Fact]
        public void TestGetProcessStation()
        {
            _contextControllerTest.InitContext();
            //创建工序
            var processs = _techTestController.CreateProcesss(1);
            Assert.Single(processs);
            //生产资源
            var wipResource = _resTestController.GetOrCreateWipResource();
            Assert.NotNull(wipResource);
            //创建工位
            var station = _techTestController.GetOrCreateStation(wipResource, processs.FirstOrDefault());
            Assert.NotNull(station);
            //测试查询全部工位
            var stationAllList = _processController.GetCheckStations();
            Assert.NotNull(stationAllList);
            var newStation = stationAllList.FirstOrDefault(w => w.Id == station.Id);
            Assert.NotNull(newStation);

            //测试查询资源工位
            var stationResourceList = _processController.GetResourceStations(wipResource.Id);
            Assert.NotNull(stationResourceList);
            //测试查询资源工位
            stationResourceList = _processController.GetStationsByResourceId(wipResource.Id, processs[0].Id);
            Assert.NotNull(stationResourceList);

            //测试查询资源工位
            stationResourceList = _stationController.GetStations(processs[0].Id, null, "");
            Assert.NotNull(stationResourceList);

            //测试查询资源工位
            stationResourceList = _stationController.GetStations(null);
            Assert.NotNull(stationResourceList);

            //测试查询资源工位
            stationResourceList = _stationController.GetStations(wipResource.Id, processs[0].Id, null, "");
            Assert.NotNull(stationResourceList);

            //测试查询资源工位
            stationResourceList = _stationController.GetStations(wipResource.Id);
            Assert.NotNull(stationResourceList);

            //测试查询资源工位
            stationResourceList = _stationController.GetStations(wipResource.Id, "", null);
            Assert.NotNull(stationResourceList);

            //测试查询资源工位
            var stationId = _stationController.GetStation(station.Id);
            Assert.NotNull(stationId);

            //测试查询资源工位
            var stationCode = _stationController.GetStation(station.Code);
            Assert.NotNull(stationCode);

            //测试查询资源工位
            var stationName = _stationController.GetStationByName(station.Name);
            Assert.NotNull(stationName);

            //测试查询资源工位
            stationResourceList = _stationController.GetStations();
            Assert.NotNull(stationResourceList);

            //测试查询资源工位
            stationResourceList = _stationController.GetLoadItemStations("", null);
            if (stationResourceList != null)
            {
                var stationResource = stationResourceList.FirstOrDefault(w => w.Id == station.Id);
                Assert.Null(stationResource);
            }

            //测试查询资源工位
            stationResourceList = _stationController.GetStationsByResourceId(wipResource.Id, processs[0].Id, null);
            Assert.NotNull(stationResourceList);


            var itemList = RT.Service.Resolve<ItemTestController>().CreateWipProduct(1);
            Assert.Single(itemList);
            station.StationItemList.Add(new StationItem() { Item = itemList[0], Warning = 1, Capacity = 2 });
            RF.Save(station);

            //测试工位物料
            bool isHave = _stationController.IsExistsStationItem(itemList[0].Id, station.Id);
            Assert.True(isHave);

            //测试工位物料
            var stationItem = _stationController.GetStationItem(itemList[0].Id, station.Id);
            Assert.NotNull(stationItem);
        }

        [Fact]
        public void TestGetProcessSkill()
        {
            _contextControllerTest.InitContext();
            //创建工序
            var processs = _techTestController.CreateProcesss(1);
            Assert.Single(processs);
            //创建技能 
            var skill = _resTestController.CreateSkill(1);
            Assert.Single(skill);
            //创建工序技能
            var processSkill = _techTestController.GetProcessSkill(processs[0].Id, skill[0].Id);
            Assert.NotNull(processSkill);
            //测试查询工序技能
            var processSkillList = _processController.GetProcessSkillList(processs[0].Id);
            Assert.NotNull(processSkillList);
            Assert.NotNull(processSkillList.Where(w => w.SkillId == skill[0].Id));
            //测试查询工序技能
            processSkillList = _processController.GetProcessSkills(new List<double>() { processs[0].Id });
            Assert.NotNull(processSkillList);

            //创建员工与技能
            var employeeSkill = _resTestController.CreateEmployeeSkill(RT.IdentityId, skill[0].Id);
            Assert.NotNull(employeeSkill);
            //测试判断员工是否具有工序所要求的技能
            var hasResult = _processController.IsEmpHasProcessSkill(processs[0].Id, RT.IdentityId);
            Assert.True(hasResult);

        }

        [Fact]
        public void TestGetProcessCollectSteps()
        {
            _contextControllerTest.InitContext();
            //创建工序及采集步骤
            var processs = _techTestController.CreateProcesss(1);
            Assert.Single(processs);
            //测试查询采集步骤
            var collectSteps = _processController.GetProcessCollectSteps(processs[0].Id);
            Assert.Single(collectSteps);

            collectSteps = _processController.GetProcessCollectSteps(processs[0].Id, new EntityList<ProcessCollectStep>());
            Assert.Single(collectSteps);
        }

        [Fact]
        public void TestProcessParameters()
        {
            _contextControllerTest.InitContext();
            //创建工序
            var processs = _techTestController.CreateProcesss(1);
            Assert.Single(processs);
            //测试查询工序参数
            var processParameters = _processController.GetProcessParameters(processs[0].Id);
            Assert.NotNull(processParameters);

            processParameters = _processController.GetProcessParameters(processs[0].Id, new EntityList<ProcessParameter>());
            Assert.NotNull(processParameters);

            processParameters = _processController.GetProcessParameterByProcessId(new double[] { processs[0].Id });
            Assert.NotNull(processParameters);
        }

        [Fact]
        public void TestProcessDefects()
        {
            _contextControllerTest.InitContext();
            //创建工序
            var processs = _techTestController.CreateProcesss(1);
            Assert.Single(processs);

            //创建工序缺陷
            var processDefects = _techTestController.CreateProcessDefects(processs.FirstOrDefault(), 1);
            Assert.NotNull(processDefects);

            //测试查询工序缺陷
            var defects = _processController.GetProcessDefects(processs[0].Id);
            Assert.NotNull(defects);
        }
    }
}