using SIE.Common.Sender;
using SIE.Core.ApiModels;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.QTimes.Enums;
using SIE.MES.QTimes.Services;
using SIE.MES.QTimes.ViewModels;
using SIE.Resources.Enterprises;
using SIE.Tech.Processs;
using SIE.Web.Data;
using SIE.Web.Json;
using System;
using System.IO;

namespace SIE.Web.MES.QTimes.DataQueryers
{
    /// <summary>
    /// QT前端数据请求
    /// </summary>
    public class QTDataQueryer : DataQueryer
    {
        /// <summary>
        /// 带出产线
        /// </summary>
        /// <param name="wipId"></param>
        /// <returns></returns>
        public BaseDataInfo GetFactoryByWipId(double wipId)
        {
            BaseDataInfo baseDataInfo = new BaseDataInfo();
            var factory = RT.Service.Resolve<EnterpriseController>().GetFactoryByWipId(wipId);
            if (factory != null)
            {
                baseDataInfo.Id = factory.Id;
                baseDataInfo.Code = factory.Code;
                baseDataInfo.Name = factory.Name;
            }
            return baseDataInfo;
        }

        /// <summary>
        /// 获取工序当前是否启用入站控制
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        public bool ProcessIsMoveIn(double processId)
        {
            return RT.Service.Resolve<ProcessController>().ProcessIsMoveIn(processId) ?? false;
        }

        /// <summary>
        /// 根据推送类型获取数据
        /// </summary>
        /// <param name="qTPushType"></param>
        /// <param name="keyword"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public EntityList<QTPushObjectViewModel> GetPushTypeDatas(QTPushType qTPushType, string keyword, PagingInfo pagingInfo)
        {
            return RT.Service.Resolve<QTimeStandardService>().GetPushTypeDatas(qTPushType, keyword, pagingInfo);
        }

        /// <summary>
        /// 获取信息模板
        /// </summary>
        /// <param name="pushPlugId"></param>
        /// <returns></returns>
        public EntityJson GetMessageTemplate(double pushPlugId)
        {
            var pushPlug = RF.GetById<PushPlug>(pushPlugId);
            var sender = RT.Service.Resolve<PushPlugController>().GetSender(pushPlug) as SenderBase;
            var messageTemplate = sender.CreateMessageTemplate();

            EntityJson templateJson = new EntityJson();
            templateJson.SetProperty("TemplateModel", messageTemplate.GetType().ToString());
            templateJson.SetProperty("TemplateJson", messageTemplate.ToString());
            return templateJson;
        }

        /// <summary>
        /// 获取Rezor语法模板
        /// </summary>
        /// <returns></returns>
        public string GetRazorTemplate()
        {
            string filePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "QTimes\\Templates\\QTRazorTemplate.txt");
            if (!File.Exists(filePath))
            {
                throw new ValidationException("提示：没有在路径“{0}”查找到QT超时报表推送消息模板说明".L10nFormat(filePath));
            }
            var temp = File.ReadAllText(filePath);
            return temp;
        }
    }
}
