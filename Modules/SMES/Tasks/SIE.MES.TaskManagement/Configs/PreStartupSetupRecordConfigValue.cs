using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.TaskManagement.Configs
{
    /// <summary>
    /// 开机准备配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("开机准备配置值")]
    public class PreStartupSetupRecordConfigValue : ConfigValue 
    {
        //#region 是否校验开机准备配置 IsValidateStartupSetupPrepare
        ///// <summary>
        ///// 是否校验开机准备配置
        ///// </summary>
        //[Label("是否校验开机准备配置")]
        //public static readonly Property<bool> IsValidateStartupSetupPrepareProperty = P<PreStartupSetupRecordConfigValue>.Register(e => e.IsValidateStartupSetupPrepare);

        ///// <summary>
        ///// 是否校验开机准备配置
        ///// </summary>
        //public bool IsValidateStartupSetupPrepare
        //{
        //    get { return this.GetProperty(IsValidateStartupSetupPrepareProperty); }
        //    set { this.SetProperty(IsValidateStartupSetupPrepareProperty, value); }
        //}
        //#endregion

        #region 是否校验工装与产品的关系 IsValidFixtureItem
        /// <summary>
        /// 是否校验工装与产品的关系
        /// </summary>
        [Label("是否校验工装与产品的关系")]
        public static readonly Property<bool> IsValidFixtureItemProperty = P<PreStartupSetupRecordConfigValue>.Register(e => e.IsValidFixtureItem);

        /// <summary>
        /// 是否校验工装与产品的关系
        /// </summary>
        public bool IsValidFixtureItem
        {
            get { return this.GetProperty(IsValidFixtureItemProperty); }
            set { this.SetProperty(IsValidFixtureItemProperty, value); }
        }
        #endregion

        #region 是否校验检具与产品的关系 IsValidCheckerItem
        /// <summary>
        /// 是否校验检具与产品的关系
        /// </summary>
        [Label("是否校验检具与产品的关系")]
        public static readonly Property<bool> IsValidCheckerItemProperty = P<PreStartupSetupRecordConfigValue>.Register(e => e.IsValidCheckerItem);

        /// <summary>
        /// 是否校验检具与产品的关系
        /// </summary>
        public bool IsValidCheckerItem
        {
            get { return this.GetProperty(IsValidCheckerItemProperty); }
            set { this.SetProperty(IsValidCheckerItemProperty, value); }
        }
        #endregion

        #region 是否校验模具与产品的关系 IsValidEquipAccountItem
        /// <summary>
        /// 是否校验模具与产品的关系
        /// </summary>
        [Label("是否校验模具与产品的关系")]
        public static readonly Property<bool> IsValidEquipAccountItemProperty = P<PreStartupSetupRecordConfigValue>.Register(e => e.IsValidEquipAccountItem);

        /// <summary>
        /// 是否校验模具与产品的关系
        /// </summary>
        public bool IsValidEquipAccountItem
        {
            get { return this.GetProperty(IsValidEquipAccountItemProperty); }
            set { this.SetProperty(IsValidEquipAccountItemProperty, value); }
        }
        #endregion


        /// <summary>
        /// 显示
        /// </summary>
        /// <returns>编码规则名称|打印模板名称</returns>
        public override string Display()
        {
            //return "是否校验开机准备配置:{0} ".L10nFormat(IsValidateStartupSetupPrepare);
            return "是否校验工装与产品的关系:{0} 是否校验检具与产品的关系:{1} 是否校验模具与产品的关系:{2}".L10nFormat(IsValidFixtureItem, IsValidCheckerItem, IsValidEquipAccountItem);
        }
    }
}
