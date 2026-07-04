using SIE.xUnit.Core;
using Xunit;

namespace SIE.xUnit.Items.ProductFamilys
{
    /// <summary>
    /// 产品族控制器测试
    /// </summary>
    public class ProductFamilyControllerTest : IClassFixture<TestStarup>
    {
        ///// <summary>
        ///// 测试通过产品族分类名称获取产品族分类
        ///// </summary>
        //[Fact, Order(2)]
        //public void TestGetProductFamilyCateByName()
        //{
        //    var category = _starup.CategoryList.FirstOrDefault();
        //    Assert.NotNull(category);
        //    //测试空参数
        //    Assert.Throws<ArgumentException>(() => { Controller.GetProductFamilyCateByName(""); });
        //    //测试正确参数
        //    var dbCategory1 = Controller.GetProductFamilyCateByName(category.Name);
        //    Assert.NotNull(dbCategory1);
        //    //测试错误参数
        //    var dbCategory2 = Controller.GetProductFamilyCateByName(category.Code);
        //    Assert.Null(dbCategory2);
        //}

        ///// <summary>
        ///// 测试通过产品族编码或名称获取产品族列表
        ///// </summary>
        //[Fact, Order(3)]
        //public void TestGetProductFamily()
        //{
        //    var family = _starup.FamilyList.FirstOrDefault();
        //    Assert.NotNull(family);
        //    //测试空参数
        //    var dbFamilies1 = Controller.GetProductFamily("", new PagingInfo() { PageSize = 5 });
        //    Assert.NotEmpty(dbFamilies1);
        //    //测试正确参数
        //    var dbFamilies2 = Controller.GetProductFamily(family.Code, null);
        //    Assert.Single(dbFamilies2);
        //    var dbFamilies3 = Controller.GetProductFamily(family.Name, null);
        //    Assert.Single(dbFamilies3);
        //    //测试错误参数
        //    var dbFamilies4 = Controller.GetProductFamily(Guid.NewGuid().ToString(), null);
        //    Assert.Empty(dbFamilies4);
        //}

        ///// <summary>
        ///// 测试通过产品族ID集合获取产品族列表
        ///// </summary>
        //[Fact, Order(4)]
        //public void TestGetProductFamilyList()
        //{
        //    List<double> ids = _starup.FamilyList.Select(p => p.Id).ToList();
        //    //测试空参数
        //    Assert.Throws<ArgumentNullException>(() => { Controller.GetProductFamilyList(null); });
        //    //测试空集合
        //    var dbFamilies1 = Controller.GetProductFamilyList(new List<double>());
        //    Assert.Empty(dbFamilies1);
        //    //测试正确参数
        //    var dbFamilies2 = Controller.GetProductFamilyList(ids);
        //    Assert.Equal(_starup.FamilyList.Count, dbFamilies2.Count);
        //    //测试错误参数
        //    var dbFamilies3 = Controller.GetProductFamilyList(new List<double>() { int.MaxValue });
        //    Assert.Empty(dbFamilies3);
        //}
    }
}