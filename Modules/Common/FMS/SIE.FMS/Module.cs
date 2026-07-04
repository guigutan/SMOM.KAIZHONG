using SIE.FMS;
using SIE.Modules;

[assembly: Module(typeof(Module))]
namespace SIE.FMS
{
    /// <summary>
    /// 模块设置
    /// </summary>
    public class Module : DomainModule
    {
        public override void Initialize(IApp app)
        {
        }
    }
}
