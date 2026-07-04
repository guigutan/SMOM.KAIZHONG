using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.SchedulingInfs.Configs
{
    /// <summary>
    /// 校验规则
    /// </summary>
    [RootEntity, Serializable]
    [Label("校验规则")]
    public class SchedulingInfCheckConfigValue: ConfigValue
    {
        #region 人员技能 PersonnelSkills
        /// <summary>
        /// 人员技能
        /// </summary>
        [Label("人员技能")]
        public static readonly Property<bool?> PersonnelSkillsProperty = P<SchedulingInfCheckConfigValue>.Register(e => e.PersonnelSkills);

        /// <summary>
        /// 人员技能
        /// </summary>
        public bool? PersonnelSkills
        {
            get { return this.GetProperty(PersonnelSkillsProperty); }
            set { this.SetProperty(PersonnelSkillsProperty, value); }
        }
        #endregion

        #region 产线状态 LineState
        /// <summary>
        /// 产线状态
        /// </summary>
        [Label("产线状态")]
        public static readonly Property<bool?> LineStateProperty = P<SchedulingInfCheckConfigValue>.Register(e => e.LineState);

        /// <summary>
        /// 产线状态
        /// </summary>
        public bool? LineState
        {
            get { return this.GetProperty(LineStateProperty); }
            set { this.SetProperty(LineStateProperty, value); }
        }
        #endregion

        #region 模具状态 MoldState
        /// <summary>
        /// 模具状态
        /// </summary>
        [Label("模具状态")]
        public static readonly Property<bool?> MoldStateProperty = P<SchedulingInfCheckConfigValue>.Register(e => e.MoldState);

        /// <summary>
        /// 模具状态
        /// </summary>
        public bool? MoldState
        {
            get { return this.GetProperty(MoldStateProperty); }
            set { this.SetProperty(MoldStateProperty, value); }
        }
        #endregion

        #region 工装状态 ToolingState
        /// <summary>
        /// 工装状态
        /// </summary>
        [Label("工装状态")]
        public static readonly Property<bool?> ToolingStateProperty = P<SchedulingInfCheckConfigValue>.Register(e => e.ToolingState);

        /// <summary>
        /// 工装状态
        /// </summary>
        public bool? ToolingState
        {
            get { return this.GetProperty(ToolingStateProperty); }
            set { this.SetProperty(ToolingStateProperty, value); }
        }
        #endregion

        #region 检具状态 InspEquipState
        /// <summary>
        /// 检具状态
        /// </summary>
        [Label("检具状态")]
        public static readonly Property<bool?> InspEquipStateProperty = P<SchedulingInfCheckConfigValue>.Register(e => e.InspEquipState);

        /// <summary>
        /// 检具状态
        /// </summary>
        public bool? InspEquipState
        {
            get { return this.GetProperty(InspEquipStateProperty); }
            set { this.SetProperty(InspEquipStateProperty, value); }
        }
        #endregion

        #region 物料齐备 ItemComplete
        /// <summary>
        /// 物料齐备
        /// </summary>
        [Label("物料齐备")]
        public static readonly Property<bool?> ItemCompleteProperty = P<SchedulingInfCheckConfigValue>.Register(e => e.ItemComplete);

        /// <summary>
        /// 物料齐备
        /// </summary>
        public bool? ItemComplete
        {
            get { return this.GetProperty(ItemCompleteProperty); }
            set { this.SetProperty(ItemCompleteProperty, value); }
        }
        #endregion

        #region 下发个数 DispatchNumber
        /// <summary>
        /// 下发个数
        /// </summary>
        [Label("下发个数")]
        public static readonly Property<int?> DispatchNumberProperty = P<SchedulingInfCheckConfigValue>.Register(e => e.DispatchNumber);

        /// <summary>
        /// 下发个数
        /// </summary>
        public int? DispatchNumber
        {
            get { return this.GetProperty(DispatchNumberProperty); }
            set { this.SetProperty(DispatchNumberProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示
        /// </summary>
        /// <returns></returns>
        public override string Display()
        {
            return "人员技能:{0};产线状态:{1};模具状态:{2};工装状态:{3};检具状态:{4};物料齐备:{5};下发个数:{6};".L10nFormat(this.PersonnelSkills == true ? "是" : "否", this.LineState == true ? "是" : "否", this.MoldState == true ? "是" : "否", this.ToolingState == true ? "是" : "否", this.InspEquipState == true ? "是" : "否", this.ItemComplete == true ? "是" : "否", this.DispatchNumber == null ? "null" : this.DispatchNumber.Value);
        }
    }
}
