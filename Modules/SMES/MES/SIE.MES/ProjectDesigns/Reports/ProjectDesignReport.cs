using SIE.Core.ProjectMaintains;
using SIE.CSM.Customers;
using SIE.Domain;
using SIE.Items;
using SIE.MES.ProjectDesigns.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ProjectDesigns.Reports
{
    /// <summary>
    /// 项目号跟踪报表
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(ProjectDesignReportCriteria))]
    [Label("项目号跟踪报表")]
    public class ProjectDesignReport : ViewModel
    {
        #region 项目号Id ProjectMaintainId
        /// <summary>
        /// 项目号Id
        /// </summary>
        [Label("项目号Id")]
        public static readonly Property<double> ProjectMaintainIdProperty = P<ProjectDesignReport>.Register(e => e.ProjectMaintainId);

        /// <summary>
        /// 项目号Id
        /// </summary>
        public double ProjectMaintainId
        {
            get { return this.GetProperty(ProjectMaintainIdProperty); }
            set { this.SetProperty(ProjectMaintainIdProperty, value); }
        }
        #endregion

        #region 项目号 ProjectMaintain
        /// <summary>
        /// 项目号
        /// </summary>
        [Label("项目号")]
        public static readonly Property<string> ProjectMaintainProperty = P<ProjectDesignReport>.Register(e => e.ProjectMaintain);

        /// <summary>
        /// 项目号
        /// </summary>
        public string ProjectMaintain
        {
            get { return this.GetProperty(ProjectMaintainProperty); }
            set { this.SetProperty(ProjectMaintainProperty, value); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<ProjectDesignReport>.Register(e => e.ProductCode);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return this.GetProperty(ProductCodeProperty); }
            set { this.SetProperty(ProductCodeProperty, value); }
        }
        #endregion

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<ProjectDesignReport>.Register(e => e.ProductName);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
            set { this.SetProperty(ProductNameProperty, value); }
        }
        #endregion

        #region 型号规格 SpecificationModel
        /// <summary>
        /// 型号规格
        /// </summary>
        [Label("型号规格")]
        public static readonly Property<string> SpecificationModelProperty = P<ProjectDesignReport>.Register(e => e.SpecificationModel);

        /// <summary>
        /// 型号规格
        /// </summary>
        public string SpecificationModel
        {
            get { return this.GetProperty(SpecificationModelProperty); }
            set { this.SetProperty(SpecificationModelProperty, value); }
        }
        #endregion

        #region 审核状态 ExamineStatus
        /// <summary>
        /// 审核状态
        /// </summary>
        [Label("审核状态")]
        public static readonly Property<ExamineStatus> ExamineStatusProperty = P<ProjectDesignReport>.Register(e => e.ExamineStatus);

        /// <summary>
        /// 审核状态
        /// </summary>
        public ExamineStatus ExamineStatus
        {
            get { return this.GetProperty(ExamineStatusProperty); }
            set { this.SetProperty(ExamineStatusProperty, value); }
        }
        #endregion

        #region 销售订单号 SaleOrderNo
        /// <summary>
        /// 销售订单号
        /// </summary>
        [Label("销售订单号")]
        public static readonly Property<string> SaleOrderNoProperty = P<ProjectDesignReport>.Register(e => e.SaleOrderNo);

        /// <summary>
        /// 销售订单号
        /// </summary>
        public string SaleOrderNo
        {
            get { return this.GetProperty(SaleOrderNoProperty); }
            set { this.SetProperty(SaleOrderNoProperty, value); }
        }
        #endregion

        #region 客户编码 CustomerCode
        /// <summary>
        /// 客户编码
        /// </summary>
        [Label("客户编码")]
        public static readonly Property<string> CustomerCodeProperty = P<ProjectDesignReport>.Register(e => e.CustomerCode);

        /// <summary>
        /// 客户编码
        /// </summary>
        public string CustomerCode
        {
            get { return this.GetProperty(CustomerCodeProperty); }
            set { this.SetProperty(CustomerCodeProperty, value); }
        }
        #endregion

        #region 客户名称 CustomerName
        /// <summary>
        /// 客户名称
        /// </summary>
        [Label("客户名称")]
        public static readonly Property<string> CustomerNameProperty = P<ProjectDesignReport>.Register(e => e.CustomerName);

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName
        {
            get { return this.GetProperty(CustomerNameProperty); }
            set { this.SetProperty(CustomerNameProperty, value); }
        }
        #endregion

        #region 订单数量 Qty
        /// <summary>
        /// 订单数量
        /// </summary>
        [Label("订单数量")]
        public static readonly Property<decimal?> QtyProperty = P<ProjectDesignReport>.Register(e => e.Qty);

        /// <summary>
        /// 订单数量
        /// </summary>
        public decimal? Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 单位 Unit
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitProperty = P<ProjectDesignReport>.Register(e => e.Unit);

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit
        {
            get { return this.GetProperty(UnitProperty); }
            set { this.SetProperty(UnitProperty, value); }
        }
        #endregion

        #region 交货日期 DeliveryDate
        /// <summary>
        /// 交货日期
        /// </summary>
        [Label("交货日期")]
        public static readonly Property<DateTime?> DeliveryDateProperty = P<ProjectDesignReport>.Register(e => e.DeliveryDate);

        /// <summary>
        /// 交货日期
        /// </summary>
        public DateTime? DeliveryDate
        {
            get { return this.GetProperty(DeliveryDateProperty); }
            set { this.SetProperty(DeliveryDateProperty, value); }
        }
        #endregion

        #region 基本属性 BaseInfo
        /// <summary>
        /// 基本属性
        /// </summary>
        [Label("基本属性")]
        public static readonly Property<ChildInfoStatus> BaseInfoProperty = P<ProjectDesignReport>.Register(e => e.BaseInfo);

        /// <summary>
        /// 基本属性
        /// </summary>
        public ChildInfoStatus BaseInfo
        {
            get { return this.GetProperty(BaseInfoProperty); }
            set { this.SetProperty(BaseInfoProperty, value); }
        }
        #endregion

        #region 工艺路线 RoutingInfo
        /// <summary>
        /// 工艺路线
        /// </summary>
        [Label("工艺路线")]
        public static readonly Property<ChildInfoStatus> RoutingInfoProperty = P<ProjectDesignReport>.Register(e => e.RoutingInfo);

        /// <summary>
        /// 工艺路线
        /// </summary>
        public ChildInfoStatus RoutingInfo
        {
            get { return this.GetProperty(RoutingInfoProperty); }
            set { this.SetProperty(RoutingInfoProperty, value); }
        }
        #endregion

        #region 产品Bom BomInfo
        /// <summary>
        /// 产品Bom
        /// </summary>
        [Label("产品Bom")]
        public static readonly Property<ChildInfoStatus> BomInfoProperty = P<ProjectDesignReport>.Register(e => e.BomInfo);

        /// <summary>
        /// 产品Bom
        /// </summary>
        public ChildInfoStatus BomInfo
        {
            get { return this.GetProperty(BomInfoProperty); }
            set { this.SetProperty(BomInfoProperty, value); }
        }
        #endregion

        #region 附件文档 AttachInfo
        /// <summary>
        /// 附件文档
        /// </summary>
        [Label("属性名")]
        public static readonly Property<ChildInfoStatus> AttachInfoProperty = P<ProjectDesignReport>.Register(e => e.AttachInfo);

        /// <summary>
        /// 附件文档
        /// </summary>
        public ChildInfoStatus AttachInfo
        {
            get { return this.GetProperty(AttachInfoProperty); }
            set { this.SetProperty(AttachInfoProperty, value); }
        }
        #endregion

        #region 设计状态 DesignStatus
        /// <summary>
        /// 设计状态
        /// </summary>
        [Label("设计状态")]
        public static readonly Property<DesignStatus> DesignStatusProperty = P<ProjectDesignReport>.RegisterReadOnly(
            e => e.DesignStatus, e => e.GetDesignStatus());
        /// <summary>
        /// 设计状态
        /// </summary>

        public DesignStatus DesignStatus
        {
            get { return this.GetProperty(DesignStatusProperty); }
        }
        private DesignStatus GetDesignStatus()
        {
            if (ExamineStatus == ExamineStatus.UnExamine 
                && BaseInfo == ChildInfoStatus.UnFill && RoutingInfo == ChildInfoStatus.UnFill && BomInfo == ChildInfoStatus.UnFill)
            {
                return DesignStatus.Create;
            }

            else if (ExamineStatus == ExamineStatus.UnExamine
                && (BaseInfo == ChildInfoStatus.UnFill || RoutingInfo == ChildInfoStatus.UnFill || BomInfo == ChildInfoStatus.UnFill))
            {
                return DesignStatus.DesignIng;
            }
            else if (ExamineStatus == ExamineStatus.UnExamine
                && BaseInfo == ChildInfoStatus.HasFilled && RoutingInfo == ChildInfoStatus.HasFilled && BomInfo == ChildInfoStatus.HasFilled)
            {
                return DesignStatus.ToExamine;
            }
            else if (ExamineStatus == ExamineStatus.Examined
                && BaseInfo == ChildInfoStatus.HasFilled && RoutingInfo == ChildInfoStatus.HasFilled && BomInfo == ChildInfoStatus.HasFilled)
            {
                return DesignStatus.Complete;
            }
            return DesignStatus.Create;
        }
        #endregion

        #region 生产状态 ProduceStatus
        /// <summary>
        /// 生产状态
        /// </summary>
        [Label("生产状态")]
        public static readonly Property<ProduceStatus> ProduceStatusProperty = P<ProjectDesignReport>.Register(e => e.ProduceStatus);

        /// <summary>
        /// 生产状态
        /// </summary>
        public ProduceStatus ProduceStatus
        {
            get { return this.GetProperty(ProduceStatusProperty); }
            set { this.SetProperty(ProduceStatusProperty, value); }
        }
        #endregion

    }
}
