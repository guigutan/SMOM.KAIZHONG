using SIE.Domain;
using SIE.Tech.Routings;
using SIE.xUnit.Core;
using SIE.xUnit.Techs;
using System.Linq;
using Xunit;

namespace SIE.xUnit.Tech.Routings
{
    public class RoutingControllerTest : IClassFixture<TestStarup>
    {
        static ContextControllerTest _contextControllerTest = RT.Service.Resolve<ContextControllerTest>();
        static TechTestController _techTestController = RT.Service.Resolve<TechTestController>();
        static RoutingController _routingControllerTest = RT.Service.Resolve<RoutingController>();
        /// <summary>
        /// 创建一个工艺路线
        /// </summary>
        [Fact]
        public void CreateAndPublishRoutingTest()
        {
            _contextControllerTest.InitContext();
            //创建工艺路线
            var routing = _techTestController.CreateRouting();
            Assert.NotNull(routing);

            //查询工艺路线全部
            var routingList = _routingControllerTest.GetRoutings();
            Assert.NotNull(routingList);
            Assert.NotNull(routingList.FirstOrDefault(w => w.Id == routing.Id));

            //查询工艺路线全部
            routingList = _routingControllerTest.GetRouting();
            Assert.NotNull(routingList);
            Assert.NotNull(routingList.FirstOrDefault(w => w.Id == routing.Id));

            //查询工艺路线版本全部
            var routingVersions = _routingControllerTest.GetRoutingVersions();
            Assert.NotNull(routingVersions);
            Assert.NotNull(routingVersions.FirstOrDefault(w => w.Id == routing.DefaultVersionId));

            //查询工艺路线最新版本
            var name = _routingControllerTest.GetRoutingVersion(routing.Id);
            Assert.Equal("V0002", name);
            var version = _routingControllerTest.GetMaxVersionNum(routing.Id);
            Assert.Equal(1, version);
            var defaultRoutingVersion = _routingControllerTest.GetDefaultRoutingVersion(routing.Id);
            Assert.NotNull(defaultRoutingVersion);
        }

        [Fact]
        public void CreateRoutingTest()
        {
            _contextControllerTest.InitContext();
            //创建工艺路线
            var routing = _techTestController.CreateAndSaveRouting();
            Assert.NotNull(routing);

            var routingVersion = _techTestController.GetRoutingVersion(routing.Id);
            Assert.NotNull(routingVersion);

            //发布工艺路线
            routingVersion = _routingControllerTest.ReleaseRoutingVersion(routingVersion.Id, routingVersion.Layout.Layout);
            Assert.NotNull(routingVersion);
            _techTestController.SetRoutingVersion(routing, routingVersion.Id);

            //引用数量
            _routingControllerTest.UpdateVersionRefTimes(routingVersion.Id, 1);
            //设置默认工艺路线
            _routingControllerTest.SetDefaultVersion(routing.Id, routingVersion.Id);
            var newRoutingVersion = RF.GetById<RoutingVersion>(routingVersion.Id);
            Assert.Equal(1, newRoutingVersion.ReferenceQty);
            Assert.True(newRoutingVersion.IsDefault == YesNo.Yes);

            //根据工艺路线名获取工艺路线
            routing = _routingControllerTest.GetRoutingByName(routing.Name);
            Assert.NotNull(routing);

            var routingProcessList = _routingControllerTest.GetRoutingProcessList(routing.Id, routingVersion.Id);
            Assert.Equal(5,routingProcessList.Count);

            var processParameters =  _routingControllerTest.GetRoutingProcessParameters(routingProcessList[0].ActivityId);
            Assert.NotNull(processParameters);

            var processBomConfigs = _routingControllerTest.GetRoutingProcessBomConfigs(routingProcessList[0].Id);
            Assert.NotNull(processBomConfigs);

            var processCollectSteps = _routingControllerTest.GetProcessCollectSteps(routingProcessList[0].Id);
            Assert.NotNull(processCollectSteps);
        }

        [Fact]
        public void GetRoutingTreeInfos()
        {
            _contextControllerTest.InitContext();
            //创建工艺路线
            var routing = _techTestController.CreateRouting();
            Assert.NotNull(routing);

            //产品族
            var routingTreeInfos = _routingControllerTest.GetRoutingTreeInfos();
            Assert.NotNull(routingTreeInfos);
            var routingTreeInfo = routingTreeInfos.FirstOrDefault(w => w.Id == routing.CategoryId);
            Assert.NotNull(routingTreeInfo);

            routingTreeInfos = _routingControllerTest.GetRoutingTreeInfosByKeyword("");
            Assert.NotNull(routingTreeInfos);
            routingTreeInfo = routingTreeInfos.FirstOrDefault(w => w.Id == routing.CategoryId);
            Assert.NotNull(routingTreeInfo);

            var routingVersionInfo = _routingControllerTest.GetAddRoutingVersionInfo(2);
            Assert.NotNull(routingVersionInfo);

           
        }


    }
}
