using SIE.Barcodes;
using SIE.EventMessages.MES.Barcodes;
using SIE.Modules;

[assembly: Module(typeof(Module))]

namespace SIE.Barcodes
{
    /// <summary>
    /// 比较简单的通用权限系统
    /// </summary>
    internal class Module : DomainModule
    {
        /// <summary>
        /// 初始化程序集
        /// </summary>
        /// <param name="app">Application</param>
        public override void Initialize(IApp app)
        {
            RT.Service.Register<IBarcode, BarcodeController>();
        }
    }
}