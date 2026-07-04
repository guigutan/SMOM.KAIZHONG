using SIE.xUnit.QMS.Common;
using Xunit;

namespace SIE.xUnit.Elec.IQC
{
    /// <summary>
    /// Ems单元测试基类
    /// </summary>
    [Collection("ElecIqcStartupCollection")]
    public abstract class ElecIqcControllerTestBase : QmsControllerTestBase
    {
    }
}
