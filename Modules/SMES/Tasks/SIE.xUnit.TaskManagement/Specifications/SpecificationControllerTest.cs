using SIE.MES.TaskManagement.Specifications;
using SIE.xUnit.Core;
using System.Linq;
using Xunit;

namespace SIE.xUnit.TaskManagement.Specifications
{
    /// <summary>
    /// 规格件测试控制器
    /// </summary>
    public class SpecificationControllerTest : IClassFixture<TestStarup>
    {
        static ContextControllerTest tsContextCt = RT.Service.Resolve<ContextControllerTest>();
        static ProductSpecificationController specCt = RT.Service.Resolve<ProductSpecificationController>();
        static TaskManagementTestController taskTestCt = RT.Service.Resolve<TaskManagementTestController>();

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void TestGetDatas()
        {
            tsContextCt.InitContext();
            var proSpecifications = taskTestCt.CreateProductSpecifications(3);
            Assert.NotNull(proSpecifications);
            Assert.Equal(3, proSpecifications.Count);
            var proSpecifications1 = specCt.GetProductSpecificationList(new ProductSpecificationCriteria() { });
            Assert.NotNull(proSpecifications1);
            var ids = proSpecifications.Select(p => p.Id).Distinct().ToList();
            var proSpecifications2 = specCt.GetProductSpecifications(ids);
            Assert.NotNull(proSpecifications2);
            var specDetails = specCt.GetProductSpecificationDetails(ids);
            Assert.NotNull(specDetails);
            var specIds = specDetails.Select(p => p.SpecificationId).Distinct().ToList();
            var specifications = specCt.GetSpecifications(specIds);
            Assert.NotNull(specifications);
            var productId = proSpecifications.Select(p => p.ProductId).Distinct().FirstOrDefault();
            var proSpecification = specCt.GetProductSpecification(productId);
            Assert.NotNull(proSpecification);
            specCt.DeletedProdSpecifications(ids);
            var proSpecifications3 = specCt.GetProductSpecifications(ids);
            Assert.Equal(0, proSpecifications3.Count);
        }
    }
}
