using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.EquipRepairs.Configs
{
    /// <summary>
    /// 配置
    /// </summary>
    [System.ComponentModel.DisplayName("是否工程评分确认")]
    [System.ComponentModel.Description("用于判断是否执行工程评分确认")]
    public class IsEngineerConfirmConfig : ModuleConfig<IsEngineerConfirmConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly IsEngineerConfirmConfigValue defaultValue = new IsEngineerConfirmConfigValue { IsEngineerConfirm = false };

        /// <summary>
        /// 默认值
        /// </summary>
        public override IsEngineerConfirmConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }

    /// <summary>
    /// 配置实体
    /// </summary>
    [RootEntity, Serializable]
    [Label("是否工程评分确认")]
    public class IsEngineerConfirmConfigValue : ConfigValue
    {
        #region 是否工程评分确认 IsEngineerConfirm
        /// <summary>
        /// 是否工程评分确认
        /// </summary>
        [Label("是否工程评分确认")]
        public static readonly Property<bool> IsEngineerConfirmProperty = P<IsEngineerConfirmConfigValue>.Register(e => e.IsEngineerConfirm);

        /// <summary>
        /// 是否工程评分确认
        /// </summary>
        public bool IsEngineerConfirm
        {
            get { return this.GetProperty(IsEngineerConfirmProperty); }
            set { this.SetProperty(IsEngineerConfirmProperty, value); }
        }
        #endregion


        /// <summary>
        ///  显示值
        /// </summary>
        /// <returns>string</returns>
        public override string Display()
        {
            return IsEngineerConfirm? "是".L10N() : "否".L10N();
        }
    }
}
