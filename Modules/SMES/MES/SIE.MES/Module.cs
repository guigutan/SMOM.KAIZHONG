using SIE.Domain;
using SIE.Domain.Query;
using SIE.EventMessages.MES.LoadItems;
using SIE.EventMessages.MES.PPSNs;
using SIE.EventMessages.MES.WIP;
using SIE.EventMessages.MES.WorkOrders;
using SIE.EventMessages.PatrolInspBills;
using SIE.EventMessages.Release;
using SIE.MES;
using SIE.MES.BatchWIP.Products;
using SIE.MES.Edge;
using SIE.MES.Events;
using SIE.MES.Interfaces.ApsTasks;
using SIE.MES.Interfaces.PatrolInspBills;
using SIE.MES.LoadItems;
using SIE.MES.PanelBindings.Listeners;
using SIE.MES.Wip.Products;
using SIE.MES.WIP;
using SIE.MES.WIP.Products;
using SIE.MES.WIP.Service;
using SIE.MES.WorkOrders;
using SIE.MES.WorkOrders.Interfaces;
using SIE.Modules;
using System;

[assembly: Module(typeof(Module))]

namespace SIE.MES
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
            app.StartupCompleted += App_StartupCompleted;
            RT.Service.Register<BatchCriteriaProvider>();
            RepositoryDataProvider.Querying += RepositoryDataProvider_Querying;
            RT.Service.Register<IPlanTaskRelease, TaskReleaseController>();
            RT.Service.Register<IPlanTaskSplit, TaskSplitController>();
            RT.Service.Register<IPatrolInspBillEvent, PatrolInspBillEvent>();
            RT.Service.Register<IProcessConditionService, ProcessConditionService>();
            RT.Service.Register<IWorkOrderQuery, WorkOrderQueryController>();
            RT.Service.Register<IWipController, WipController>();
            RT.Service.Register<IEdgeWipService, EdgeWipService>();
            RT.Service.Register<IEdgeWipDao, EdgeWipDao>();
            RT.Service.Register<ILoadItemService, LoadItemService>();
            RT.Service.Register<IGetWorkOrderWipInfo, GetWorkOrderWipInfoController>();
            RT.Service.Register<IPPSNQuery, PPSNQuery>();
            RT.Service.Register<IWoCostItem, WoCostItemController>();

        }

        /// <summary>
        /// 查询前排序
        /// </summary>
        /// <param name="sender">数据提供者</param>
        /// <param name="e">参数</param>
        private void RepositoryDataProvider_Querying(object sender, QueryingEventArgs e)
        {
            var provider = sender as MesCoreEntityDataProvider;
            if (provider != null && provider.Repository.EntityType == typeof(BatchWipProductProcess))
            {
                e.Args.Query.OrderBy.Clear();
                e.Args.Query.OrderBy.Add(QueryFactory.Instance.OrderBy(e.Args.Query.MainTable.FindColumn(BatchWipProductProcess.InputDateProperty)));
            }
        }

        /// <summary>
        /// 程序启动完成事件
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void App_StartupCompleted(object sender, EventArgs e)
        {
            InitEventListener();
        }

        /// <summary>
        /// 初始化事件监听
        /// </summary>
        private void InitEventListener()
        {
            WorkOrderEventListener.Instance.Start();
            InspEventListener.Instance.Start();
            PanelScrapListener.Instance.Start();
            WoInfoEventListener.Instance.Start();
        }
    }
}