using AutoFixture;

namespace SIE.xUnit.Core
{
    /// <summary>
    /// 测试构造者基类
    /// </summary>
    public abstract class BuilderBase
    {
        /// <summary>
        /// 构造器
        /// </summary>
        protected Fixture Fix { get; set; } = new Fixture();

    }
}
