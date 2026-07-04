using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.WorkReportPlans
{
    /// <summary>
    /// 开工按钮校验逻辑设置
    /// </summary>
    public partial class WorkReportPlan
    {

        #region 是否检查工单状态 IsCheckWOStatus
        /// <summary>
        /// 是否检查工单状态 
        /// </summary>
        [Label("校验工单状态")]
        public static readonly Property<bool> IsCheckWOStatusProperty = P<WorkReportPlan>.Register(e => e.IsCheckWOStatus);

        /// <summary>
        /// 是否检查工单状态
        /// </summary>
        public bool IsCheckWOStatus
        {
            get { return this.GetProperty(IsCheckWOStatusProperty); }
            set { this.SetProperty(IsCheckWOStatusProperty, value); }
        }
        #endregion

        #region 校验员工技能 IsCheckWOStatus
        /// <summary>
        /// 校验员工技能 
        /// </summary>
        [Label("校验员工技能")]
        public static readonly Property<bool> IsCheckEmployeeSkillsProperty = P<WorkReportPlan>.Register(e => e.IsCheckEmployeeSkills);

        /// <summary>
        /// 校验员工技能
        /// </summary>
        public bool IsCheckEmployeeSkills
        {
            get { return this.GetProperty(IsCheckEmployeeSkillsProperty); }
            set { this.SetProperty(IsCheckEmployeeSkillsProperty, value); }
        }
        #endregion

        #region 校验物料齐套 IsMaterialKitCompleteness
        /// <summary>
        /// 校验物料齐套 
        /// </summary>
        [Label("校验物料齐套")]
        public static readonly Property<bool> IsMaterialKitCompletenessProperty = P<WorkReportPlan>.Register(e => e.IsMaterialKitCompleteness);

        /// <summary>
        /// 校验物料齐套
        /// </summary>
        public bool IsMaterialKitCompleteness
        {
            get { return this.GetProperty(IsMaterialKitCompletenessProperty); }
            set { this.SetProperty(IsMaterialKitCompletenessProperty, value); }
        }
        #endregion

        #region 校验设备点检 IsEquipmentSpotCheck
        /// <summary>
        /// 校验设备点检 
        /// </summary>
        [Label("校验设备点检")]
        public static readonly Property<bool> IsEquipmentSpotCheckProperty = P<WorkReportPlan>.Register(e => e.IsEquipmentSpotCheck);

        /// <summary>
        /// 校验设备点检
        /// </summary>
        public bool IsEquipmentSpotCheck
        {
            get { return this.GetProperty(IsEquipmentSpotCheckProperty); }
            set { this.SetProperty(IsEquipmentSpotCheckProperty, value); }
        }
        #endregion

        #region 校验摸具点检 IsMoldSpotCheck
        /// <summary>
        /// 校验摸具点检 
        /// </summary>
        [Label("校验摸具点检")]
        public static readonly Property<bool> IsMoldSpotCheckProperty = P<WorkReportPlan>.Register(e => e.IsMoldSpotCheck);

        /// <summary>
        /// 校验摸具点检
        /// </summary>
        public bool IsMoldSpotCheck
        {
            get { return this.GetProperty(IsMoldSpotCheckProperty); }
            set { this.SetProperty(IsMoldSpotCheckProperty, value); }
        }
        #endregion
    }
}
