using SIE.Domain;
using SIE.Items;
using SIE.MES.PrepareProducts.Enums;
using SIE.MES.PrepareProducts.Services;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.PrepareProducts
{
    /// <summary>
    /// 产前准备项目维护查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("产前准备项目维护查询实体")]
    public class PrepareProductCriteria : Criteria
    {
        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<PrepareProductCriteria>.Register(e => e.ProductCode);

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
        public static readonly Property<string> ProductNameProperty = P<PrepareProductCriteria>.Register(e => e.ProductName);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
            set { this.SetProperty(ProductNameProperty, value); }
        }
        #endregion

        #region 产品族 ProductFamily
        /// <summary>
        /// 产品族Id
        /// </summary>
        [Label("产品族")]
        public static readonly IRefIdProperty ProductFamilyIdProperty =
            P<PrepareProductCriteria>.RegisterRefId(e => e.ProductFamilyId, ReferenceType.Normal);

        /// <summary>
        /// 产品族Id
        /// </summary>
        public double? ProductFamilyId
        {
            get { return (double?)this.GetRefNullableId(ProductFamilyIdProperty); }
            set { this.SetRefNullableId(ProductFamilyIdProperty, value); }
        }

        /// <summary>
        /// 产品族
        /// </summary>
        public static readonly RefEntityProperty<ProductFamily> ProductFamilyProperty =
            P<PrepareProductCriteria>.RegisterRef(e => e.ProductFamily, ProductFamilyIdProperty);

        /// <summary>
        /// 产品族
        /// </summary>
        public ProductFamily ProductFamily
        {
            get { return this.GetRefEntity(ProductFamilyProperty); }
            set { this.SetRefEntity(ProductFamilyProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty =
            P<PrepareProductCriteria>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double? ProcessId
        {
            get { return (double?)this.GetRefNullableId(ProcessIdProperty); }
            set { this.SetRefNullableId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty =
            P<PrepareProductCriteria>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return this.GetRefEntity(ProcessProperty); }
            set { this.SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 项目编码 ProjectCode
        /// <summary>
        /// 项目编码
        /// </summary>
        [Label("项目编码")]
        public static readonly Property<string> ProjectCodeProperty = P<PrepareProductCriteria>.Register(e => e.ProjectCode);

        /// <summary>
        /// 项目编码
        /// </summary>
        public string ProjectCode
        {
            get { return this.GetProperty(ProjectCodeProperty); }
            set { this.SetProperty(ProjectCodeProperty, value); }
        }
        #endregion

        #region 项目名称 ProjectName
        /// <summary>
        /// 项目名称
        /// </summary>
        [Label("项目名称")]
        public static readonly Property<string> ProjectNameProperty = P<PrepareProductCriteria>.Register(e => e.ProjectName);

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName
        {
            get { return this.GetProperty(ProjectNameProperty); }
            set { this.SetProperty(ProjectNameProperty, value); }
        }
        #endregion

        #region 项目类型 ProjectType
        /// <summary>
        /// 项目类型
        /// </summary>
        [Label("项目类型")]
        public static readonly Property<PrepareProjectType?> ProjectTypeProperty = P<PrepareProductCriteria>.Register(e => e.ProjectType);

        /// <summary>
        /// 项目类型
        /// </summary>
        public PrepareProjectType? ProjectType
        {
            get { return this.GetProperty(ProjectTypeProperty); }
            set { this.SetProperty(ProjectTypeProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询逻辑
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<PrepareProductService>().QueryPreProductEntityList(this);
        }
    }


}
