using SIE.Common.Catalogs;
using SIE.Core.Common.Controllers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.xUnit.Items
{
    /// <summary>
    /// 测试物料数据控制器
    /// </summary>
    public partial class ItemTestController : DomainController
    {
        #region 物料 
        /// <summary>
        /// 创建物料
        /// </summary>
        /// <param name="count">单位数据数量</param>
        /// <returns>物料集合</returns>
        public virtual EntityList<Item> CreateWipProduct(int count)
        {
            if (count == 0)
                throw new ValidationException("数据数量必须大于0");
            var unit = CreateUnit(1).FirstOrDefault();
            EntityList<Item> items = new EntityList<Item>();
            for (int i = 0; i < count; i++)
            {
                var item = new Item();
                item.GenerateId();
                double id = item.Id;
                item.Code = $"产品{id}";
                item.Name = $"产品{id}";
                item.Type = ItemType.Product;
                item.Unit = unit;
                items.Add(item);
            }
            RF.Save(items);
            return items;
        }

        /// <summary>
        /// 创建任务单对应工单的产品
        /// </summary>
        /// <param name="isVirtualPart">是否虚拟件</param>
        /// <param name="familyId">产品族Id</param>
        /// <returns>产品</returns>
        public virtual Item CreateTaskProduct(bool isVirtualPart, double? familyId)
        {
            using (var tran = DB.TransactionScope(ItemEntityDataTestProvider.ConnectionStringName))
            {
                var unit = CreateUnit(1).FirstOrDefault();
                var item = new Item();
                item.GenerateId();
                double id = item.Id;
                item.Code = $"产品{id}";
                item.Name = $"产品{id}";
                item.Type = ItemType.Product;
                item.ProductFamilyId = familyId;
                item.IsVirtualPart = isVirtualPart;
                item.Unit = unit;
                RF.Save(item);
                tran.Complete();
                return item;
            }
        }

        #endregion

        #region 产品BOM 
        /// <summary>
        /// 创建产品BOM
        /// </summary>
        /// <param name="count">数据数量</param>
        /// <returns>产品BOM集合</returns>
        public virtual EntityList<ProductBom> CreateBom(int count)
        {
            if (count == 0)
                throw new ValidationException("数据数量必须大于0");
            EntityList<ProductBom> boms = new EntityList<ProductBom>();
            for (int i = 0; i < count; i++)
            {
                var bom = new ProductBom();
                bom.GenerateId();
                double id = bom.Id;
                bom.Code = $"ProductBomCode{id}";
                bom.Name = $"ProductBomName{id}";
                bom.Version = id.ToString();
                boms.Add(bom);
            }
            RF.Save(boms);
            return boms;
        }
        #endregion

        #region 产品族 
        /// <summary>
        /// 创建产品族
        /// </summary>
        /// <param name="count">产品族数据数量</param>
        /// <returns>产品族集合</returns>
        public virtual EntityList<ProductFamily> CreateProductFamily(int count)
        {
            if (count == 0)
                throw new ValidationException("数据数量必须大于0");
            var category = CreateProductFamilyCategory();
            EntityList<ProductFamily> families = new EntityList<ProductFamily>();
            for (int i = 0; i < count; i++)
            {
                var family = new ProductFamily();
                family.GenerateId();
                double id = family.Id;
                family.Code = $"FamilyCode{id}";
                family.Name = $"FamilyName{id}";
                family.Category = category;
                families.Add(family);
            }
            RF.Save(families);
            return families;
        }

        /// <summary>
        /// 创建产品族分类
        /// </summary>
        /// <returns>产品族分类</returns>
        public virtual ProductFamilyCategory CreateProductFamilyCategory()
        {
            var category = new ProductFamilyCategory();
            category.GenerateId();
            category.Code = $"CategoryCode{category.Id}";
            category.Name = $"CategoryName{category.Id}";
            RF.Save(category);
            return category;
        }

        /// <summary>
        /// 获取一个产品族分类，没有时新建
        /// </summary>
        /// <returns>产品族分类</returns>
        public virtual ProductFamilyCategory GetFirstProductFamilyCategory()
        {
            var productFamilyCategory = Query<ProductFamilyCategory>().FirstOrDefault();
            if (productFamilyCategory == null)
                productFamilyCategory = CreateProductFamilyCategory();
            return productFamilyCategory;
        }

        /// <summary>
        /// 根据产品族分类ID获取一个产品族，没有时新建
        /// </summary>
        /// <returns>产品族</returns>
        public virtual ProductFamily GetFirstProductFamily(double categoryId)
        {
            var productFamily = Query<ProductFamily>().Where(p => p.CategoryId == categoryId).FirstOrDefault();
            if (productFamily == null)
            {
                productFamily = new ProductFamily();
                productFamily.GenerateId();
                double id = productFamily.Id;
                productFamily.Code = $"FamilyCode{id}";
                productFamily.Name = $"FamilyName{id}";
                productFamily.CategoryId = categoryId;
                RF.Save(productFamily);
            }
            return productFamily;
        }

        #endregion

        #region 产品机型  
        /// <summary>
        /// 创建产品机型
        /// </summary>
        /// <param name="count">数据数量</param>
        /// <returns>产品机型集合</returns>
        public virtual EntityList<ProductModel> CreateProductModel(int count)
        {
            if (count == 0)
                throw new ValidationException("数据数量必须大于0");
            Random random = new Random();
            EntityList<ProductModel> productModels = new EntityList<ProductModel>();
            for (int i = 0; i < count; i++)
            {
                var model = new ProductModel();
                model.GenerateId();
                double id = model.Id;
                model.Code = $"ProductModelCode{id}";
                model.Name = $"ProductModelName{id}";
                model.WorkingHours = random.Next(0, 36000);
                model.SendingHours = 24;
                productModels.Add(model);
            }
            RF.Save(productModels);
            return productModels;
        }
        #endregion

        #region 单位
        /// <summary>
        /// 创建单位
        /// </summary>
        /// <param name="count">单位数据数量</param>
        /// <returns>单位集合</returns>
        public virtual Unit GetOrCreateUnit(string code)
        {
            var unit = RT.Service.Resolve<CommonController>().GetData<Unit>(p => p.Code == code && p.Name == code);
            if (unit != null)
                return unit;
            var unitTypes = GetUnitTypes();
            Random random = new Random();
            unit = new Unit();
            unit.GenerateId();
            unit.Code = code;
            unit.Name = code;
            unit.Type = unitTypes.Count > 0 ? unitTypes[random.Next(0, unitTypes.Count)] : "";
            unit.Precision = random.Next(0, 9);
            RF.Save(unit);
            return unit;
        }

        /// <summary>
        /// 创建单位
        /// </summary>
        /// <param name="count">单位数据数量</param>
        /// <returns>单位集合</returns>
        public virtual EntityList<Unit> CreateUnit(int count)
        {
            if (count == 0)
                throw new ValidationException("数据数量必须大于0");
            var unitTypes = GetUnitTypes();
            Random random = new Random();
            EntityList<Unit> units = new EntityList<Unit>();
            for (int i = 0; i < count; i++)
            {
                var unit = new Unit();
                unit.GenerateId();
                double id = unit.Id;
                unit.Code = $"UnitCode{id}";
                unit.Name = $"UnitName{id}";
                unit.Type = unitTypes.Count > 0 ? unitTypes[random.Next(0, unitTypes.Count)] : "";
                unit.Precision = random.Next(0, 9);
                units.Add(unit);
            }
            RF.Save(units);
            return units;
        }

        /// <summary>
        /// 获取单位类型
        /// </summary>
        /// <returns>单位类型快码编码列表</returns>
        private List<string> GetUnitTypes()
        {
            return RT.Service.Resolve<CatalogController>().GetCatalogList(Unit.CatalogType).Select(p => p.Code).ToList();
        }
        #endregion 
    }
}