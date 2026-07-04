using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Checks.Configs
{

    /// <summary>
    /// 点检确认部门配置
    /// </summary>
    [System.ComponentModel.DisplayName("点检确认部门配置")]
    [System.ComponentModel.Description("用于配置点检确认部门")]
    public class CheckConfirmDepartConfig : ModuleConfig<CheckConfirmDepartConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly CheckConfirmDepartConfigValue defaultValue = new CheckConfirmDepartConfigValue
        {
            DepartmentNames = String.Empty,
            DepartmentIds = String.Empty,
            IsMarkScore = false,
        };

        /// <summary>
        /// 默认值
        /// </summary>
        public override CheckConfirmDepartConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }

    /// <summary>
    /// 点检确认部门配置
    /// </summary>
    [RootEntity, Serializable]
    [Label("点检确认部门配置")]
    public class CheckConfirmDepartConfigValue : ConfigValue
    {
        #region 点检确认部门 DepartmentNames
        /// <summary>
        /// 点检确认部门 
        /// </summary>
        [Label("点检确认部门")]
        [MaxLength(1000)]
        public static readonly Property<string> DepartmentNamesProperty = P<CheckConfirmDepartConfigValue>.Register(e => e.DepartmentNames);

        /// <summary>
        /// 点检确认部门 
        /// </summary>
        public string DepartmentNames
        {
            get { return this.GetProperty(DepartmentNamesProperty); }
            set { this.SetProperty(DepartmentNamesProperty, value); }
        }
        #endregion

        #region 部门Id列表 DepartmentIds
        /// <summary>
        /// 部门Id列表
        /// </summary>
        [Label("部门Id列表")]
        [MaxLength(1000)]
        public static readonly Property<string> DepartmentIdsProperty = P<CheckConfirmDepartConfigValue>.Register(e => e.DepartmentIds);

        /// <summary>
        /// 部门Id列表
        /// </summary>
        public string DepartmentIds
        {
            get { return this.GetProperty(DepartmentIdsProperty); }
            set { this.SetProperty(DepartmentIdsProperty, value); }
        }
        #endregion

        #region 是否进行评分 IsMarkScore
        /// <summary>
        /// 是否进行评分
        /// </summary>
        [Label("是否进行评分")]
        public static readonly Property<bool> IsMarkScoreProperty = P<CheckConfirmDepartConfigValue>.Register(e => e.IsMarkScore);

        /// <summary>
        /// 是否进行评分
        /// </summary>
        public bool IsMarkScore
        {
            get { return this.GetProperty(IsMarkScoreProperty); }
            set { this.SetProperty(IsMarkScoreProperty, value); }
        }
        #endregion


        /// <summary>
        ///  显示值
        /// </summary>
        /// <returns>string</returns>
        public override string Display()
        {
            if (DepartmentIds.IsNullOrEmpty())
            {
                return "点检确认部门：NIL;".L10N();
            }
            else
            {
                return "点检确认部门：{0}".L10nFormat(DepartmentNames);
            }
        }
    }
}
