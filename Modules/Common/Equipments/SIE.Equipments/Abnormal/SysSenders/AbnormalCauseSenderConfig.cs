using Newtonsoft.Json;
using SIE.Common.Sender;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Equipments.Abnormal.SysSenders
{
    /// <summary>
    /// 停线管理推送配置类
    /// </summary>
    [RootEntity, Serializable]
    [DisplayMember(nameof(Title))]
    public class AbnormalCauseSenderConfig : SenderConfg
    {
        #region 标题 Title
        /// <summary>
        /// 标题
        /// </summary>
        [Label("标题")]
        public static readonly Property<string> TitleProperty = P<AbnormalCauseSenderConfig>.Register(e => e.Title);
        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return GetProperty(TitleProperty); }
            set { SetProperty(TitleProperty, value); }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public override void Initialize(string value)
        {
            if (value.IsNullOrWhiteSpace())
                return;
            var config = JsonConvert.DeserializeObject<AbnormalCauseSenderConfig>(value);
            Title = config.Title;
        }

        /// <summary>
        /// 返回JSON字符串
        /// </summary>
        /// <returns>JSON字符串</returns>
        public override string ToString()
        {
            var temp = new
            {
                Title,
            };
            return JsonConvert.SerializeObject(temp);
        }
    }
}
