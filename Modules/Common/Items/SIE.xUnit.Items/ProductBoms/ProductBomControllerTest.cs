using SIE.Domain;
using SIE.Items;
using SIE.xUnit.Core;
using SIE.xUnit.Resources.WipResources;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SIE.xUnit.Items.ProductBoms
{
    /// <summary>
    /// 产品BOM控制器单元测试
    /// </summary>
    public class ProductBomControllerTest : IClassFixture<TestStarup>
    {
        /// <summary>
        /// 是否创建默认数据
        /// </summary>
        public bool IsDefault { get; set; } = true;

        /// <summary>
        /// 产品BOM（CreateAPSProductBoms调用自动生成）
        /// </summary>
        public EntityList<ProductBom> NewProductBomList { get; set; }

        /// <summary>
        /// 物料（CreateAPSProductBoms调用自动生成）
        /// </summary>
        public EntityList<Item> NewItemList { get; set; }

        /// <summary>
        /// 单位（CreateAPSProductBoms调用自动生成）
        /// </summary>
        public EntityList<Unit> NewUnitList { get; set; }

        /// <summary>
        /// 产品机型（CreateAPSProductBoms调用自动生成）
        /// </summary>
        public EntityList<ProductModel> NewProductModelList { get; set; }

        /// <summary>
        /// 生产资源数据创建
        /// </summary>
        public WipResourceUnitTest WipResourceUnitTest { get; set; }

        /// <summary>
        /// 创建产品BOM
        /// 1、创建班制、日历方案
        /// 2、创建企业层级、企业模型
        /// 3、创建制程工艺、工艺类型、工段
        /// 4、同步生产资源数据
        /// 5、设置产线的日历方案
        /// 6、设置产线的制程工艺类型
        /// 7、启用生产资源       
        /// 8、创建单位、产品机型、物料、产品BOM
        /// </summary>
        [Fact]
        public void CreateAPSProductBoms()
        {
            // 指定用户和库存组织
            RT.Service.Resolve<ContextControllerTest>().InitContext();

            WipResourceUnitTest = new WipResourceUnitTest() { IsDefault = this.IsDefault };
            WipResourceUnitTest.SyncWipResourceTest();

            // 创建单位、产品机型、物料、产品BOM
            var ctl = RT.Service.Resolve<ItemTestController>();
            var productBoms = ctl.CreateAPSProductBom(WipResourceUnitTest.ProcessTechUnitTest.NewProcessSegmentList);

            var itemList = new EntityList<Item>();
            var items = new List<Item>();
            var unitList = new EntityList<Unit>();
            var productModelList = new EntityList<ProductModel>();

            items.AddRange(productBoms.Select(p => p.Product));
            items.AddRange(productBoms.SelectMany(p => p.DetailList.Select(q => q.Item)).Where(p => !items.Any(q => q.Id == p.Id)));
            itemList.AddRange(items.Distinct().ToList());

            itemList.ForEach(p =>
            {
                if (!unitList.Any(q => q.Id == p.UnitId))
                    unitList.Add(p.Unit);
            });
            itemList.ForEach(p =>
            {
                if (!productModelList.Any(q => q.Id == p.ModelId))
                    productModelList.Add(p.Model);
            });

            using (var tran = DB.TransactionScope(ItemEntityDataProvider.ConnectionStringName))
            {
                RF.Save(unitList);
                RF.Save(productModelList);
                RF.Save(itemList);
                RF.Save(productBoms);
                tran.Complete();
            }

            if (productBoms.Any(p => !p.IsDefault))
            {
                productBoms.Where(p => !p.IsDefault).ForEach(p =>
                {
                    RT.Service.Resolve<ItemController>().SetDefaultProductBom(p.ProductId, p.Id);
                });
            }

            NewProductBomList = ctl.GetAPSProductBom(productBoms.Select(p => p.Code).ToList());
            NewItemList = new EntityList<Item>();
            NewUnitList = new EntityList<Unit>();
            NewProductModelList = new EntityList<ProductModel>();

            NewItemList.AddRange(NewProductBomList.Select(p => p.Product));
            NewItemList.AddRange(NewProductBomList.SelectMany(p => p.DetailList.Select(q => q.Item)).Where(p => !NewItemList.Any(q => q.Id == p.Id)));


            NewItemList.ForEach(p =>
            {
                if (!NewUnitList.Any(q => q.Id == p.UnitId))
                    NewUnitList.Add(p.Unit);
            });
            NewItemList.ForEach(p =>
            {
                if (!NewProductModelList.Any(q => q.Id == p.ModelId))
                    NewProductModelList.Add(p.Model);
            });

            Assert.Equal(productBoms.Count, NewProductBomList.Count);
            Assert.Equal(itemList.Count, NewItemList.Count);
            Assert.Equal(unitList.Count, NewUnitList.Count);
            Assert.Equal(productModelList.Count, NewProductModelList.Count);
        }

        ///// <summary>
        ///// 
        ///// </summary>
        //[Fact, Order(2)]
        //public void TestGetProductBomsByIds()
        //{
        //    Controller.GetProductBomsByIds(new double[] { });
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        //[Fact, Order(3)]
        //public void TestGetProductBomsByItemIds()
        //{
        //    Controller.GetProductBomsByItemIds(new List<double>());
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        //[Fact, Order(4)]
        //public void TestGetProductBomsByCodes()
        //{
        //    Controller.GetProductBoms(new List<string>());
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        //[Fact, Order(5)]
        //public void TestGetBomDetailPropertyValuesByDetailIds()
        //{
        //    Controller.GetProductBomDetails(new List<double>());
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        //[Fact, Order(6)]
        //public void TestGetBomDetailPropertyValues()
        //{
        //    Controller.GetBomDetailPropertyValues(new List<double>());
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        //[Fact, Order(5)]
        //public void TestSaveProductBomDetailPropertyValues()
        //{
        //    Controller.GetBomDetailPropertyValues(1, 1);
        //}
    }
}