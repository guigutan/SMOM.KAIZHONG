using SIE.Domain;
using SIE.EMS.Enums;
using SIE.ObjectModel;
using System;

namespace SIE.Web.EMS.EquipMaint.Maintains.Plans.ViewModels
{
    /// <summary>
    /// 选择部门VM
    /// </summary>
    [RootEntity, Serializable]
    public class SelDepartmentViewModel : ViewModel
    {
        #region 保养执行ID MaintainPlanId
        /// <summary>
        /// 保养执行ID
        /// </summary>
        [Label("保养执行ID")]
        public static readonly Property<double> MaintainPlanIdProperty = P<SelDepartmentViewModel>.Register(e => e.MaintainPlanId);

        /// <summary>
        /// 保养执行ID
        /// </summary>
        public double MaintainPlanId
        {
            get { return this.GetProperty(MaintainPlanIdProperty); }
            set { this.SetProperty(MaintainPlanIdProperty, value); }
        }
        #endregion

        #region 保养单号 MaintainPlanNo
        /// <summary>
        /// 保养单号
        /// </summary>
        [Label("保养单号")]
        public static readonly Property<string> MaintainPlanNoProperty = P<SelDepartmentViewModel>.Register(e => e.MaintainPlanNo);

        /// <summary>
        /// 保养单号
        /// </summary>
        public string MaintainPlanNo
        {
            get { return this.GetProperty(MaintainPlanNoProperty); }
            set { this.SetProperty(MaintainPlanNoProperty, value); }
        }
        #endregion

        #region 部门Id DepartmentId
        /// <summary>
        /// 部门Id
        /// </summary>
        [Label("部门Id")]
        public static readonly Property<double?> DepartmentIdProperty = P<SelDepartmentViewModel>.Register(e => e.DepartmentId);

        /// <summary>
        /// 部门Id
        /// </summary>
        public double? DepartmentId
        {
            get { return this.GetProperty(DepartmentIdProperty); }
            set { this.SetProperty(DepartmentIdProperty, value); }
        }
        #endregion

        #region 部门 DepartmentName
        /// <summary>
        /// 部门
        /// </summary>
        [Label("部门")]
        public static readonly Property<string> DepartmentNameProperty = P<SelDepartmentViewModel>.Register(e => e.DepartmentName);

        /// <summary>
        /// 部门
        /// </summary>
        public string DepartmentName
        {
            get { return this.GetProperty(DepartmentNameProperty); }
            set { this.SetProperty(DepartmentNameProperty, value); }
        }
        #endregion

        #region 设备台账ID EquipAccountId
        /// <summary>
        /// 设备台账ID
        /// </summary>
        [Label("设备台账ID")]
        public static readonly Property<double?> EquipAccountIdProperty = P<SelDepartmentViewModel>.Register(e => e.EquipAccountId);

        /// <summary>
        /// 设备台账ID
        /// </summary>
        public double? EquipAccountId
        {
            get { return this.GetProperty(EquipAccountIdProperty); }
            set { this.SetProperty(EquipAccountIdProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<MaintExeState> StateProperty = P<SelDepartmentViewModel>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public MaintExeState State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        #region 是否打开确认界面 IfOpenConfirmationTab
        /// <summary>
        /// 是否打开确认界面
        /// </summary>
        [Label("是否打开确认界面")]
        public static readonly Property<bool> IfOpenConfirmationTabProperty = P<SelDepartmentViewModel>.Register(e => e.IfOpenConfirmationTab);

        /// <summary>
        /// 是否打开确认界面
        /// </summary>
        public bool IfOpenConfirmationTab
        {
            get { return this.GetProperty(IfOpenConfirmationTabProperty); }
            set { this.SetProperty(IfOpenConfirmationTabProperty, value); }
        }
        #endregion

        #region 是否保养确认人 IsConfirm
        /// <summary>
        /// 是否保养确认人
        /// </summary>
        [Label("是否保养确认人")]
        public static readonly Property<bool> IsConfirmProperty = P<SelDepartmentViewModel>.Register(e => e.IsConfirm);

        /// <summary>
        /// 是否保养确认人
        /// </summary>
        public bool IsConfirm
        {
            get { return this.GetProperty(IsConfirmProperty); }
            set { this.SetProperty(IsConfirmProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 选择部门VM 视图
    /// </summary>
    public class SelDepartmentViewModelViewConfig : WebViewConfig<SelDepartmentViewModel>
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigView()
        {
            View.DomainName("选择部门");
            View.WithoutPaging();
            using (View.OrderProperties())
            {
                View.Property(p => p.MaintainPlanNo).Show().ShowInList(width: 200).Readonly();
                View.Property(p => p.DepartmentId).Show(ShowInWhere.Hide).Readonly();
                View.Property(p => p.DepartmentName).Show().Readonly();
                View.Property(p => p.State).Show().Readonly();
                View.Property(p => p.IfOpenConfirmationTab).Show(ShowInWhere.Hide).Readonly();
            }
        }
    }
}
