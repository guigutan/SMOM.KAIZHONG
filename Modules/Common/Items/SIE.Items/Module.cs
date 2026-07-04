using SIE.EventMessages.Items;
using SIE.Items;
using SIE.Items.Items;
using SIE.Modules;
using SIE.Units;

[assembly: Module(typeof(Module))]

namespace SIE.Items
{
    /// <summary>
    /// 比较简单的通用权限系统
    /// </summary>
    internal class Module : DomainModule
    {
        /// <summary>
        /// 初始化程序集
        /// </summary>
        /// <param name="app">应用程序</param>
        public override void Initialize(IApp app)
        {
            // RT.Service.Register<IItemUnit, ItemUnitController>();
            RT.Service.Register<IItem, ItemController>();
        }
    }
}
