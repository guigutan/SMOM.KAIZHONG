using SIE.Common.Configs;
using SIE.Common.Sender;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Andon.Andons.Configs
{
    /// <summary>
    /// 安灯推送模块默认值
    /// </summary>
    [RootEntity, Serializable]
    [Label("推送模块默认值")]
    public class AndonPushPlugConfigValue : ConfigValue
    {
        #region 推送模板默认值 PushPugDefault
        /// <summary>
        /// 推送模板默认值Id
        /// </summary>
        [Label("推送模板默认值")]
        public static readonly IRefIdProperty PushPugDefaultIdProperty =
            P<AndonPushPlugConfigValue>.RegisterRefId(e => e.PushPugDefaultId, ReferenceType.Normal);

        /// <summary>
        /// 推送模板默认值Id
        /// </summary>
        public double PushPugDefaultId
        {
            get { return (double)this.GetRefId(PushPugDefaultIdProperty); }
            set { this.SetRefId(PushPugDefaultIdProperty, value); }
        }

        /// <summary>
        /// 推送模板默认值
        /// </summary>
        public static readonly RefEntityProperty<PushPlug> PushPugDefaultProperty =
            P<AndonPushPlugConfigValue>.RegisterRef(e => e.PushPugDefault, PushPugDefaultIdProperty);

        /// <summary>
        /// 推送模板默认值
        /// </summary>
        public PushPlug PushPugDefault
        {
            get { return this.GetRefEntity(PushPugDefaultProperty); }
            set { this.SetRefEntity(PushPugDefaultProperty, value); }
        }
        #endregion

        /// <summary>
        /// 默认值显示
        /// </summary>
        /// <returns></returns>
        public override string Display()
        {
            if (this.PushPugDefault == null)
            {
                return "NULL";
            }
            return PushPugDefault.Name;
        }
    }
}
