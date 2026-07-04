using SIE.EMS.Job;
using SIE.Modules;

[assembly: Module(typeof(Module))]
namespace SIE.EMS.Job
{
    /// <summary>
    /// 模块配置
    /// </summary>
    public class Module : DomainModule
    {
        /// <summary>
        /// 初始化方法
        /// </summary>
        /// <param name="app">app</param>
        public override void Initialize(IApp app)
        {
        }
    }
}