using SIE.Domain;
using SIE.EMS.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EMS.Maintains.Plans
{
    /// <summary>
    /// 保养确认项
    /// </summary>
    [RootEntity, Serializable]
    [Label("保养确认项")]
    public class MaintainPlanConfirmItem : DataEntity
    {
        #region 保养计划 MaintainPlan
        /// <summary>
        /// 保养计划Id
        /// </summary>
        [Label("保养计划")]
        public static readonly IRefIdProperty MaintainPlanIdProperty =
            P<MaintainPlanConfirmItem>.RegisterRefId(e => e.MaintainPlanId, ReferenceType.Normal);

        /// <summary>
        /// 保养计划Id
        /// </summary>
        public double MaintainPlanId
        {
            get { return (double)this.GetRefId(MaintainPlanIdProperty); }
            set { this.SetRefId(MaintainPlanIdProperty, value); }
        }

        /// <summary>
        /// 保养计划
        /// </summary>
        public static readonly RefEntityProperty<MaintainPlan> MaintainPlanProperty =
            P<MaintainPlanConfirmItem>.RegisterRef(e => e.MaintainPlan, MaintainPlanIdProperty);

        /// <summary>
        /// 保养计划
        /// </summary>
        public MaintainPlan MaintainPlan
        {
            get { return this.GetRefEntity(MaintainPlanProperty); }
            set { this.SetRefEntity(MaintainPlanProperty, value); }
        }
        #endregion

        #region 确认部门 Department
        /// <summary>
        /// 确认部门Id
        /// </summary>
        [Label("确认部门")]
        public static readonly IRefIdProperty DepartmentIdProperty =
            P<MaintainPlanConfirmItem>.RegisterRefId(e => e.DepartmentId, ReferenceType.Normal);

        /// <summary>
        /// 确认部门Id
        /// </summary>
        public double DepartmentId
        {
            get { return (double)this.GetRefId(DepartmentIdProperty); }
            set { this.SetRefId(DepartmentIdProperty, value); }
        }

        /// <summary>
        /// 确认部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> DepartmentProperty =
            P<MaintainPlanConfirmItem>.RegisterRef(e => e.Department, DepartmentIdProperty);

        /// <summary>
        /// 确认部门
        /// </summary>
        public Enterprise Department
        {
            get { return this.GetRefEntity(DepartmentProperty); }
            set { this.SetRefEntity(DepartmentProperty, value); }
        }
        #endregion

        #region 保养状态 MaintExeState
        /// <summary>
        /// 保养状态
        /// </summary>
        [Label("保养状态")]
        public static readonly Property<MaintExeState> MaintExeStateProperty = P<MaintainPlanConfirmItem>.Register(e => e.MaintExeState);

        /// <summary>
        /// 保养状态
        /// </summary>
        public MaintExeState MaintExeState
        {
            get { return this.GetProperty(MaintExeStateProperty); }
            set { this.SetProperty(MaintExeStateProperty, value); }
        }
        #endregion

        #region 确认结果 ConfirmResult
        /// <summary>
        /// 确认结果
        /// </summary>
        [Label("确认结果")]
        public static readonly Property<ConfirmResult?> ConfirmResultProperty = P<MaintainPlanConfirmItem>.Register(e => e.ConfirmResult);

        /// <summary>
        /// 确认结果
        /// </summary>
        public ConfirmResult? ConfirmResult
        {
            get { return this.GetProperty(ConfirmResultProperty); }
            set { this.SetProperty(ConfirmResultProperty, value); }
        }
        #endregion

        #region 确认备注 ConfirmNote
        /// <summary>
        /// 确认备注
        /// </summary>
        [Label("确认备注")]
        [MaxLength(1000)]
        public static readonly Property<string> ConfirmNoteProperty = P<MaintainPlanConfirmItem>.Register(e => e.ConfirmNote);

        /// <summary>
        /// 确认备注
        /// </summary>
        public string ConfirmNote
        {
            get { return this.GetProperty(ConfirmNoteProperty); }
            set { this.SetProperty(ConfirmNoteProperty, value); }
        }
        #endregion

        #region 保养计划号 MaintainPlanNo
        /// <summary>
        /// 保养计划号
        /// </summary>
        [Label("保养计划号")]
        public static readonly Property<string> MaintainPlanNoProperty = P<MaintainPlanConfirmItem>.RegisterView(e => e.MaintainPlanNo, p => p.MaintainPlan.MaintainNo);

        /// <summary>
        /// 保养计划号
        /// </summary>
        public string MaintainPlanNo
        {
            get { return this.GetProperty(MaintainPlanNoProperty); }
        }
        #endregion

        #region 部门编码 DeptCode
        /// <summary>
        /// 部门编码
        /// </summary>
        [Label("部门编码")]
        public static readonly Property<string> DeptCodeProperty = P<MaintainPlanConfirmItem>.RegisterView(e => e.DeptCode, p => p.Department.Code);

        /// <summary>
        /// 部门编码
        /// </summary>
        public string DeptCode
        {
            get { return this.GetProperty(DeptCodeProperty); }
        }
        #endregion

        #region 部门名称 DeptName
        /// <summary>
        /// 部门名称
        /// </summary>
        [Label("部门名称")]
        public static readonly Property<string> DeptNameProperty = P<MaintainPlanConfirmItem>.RegisterView(e => e.DeptName, p => p.Department.Name);

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DeptName
        {
            get { return this.GetProperty(DeptNameProperty); }
        }
        #endregion

    }

    /// <summary>
    /// 数据源配置
    /// </summary>
    public class MaintainPlanConfirmItemConfig : EntityConfig<MaintainPlanConfirmItem>
    {
        /// <summary>
        /// 数据库
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_MAINTAIN_PLAN_ITEM").MapAllProperties();
            Meta.Property(MaintainPlanConfirmItem.ConfirmNoteProperty).ColumnMeta.HasLength(2000);
            Meta.EnablePhantoms();
        }
    }
}
