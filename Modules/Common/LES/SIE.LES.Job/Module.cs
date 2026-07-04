using SIE.Modules;

[assembly: Module(typeof(SIE.LES.Job.Module))]

namespace SIE.LES.Job
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