using SIE.ESop;
using SIE.ESop.Documents;
using SIE.EventMessages.MES.WipRecords;
using SIE.Modules;

[assembly: Module(typeof(Module))]

namespace SIE.ESop
{
    /// <summary>
    /// 模块配置
    /// </summary>
    class Module : DomainModule
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="app">app</param>
        public override void Initialize(IApp app)
        {
            RegisterService();
        }

        /// <summary>
        /// 注册服务
        /// </summary>
        private void RegisterService()
        {
            RT.Service.Register<IWipSop, DocumentCollectionController>();
        }
    }
}
