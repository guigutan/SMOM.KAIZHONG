using AutoFixture.Dsl;

namespace SIE.xUnit.Core.Builders
{
    /// <summary>
    /// 实体构造器
    /// </summary>
    public abstract class EntityBuilder<T> : BuilderBase
    {
        /// <summary>
        /// 实体构造
        /// </summary>
        /// <returns></returns>
        public abstract IPostprocessComposer<T> Build();
    }
}
