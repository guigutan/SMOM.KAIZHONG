using SIE.xUnit.Core;
using Xunit;

namespace SIE.xUnit.Items.ProductModels
{
    /// <summary>
    /// 产品机型控制器测试
    /// </summary>
    public class ProductModelControllerTest : IClassFixture<TestStarup>
    {
        ///// <summary>
        ///// 测试通过ID获取产品机型
        ///// </summary>
        //[Fact, Order(2)]
        //public void TestGetProductModelById()
        //{
        //    var model = _starup.ProductModels.FirstOrDefault();
        //    Assert.NotNull(model);
        //    //测试非法参数
        //    Assert.Throws<ArgumentException>(() => Controller.GetProductModel(0));
        //    //测试不存在参数
        //    var dbModel1 = Controller.GetProductModel(9999999999);
        //    Assert.Null(dbModel1);
        //    //测试正确参数
        //    var dbModel2 = Controller.GetProductModel(model.Id);
        //    Assert.NotNull(dbModel2);
        //}

        ///// <summary>
        ///// 测试编码或者名称获取产品机型
        ///// </summary>
        //[Fact, Order(3)]
        //public void TestGetProductModelByCodeOrName()
        //{
        //    var model = _starup.ProductModels.FirstOrDefault();
        //    Assert.NotNull(model);
        //    //测试非法参数
        //    Assert.Throws<ArgumentException>(() => Controller.GetProductModel(""));
        //    //测试不存在参数
        //    var dbModel1 = Controller.GetProductModel(Guid.NewGuid().ToString());
        //    Assert.Null(dbModel1);
        //    //测试正确参数
        //    var dbModel2 = Controller.GetProductModel(model.Code);
        //    Assert.NotNull(dbModel2);
        //    var dbModel3 = Controller.GetProductModel(model.Name);
        //    Assert.NotNull(dbModel3);
        //}

        ///// <summary>
        ///// 测试编码或者名称获取产品机型列表
        ///// </summary>
        //[Fact, Order(4)]
        //public void TestGetProductModelsByCodeOrName()
        //{
        //    var model = _starup.ProductModels.FirstOrDefault();
        //    Assert.NotNull(model);
        //    //测试不存在参数
        //    var dbModels1 = Controller.GetProductModels(Guid.NewGuid().ToString(), null);
        //    Assert.Empty(dbModels1);
        //    //测试正确参数
        //    var dbModels2 = Controller.GetProductModels(model.Code, null);
        //    Assert.Single(dbModels2);
        //    var dbModels3 = Controller.GetProductModels(model.Name, null);
        //    Assert.Single(dbModels3);
        //    var dbModels4 = Controller.GetProductModels("", null);
        //    Assert.InRange(dbModels4.Count, _starup.ProductModels.Count, int.MaxValue);
        //}
    }
}