using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ProjectDesigns.ViewModels
{
    /// <summary>
    /// 项目号需求设计-工艺卡
    /// </summary>
    [RootEntity, Serializable]
    [Label("项目号需求设计-工艺卡")]
    public class ProjectDesignCard : ViewModel
    {
        #region 工单号 WoNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WoNoProperty = P<ProjectDesignCard>.Register(e => e.WoNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WoNo
        {
            get { return this.GetProperty(WoNoProperty); }
            set { this.SetProperty(WoNoProperty, value); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<ProjectDesignCard>.Register(e => e.ProductCode);

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
        public static readonly Property<string> ProductNameProperty = P<ProjectDesignCard>.Register(e => e.ProductName);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
            set { this.SetProperty(ProductNameProperty, value); }
        }
        #endregion

        #region 产品Id ProductId
        /// <summary>
        /// 产品Id
        /// </summary>
        [Label("产品Id")]
        public static readonly Property<double> ProductIdProperty = P<ProjectDesignCard>.Register(e => e.ProductId);

        /// <summary>
        /// 产品Id
        /// </summary>
        public double ProductId
        {
            get { return this.GetProperty(ProductIdProperty); }
            set { this.SetProperty(ProductIdProperty, value); }
        }
        #endregion

        #region 计划数 PlanQty
        /// <summary>
        /// 计划数
        /// </summary>
        [Label("计划数")]
        public static readonly Property<decimal> PlanQtyProperty = P<ProjectDesignCard>.Register(e => e.PlanQty);

        /// <summary>
        /// 计划数
        /// </summary>
        public decimal PlanQty
        {
            get { return this.GetProperty(PlanQtyProperty); }
            set { this.SetProperty(PlanQtyProperty, value); }
        }
        #endregion

        #region 资源 WipResource
        /// <summary>
        /// 资源
        /// </summary>
        [Label("资源")]
        public static readonly Property<string> WipResourceNameProperty = P<ProjectDesignCard>.Register(e => e.WipResource);

        /// <summary>
        /// 资源
        /// </summary>
        public string WipResource
        {
            get { return this.GetProperty(WipResourceNameProperty); }
            set { this.SetProperty(WipResourceNameProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序
        /// </summary>
        [Label("工序")]
        public static readonly Property<string> ProcessProperty = P<ProjectDesignCard>.Register(e => e.Process);

        /// <summary>
        /// 工序
        /// </summary>
        public string Process
        {
            get { return this.GetProperty(ProcessProperty); }
            set { this.SetProperty(ProcessProperty, value); }
        }
        #endregion

        #region 工序Id ProcessId
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序Id")]
        public static readonly Property<double> ProcessIdProperty = P<ProjectDesignCard>.Register(e => e.ProcessId);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double ProcessId
        {
            get { return this.GetProperty(ProcessIdProperty); }
            set { this.SetProperty(ProcessIdProperty, value); }
        }
        #endregion

        #region 项目号 ProjectNo
        /// <summary>
        /// 项目号
        /// </summary>
        [Label("项目号")]
        public static readonly Property<string> ProjectNoProperty = P<ProjectDesignCard>.Register(e => e.ProjectNo);

        /// <summary>
        /// 项目号
        /// </summary>
        public string ProjectNo
        {
            get { return this.GetProperty(ProjectNoProperty); }
            set { this.SetProperty(ProjectNoProperty, value); }
        }
        #endregion

        #region 项目号Id ProjectId
        /// <summary>
        /// 项目号Id
        /// </summary>
        [Label("项目号Id")]
        public static readonly Property<double?> ProjectIdProperty = P<ProjectDesignCard>.Register(e => e.ProjectId);

        /// <summary>
        /// 项目号Id
        /// </summary>
        public double? ProjectId
        {
            get { return this.GetProperty(ProjectIdProperty); }
            set { this.SetProperty(ProjectIdProperty, value); }
        }
        #endregion

    }
}
