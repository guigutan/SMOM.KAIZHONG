using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.EquipRepair.EquipRepairs.Configs
{
    /// <summary>
    /// 配置
    /// </summary>
    [System.ComponentModel.DisplayName("是否交机确认")]
    [System.ComponentModel.Description("用于判断是否执行交机确认")]
    public class IsHandoverConfirmConfig : ModuleConfig<IsHandoverConfirmConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly IsHandoverConfirmConfigValue defaultValue = new IsHandoverConfirmConfigValue { IsHandoverConfirm = false };

        /// <summary>
        /// 默认值
        /// </summary>
        public override IsHandoverConfirmConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }

    /// <summary>
    /// 配置实体
    /// </summary>
    [RootEntity, Serializable]
    [Label("是否交机确认")]
    public class IsHandoverConfirmConfigValue : ConfigValue
    {
        #region 是否交机确认 IsHandoverConfirm
        /// <summary>
        /// 是否交机确认
        /// </summary>
        [Label("是否交机确认")]
        public static readonly Property<bool> IsHandoverConfirmProperty = P<IsHandoverConfirmConfigValue>.Register(e => e.IsHandoverConfirm);

        /// <summary>
        /// 是否交机确认
        /// </summary>
        public bool IsHandoverConfirm
        {
            get { return this.GetProperty(IsHandoverConfirmProperty); }
            set { this.SetProperty(IsHandoverConfirmProperty, value); }
        }
        #endregion


        /// <summary>
        ///  显示值
        /// </summary>
        /// <returns>string</returns>
        public override string Display()
        {
            return IsHandoverConfirm ? "是".L10N() : "否".L10N();
        }
    }
}
