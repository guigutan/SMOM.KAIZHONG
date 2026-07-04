using SIE.Common.Sender;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Security;
using SIE.Web.Data;
using SIE.Web.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SIE.Web.Andon.Andons.DataQuery
{
    /// <summary>
    /// 安灯类型获取消息推送模板
    /// </summary>
    [AllowAnonymous]
    public class AndonTypeMessageTemplateDataQuery : DataQueryer
    {
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
            string filePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "AndonRazorTemplate.txt");
            if (!File.Exists(filePath))
            {
                throw new ValidationException("提示：没有在路径“{0}”查找到安灯推送消息模板说明".L10nFormat(filePath));
            }
            var temp = File.ReadAllText(filePath);
            return temp;
        }
    }
}
