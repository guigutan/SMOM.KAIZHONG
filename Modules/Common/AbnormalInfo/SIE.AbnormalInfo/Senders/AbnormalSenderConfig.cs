using Newtonsoft.Json;
using SIE.Common.Sender;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.AbnormalInfo.Senders
{
    /// <summary>
    /// 异常管理推送配置类
    /// </summary>
    [RootEntity, Serializable]
    [DisplayMember(nameof(AbnormalSenderConfig.Title))]
    public class AbnormalSenderConfig : SenderConfg
    {
        #region 标题 Title
        /// <summary>
        /// 标题
        /// </summary>
        [Label("标题")]
        public static readonly Property<string> TitleProperty = P<AbnormalSenderConfig>.Register(e => e.Title);
        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return this.GetProperty(TitleProperty); }
            set { this.SetProperty(TitleProperty, value); }
        }
        #endregion

        /// <summary>
        /// 重写方法
        /// </summary>
        /// <param name="value"></param>
        public override void Initialize(string value)
        {
            if (value.IsNullOrWhiteSpace())
            {
                return;
            }
            var config = JsonConvert.DeserializeObject<AbnormalSenderConfig>(value);
            this.Title = config.Title;
        }

        /// <summary>
        /// 返回JSON字符串
        /// </summary>
        /// <returns>JSON字符串</returns>
        public override string ToString()
        {
            var temp = new
            {
                Title = this.Title,
            };
            return JsonConvert.SerializeObject(temp);
        }
    }
}
