using SIE.Resources.CalendarSchemes;
using SIE.Resources.ShiftTypes;
using SIE.xUnit.Core;
using SIE.xUnit.Resources.ShiftTypes;
using Xunit;
using System.Linq;
using SIE.Domain;
using SIE.Resources;

namespace SIE.xUnit.Resources.CalendarSchemes
{
    /// <summary>
    /// 日历方案单元测试类
    /// </summary>
    public class CalendarSchemeUnitTest : IClassFixture<TestStarup>
    {
        /// <summary>
        /// 是否创建默认数据
        /// </summary>
        public bool IsDefault { get; set; } = true;

        /// <summary>
        /// 日历方案 (CreateCalendarSchemeTest调用自动生成)
        /// </summary>
        public CalendarScheme NewCalendarScheme { get; set; }

        /// <summary>
        /// 班制 (CreateCalendarSchemeTest调用自动生成)
        /// </summary>
        public ShiftType NewShiftType { get; set; }

        /// <summary>
        /// 创建班制、日历方案单元测试方法
        /// </summary>
        [Fact]
        public void CreateCalendarSchemeTest()
        {
            // 指定用户和库存组织
            RT.Service.Resolve<ContextControllerTest>().InitContext();

            // 创建默认的班制、日历方案
            var ctrl = RT.Service.Resolve<CalendarSchemeTestController>();
            var ctrl1 = RT.Service.Resolve<ShiftTypeTestController>();
            NewShiftType = ctrl1.CreateShiftType(IsDefault);
            NewCalendarScheme = ctrl.CreateCalendarScheme(IsDefault);

            // 保存班制、日历方案
            NewCalendarScheme.SchemeWeeks.ForEach(p => p.ShiftTypeId = NewShiftType.Id);
            using (var tran = DB.TransactionScope(ResourcesEntityDataProvider.ConnectionStringName))
            {
                RF.Save(NewShiftType);
                RF.Save(NewCalendarScheme);
                tran.Complete();
            }

            // 启用
            RT.Service.Resolve<CalendarSchemeController>().EnableCalendarScheme(new System.Collections.Generic.List<CalendarScheme>() { NewCalendarScheme });

            // 设置缺省
            RT.Service.Resolve<CalendarSchemeController>().SetDefault(NewCalendarScheme);

            CalendarScheme dbCalendarScheme = ctrl.GetCalendarScheme(NewCalendarScheme.Name);
            Assert.NotNull(dbCalendarScheme);
            Assert.Equal(YesNo.Yes, dbCalendarScheme.IsDefault);
            Assert.Equal(YesNo.Yes, dbCalendarScheme.IsEnable);
        }
    }
}
