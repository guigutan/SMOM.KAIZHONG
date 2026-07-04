using SIE.Items.Units;
using SIE.xUnit.Core;
using Xunit;

namespace SIE.xUnit.Items.Units
{
    /// <summary>
    /// 单位控制器单元测试
    /// </summary>
    public class UnitControllerTest : IClassFixture<TestStarup>, IClassFixture<UnitsController>
    {

        /// <summary>
        /// 测试名称获取单位方法
        /// </summary>
        [Fact]
        public void TestGetUnitFromName()
        { 
            //var unit = _starup.Units.FirstOrDefault();
            //Assert.NotNull(unit);
            ////测试空参数
            //Assert.Throws<ArgumentException>(() => { Controller.GetUnitFromName(""); });
            ////测试正确参数
            //var dbUnit1 = Controller.GetUnitFromName(unit.Name);
            //Assert.NotNull(dbUnit1);
            ////测试错误参数
            //var dbUnit2 = Controller.GetUnitFromName(unit.Code);
            //Assert.Null(dbUnit2);
        }

        /// <summary>
        /// 测试单位类型获取单位
        /// </summary>
        [Fact]
        public void TestGetUnitList()
        {
            //var unit = _starup.Units.FirstOrDefault();
            //Assert.NotNull(unit);
            ////测试正确参数
            //var dbUnit2 = Controller.GetUnitList(unit.Type);
            //Assert.InRange(dbUnit2.Count, 1, int.MaxValue);
            ////测试错误参数
            //var dbUnit1 = Controller.GetUnitList(Guid.NewGuid().ToString());
            //Assert.Empty(dbUnit1);
        }
    }
}