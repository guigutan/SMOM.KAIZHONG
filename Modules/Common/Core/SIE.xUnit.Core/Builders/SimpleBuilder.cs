using AutoFixture.Dsl;

namespace SIE.xUnit.Core.Builders
{
    /// <summary>
    /// 简单构造器
    /// </summary>
    public class SimpleBuilder : BuilderBase
    {
        public IPostprocessComposer<T> Build<T>()
        {
            return Fix.Build<T>();
        }
    }
}
