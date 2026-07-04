using SIE.Domain;
using SIE.Resources;
using SIE.Resources.ProcessTechTypes;
using SIE.xUnit.Core;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SIE.xUnit.Resources.ProcessTechTypes
{
    /// <summary>
    /// 制程工艺类型单元测试
    /// </summary>
    public class ProcessTechTypeUnitTest : IClassFixture<TestStarup>
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
            var ctrl = RT.Service.Resolve<ProcessTechTypeTestController>();
            EntityList<ProcessTechType> processTechTypes = ctrl.CreateProcessTechType(true);

            using (var tran = DB.TransactionScope(ResourcesEntityDataProvider.ConnectionStringName))
            {
                // 保存默认的班制
                RF.Save(processTechTypes);
                tran.Complete();
            }

            List<string> codes = processTechTypes.Select(p => p.Code).ToList();
            EntityList<ProcessTechType> dbProcessTechTypes = ctrl.GetProcessTechType(codes);

            Assert.Equal(processTechTypes.Count, dbProcessTechTypes.Count);
        }
    }
}
