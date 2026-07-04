using SIE.xUnit.Core;
using Xunit;

namespace SIE.xUnit.EMS
{
    /// <summary>
    /// Ems单元测试基类
    /// </summary>
    [Collection("EmsStartupCollection")]
    public abstract class EmsControllerTestBase : TestBase
    {
    }
}
