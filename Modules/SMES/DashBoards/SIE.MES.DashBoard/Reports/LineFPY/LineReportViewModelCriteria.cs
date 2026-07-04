using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources.ShiftTypes;
using SIE.Resources.WipResources;
using System;

namespace SIE.MES.DashBoard.Reports.LineFPY
{
    [QueryEntity, Serializable]
    public class LineReportViewModelCriteria : Criteria
    {
        public LineReportViewModelCriteria()
        {
            CollectDate = new DateRange();
        }

        #region 资源 Line
        /// <summary>
        /// 资源Id
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty ResourceIdProperty =
            P<LineReportViewModelCriteria>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源Id
        /// </summary>
        public double? ResourceId
        {
            get { return (double?)this.GetRefNullableId(ResourceIdProperty); }
            set { this.SetRefNullableId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty =
            P<LineReportViewModelCriteria>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return this.GetRefEntity(ResourceProperty); }
            set { this.SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 班次 Shift
        /// <summary>
        /// 班次Id
        /// </summary>
        [Label("班次")]
        public static readonly IRefIdProperty ShiftIdProperty =
            P<LineReportViewModelCriteria>.RegisterRefId(e => e.ShiftId, ReferenceType.Normal);

        /// <summary>
        /// 班次Id
        /// </summary>
        public double? ShiftId
        {
            get { return (double?)this.GetRefNullableId(ShiftIdProperty); }
            set { this.SetRefNullableId(ShiftIdProperty, value); }
        }

        /// <summary>
        /// 班次
        /// </summary>
        public static readonly RefEntityProperty<Shift> ShiftProperty =
            P<LineReportViewModelCriteria>.RegisterRef(e => e.Shift, ShiftIdProperty);

        /// <summary>
        /// 班次
        /// </summary>
        public Shift Shift
        {
            get { return this.GetRefEntity(ShiftProperty); }
            set { this.SetRefEntity(ShiftProperty, value); }
        }
        #endregion

        #region 日期 CollectDate
        /// <summary>
        /// 日期
        /// </summary>
        [Label("日期")]
        public static readonly Property<DateRange> CollectDateProperty = P<LineReportViewModelCriteria>.Register(e => e.CollectDate);

        /// <summary>
        /// 日期
        /// </summary>
        public DateRange CollectDate
        {
            get { return this.GetProperty(CollectDateProperty); }
            set { this.SetProperty(CollectDateProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            EntityList<LineReportViewModel> modelList = new EntityList<LineReportViewModel>();
            modelList.Add(RT.Service.Resolve<LineReportViewModelController>().GetLineReportViewModel(this));
            return modelList;
        }
    }
}
