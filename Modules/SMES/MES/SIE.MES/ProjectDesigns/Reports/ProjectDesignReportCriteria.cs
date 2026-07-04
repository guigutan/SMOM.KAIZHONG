using SIE.Core.ProjectMaintains;
using SIE.Domain;
using SIE.Items;
using SIE.MES.ProjectDesigns.Enums;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ProjectDesigns.Reports
{
    /// <summary>
    /// 项目号跟踪报表查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("项目号跟踪报表查询实体")]
    public class ProjectDesignReportCriteria : Criteria
    {
        #region 项目号 ProjectMaintain
        /// <summary>
        /// 项目号Id
        /// </summary>
        [Label("项目号")]
        public static readonly IRefIdProperty ProjectMaintainIdProperty =
            P<ProjectDesignReportCriteria>.RegisterRefId(e => e.ProjectMaintainId, ReferenceType.Normal);

        /// <summary>
        /// 项目号Id
        /// </summary>
        public double? ProjectMaintainId
        {
            get { return (double?)this.GetRefNullableId(ProjectMaintainIdProperty); }
            set { this.SetRefNullableId(ProjectMaintainIdProperty, value); }
        }

        /// <summary>
        /// 项目号
        /// </summary>
        public static readonly RefEntityProperty<ProjectMaintain> ProjectMaintainProperty =
            P<ProjectDesignReportCriteria>.RegisterRef(e => e.ProjectMaintain, ProjectMaintainIdProperty);

        /// <summary>
        /// 项目号
        /// </summary>
        public ProjectMaintain ProjectMaintain
        {
            get { return this.GetRefEntity(ProjectMaintainProperty); }
            set { this.SetRefEntity(ProjectMaintainProperty, value); }
        }
        #endregion

        #region 产品 Product
        /// <summary>
        /// 产品Id
        /// </summary>
        [Label("产品")]
        public static readonly IRefIdProperty ProductIdProperty =
            P<ProjectDesignReportCriteria>.RegisterRefId(e => e.ProductId, ReferenceType.Normal);

        /// <summary>
        /// 产品Id
        /// </summary>
        public double? ProductId
        {
            get { return (double?)this.GetRefNullableId(ProductIdProperty); }
            set { this.SetRefNullableId(ProductIdProperty, value); }
        }

        /// <summary>
        /// 产品
        /// </summary>
        public static readonly RefEntityProperty<Item> ProductProperty =
            P<ProjectDesignReportCriteria>.RegisterRef(e => e.Product, ProductIdProperty);

        /// <summary>
        /// 产品
        /// </summary>
        public Item Product
        {
            get { return this.GetRefEntity(ProductProperty); }
            set { this.SetRefEntity(ProductProperty, value); }
        }
        #endregion

        #region 销售订单号 SaleOrderNo
        /// <summary>
        /// 销售订单号
        /// </summary>
        [Label("销售订单号")]
        public static readonly Property<string> SaleOrderNoProperty = P<ProjectDesignReportCriteria>.Register(e => e.SaleOrderNo);

        /// <summary>
        /// 销售订单号
        /// </summary>
        public string SaleOrderNo
        {
            get { return this.GetProperty(SaleOrderNoProperty); }
            set { this.SetProperty(SaleOrderNoProperty, value); }
        }
        #endregion

        #region 设计状态 DesignStatus
        /// <summary>
        /// 设计状态
        /// </summary>
        [Label("设计状态")]
        public static readonly Property<DesignStatus?> DesignStatusProperty = P<ProjectDesignReportCriteria>.Register(e => e.DesignStatus);

        /// <summary>
        /// 设计状态
        /// </summary>
        public DesignStatus? DesignStatus
        {
            get { return this.GetProperty(DesignStatusProperty); }
            set { this.SetProperty(DesignStatusProperty, value); }
        }
        #endregion

        #region 生产状态 ProduceStatus
        /// <summary>
        /// 生产状态
        /// </summary>
        [Label("生产状态")]
        public static readonly Property<ProduceStatus?> ProduceStatusProperty = P<ProjectDesignReportCriteria>.Register(e => e.ProduceStatus);

        /// <summary>
        /// 生产状态
        /// </summary>
        public ProduceStatus? ProduceStatus
        {
            get { return this.GetProperty(ProduceStatusProperty); }
            set { this.SetProperty(ProduceStatusProperty, value); }
        }
        #endregion

        #region 交货日期 DeliveryDate
        /// <summary>
        /// 交货日期
        /// </summary>
        [Label("交货日期")]
        public static readonly Property<DateRange> DeliveryDateProperty = P<ProjectDesignReportCriteria>.Register(e => e.DeliveryDate);

        /// <summary>
        /// 交货日期
        /// </summary>
        public DateRange DeliveryDate
        {
            get { return this.GetProperty(DeliveryDateProperty); }
            set { this.SetProperty(DeliveryDateProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<ProjectDesignReportController>().QueryProjectDesignReport(this);
        }
    }
}
