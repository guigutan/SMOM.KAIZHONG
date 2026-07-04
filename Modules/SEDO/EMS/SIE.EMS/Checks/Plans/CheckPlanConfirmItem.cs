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

namespace SIE.EMS.Checks.Plans
{
    /// <summary>
    /// 点检确认部门项目
    /// </summary>
    [RootEntity, Serializable]
    [Label("点检确认部门项目")]
    public class CheckPlanConfirmItem : DataEntity
    {
        #region 点检计划 CheckPlan
        /// <summary>
        /// 点检计划Id
        /// </summary>
        [Label("点检计划")]
        public static readonly IRefIdProperty CheckPlanIdProperty =
            P<CheckPlanConfirmItem>.RegisterRefId(e => e.CheckPlanId, ReferenceType.Normal);

        /// <summary>
        /// 点检计划Id
        /// </summary>
        public double CheckPlanId
        {
            get { return (double)this.GetRefId(CheckPlanIdProperty); }
            set { this.SetRefId(CheckPlanIdProperty, value); }
        }

        /// <summary>
        /// 点检计划
        /// </summary>
        public static readonly RefEntityProperty<CheckPlan> CheckPlanProperty =
            P<CheckPlanConfirmItem>.RegisterRef(e => e.CheckPlan, CheckPlanIdProperty);

        /// <summary>
        /// 点检计划
        /// </summary>
        public CheckPlan CheckPlan
        {
            get { return this.GetRefEntity(CheckPlanProperty); }
            set { this.SetRefEntity(CheckPlanProperty, value); }
        }
        #endregion

        #region 部门 Department
        /// <summary>
        /// 部门Id
        /// </summary>
        [Label("部门")]
        public static readonly IRefIdProperty DepartmentIdProperty =
            P<CheckPlanConfirmItem>.RegisterRefId(e => e.DepartmentId, ReferenceType.Normal);

        /// <summary>
        /// 部门Id
        /// </summary>
        public double DepartmentId
        {
            get { return (double)this.GetRefId(DepartmentIdProperty); }
            set { this.SetRefId(DepartmentIdProperty, value); }
        }

        /// <summary>
        /// 部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> DepartmentProperty =
            P<CheckPlanConfirmItem>.RegisterRef(e => e.Department, DepartmentIdProperty);

        /// <summary>
        /// 部门
        /// </summary>
        public Enterprise Department
        {
            get { return this.GetRefEntity(DepartmentProperty); }
            set { this.SetRefEntity(DepartmentProperty, value); }
        }
        #endregion

        #region 状态 CheckExeState
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<CheckExeState> CheckExeStateProperty = P<CheckPlanConfirmItem>.Register(e => e.CheckExeState);

        /// <summary>
        /// 状态
        /// </summary>
        public CheckExeState CheckExeState
        {
            get { return this.GetProperty(CheckExeStateProperty); }
            set { this.SetProperty(CheckExeStateProperty, value); }
        }
        #endregion

        #region 确认结果 ConfirmResult
        /// <summary>
        /// 确认结果
        /// </summary>
        [Label("确认结果")]
        public static readonly Property<ConfirmResult?> ConfirmResultProperty = P<CheckPlanConfirmItem>.Register(e => e.ConfirmResult);

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
        public static readonly Property<string> ConfirmNoteProperty = P<CheckPlanConfirmItem>.Register(e => e.ConfirmNote);

        /// <summary>
        /// 确认备注
        /// </summary>
        public string ConfirmNote
        {
            get { return this.GetProperty(ConfirmNoteProperty); }
            set { this.SetProperty(ConfirmNoteProperty, value); }
        }
        #endregion

        #region 点检单号 CheckPlanNo
        /// <summary>
        /// 点检单号
        /// </summary>
        [Label("点检单号")]
        public static readonly Property<string> CheckPlanNoProperty = P<CheckPlanConfirmItem>.RegisterView(e => e.CheckPlanNo, p => p.CheckPlan.CheckPlanNo);

        /// <summary>
        /// 点检单号
        /// </summary>
        public string CheckPlanNo
        {
            get { return this.GetProperty(CheckPlanNoProperty); }
        }
        #endregion

        #region 部门编码 DeptCode
        /// <summary>
        /// 部门编码
        /// </summary>
        [Label("部门编码")]
        public static readonly Property<string> DeptCodeProperty = P<CheckPlanConfirmItem>.RegisterView(e => e.DeptCode, p => p.Department.Code);

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
        public static readonly Property<string> DeptNameProperty = P<CheckPlanConfirmItem>.RegisterView(e => e.DeptName, p => p.Department.Name);

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
    /// 数据配置
    /// </summary>
    public class CheckPlanConfirmItemConfig : EntityConfig<CheckPlanConfirmItem>
    {
        /// <summary>
        /// 数据库
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_CHECK_PLAN_ITEM").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
