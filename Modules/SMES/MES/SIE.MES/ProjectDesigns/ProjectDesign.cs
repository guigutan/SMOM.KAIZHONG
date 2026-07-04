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
    /// 项目号需求设计
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(ProjectDesignCriteria))]
    [Label("项目号需求设计")]
    public class ProjectDesign : ProjectDesignBase
    {
        /// <summary>
        /// 构造
        /// </summary>
        public ProjectDesign()
        {
            State = State.Disable;
        }

        #region 子列表
        #region 操作日志 ProjectDesignLogList
        /// <summary>
        /// 操作日志
        /// </summary>
        [Label("操作日志")]
        public static readonly ListProperty<EntityList<ProjectDesignLog>> ProjectDesignLogListProperty = P<ProjectDesign>.RegisterList(e => e.ProjectDesignLogList);

        /// <summary>
        /// 操作日志
        /// </summary>
        public EntityList<ProjectDesignLog> ProjectDesignLogList
        {
            get { return this.GetLazyList(ProjectDesignLogListProperty); }
        }
        #endregion

        #endregion

        #region 视图属性
        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<ProjectDesign>.RegisterView(e => e.ProductCode, p => p.Product.Code);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return this.GetProperty(ProductCodeProperty); }
        }
        #endregion

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<ProjectDesign>.RegisterView(e => e.ProductName, p => p.Product.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
        }
        #endregion

        #region 规格型号 SpecificationModel
        /// <summary>
        /// 规格型号
        /// </summary>
        [Label("规格型号")]
        public static readonly Property<string> SpecificationModelProperty = P<ProjectDesign>.RegisterView(e => e.SpecificationModel, p => p.Product.SpecificationModel);

        /// <summary>
        /// 规格型号
        /// </summary>
        public string SpecificationModel
        {
            get { return this.GetProperty(SpecificationModelProperty); }
        }
        #endregion

        #region 客户编码 CustomerCode
        /// <summary>
        /// 客户编码
        /// </summary>
        [Label("客户编码")]
        public static readonly Property<string> CustomerCodeProperty = P<ProjectDesign>.RegisterView(e => e.CustomerCode, p => p.Customer.Code);

        /// <summary>
        /// 客户编码
        /// </summary>
        public string CustomerCode
        {
            get { return this.GetProperty(CustomerCodeProperty); }
        }
        #endregion

        #region 客户名称 CustomerName
        /// <summary>
        /// 客户名称
        /// </summary>
        [Label("客户名称")]
        public static readonly Property<string> CustomerNameProperty = P<ProjectDesign>.RegisterView(e => e.CustomerName, p => p.Customer.Name);

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName
        {
            get { return this.GetProperty(CustomerNameProperty); }
        }
        #endregion

        #region 项目号编码 ProjectMaintainCode
        /// <summary>
        /// 项目号编码
        /// </summary>
        [Label("项目号编码")]
        public static readonly Property<string> ProjectMaintainCodeProperty = P<ProjectDesign>.RegisterView(e => e.ProjectMaintainCode, p => p.ProjectMaintain.Code);

        /// <summary>
        /// 项目号编码
        /// </summary>
        public string ProjectMaintainCode
        {
            get { return this.GetProperty(ProjectMaintainCodeProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 实体配置
    /// </summary>
    public class ProjectDesignConfig : EntityConfig<ProjectDesign>
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
