using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Maintains.Configs
{
    /// <summary>
    /// 保养确认部门配置
    /// </summary>
    [System.ComponentModel.DisplayName("保养确认部门配置")]
    [System.ComponentModel.Description("用于配置保养确认部门")]
    public class MaintainConfirmDepartConfig : ModuleConfig<MaintainConfirmDepartConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly MaintainConfirmDepartConfigValue defaultValue = new MaintainConfirmDepartConfigValue
        {
            DepartmentNames = String.Empty,
            DepartmentIds = String.Empty,
            IsMarkScore = false,
        };

        /// <summary>
        /// 默认值
        /// </summary>
        public override MaintainConfirmDepartConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }

    /// <summary>
    /// 保养确认部门配置
    /// </summary>
    [RootEntity, Serializable]
    [Label("保养确认部门配置")]
    public class MaintainConfirmDepartConfigValue : ConfigValue
    {
        #region 保养确认部门 DepartmentNames
        /// <summary>
        /// 保养确认部门 
        /// </summary>
        [Label("保养确认部门")]
        [MaxLength(1000)]
        public static readonly Property<string> DepartmentNamesProperty = P<MaintainConfirmDepartConfigValue>.Register(e => e.DepartmentNames);

        /// <summary>
        /// 保养确认部门 
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
        public static readonly Property<string> DepartmentIdsProperty = P<MaintainConfirmDepartConfigValue>.Register(e => e.DepartmentIds);

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
        public static readonly Property<bool> IsMarkScoreProperty = P<MaintainConfirmDepartConfigValue>.Register(e => e.IsMarkScore);

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
                return "保养确认部门：NIL;".L10N();
            }
            else
            {
                return "保养确认部门：{0}".L10nFormat(DepartmentNames);
            }
        }
    }
}
