using SIE.EventMessages.MES.WIP;
using SIE.Mes.Mq;
using SIE.Mes.Mq.Edge;
using SIE.Mes.Mq.Listener;
using SIE.MES.Edge;
using SIE.MES.Edge.Models;
using SIE.MetaModel;
using SIE.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: Module(typeof(Module))]
namespace SIE.Mes.Mq
{
    /// <summary>
    /// 模块配置
    /// </summary>
    class Module : DomainModule
    { 
        /// <summary>
        /// 模块初始化
        /// </summary>
        /// <param name="app">应用程序</param>  
        public override void Initialize(IApp app)
        {
            RT.Logger.Info("边缘采集消息 SIE.Mes.Mq Module.Initialize".L10N());
            app.StartupCompleted += App_StartupCompleted;
            RT.Service.Register<ICollectDataDao, CollectDataDao>( Services.ServiceLifeStyle.Singleton);
            RT.Service.Register<ICollectMaterialDao, CollectMaterialDao>( Services.ServiceLifeStyle.Singleton);
            RT.Service.Register<IEdgeErrorMessageDao, EdgeErrorMessageDao>( Services.ServiceLifeStyle.Singleton);
            RT.Service.Register<ICollectDataService, CollectDataService>( Services.ServiceLifeStyle.Singleton);
            RT.Service.Register<ICollectMaterialService, CollectMaterialService>(Services.ServiceLifeStyle.Singleton);
            RT.Service.Register<IMesEdgeListener, MesEdgeListener>(Services.ServiceLifeStyle.Singleton);
            RT.Service.Register<IMessageService, EdgeMessageController>(Services.ServiceLifeStyle.Singleton);
        }

        /// <summary>
        /// 程序启动完成事件
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void App_StartupCompleted(object sender, EventArgs e)
        {
            RT.Logger.Info("边缘采集消息 SIE.Mes.Mq Module.App_StartupCompleted。".L10N());
            InitEventListener();
        }

        /// <summary>
        /// 初始化事件监听
        /// </summary>
        private void InitEventListener()
        {
            RT.Service.Resolve<IMesEdgeListener>().Start();
        }
    }
}
