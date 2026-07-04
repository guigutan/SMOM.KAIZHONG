using SIE.MES.WIP;
using SIE.Threading;
using System;
using System.Threading.Tasks;

namespace SIE.MES.Events
{
    /// <summary>
    /// 成品检验报检监听器
    /// </summary>
    public class InspEventListener
    {
        /// <summary>
        /// 实例
        /// </summary>
        public static InspEventListener Instance { get; } = new InspEventListener();

        /// <summary>
        /// 订阅事件总线
        /// </summary>
        public void Start()
        {
            RT.EventBus.Subscribe<WipCollectedEvent>(this, e =>
             {
                 //以下代码执行在另一线程中。
                 Task.Run(new Action(() =>
                 {
                     RT.Service.Resolve<InspController>().CreateInspRecords(e);
                     RT.Service.Resolve<InspController>().CreateFirstInsps(e);
                 }).WithCurrentThreadContext());
             });

            RT.EventBus.Subscribe<WipFinishedEvent>(this, e =>
            {
                //以下代码执行在另一线程中。
                Task.Run(new Action(() =>
                {
                    RT.Service.Resolve<InspController>().CreateStorageBarcode(e);
                }).WithCurrentThreadContext());
            });
        }
    }
}