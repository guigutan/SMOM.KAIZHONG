using SIE.Domain;
using SIE.Domain.Query;
using SIE.EventMessages.Inspection;
using SIE.EventMessages.MES.Inspection;
using SIE.EventMessages.MES.ProductStorage;
using SIE.EventMessages.MES.WIP;
using SIE.EventMessages.Receipt;
using SIE.MES.WorkOrders.Reworks;
using SIE.Modules;
using SIE.Packages.Packings;
using SIE.ProductIntfc;
using SIE.ProductIntfc.InspInterfaces;
using SIE.ProductIntfc.InspLogs;
using SIE.ProductIntfc.InspLogs.Reworks;
using SIE.ProductIntfc.InspRecords;
using SIE.ProductIntfc.ProductStorages;
using SIE.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[assembly: Module(typeof(Module))]

namespace SIE.ProductIntfc
{
    /// <summary>
    /// 模块
    /// </summary>
    /// <seealso cref="SIE.Modules.DomainModule" />
    public class Module : DomainModule
    {
        /// <summary>
        /// 模块的初始化方法。
        /// 框架会在启动时根据启动级别顺序调用本方法。
        /// 方法有两个职责：
        /// 1.依赖注入。
        /// 2.注册 app 生命周期中事件，进行特定的初始化工作。
        /// </summary>
        /// <param name="app">应用程序对象。</param>
        public override void Initialize(IApp app)
        {
            RegisterService();
            SubscribeEvent();
            app.Exit += App_Exit;

            RepositoryDataProvider.Querying += RepositoryDataProvider_Querying;
        }

        private void SubscribeEvent()
        {
            //订阅打包事件
            RT.EventBus.Subscribe<DoPackingEvent>(this, e =>
            {
                //以下代码执行在另一线程中。
                Task.Run(new Action(() =>
                {
                    RT.Service.Resolve<ProductStorageController>().AddPackingStore(e);
                }).WithCurrentThreadContext());
            });
            //订阅WMS单号回传事件
            //RT.EventBus.Subscribe<RemoteAsnNo>(this, e =>
            //{
            //    //以下代码执行在另一线程中。
            //    Task.Run(new Action(() =>
            //    {
            //  RT.Service.Resolve<ProductStorageController>().UpdateFromWMSAsn(e);

            //    }).WithCurrentThreadContext());
            //});

            //订阅WMS条码入库状态回传事件
            RT.EventBus.Subscribe<List<UpdateMesSnInfo>>(this, e =>
            {
                //以下代码执行在另一线程中。
                Task.Run(new Action(() =>
                {
                    RT.Service.Resolve<ProductStorageController>().UpdateSNStorageFromWMS(e);

                }).WithCurrentThreadContext());
            });
        }

        private static void RegisterService()
        {
            RT.Service.Register<IProductInsp, InspRecordController>();
            RT.Service.Register<IFirstInsp, InspLogController>();
            RT.Service.Register<IToStorageBarcode, IProductStorage, IDirectPackage, ProductStorageController>();
            RT.Service.Register<IReworkBarcode, InspReworkBarcode>();
            RT.Service.Register<IInspBarcode, InspBarcodeImpl>();
        }

        /// <summary>
        /// 已入库单默认按创建时间倒序
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void RepositoryDataProvider_Querying(object sender, QueryingEventArgs e)
        {
            var provider = sender as ProductIntfcEntityDataProvider;
            if (provider != null && provider.Repository.EntityType == (typeof(InStorageBill)))
            {
                var orderBy = e.Args.Query.OrderBy.FirstOrDefault(p => p.Column.ColumnName == "CREATE_DATE");
                if (orderBy != null)
                    e.Args.Query.OrderBy.Remove(orderBy);
                e.Args.Query.OrderBy.Add(QueryFactory.Instance.OrderBy(e.Args.Query.MainTable.FindColumn(InStorageBill.CreateDateProperty), System.ComponentModel.ListSortDirection.Descending));
            }
        }

        private void App_Exit(object sender, EventArgs e)
        {
            RT.EventBus.Unsubscribe<DoPackingEvent>(this);
            //RT.EventBus.Unsubscribe<RemoteAsnNo>(this);
        }
    }
}