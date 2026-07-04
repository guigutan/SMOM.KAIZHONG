using SIE.Domain;
using SIE.Resources;
using SIE.Resources.Enterprises;
using SIE.xUnit.Core;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SIE.xUnit.Resources.Enterprises
{
    /// <summary>
    /// 企业层级 单元测试
    /// </summary>
    public class EnterpriseUnitTest : IClassFixture<TestStarup>
    {
        /// <summary>
        /// 是否创建默认数据
        /// </summary>
        public bool IsDefault { get; set; } = true;

        /// <summary>
        /// 企业模型 (CreateEnterpriseTest调用自动生成)
        /// </summary>
        public EntityList<Enterprise> NewEnterpriseList { get; set; }

        /// <summary>
        /// 企业层级 (CreateEnterpriseTest调用自动生成)
        /// </summary>
        public EntityList<EnterpriseLevel> NewEnterpriseLevelList { get; set; }

        /// <summary>
        /// 创建企业层级单元测试方法
        /// </summary>
        [Fact]
        public void CreateEnterpriseLevelTest()
        {
            // 指定用户和库存组织
            RT.Service.Resolve<ContextControllerTest>().InitContext();

            // 创建默认的班制
            var ctrl = RT.Service.Resolve<EnterpriseLevelTestController>();
            List<EnterpriseLevel> levels = ctrl.CreateEnterpriseLevel(IsDefault);

            // 保存默认的班制
            EntityList<EnterpriseLevel> levelList = new EntityList<EnterpriseLevel>();
            levelList.AddRange(levels);
            using (var tran = DB.TransactionScope(ResourcesEntityDataProvider.ConnectionStringName))
            {
                RF.Save(levelList);
                tran.Complete();
            }

            // 启用
            foreach (var line in levelList.Where(p => p.Type == EnterpriseType.Line))
            {
                RT.Service.Resolve<EnterpriseController>().SetResource(line.Id, true);
            }

            List<EnterpriseLevel> dbLevels = ctrl.GetEnterpriseLevel(levelList.First(p => p.Type == EnterpriseType.Plant).Name);
            Assert.NotEmpty(dbLevels);
            Assert.Equal(levels.Count, dbLevels.Count);
            Assert.Equal(2, dbLevels.Count(p => p.IsResource && p.Type == EnterpriseType.Line));
        }

        /// <summary>
        /// 创建企业模型单元测试
        /// </summary>
        [Fact]
        public void CreateEnterpriseTest()
        {
            // 指定用户和库存组织
            RT.Service.Resolve<ContextControllerTest>().InitContext();

            // 创建企业层级、企业模型
            var ctrl = RT.Service.Resolve<EnterpriseTestController>();
            List<Enterprise> enterprises = ctrl.CreateEnterprise(IsDefault);

            // 保存默认的班制
            NewEnterpriseList = new EntityList<Enterprise>();
            NewEnterpriseList.AddRange(enterprises);
            NewEnterpriseLevelList = new EntityList<EnterpriseLevel>();
            NewEnterpriseLevelList.AddRange(NewEnterpriseList.Select(p => p.Level));
            //enterpriseList.Where(p => p.InvOrgId > 0).ForEach(p => p.PersistenceStatus = PersistenceStatus.Deleted);
            using (var tran = DB.TransactionScope(ResourcesEntityDataProvider.ConnectionStringName))
            {
                RF.Save(NewEnterpriseLevelList);
                RF.Save(NewEnterpriseList);
                tran.Complete();
            }

            List<Enterprise> dbEnterprises = ctrl.GetEnterprise(NewEnterpriseList.First(p => p.Name.Contains("工厂")).Name);
            Assert.NotEmpty(dbEnterprises);
            Assert.Equal(enterprises.Count, dbEnterprises.Count);
        }
    }
}