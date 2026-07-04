using SIE.Domain;
using SIE.Resources;
using SIE.Resources.ProcessTechTypes;
using SIE.Resources.WipResources;
using SIE.xUnit.Core;
using SIE.xUnit.Resources.CalendarSchemes;
using SIE.xUnit.Resources.Enterprises;
using SIE.xUnit.Resources.ProcessTechs;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SIE.xUnit.Resources.WipResources
{
    /// <summary>
    /// 生产资源单元测试
    /// </summary>
    public class WipResourceUnitTest : IClassFixture<TestStarup>
    {
        /// <summary>
        /// 是否创建默认数据
        /// </summary>
        public bool IsDefault { get; set; } = true;

        /// <summary>
        /// 生产资源 （SyncWipResourceTest调用自动生成）
        /// </summary>
        public EntityList<WipResource> NewWipResources { get; set; }

        /// <summary>
        /// 班制、日历方案 （SyncWipResourceTest调用自动生成）
        /// </summary>
        public CalendarSchemeUnitTest CalendarSchemeUnitTest { get; set; }

        /// <summary>
        /// 企业模型、企业层级 （SyncWipResourceTest调用自动生成）
        /// </summary>
        public EnterpriseUnitTest EnterpriseUnitTest { get; set; }

        /// <summary>
        /// 制程工艺、工段、工艺类型 （SyncWipResourceTest调用自动生成）
        /// </summary>
        public ProcessTechUnitTest ProcessTechUnitTest { get; set; }

        /// <summary>
        /// 同步生产资源单元测试方法
        /// 1、创建班制、日历方案
        /// 2、创建企业层级、企业模型
        /// 3、创建制程工艺、工艺类型、工段
        /// 4、同步生产资源数据
        /// 5、设置产线的日历方案
        /// 6、设置产线的制程工艺类型
        /// 7、启用生产资源
        /// </summary>
        [Fact]
        public void SyncWipResourceTest()
        {
            // 指定用户和库存组织
            RT.Service.Resolve<ContextControllerTest>().InitContext();

            // 创建班制、日历方案
            CalendarSchemeUnitTest = new CalendarSchemeUnitTest() { IsDefault = this.IsDefault };
            CalendarSchemeUnitTest.CreateCalendarSchemeTest();

            // 创建企业层级、企业模型
            EnterpriseUnitTest = new EnterpriseUnitTest() { IsDefault = this.IsDefault };
            EnterpriseUnitTest.CreateEnterpriseTest();

            // 创建制程工艺、工艺类型、工段
            ProcessTechUnitTest = new ProcessTechUnitTest() { IsDefault = this.IsDefault };
            ProcessTechUnitTest.CreateProcessTechTest();

            // 同步生产资源数据
             RT.Service.Resolve<WipResourceController>().RunSync();

            List<string> codes = EnterpriseUnitTest.NewEnterpriseList.Where(p => p.IsResource).Select(p => p.Code).ToList();
            NewWipResources = RT.Service.Resolve<WipResourceTestController>().GetWipResource(codes);
            Assert.Equal(codes.Count, NewWipResources.Count);

            // 设置产线的日历方案
            NewWipResources.ForEach(p => { p.SchemeId = CalendarSchemeUnitTest.NewCalendarScheme.Id; p.AutomationType = AutomationType.SemiAutomatic; });

            // 设置产线的制程工艺类型
            string smtCode = "SMT";
            ProcessTechType smtType = ProcessTechUnitTest.NewProcessTechTypeList.First(p => p.Code.Contains(smtCode));
            NewWipResources.Where(p => p.Code.Contains(smtCode)).ForEach(p => { p.ProcessTechTypeId = smtType.Id; p.ProcessTechType = smtType; });

            string dipCode = "DIP";
            ProcessTechType dipType = ProcessTechUnitTest.NewProcessTechTypeList.First(p => p.Code.Contains(dipCode));
            NewWipResources.Where(p => p.Code.Contains(dipCode)).ForEach(p => { p.ProcessTechTypeId = dipType.Id; p.ProcessTechType = dipType; });

            string assyCode = "ASSY";
            ProcessTechType assyType = ProcessTechUnitTest.NewProcessTechTypeList.First(p => p.Code.Contains(assyCode));
            NewWipResources.Where(p => p.Code.Contains(assyCode)).ForEach(p => { p.ProcessTechTypeId = assyType.Id; p.ProcessTechType = assyType; });

            using (var tran = DB.TransactionScope(ResourcesEntityDataProvider.ConnectionStringName))
            {
                // 保存默认的制程工艺
                RF.Save(NewWipResources);
                tran.Complete();
            }

            // 启用
            RT.Service.Resolve<WipResourceController>().EnableWipResource(NewWipResources.ToList());
            NewWipResources = RT.Service.Resolve<WipResourceTestController>().GetWipResource(codes);
            Assert.True(NewWipResources.All(p => p.ResourceState == ResourceState.Actived));
        }
    }
}
