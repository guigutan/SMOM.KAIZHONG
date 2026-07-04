using SIE.Domain;
using SIE.Warehouses;
using SIE.xUnit.Core;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Extensions.Ordering;

namespace SIE.xUnit.Warehouses
{
    /// <summary>
    /// 仓库单元测试
    /// </summary>
    public class WarehouseControllerTest : IClassFixture<TestStarup>
    {
        /// <summary>
        /// 获取登录用户仓库信息
        /// </summary>
        public void GetEnableWarehouses()
        {
            //var results = Controller.GetEnableWarehouses();
            //Assert.NotNull(results);
        }

        /// <summary>
        /// 禁用
        /// </summary>
        public void DisableWarehouses()
        {
            //var ctl = RT.Service.Resolve<WarehouseController>();
            //ctl.DisableWarehouses(idList);
            //var results = ctl.GetWarehouses(idList);
            //Assert.All(results, t => Assert.Equal(State.Disable, t.State));
        }

        /// <summary>
        /// 启用
        /// </summary>
        public void EnabelWarehouses1()
        {
            //var idList = _starup.WarehouseList.Select(p => p.Id).Distinct().ToList();
            //Controller.EnabelWarehouses(idList);
            //var results = Controller.GetWarehouses(idList);
            //Assert.All(results, t => Assert.Equal(State.Enable, t.State));
        }

        /// <summary>
        /// 冻结
        /// </summary>
        public void FrozenWarehouses()
        {
            //var idList = _starup.WarehouseList.Select(p => p.Id).Distinct().ToList();
            //Controller.FrozenWarehouses(idList);
            //var frozenResults = Controller.GetWarehouses(idList);
            //Assert.All(frozenResults, t => Assert.True(t.IsFrozen));

            //Controller.FrozenWarehouses(idList);
            //var results = Controller.GetWarehouses(idList);
            //Assert.All(results, t => Assert.False(t.IsFrozen));
        }

        [Fact]
        public void StorageLocationTest()
        {
            var baseCtl = RT.Service.Resolve<ContextControllerTest>();
            var whCtl = RT.Service.Resolve<WhTestController>();
            var wareCtl = RT.Service.Resolve<WarehouseController>();
            baseCtl.InitContext();
            var warehouse = whCtl.CreateWarehouse();
            var location = whCtl.CreateLocation(warehouse.Id);

            //禁用
            wareCtl.DisableStorageLocation(new List<double>() { location.Id });
            var disableLoc = wareCtl.GetStorageLocation(warehouse.Id, location.Code);
            Assert.Equal(State.Disable, disableLoc.State);

            //启用
            wareCtl.EnableStorageLocation(new List<double>() { location.Id });
            var enableLoc = wareCtl.GetStorageLocation(warehouse.Id, location.Code);
            Assert.Equal(State.Enable, enableLoc.State);

        }
    }
}
