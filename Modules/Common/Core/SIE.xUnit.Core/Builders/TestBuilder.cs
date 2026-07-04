using AutoFixture.Dsl;

namespace SIE.xUnit.Core.Builders
{
    /// <summary>
    /// 测试数据构造者
    /// </summary>
    public static class TestBuilder
    {
        static SimpleBuilder builderBase = new SimpleBuilder();
        
        /// <summary>
        /// 构造数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IPostprocessComposer<T> Build<T>()
        {
            return builderBase.Build<T>();
        }

        /// <summary>
        /// 构造实体
        /// </summary>
        /// <typeparam name="TBuilder"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IPostprocessComposer<T> BuildEntity<TBuilder, T>() where TBuilder : EntityBuilder<T>, new()
        {
            TBuilder builder = new TBuilder();
            return builder.Build();
        }
    }
}
