using SIE.EventMessages.LES;
using SIE.EventMessages.WMS.Shipment;
using SIE.LES;
using SIE.LES.Interfaces;
using SIE.LES.MaterialMoves;
using SIE.LES.MaterialPreparations;
using SIE.LES.MaterialReceives;
using SIE.LES.MaterialReturnApplys;
using SIE.Modules;

[assembly: Module(typeof(Module))]
namespace SIE.LES
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
            RT.Service.Register<IIsStartLes, WoInfoForLesController>();
            RT.Service.Register<ISoUpdateStock, StockOrderController>();            
            RT.Service.Register<ILesMaterialPrepare, MaterialPreparationController>();
            RT.Service.Register<ILesMaterialReceive, MaterialReceiveController>();
            RT.Service.Register<ILesMaterialReturn, MaterialReturnApplyController>();
            RT.Service.Register<ILesMaterialMove, MaterialMoveRecordController>();
        }
    }
}