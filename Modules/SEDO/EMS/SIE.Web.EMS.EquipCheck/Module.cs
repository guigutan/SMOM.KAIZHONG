using SIE.MetaModel;
using SIE.Modules;
using SIE.Web.EMS.EquipCheck;
using System;

[assembly: Module(typeof(Module))]
namespace SIE.Web.EMS.EquipCheck
{
    /// <summary>
    /// 模块配置
    /// </summary>
    public class Module : UIModule
    {
        /// <summary>
        /// 初始化模块
        /// </summary>
        /// <param name="app">应用程序</param>
        public override void Initialize(IApp app)
        {
        }
    }
}
