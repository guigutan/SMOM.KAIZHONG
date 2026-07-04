using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources.Employees;

namespace SIE.Web.MES.TeamManagement.ShiftSchedules.ViewModels
{
    /// <summary>
    /// 班组切换视图模型
    /// </summary>
    [RootEntity]
    public class ChangeWorkGroupViewModel : ViewModel
    {
        #region 班组 WorkGroup
        /// <summary>
        /// 班组Id
        /// </summary>
        [Label("班组")]
        public static readonly IRefIdProperty WorkGroupIdProperty =
            P<ChangeWorkGroupViewModel>.RegisterRefId(e => e.WorkGroupId, ReferenceType.Normal);

        /// <summary>
        /// 班组Id
        /// </summary>
        public double? WorkGroupId
        {
            get { return (double?)this.GetRefNullableId(WorkGroupIdProperty); }
            set { this.SetRefNullableId(WorkGroupIdProperty, value); }
        }

        /// <summary>
        /// 班组
        /// </summary>
        public static readonly RefEntityProperty<WorkGroup> WorkGroupProperty =
            P<ChangeWorkGroupViewModel>.RegisterRef(e => e.WorkGroup, WorkGroupIdProperty);

        /// <summary>
        /// 班组
        /// </summary>
        public WorkGroup WorkGroup
        {
            get { return this.GetRefEntity(WorkGroupProperty); }
            set { this.SetRefEntity(WorkGroupProperty, value); }
        }
        #endregion

        #region 清除班组 Clear
        /// <summary>
        /// 清除班组
        /// </summary>
        [Label("清除班组")]
        public static readonly Property<bool> ClearProperty = P<ChangeWorkGroupViewModel>.Register(e => e.Clear);

        /// <summary>
        /// 清除班组
        /// </summary>
        public bool Clear
        {
            get { return this.GetProperty(ClearProperty); }
            set { this.SetProperty(ClearProperty, value); }
        }
        #endregion 
    }

    /// <summary>
    /// 班组切换视图模型视图配置
    /// </summary>
    public class ChangeWorkGroupViewConfig : WebViewConfig<ChangeWorkGroupViewModel>
    {
        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.UseDetail(2);
            View.Property(p => p.WorkGroup);
            View.Property(p => p.Clear).UseDisplayEditor(p => { p.XType = "clearWorkGroupEditor"; }).HasLabel(string.Empty);
        }
    }
}