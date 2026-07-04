using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SIE;
using SIE.Domain;
using SIE.Event.MQueue;
using SIE.Mes.Mq.Edge;
using SIE.MES.Edge.Models;

namespace SIE.Mes.Mq.Listener
{
    /// <summary>
    /// 边缘消息
    /// </summary>

    public class MesEdgeListener : IDisposable, IMesEdgeListener
    {
        /// <summary>
        /// 消息处理线程
        /// </summary>
        private const int MAX_THREADS = 3;

        private readonly CancellationTokenSource cancelToken = new CancellationTokenSource();
        /// <summary>
        /// 采集服务
        /// </summary>
        private readonly ICollectDataService collectService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="collectService"></param>
        public MesEdgeListener(ICollectDataService collectService)
        {
            this.collectService = collectService;
        }



        /// <summary>
        /// 启动
        /// </summary>
        public void Start()
        {
            //只在WebApiHost端运行
            if (!AppRuntime.IsOnServer() || RT.IsOnScheduleServer())
            {
                return;
            }
            RT.Logger.Info("边缘采集消息订阅启动。".L10N());
            try
            {
                Task.Factory.StartNew(async o =>
                {
                    await RunCollectAsync();
                }, 1);
            }
            catch (Exception ex)
            {
                RT.Logger.Error("边缘采集消息订阅启动异常。".L10N() + ex.Message, ex);
                throw;
            }
            RT.Logger.Info("边缘采集消息订阅启动完成,线程数[{0}]。".L10nFormat(MAX_THREADS));
        }



        /// <summary>
        /// 执行采集数据监听
        /// </summary>
        private async Task RunCollectAsync()
        {
            await RT.Service.Resolve<IMQueueEventBus>().SubscribeAsync<EdgeMessage>((em) =>
            {
                //创建消息接收日志
                EdgeErrorMessage receviceLog = new EdgeErrorMessage();
                receviceLog.MsgId = em.Body?.Id;
                receviceLog.Name = em.Body?.Name;
                receviceLog.Bodys = em.Body?.Body?.ToString();
                receviceLog.MsgInvOrg = em.Body?.InvOrg;

                //校验接收内容是否为空
                if (em.Body == null || receviceLog.Bodys.IsNullOrEmpty())
                {
                    string errMsg = "采集处理收到非法的消息 ,em.Body is NULL，传送的消息请按照EdgeMessage类结构进行序列化".L10N();
                    RT.Logger.Error(errMsg);
                    receviceLog.ErrorContent = errMsg;
                    receviceLog.IsError = YesNo.Yes;
                }

                //解析接收消息数据
                var data = receviceLog.IsError != YesNo.Yes ? JsonConvert.DeserializeObject<EdgeCollectData>(receviceLog.Bodys) : new EdgeCollectData();
                if (receviceLog.IsError != YesNo.Yes)
                {
                    try
                    {
                        //解析采集数据
                        receviceLog.Guid = data.Guid;
                        receviceLog.Barcode = data.Barcode;
                        receviceLog.WorkOrderId = double.Parse(data.WorkOrderId.IsNullOrEmpty() ? "0" : data.WorkOrderId);
                        receviceLog.ProcessId = double.Parse(data.ProcessId.IsNullOrEmpty() ? "0" : data.ProcessId);
                        receviceLog.IsError = YesNo.No;

                        //提交采集数据
                        collectService.CollectData(em.Body);
                    }
                    catch (Exception ex)
                    {
                        RT.Logger.Error("边缘采集数据异常：".L10N() + ex.Message);
                        receviceLog.ErrorContent = ex.Message;
                        receviceLog.IsError = YesNo.Yes;
                    }
                }

                //设置库存组织
                RT.InvOrg = int.Parse(em.Body == null ? "1" : (em.Body.InvOrg.IsNullOrEmpty() ? "1" : em.Body.InvOrg));

                //根据采集用户数据获取当前用户身份
                double employeeId = double.Parse(data.EmployeeId.IsNullOrEmpty() ? "1" : data.EmployeeId);
                double? userId = RF.GetById<Resources.Employees.Employee>(employeeId)?.UserId;
                if (RT.IdentityId != employeeId && userId != null)
                {
                    RT.Principal = new DataPortal.DataPortalPrincipal(employeeId, userId.Value, "");
                }

                //保存消息接收日志
                RF.Save(receviceLog);
                em.Ack = true;

            }, cancelToken, options: new SIE.Event.MQueue.SubscribeOptions() { AutoAck = false });
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
