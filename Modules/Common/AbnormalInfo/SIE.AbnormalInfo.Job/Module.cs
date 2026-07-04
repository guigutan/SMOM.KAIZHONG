using SIE.AbnormalInfo.Job;
using SIE.Modules;

[assembly: Module(typeof(Module))]
namespace SIE.AbnormalInfo.Job
{
    /// <summary>
    /// 模块配置
    /// </summary>
    public class Module : DomainModule
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="app">app</param>
        public override void Initialize(IApp app)
        {
            //初始化处理
        }
    }
}
