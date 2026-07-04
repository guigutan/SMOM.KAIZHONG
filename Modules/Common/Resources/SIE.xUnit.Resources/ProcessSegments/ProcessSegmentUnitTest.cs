using SIE.Domain;
using SIE.Resources;
using SIE.Resources.ProcessSegments;
using SIE.xUnit.Core;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SIE.xUnit.Resources.ProcessSegments
{
    public class ProcessSegmentUnitTest : IClassFixture<TestStarup>
    {
        /// <summary>
        /// 创建工段单元测试方法
        /// </summary>
        [Fact]
        public void CreateShiftTypeTest()
        {
            // 指定用户和库存组织
            RT.Service.Resolve<ContextControllerTest>().InitContext();

            // 创建默认的工段
            var ctrl = RT.Service.Resolve<ProcessSegmentTestController>();
            EntityList<ProcessSegment> segments = ctrl.CreateProcessSegment(true);

            using (var tran = DB.TransactionScope(ResourcesEntityDataProvider.ConnectionStringName))
            {
                // 保存默认的工段
                RF.Save(segments);
                tran.Complete();
            }

            List<string> codes = segments.Select(p => p.Code).ToList();
            EntityList<ProcessSegment> dbSegments = ctrl.GetProcessSegment(codes);

            Assert.Equal(segments.Count, dbSegments.Count);
        }
    }
}