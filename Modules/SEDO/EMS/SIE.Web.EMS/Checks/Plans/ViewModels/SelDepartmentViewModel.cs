using SIE.Domain;
using SIE.EMS.Enums;
using SIE.ObjectModel;
using System;

namespace SIE.Web.EMS.Checks.Plans.ViewModels
{
    /// <summary>
    /// 选择部门VM
    /// </summary>
    [RootEntity, Serializable]
    public class SelDepartmentViewModel : ViewModel
    {
        #region 点检计划ID CheckPlanId
        /// <summary>
        /// 点检计划ID
        /// </summary>
        [Label("点检计划ID")]
        public static readonly Property<double> CheckPlanIdProperty = P<SelDepartmentViewModel>.Register(e => e.CheckPlanId);

        /// <summary>
        /// 点检计划ID
        /// </summary>
        public double CheckPlanId
        {
            get { return this.GetProperty(CheckPlanIdProperty); }
            set { this.SetProperty(CheckPlanIdProperty, value); }
        }
        #endregion

        #region 点检单号 CheckPlanNo
        /// <summary>
        /// 点检单号
        /// </summary>
        [Label("点检单号")]
        public static readonly Property<string> CheckPlanNoProperty = P<SelDepartmentViewModel>.Register(e => e.CheckPlanNo);

        /// <summary>
        /// 点检单号
        /// </summary>
        public string CheckPlanNo
        {
            get { return this.GetProperty(CheckPlanNoProperty); }
            set { this.SetProperty(CheckPlanNoProperty, value); }
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
        public static readonly Property<CheckExeState> StateProperty = P<SelDepartmentViewModel>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public CheckExeState State
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

        #region 是否确认 IsConfirm
        /// <summary>
        /// 是否确认
        /// </summary>
        [Label("是否确认")]
        public static readonly Property<bool> IsConfirmProperty = P<SelDepartmentViewModel>.Register(e => e.IsConfirm);

        /// <summary>
        /// 是否确认
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
                View.Property(p => p.CheckPlanNo).ShowInList(width: 150).Readonly();
                View.Property(p => p.DepartmentId).Show(ShowInWhere.Hide).Readonly();
                View.Property(p => p.DepartmentName).Show().Readonly();
                View.Property(p => p.State).Show().Readonly();
                View.Property(p => p.IfOpenConfirmationTab).Show(ShowInWhere.Hide).Readonly();
            }
        }
    }
}
