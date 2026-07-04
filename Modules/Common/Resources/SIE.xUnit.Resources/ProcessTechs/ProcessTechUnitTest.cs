using SIE.Domain;
using SIE.Resources;
using SIE.Resources.ProcessSegments;
using SIE.Resources.ProcessTechs;
using SIE.Resources.ProcessTechTypes;
using SIE.xUnit.Core;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SIE.xUnit.Resources.ProcessTechs
{
    /// <summary>
    /// 制程工艺 单元测试
    /// </summary>
    public class ProcessTechUnitTest : IClassFixture<TestStarup>
    {
        /// <summary>
        /// 是否创建默认数据
        /// </summary>
        public bool IsDefault { get; set; } = true;

        /// <summary>
        /// 制程工艺 （CreateProcessTechTest调用自动生成）
        /// </summary>
        public EntityList<ProcessTech> NewProcessTechList { get; set; }

        /// <summary>
        /// 制程工艺类型 （CreateProcessTechTest调用自动生成）
        /// </summary>
        public EntityList<ProcessTechType> NewProcessTechTypeList { get; set; }

        /// <summary>
        /// 工段 （CreateProcessTechTest调用自动生成）
        /// </summary>
        public EntityList<ProcessSegment> NewProcessSegmentList { get; set; }

        /// <summary>
        /// 创建制程工艺单元测试方法
        /// </summary>
        [Fact]
        public void CreateProcessTechTest()
        {
            // 指定用户和库存组织
            RT.Service.Resolve<ContextControllerTest>().InitContext();

            // 创建默认的制程工艺
            var ctrl = RT.Service.Resolve<ProcessTechTestController>();
            NewProcessTechList = ctrl.CreateProcessTech(IsDefault);
            NewProcessTechTypeList = new EntityList<ProcessTechType>();
            NewProcessSegmentList = new EntityList<ProcessSegment>();
            NewProcessTechTypeList.AddRange(NewProcessTechList.Select(p => p.ProcessTechType));
            NewProcessSegmentList.AddRange(NewProcessTechList.Select(p => p.ProcessSegment));
            using (var tran = DB.TransactionScope(ResourcesEntityDataProvider.ConnectionStringName))
            {
                // 保存默认的制程工艺
                RF.Save(NewProcessTechTypeList);
                RF.Save(NewProcessSegmentList);
                RF.Save(NewProcessTechList);
                tran.Complete();
            }

            List<string> codes = NewProcessTechList.Select(p => p.Code).ToList();
            EntityList<ProcessTech> dbTechs = ctrl.GetProcessTech(codes);

            Assert.Equal(NewProcessTechList.Count, dbTechs.Count);
        }
    }
}
