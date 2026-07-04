using SIE.Modules;
using SIE.EMS.SpecialEquipment;
using SIE.EventMessages.EMS.SpecialEquipments;
using SIE.EMS.SpecialEquipment.RegularInspections;

[assembly: Module(typeof(Module))]

namespace SIE.EMS.SpecialEquipment
{
    /// <summary>
    /// 当前工程所对应的模块类
    /// </summary>
    internal class Module : DomainModule
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="app">应用程序对象</param>
        public override void Initialize(IApp app)
        {
            RegisterService();
        }

        /// <summary>                                                                                                      
        /// 注册服务
        /// </summary>
        private void RegisterService()
        {
            RT.Service.Register<IRegularInspection, RegularInspectionController>();
        }
    }
}
