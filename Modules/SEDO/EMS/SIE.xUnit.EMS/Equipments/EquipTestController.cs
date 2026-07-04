using SIE.Common.Catalogs;
using SIE.Domain;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipModels;
using SIE.Equipments.EquipTypes;
using SIE.Resources.Enterprises;
using SIE.xUnit.CSM;
using System.Linq;

namespace SIE.xUnit.EMS.Equipments
{
    /// <summary>
    /// 设备测试数据控制器
    /// </summary>
    public class EquipTestController : DomainController
    {
        public const string TypeCategoryCode = "0001";
        public const string ProjectCategory = "校准";

        /// <summary>
        /// 创建设备台账
        /// </summary>
        /// <param name="isSave">是否保存</param>
        /// <returns></returns>
        public virtual EquipAccount CreateEquipAccount(bool isSave = true)
        {
            var equip = new EquipAccount();
            equip.GenerateId();
            equip.Code = $"Code{equip.Id}";
            equip.Name = $"Name{equip.Id}";
            equip.UseLevel = GetOrCreateUseLevel("A");
            var model = CreateEquipModel();
            equip.EquipModel = model;
            equip.EquipModelId = model.Id;
            equip.ResPersonId = RT.IdentityId;
            equip.UseDepartmentId = CreateEnterprise().Id;
            var supplier = RT.Service.Resolve<CsmTestComtroller>().CreateSupplier();
            equip.SupplierId = supplier.Id;
            if (isSave)
                RF.Save(equip);
            return equip;
        }

        /// <summary>
        /// 创建部门
        /// </summary>
        /// <returns></returns>
        public virtual Enterprise CreateEnterprise()
        {
            Enterprise enter = new Enterprise();
            enter.GenerateId();
            enter.Code = $"Code{enter.Id}";
            enter.Name = $"Name{enter.Id}";
            enter.LevelId = CreateEnterpriseLevel().Id;
            RF.Save(enter);
            return enter;
        }

        /// <summary>
        /// 创建企业层级
        /// </summary>
        /// <returns></returns>
        public virtual EnterpriseLevel CreateEnterpriseLevel()
        {
            EnterpriseLevel level = new EnterpriseLevel();
            level.GenerateId();
            level.Code = $"Code{level.Id}";
            level.Name = $"Name{level.Id}";
            level.Type = EnterpriseType.Department;
            RF.Save(level);
            return level;
        }

        /// <summary>
        /// 创建设备型号
        /// </summary>
        /// <param name="isSave"></param>
        /// <returns></returns>
        public virtual EquipModel CreateEquipModel(bool isSave = true)
        {
            var model = new EquipModel();
            model.GenerateId();
            model.Code = $"Code{model.Id}";
            model.Name = $"Name{model.Id}";
            model.EquipType = CreateEquipType();
            if (isSave)
                RF.Save(model);
            return model;
        }

        /// <summary>
        /// 创建设备类型
        /// </summary>
        /// <returns></returns>
        public virtual EquipType CreateEquipType()
        {
            var entity = new EquipType();
            entity.GenerateId();
            entity.TypeCode = $"Code{entity.Id}";
            entity.TypeName = $"Name{entity.Id}";            
            entity.TypeCategory = GetOrCreateTypeCategory();
            RF.Save(entity);
            return entity;
        }

        /// <summary>
        /// 获取或创建类别快码
        /// </summary>
        /// <returns></returns>
        public virtual string GetOrCreateTypeCategory()
        {
            CheckCatalogCategory();
            return RT.Service.Resolve<CatalogController>().GetCatalog(EquipType.EquipTypeCatalogType, TypeCategoryCode).Name;
        }

        /// <summary>
        /// 检查快码
        /// </summary>
        public virtual void CheckCatalogCategory()
        {
            if (RT.Service.Resolve<CatalogController>().GetCatalogTypeList().FirstOrDefault(p => p.Code == EquipType.EquipTypeCatalogType) == null)
            {
                var catelogType = new CatalogType()
                {
                    Code = EquipType.EquipTypeCatalogType,
                    Name = "设备类型类别",
                    Description = "EMS设备类型类别"
                };
                catelogType.CatalogList.Add(new Catalog() { Code = "0001", Name = "生产设备", Description = "生产设备" });
                catelogType.CatalogList.Add(new Catalog() { Code = "0002", Name = "辅助设备", Description = "辅助设备" });
                catelogType.CatalogList.Add(new Catalog() { Code = "0003", Name = "办公设备", Description = "办公设备" });
                catelogType.CatalogList.Add(new Catalog() { Code = "0004", Name = "公用设备", Description = "公用设备" });
                RF.Save(catelogType);
            }
        }

        /// <summary>
        /// 获取或创建使用等级
        /// </summary>
        /// <returns></returns>
        public virtual string GetOrCreateUseLevel(string useLevel)
        {
            CheckCatalogUseLevel();
            return RT.Service.Resolve<CatalogController>()
                .GetCatalog(EquipAccount.EquipAccountUseLevel, useLevel).Name;
        }

        /// <summary>
        /// 检查快码-使用等级
        /// </summary>
        public virtual void CheckCatalogUseLevel()
        {
            if (RT.Service.Resolve<CatalogController>().GetCatalogTypeList()
                .FirstOrDefault(p => p.Code == EquipAccount.EquipAccountUseLevel) == null)
            {
                var catelogType = new CatalogType()
                {
                    Code = EquipAccount.EquipAccountUseLevel,
                    Name = "使用级别",
                    Description = "使用级别"
                };
                catelogType.CatalogList.Add(new Catalog() { Code = "A", Name = "A", Description = "A" });
                catelogType.CatalogList.Add(new Catalog() { Code = "B", Name = "B", Description = "B" });
                catelogType.CatalogList.Add(new Catalog() { Code = "C", Name = "C", Description = "C" });
                RF.Save(catelogType);
            }
        }
    }
}
