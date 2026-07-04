using SIE.Kit.QMS;
using SIE.Kit.QMS.InspectionGroups;
using SIE.Modules;
using SIE.QMS.InspectionGroups;

[assembly: Module(typeof(Module))]
namespace SIE.Kit.QMS
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
            RT.Service.Register<InspectionGroupController, KitInspectionGroupController>();   //注入检验组控制器
        }
    }
}
