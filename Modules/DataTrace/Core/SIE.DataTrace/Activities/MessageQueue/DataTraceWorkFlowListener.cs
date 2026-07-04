using SIE.DataTrace.DataSync.Core;
using SIE.MQueue;
using System;
using System.Threading;

namespace SIE.DataTrace.Activities.MessageQueue
{
    /// <summary>
    /// 数据追溯工作流程监听器
    /// </summary>
    public class DataTraceWorkFlowListener : IDisposable
    {
        /// <summary>
        /// 实例
        /// </summary>
        public static DataTraceWorkFlowListener Instance { get; set; } = new DataTraceWorkFlowListener();

        private readonly CancellationTokenSource cancelToken = new CancellationTokenSource();

        /// <summary>
        /// 订阅追溯流程接收信息
        /// </summary>
        public void Start()
        {
            try
            {
                if (AppRuntime.IsOnServer() && !RT.IsOnScheduleServer())
                {
                    AppRuntime.Logger.Info("启动追溯流程监听。".L10N());
                    //根据节点来触发流程
                    AppRuntime.MQueueEventBus.SubscribeAsync<DataTraceWorkFlowQueueInfo>(p => TriggerWorkFlowByActivity(p), cancelToken, new Event.MQueue.SubscribeOptions() { AutoAck = false });
                    //根据实体类型来触发流程
                    AppRuntime.MQueueEventBus.SubscribeAsync<DataTraceWorkFlowQueueEntityInfo>(p => TriggerWorkFlowByEntity(p), cancelToken, new Event.MQueue.SubscribeOptions() { AutoAck = false });
                }
            }
            catch (Exception ex)
            {
                AppRuntime.Logger.Error("启动追溯流程监听失败。".L10N(), ex);
            }
        }

        /// <summary>
        /// 触发追溯流程节点流转-根据节点ID
        /// </summary>
        /// <param name="args"></param>
        private void TriggerWorkFlowByActivity(SubscribeEventArgs<DataTraceWorkFlowQueueInfo> args)
        {
            AppRuntime.Logger.Info("接收到追溯流程触发请求。".L10N());
            if (args?.Body == null)
            {
                if (args != null)
                    args.Ack = true;
                return;
            }
            try
            {
                var queueInfo = args.Body;
                AppRuntime.InvOrg = queueInfo.InvOrg;   //指定库存组织
                DataSyncParam para = new DataSyncParam()
                {
                    FlowInstanceId = queueInfo.FlowInstanceId,
                    TraceMainDataId = queueInfo.TraceMainDataId,
                    ActivityId = queueInfo.ActivityId
                };
                AppRuntime.Service.Resolve<DataTraceWorkFlowController>().TriggerDataTraceActivity(para);
            }
            catch (Exception ex)
            {
                AppRuntime.Logger.Error($"追溯流程触发处理出现异常。{ex.Message}。流程进度Id：{args?.Body.FlowInstanceId}。流程节点Id：{args?.Body.ActivityId}。 ".L10N());
            }
            args.Ack = true;
        }

        /// <summary>
        /// 触发追溯流程节点流转-根据实体类型
        /// </summary>
        /// <param name="args"></param>
        private void TriggerWorkFlowByEntity(SubscribeEventArgs<DataTraceWorkFlowQueueEntityInfo> args)
        {
            AppRuntime.Logger.Info("接收到追溯流程（根据实体）触发请求。".L10N());
            if (args?.Body == null)
            {
                if (args != null)
                    args.Ack = true;
                return;
            }
            try
            {
                var queueInfo = args.Body;
                AppRuntime.InvOrg = queueInfo.InvOrg;   //指定库存组织

                AppRuntime.Service.Resolve<DataTraceWorkFlowController>().TriggerDataTraceActivityByEntity(queueInfo.FlowInstanceId, queueInfo.TraceMainDataId, queueInfo.EntityType);
            }
            catch (Exception ex)
            {
                AppRuntime.Logger.Error($"追溯流程触发处理出现异常。{ex.Message}。流程进度Id：{args?.Body.FlowInstanceId}。流程实体类型：{args?.Body.EntityType}。 ".L10N());
            }
            args.Ack = true;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放数据
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                cancelToken.Cancel();
            }
        }
    }
}
