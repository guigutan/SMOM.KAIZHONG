using SIE.Modules;
using SIE.MES.Report;
using SIE.MES.Report.BatchWipProducts;

[assembly: Module(typeof(Module))]
namespace SIE.MES.Report
{
    /// <summary>
    /// 模块定义
    /// </summary>
    public class Module : DomainModule
    {
        /// <summary>
        /// 模块初始化
        /// </summary>
        /// <param name="app">应用程序</param>
        public override void Initialize(IApp app)
        {
            RT.Service.Register<BatchReportCriteriaProvider>();
        }
    }
}
