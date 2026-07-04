using SIE.Core.CheckAlgorithms.ManufacturedSN;
using Xunit;

namespace SIE.xUnit.Core.CheckAlgorithms
{
    /// <summary>
    /// 序列号校验码算法单元测试类
    /// </summary>
    public class ComputeCheckTest
    {
        /// <summary>
        /// 序列号校验码算法类
        /// </summary>
        public ManufacturedSnCheckAlgorithm algorithm { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public ComputeCheckTest()
        {
            algorithm = new ManufacturedSnCheckAlgorithm();
        }

        /// <summary>
        /// 计算
        /// </summary>
        [Fact]
        public void ComputeTest1()
        {
            var code = "F12107V001230";
            var result = algorithm.Compute(code);
            Assert.Equal("0", result);
        }

        /// <summary>
        /// 计算
        /// </summary>
        [Fact]
        public void ComputeTest2()
        {
            var code = "E12107V00123";
            var result = algorithm.Compute(code);
            Assert.Equal("C", result);
        }

        /// <summary>
        /// 计算
        /// </summary>
        [Fact]
        public void ComputeTest3()
        {
            var code = "G12107V00123";
            var result = algorithm.Compute(code);
            Assert.Equal("1", result);
        }

        /// <summary>
        /// 计算
        /// </summary>
        [Fact]
        public void ComputeTest4()
        {
            var code = "000000000000000";
            var result = algorithm.Compute(code);
            Assert.Equal("0", result);
        }

        /// <summary>
        /// 计算,位数不足
        /// </summary>
        [Fact]
        public void ComputeTest5()
        {
            var code = "123456789";
            var result = algorithm.Compute(code);
            Assert.Equal("", result);
        }
    }
}
