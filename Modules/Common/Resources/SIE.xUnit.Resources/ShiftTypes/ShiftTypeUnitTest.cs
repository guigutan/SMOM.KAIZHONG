using SIE.Domain;
using SIE.Resources.ShiftTypes;
using SIE.xUnit.Core;
using Xunit;

namespace SIE.xUnit.Resources.ShiftTypes
{
    /// <summary>
    /// 班制单元测试
    /// </summary>
    public class ShiftTypeUnitTest : IClassFixture<TestStarup>
    {
        /// <summary>
        /// 创建班制单元测试方法
        /// </summary>
        [Fact]
        public void CreateShiftTypeTest()
        {
            // 指定用户和库存组织
            RT.Service.Resolve<ContextControllerTest>().InitContext();

            // 创建默认的班制
            var ctrl = RT.Service.Resolve<ShiftTypeTestController>();
            ShiftType shiftType = ctrl.CreateShiftType(true);

            // 保存默认的班制
            RF.Save(shiftType);

            ShiftType dbShiftType = ctrl.GetShiftType(shiftType.Code);

            Assert.NotNull(dbShiftType);
        }
    }
}