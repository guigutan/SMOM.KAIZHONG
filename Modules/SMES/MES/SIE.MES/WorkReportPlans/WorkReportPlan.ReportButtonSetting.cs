using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.WorkReportPlans
{

    /// <summary>
    /// 报工按钮校验逻辑设置
    /// </summary>
    public partial class WorkReportPlan
    {
        #region 是否检查工单状态 IsReportCheckWOStatus
        /// <summary>
        /// 是否检查工单状态(报工按钮)
        /// </summary>
        [Label("校验工单状态")]
        public static readonly Property<bool> IsReportCheckWOStatusProperty = P<WorkReportPlan>.Register(e => e.IsReportCheckWOStatus);

        /// <summary>
        /// 是否检查工单状态(报工按钮)
        /// </summary>
        public bool IsReportCheckWOStatus
        {
            get { return this.GetProperty(IsReportCheckWOStatusProperty); }
            set { this.SetProperty(IsReportCheckWOStatusProperty, value); }
        }
        #endregion



        #region 校验员工技能 IsRepCheckEmpSkills
        /// <summary>
        /// 校验员工技能 (报工按钮)
        /// </summary>
        [Label("校验员工技能")]
        public static readonly Property<bool> IsRepCheckEmpSkillsProperty = P<WorkReportPlan>.Register(e => e.IsRepCheckEmpSkills);

        /// <summary>
        /// 校验员工技能(报工按钮)
        /// </summary>
        public bool IsRepCheckEmpSkills
        {
            get { return this.GetProperty(IsRepCheckEmpSkillsProperty); }
            set { this.SetProperty(IsRepCheckEmpSkillsProperty, value); }
        }
        #endregion

        
        #region 校验物料齐套 IsRepMaterialKitComp
        /// <summary>
        /// 校验物料齐套 (报工按钮)
        /// </summary>
        [Label("校验物料齐套")]
        public static readonly Property<bool> IsRepMaterialKitCompProperty = P<WorkReportPlan>.Register(e => e.IsRepMaterialKitComp);

        /// <summary>
        /// 校验物料齐套(报工按钮)
        /// </summary>
        public bool IsRepMaterialKitComp
        {
            get { return this.GetProperty(IsRepMaterialKitCompProperty); }
            set { this.SetProperty(IsRepMaterialKitCompProperty, value); }
        }
        #endregion

        #region 校验设备状态 IsReportEquipmentStatus
        /// <summary>
        /// 校验设备状态 (报工按钮)
        /// </summary>
        [Label("校验设备状态")]
        public static readonly Property<bool> IsReportEquipmentStatusProperty = P<WorkReportPlan>.Register(e => e.IsReportEquipmentStatus);

        /// <summary>
        /// 校验设备状态 (报工按钮)
        /// </summary>
        public bool IsReportEquipmentStatus
        {
            get { return this.GetProperty(IsReportEquipmentStatusProperty); }
            set { this.SetProperty(IsReportEquipmentStatusProperty, value); }
        }
        #endregion

        #region 校验摸具状态 IsReportMoldStatus
        /// <summary>
        /// 校验摸具状态 (报工按钮)
        /// </summary>
        [Label("校验摸具状态")]
        public static readonly Property<bool> IsReportMoldStatusProperty = P<WorkReportPlan>.Register(e => e.IsReportMoldStatus);

        /// <summary>
        /// 校验摸具状态(报工按钮)
        /// </summary>
        public bool IsReportMoldStatus
        {
            get { return this.GetProperty(IsReportMoldStatusProperty); }
            set { this.SetProperty(IsReportMoldStatusProperty, value); }
        }
        #endregion

        #region 报工数量允许大于前工序报工数量 IsReportQuantity
        /// <summary>
        /// 报工数量允许大于前工序报工数量 (报工按钮)
        /// </summary>
        [Label("报工数量允许大于前工序报工数量")]
        public static readonly Property<bool> IsReportQuantityProperty = P<WorkReportPlan>.Register(e => e.IsReportQuantity);

        /// <summary>
        /// 报工数量允许大于前工序报工数量(报工按钮)
        /// </summary>
        public bool IsReportQuantity
        {
            get { return this.GetProperty(IsReportQuantityProperty); }
            set { this.SetProperty(IsReportQuantityProperty, value); }
        }
        #endregion

        #region 是否启用报工确认 IsReportQuantity
        /// <summary>
        /// 是否启用报工确认
        /// </summary>
        [Label("是否启用报工确认")]
        public static readonly Property<bool> IsNeedCheckProperty = P<WorkReportPlan>.Register(e => e.IsNeedCheck);

        /// <summary>
        /// 是否启用报工确认
        /// </summary>
        public bool IsNeedCheck
        {
            get { return this.GetProperty(IsNeedCheckProperty); }
            set { this.SetProperty(IsNeedCheckProperty, value); }
        }
        #endregion


        

    }
}
