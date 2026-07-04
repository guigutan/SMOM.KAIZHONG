using SIE.xUnit.Core;
using Xunit;

namespace SIE.xUnit.CSM.Customers
{
    /// <summary>
    /// 客户单元测试
    /// </summary>
    public class CustomerControllerTest : IClassFixture<TestStarup>
    {
        /// <summary>
        /// 获取所有货主
        /// </summary>
        public void GetAllShipper()
        {
            //var customers = Controller.GetAllShipper();
            //Assert.All(_starup.CustomerList.Where(p => p.CustomerType == CustomerType.SHIPPER).ToList(), t => Assert.Contains(t.Code, customers.Select(p => p.Code).ToList()));
        }

        /// <summary>
        /// 验证货主信息
        /// </summary>
        public void CheckCustomerData()
        {
            //var customer = _starup.CustomerList.FirstOrDefault(p => p.CustomerType == CustomerType.SHIPPER);
            //Assert.NotNull(customer);
            //Controller.CheckCustomerData(customer.Code);
            //Assert.Throws<ValidationException>(() => Controller.CheckCustomerData("不存在"));
        }
    }
}
