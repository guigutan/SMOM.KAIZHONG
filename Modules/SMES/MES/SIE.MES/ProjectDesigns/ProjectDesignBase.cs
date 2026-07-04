using SIE.Core.ProjectMaintains;
using SIE.CSM.Customers;
using SIE.Domain;
using SIE.Items;
using SIE.MES.ProjectDesigns.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ProjectDesigns
{
    /// <summary>
    /// 项目号需求设计基类
    /// </summary>
    [RootEntity, Serializable]
    [Label("项目号需求设计基类")]
    public class ProjectDesignBase : DataEntity
    {
        #region 项目号 ProjectMaintain
        /// <summary>
        /// 项目号Id
        /// </summary>
        [Label("项目号")]
        public static readonly IRefIdProperty ProjectMaintainIdProperty =
            P<ProjectDesignBase>.RegisterRefId(e => e.ProjectMaintainId, ReferenceType.Normal);

        /// <summary>
        /// 项目号Id
        /// </summary>
        public double ProjectMaintainId
        {
            get { return (double)this.GetRefId(ProjectMaintainIdProperty); }
            set { this.SetRefId(ProjectMaintainIdProperty, value); }
        }

        /// <summary>
        /// 项目号
        /// </summary>
        public static readonly RefEntityProperty<ProjectMaintain> ProjectMaintainProperty =
            P<ProjectDesignBase>.RegisterRef(e => e.ProjectMaintain, ProjectMaintainIdProperty);

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
            P<ProjectDesignBase>.RegisterRefId(e => e.ProductId, ReferenceType.Normal);

        /// <summary>
        /// 产品Id
        /// </summary>
        public double ProductId
        {
            get { return (double)this.GetRefId(ProductIdProperty); }
            set { this.SetRefId(ProductIdProperty, value); }
        }

        /// <summary>
        /// 产品
        /// </summary>
        public static readonly RefEntityProperty<Item> ProductProperty =
            P<ProjectDesignBase>.RegisterRef(e => e.Product, ProductIdProperty);

        /// <summary>
        /// 产品
        /// </summary>
        public Item Product
        {
            get { return this.GetRefEntity(ProductProperty); }
            set { this.SetRefEntity(ProductProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<State> StateProperty = P<ProjectDesignBase>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public State State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        #region 审核状态 ExamineStatus
        /// <summary>
        /// 审核状态
        /// </summary>
        [Label("审核状态")]
        public static readonly Property<ExamineStatus> ExamineStatusProperty = P<ProjectDesignBase>.Register(e => e.ExamineStatus);

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
        public static readonly Property<string> SaleOrderNoProperty = P<ProjectDesignBase>.Register(e => e.SaleOrderNo);

        /// <summary>
        /// 销售订单号
        /// </summary>
        public string SaleOrderNo
        {
            get { return this.GetProperty(SaleOrderNoProperty); }
            set { this.SetProperty(SaleOrderNoProperty, value); }
        }
        #endregion

        #region 客户 Customer
        /// <summary>
        /// 客户Id
        /// </summary>
        [Label("客户")]
        public static readonly IRefIdProperty CustomerIdProperty =
            P<ProjectDesignBase>.RegisterRefId(e => e.CustomerId, ReferenceType.Normal);

        /// <summary>
        /// 客户Id
        /// </summary>
        public double? CustomerId
        {
            get { return (double?)this.GetRefNullableId(CustomerIdProperty); }
            set { this.SetRefNullableId(CustomerIdProperty, value); }
        }

        /// <summary>
        /// 客户
        /// </summary>
        public static readonly RefEntityProperty<Customer> CustomerProperty =
            P<ProjectDesignBase>.RegisterRef(e => e.Customer, CustomerIdProperty);

        /// <summary>
        /// 客户
        /// </summary>
        public Customer Customer
        {
            get { return this.GetRefEntity(CustomerProperty); }
            set { this.SetRefEntity(CustomerProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<decimal?> QtyProperty = P<ProjectDesignBase>.Register(e => e.Qty);

        /// <summary>
        /// 数量
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
        public static readonly Property<string> UnitProperty = P<ProjectDesignBase>.Register(e => e.Unit);

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
        public static readonly Property<DateTime?> DeliveryDateProperty = P<ProjectDesignBase>.Register(e => e.DeliveryDate);

        /// <summary>
        /// 交货日期
        /// </summary>
        public DateTime? DeliveryDate
        {
            get { return this.GetProperty(DeliveryDateProperty); }
            set { this.SetProperty(DeliveryDateProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<ProjectDesignBase>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return this.GetProperty(RemarkProperty); }
            set { this.SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 基本属性 BaseInfo
        /// <summary>
        /// 基本属性
        /// </summary>
        [Label("基本属性")]
        public static readonly Property<ChildInfoStatus> BaseInfoProperty = P<ProjectDesignBase>.Register(e => e.BaseInfo);

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
        public static readonly Property<ChildInfoStatus> RoutingInfoProperty = P<ProjectDesignBase>.Register(e => e.RoutingInfo);

        /// <summary>
        /// 工艺路线
        /// </summary>
        public ChildInfoStatus RoutingInfo
        {
            get { return this.GetProperty(RoutingInfoProperty); }
            set { this.SetProperty(RoutingInfoProperty, value); }
        }
        #endregion

        #region 产品BOM BomInfo
        /// <summary>
        /// 产品BOM
        /// </summary>
        [Label("产品BOM")]
        public static readonly Property<ChildInfoStatus> BomInfoProperty = P<ProjectDesignBase>.Register(e => e.BomInfo);

        /// <summary>
        /// 产品BOM
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
        [Label("附件文档")]
        public static readonly Property<ChildInfoStatus> AttachInfoProperty = P<ProjectDesignBase>.Register(e => e.AttachInfo);

        /// <summary>
        /// 附件文档
        /// </summary>
        public ChildInfoStatus AttachInfo
        {
            get { return this.GetProperty(AttachInfoProperty); }
            set { this.SetProperty(AttachInfoProperty, value); }
        }
        #endregion

        #region 审核日期 ExamineDate
        /// <summary>
        /// 审核日期
        /// </summary>
        [Label("审核日期")]
        public static readonly Property<DateTime?> ExamineDateProperty = P<ProjectDesignBase>.Register(e => e.ExamineDate);

        /// <summary>
        /// 审核日期
        /// </summary>
        public DateTime? ExamineDate
        {
            get { return this.GetProperty(ExamineDateProperty); }
            set { this.SetProperty(ExamineDateProperty, value); }
        }
        #endregion

        #region 审核人 Examiner
        /// <summary>
        /// 审核人Id
        /// </summary>
        [Label("审核人")]
        public static readonly IRefIdProperty ExaminerIdProperty =
            P<ProjectDesignBase>.RegisterRefId(e => e.ExaminerId, ReferenceType.Normal);

        /// <summary>
        /// 审核人Id
        /// </summary>
        public double? ExaminerId
        {
            get { return (double?)this.GetRefNullableId(ExaminerIdProperty); }
            set { this.SetRefNullableId(ExaminerIdProperty, value); }
        }

        /// <summary>
        /// 审核人
        /// </summary>
        public static readonly RefEntityProperty<Employee> ExaminerProperty =
            P<ProjectDesignBase>.RegisterRef(e => e.Examiner, ExaminerIdProperty);

        /// <summary>
        /// 审核人
        /// </summary>
        public Employee Examiner
        {
            get { return this.GetRefEntity(ExaminerProperty); }
            set { this.SetRefEntity(ExaminerProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 实体配置
    /// </summary>
    public class ProjectDesignBaseConfig : EntityConfig<ProjectDesignBase>
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("MES_PRO_DESIGN").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
