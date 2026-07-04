using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.AbnormalInfo.AbnormalMonitors.Configs
{
    /// <summary>
    /// 预警定义-红牌管理配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("预警定义-红牌管理配置值")]
    public class WarnDefineForRedCardConfigValue : ConfigValue
    {
        #region 异常预警 AbnormalWarnDefine
        /// <summary>
        /// 异常预警Id
        /// </summary>
        [Label("异常预警")]
        [Required]
        public static readonly IRefIdProperty AbnormalWarnDefineIdProperty = P<WarnDefineForRedCardConfigValue>.RegisterRefId(e => e.AbnormalWarnDefineId, ReferenceType.Normal);

        /// <summary>
        /// 异常预警Id
        /// </summary>
        public double? AbnormalWarnDefineId
        {
            get { return (double?)GetRefNullableId(AbnormalWarnDefineIdProperty); }
            set { SetRefNullableId(AbnormalWarnDefineIdProperty, value); }
        }

        /// <summary>
        /// 异常预警
        /// </summary>
        public static readonly RefEntityProperty<AbnormalWarnDefine> AbnormalWarnDefineProperty = P<WarnDefineForRedCardConfigValue>.RegisterRef(e => e.AbnormalWarnDefine, AbnormalWarnDefineIdProperty);

        /// <summary>
        /// 异常预警
        /// </summary>
        public AbnormalWarnDefine AbnormalWarnDefine
        {
            get { return GetRefEntity(AbnormalWarnDefineProperty); }
            set { SetRefEntity(AbnormalWarnDefineProperty, value); }
        }
        #endregion

        /// <summary>
        /// 把值显示出来
        /// </summary>
        /// <returns>string</returns>
        public override string Display()
        {
            if (AbnormalWarnDefine == null)
                return "";
            return AbnormalWarnDefine?.Code;
        }
    }
}
