using SIE.Modules;
using SIE.EMS.MeteringEquipment;
using SIE.EventMessages.EMS.MeteringEquipments;
using SIE.EMS.MeteringEquipment.Calibrations;

[assembly: Module(typeof(Module))]
namespace SIE.EMS.MeteringEquipment
{
    /// <summary>
    /// 模块配置
    /// </summary>
    public class Module : DomainModule
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
            RT.Service.Register<ICalibration, CalibrationController>();
        }
    }
}
