using SIE.EventMessages.MES.Barcodes;
using SIE.EventMessages.MES.WorkOrders.Models;
using SIE.MES.Edge;
using SIE.Threading;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Events
{
    /// <summary>
    /// 工单信息变更监听器
    /// </summary>
    public class WoInfoEventListener
    {
        /// <summary>
        /// 实例
        /// </summary>
        public static WoInfoEventListener Instance { get; } = new WoInfoEventListener();

        /// <summary>
        /// 订阅事件总线
        /// </summary>
        public void Start()
        {
            RT.EventBus.Subscribe<WorkOrderInfo>(this, e =>
            {
                //以下代码执行在另一线程中。
                Task.Run(new Action(() =>
                {
                    RT.Service.Resolve<EdgeWipController>().CreateAndPublishWipWoInfo(e);
                }).WithCurrentThreadContext());
            });

            RT.EventBus.Subscribe<PrintBarcodeInfo>(this, e =>
            {
                //以下代码执行在另一线程中。
                Task.Run(new Action(() =>
                {
                    RT.Service.Resolve<EdgeWipController>().CreateAndPublishWipBarcodeInfo(e);
                }).WithCurrentThreadContext());
            });

            RT.EventBus.Subscribe<PackingBarcodeInfo>(this, e =>
            {
                //以下代码执行在另一线程中。
                Task.Run(new Action(() =>
                {
                    RT.Service.Resolve<EdgeWipController>().CreateAndPublishWipPackCodeInfo(e);
                }).WithCurrentThreadContext());
            });
        }
    }
}
