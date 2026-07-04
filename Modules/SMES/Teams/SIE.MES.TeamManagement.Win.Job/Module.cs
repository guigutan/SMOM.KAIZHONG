using SIE.MES.TeamManagement.Win.Job;
using SIE.Modules;

[assembly: Module(typeof(Module))]
namespace SIE.MES.TeamManagement.Win.Job
{
    /// <summary>
    /// 模块配置
    /// </summary>
    public class Module : DomainModule
    {
        /// <summary>
        /// 模块初始化
        /// </summary>
        /// <param name="app">应用程序</param>
        public override void Initialize(IApp app)
        {
        }
    }
}