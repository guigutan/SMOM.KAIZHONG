using SIE.Domain;
using SIE.ObjectModel;
using SIE.Web.Tech;
using System;

namespace SIE.Web.MES.WorkOrders
{
    /// <summary>
    /// 属性值ViewModel
    /// </summary>
    [Serializable]
    [RootEntity]
    [Label("产品属性")]
    public class WorkOrderRouting : ViewModel
    {
        #region 版本Id VersionId
        /// <summary>
        /// 版本Id
        /// </summary>
        [Label("版本Id")]
        public static readonly Property<double> VersionIdProperty = P<WorkOrderRouting>.Register(e => e.VersionId);

        /// <summary>
        /// 版本Id
        /// </summary>
        public double VersionId
        {
            get { return this.GetProperty(VersionIdProperty); }
            set { this.SetProperty(VersionIdProperty, value); }
        }
        #endregion

    }

    internal class WorkOrderRoutingViewConfig : WebViewConfig<WorkOrderRouting>
    {

        protected override void ConfigView()
        {
            View.FormEdit();
        }

        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            View.UseDetail(4);
            View.Property(p => p.VersionId).UseRoutingDisplayEditor(e =>
            {
                e.Canvas = "workOrderRoutingCanvas";
                e.Property = "VersionId";
            }).ShowInDetail(hideLabel: true, width: "auto", columnSpan: 4).HasLabel(string.Empty);
        }

    }
}
