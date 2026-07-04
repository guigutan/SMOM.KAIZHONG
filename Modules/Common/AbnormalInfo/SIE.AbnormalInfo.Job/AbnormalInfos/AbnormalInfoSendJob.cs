using SIE.AbnormalInfo.AbnormalInfos;
using SIE.AbnormalInfo.AbnormalInfos.Pushers;
using System;
using System.Collections.Generic;
using SIE.Common.Schdules;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Logging;
using SIE.Common.Sender;

namespace SIE.AbnormalInfo.Job.AbnormalInfos
{
    /// <summary>
    /// 异常信息自动推送调度
    /// </summary>
    [Job("异常信息自动推送调度", typeof(JobParameter))]
    public class AbnormalInfoSendJob : JobBase
    {
        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="param"></param>
        protected override void ExecuteJob(object param)
        {
            var notClosedSettingList = RT.Service.Resolve<AbnormalInfoController>().GetNotClosedAbnormalInfos();    //获取未完成且未推送的异常信息及推送升级设置
            if (notClosedSettingList != null && notClosedSettingList?.Count > 0)
            {
                var toSendList = new List<AbnormalInfoAndSetting>();
                EntityList<SendLog> sendLogList = new EntityList<SendLog>();
                DateTime currentTime = DateTime.Now;
                foreach (var setting in notClosedSettingList)
                {
                    var sendTime = CalSendTime(setting);
                    if (currentTime >= sendTime)
                    {
                        toSendList.Add(setting);
                        sendLogList.Add(new SendLog()
                        {
                            AbnormalInforId = setting.AbnormalInfoId,
                            SenderSettingId = setting.SettingId
                        });
                    }
                }
                //推送
                SendAbnormalInfos(toSendList, sendLogList);
            }
        }

        /// <summary>
        /// 推送异常信息
        /// </summary>
        /// <param name="toSendList"></param>
        /// <param name="sendLogList"></param>
        private void SendAbnormalInfos(List<AbnormalInfoAndSetting> toSendList, EntityList<SendLog> sendLogList)
        {
            if (toSendList?.Count > 0)
            {
                foreach (var toSend in toSendList)
                {
                    //推送邮件
                    var pusher = toSend.Setting.Pusher;
                 
                    if (pusher != null)
                    {
                        try
                        {
                            AbnormalInfoAlertResult result = CreateAlertResult(toSend.AbnormalInfo);
                            RT.Service.Resolve<PushPlugController>().Send(pusher, result);
                        }
                        catch (Exception ex)
                        {
                            LogManager.Logger.Error("异常信息自动推送邮件失败。".L10N() + ex.GetExceptionMessage(), ex);
                        }
                    }
                }
            }
            RF.Save(sendLogList);//保存推送记录
        }

        /// <summary>
        /// 生成异常信息推送信息
        /// </summary>
        /// <param name="abnormal"></param>
        /// <returns></returns>
        protected AbnormalInfoAlertResult CreateAlertResult(AbnormalInfor abnormal)
        {
            var result = new AbnormalInfoAlertResult() { AlertInfoList = new List<AbnormalInfoPusher>() };
            result.AlertInfoList.Add(new AbnormalInfoPusher()
            {
                Name = "异常信息单号".L10N(),
                Value = abnormal.No
            });
            result.AlertInfoList.Add(new AbnormalInfoPusher()
            {
                Name = "异常信息描述".L10N(),
                Value = abnormal.AbnormalInfoDefinition.Desc
            });
            result.AlertInfoList.Add(new AbnormalInfoPusher()
            {
                Name = "异常信息分类".L10N(),
                Value = abnormal.AbnormalInfoDefinition.AbnormalCategory.Desc
            });
            result.AlertInfoList.Add(new AbnormalInfoPusher()
            {
                Name = "异常任务发生时间".L10N(),
                Value = abnormal.CreateDate.ToString("yyyy年MM月dd日 HH:mm")
            });
            result.AlertInfoList.Add(new AbnormalInfoPusher()
            {
                Name = "异常任务状态".L10N(),
                Value = abnormal.AbnormalStatus.ToLabel()
            });
            return result;
        }


        /// <summary>
        /// 计算发送时间
        /// </summary>
        /// <param name="abnormalSetting"></param>
        /// <returns></returns>
        private DateTime CalSendTime(AbnormalInfoAndSetting abnormalSetting)
        {
            var createTime = abnormalSetting.AbnormalInfo.CreateDate;
            DateTime sendTime = createTime;
            var setting = abnormalSetting.Setting;
            switch (setting.UnitType)
            {
                case UnitType.Days:
                    sendTime = sendTime.AddDays(setting.TimeType);
                    break;
                case UnitType.Hours:
                    sendTime = sendTime.AddHours(setting.TimeType);
                    break;
                case UnitType.Minute:
                    sendTime = sendTime.AddMinutes(setting.TimeType);
                    break;
                default:
                    throw new ValidationException("推送升级设置的单位类型不正确。".L10N());
            }
            return sendTime;
        }
    }
}
