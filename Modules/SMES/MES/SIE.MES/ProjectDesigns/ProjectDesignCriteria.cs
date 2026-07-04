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

namespace SIE.MES.ProjectDesigns
{
    /// <summary>
    /// 项目号需求设计
    /// </summary>
    [QueryEntity, Serializable]
    [Label("项目号需求设计")]
    public class ProjectDesignCriteria : Criteria
    {
        #region 项目号 ProjectMaintain
        /// <summary>
        /// 项目号Id
        /// </summary>
        [Label("项目号")]
        public static readonly IRefIdProperty ProjectMaintainIdProperty =
            P<ProjectDesignCriteria>.RegisterRefId(e => e.ProjectMaintainId, ReferenceType.Normal);

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
            P<ProjectDesignCriteria>.RegisterRef(e => e.ProjectMaintain, ProjectMaintainIdProperty);

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
            P<ProjectDesignCriteria>.RegisterRefId(e => e.ProductId, ReferenceType.Normal);

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
            P<ProjectDesignCriteria>.RegisterRef(e => e.Product, ProductIdProperty);

        /// <summary>
        /// 产品
        /// </summary>
        public Item Product
        {
            get { return this.GetRefEntity(ProductProperty); }
            set { this.SetRefEntity(ProductProperty, value); }
        }
        #endregion

        #region 销售订单 SaleOrderNo
        /// <summary>
        /// 销售订单
        /// </summary>
        [Label("销售订单")]
        public static readonly Property<string> SaleOrderNoProperty = P<ProjectDesignCriteria>.Register(e => e.SaleOrderNo);

        /// <summary>
        /// 销售订单
        /// </summary>
        public string SaleOrderNo
        {
            get { return this.GetProperty(SaleOrderNoProperty); }
            set { this.SetProperty(SaleOrderNoProperty, value); }
        }
        #endregion

        #region 启用状态 State
        /// <summary>
        /// 启用状态
        /// </summary>
        [Label("启用状态")]
        public static readonly Property<State?> StateProperty = P<ProjectDesignCriteria>.Register(e => e.State);

        /// <summary>
        /// 启用状态
        /// </summary>
        public State? State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        #region 审核状态 ExamineStatus
        /// <summary>
        /// 审核状态
        /// </summary>
        [Label("属性名")]
        public static readonly Property<ExamineStatus?> ExamineStatusProperty = P<ProjectDesignCriteria>.Register(e => e.ExamineStatus);

        /// <summary>
        /// 审核状态
        /// </summary>
        public ExamineStatus? ExamineStatus
        {
            get { return this.GetProperty(ExamineStatusProperty); }
            set { this.SetProperty(ExamineStatusProperty, value); }
        }
        #endregion

        #region 创建日期 CreateDate
        /// <summary>
        /// 创建日期
        /// </summary>
        [Label("创建日期")]
        public static readonly Property<DateRange> CreateDateProperty = P<ProjectDesignCriteria>.Register(e => e.CreateDate);

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateRange CreateDate
        {
            get { return this.GetProperty(CreateDateProperty); }
            set { this.SetProperty(CreateDateProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<ProjectDesignController>().QueryProjectDesign(this);
        }
    }
}
