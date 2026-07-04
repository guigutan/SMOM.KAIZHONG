using SIE.EMS;
using SIE.EMS.Equipments;
using SIE.Equipments.EquipAccounts;
using SIE.EventMessages.EAP.Equipments;
using SIE.EventMessages.EMS.EquipAccount;
using SIE.EventMessages.EMS.Equipments;
using SIE.Modules;
using SIE.SMDC;

[assembly: Module(typeof(Module))]
namespace SIE.EMS
{
    /// <summary>
    /// 模块定义
    /// </summary>
    class Module : DomainModule
    {
        /// <summary>
        /// 模块的初始化方法
        /// </summary>
        /// <param name="app">应用程序对象</param>
        public override void Initialize(IApp app)
        {
            RegisterService();
           
            DataAuth.EmsAuthInterceptor.Intercept();
        }
        /// <summary>
        /// IOC注册服务
        /// </summary>
        private void RegisterService()
        {
            RT.Service.Register(typeof(IEquipmentQuery), typeof(EquipmentQueryController));
            RT.Service.Register(typeof(IEquipmentEap), typeof(EquipmentSmdcController));
            RT.Service.Register(typeof(IEquipAccount), typeof(EquipController));

            //启用EMS项目，设备台账的使用部门需要按权限过滤
            RT.Service.Register(typeof(UserDepartmentController), typeof(UserDepartmentPermissionController));
        }
    }
}