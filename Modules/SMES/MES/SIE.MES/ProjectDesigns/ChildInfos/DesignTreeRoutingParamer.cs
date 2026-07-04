using SIE.Domain;
using SIE.MES.Projects;
using SIE.MES.Projects.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ProjectDesigns.ChildInfos
{
    /// <summary>
    /// 工艺资料-产品工艺路线工序参数
    /// </summary>
    [ChildEntity, Serializable]
    [Label("工艺资料-产品工艺路线工序参数")]
    public class DesignTreeRoutingParamer : DataEntity
    {
        #region 产品工艺路线设置 DesignTreeRouting
        /// <summary>
        /// 产品工艺路线设置Id
        /// </summary>
        [Label("产品工艺路线设置")]
        public static readonly IRefIdProperty DesignTreeRoutingIdProperty =
            P<DesignTreeRoutingParamer>.RegisterRefId(e => e.DesignTreeRoutingId, ReferenceType.Parent);

        /// <summary>
        /// 产品工艺路线设置Id
        /// </summary>
        public double DesignTreeRoutingId
        {
            get { return (double)this.GetRefId(DesignTreeRoutingIdProperty); }
            set { this.SetRefId(DesignTreeRoutingIdProperty, value); }
        }

        /// <summary>
        /// 产品工艺路线设置
        /// </summary>
        public static readonly RefEntityProperty<DesignTreeRouting> DesignTreeRoutingProperty =
            P<DesignTreeRoutingParamer>.RegisterRef(e => e.DesignTreeRouting, DesignTreeRoutingIdProperty);

        /// <summary>
        /// 产品工艺路线设置
        /// </summary>
        public DesignTreeRouting DesignTreeRouting
        {
            get { return this.GetRefEntity(DesignTreeRoutingProperty); }
            set { this.SetRefEntity(DesignTreeRoutingProperty, value); }
        }
        #endregion

        #region 项目参数 ProjectParam
        /// <summary>
        /// 项目参数Id
        /// </summary>
        [Label("项目参数")]
        public static readonly IRefIdProperty ProjectParamIdProperty =
            P<DesignTreeRoutingParamer>.RegisterRefId(e => e.ProjectParamId, ReferenceType.Normal);

        /// <summary>
        /// 项目参数Id
        /// </summary>
        public double ProjectParamId
        {
            get { return (double)this.GetRefId(ProjectParamIdProperty); }
            set { this.SetRefId(ProjectParamIdProperty, value); }
        }

        /// <summary>
        /// 项目参数
        /// </summary>
        public static readonly RefEntityProperty<ProjectParam> ProjectParamProperty =
            P<DesignTreeRoutingParamer>.RegisterRef(e => e.ProjectParam, ProjectParamIdProperty);

        /// <summary>
        /// 项目参数
        /// </summary>
        public ProjectParam ProjectParam
        {
            get { return this.GetRefEntity(ProjectParamProperty); }
            set { this.SetRefEntity(ProjectParamProperty, value); }
        }
        #endregion

        #region 参数值类型 ProcessStDtlValueType
        /// <summary>
        /// 参数值类型
        /// </summary>
        [Label("参数值类型")]
        public static readonly Property<ProcessStDtlValueType> ProcessStDtlValueTypeProperty = P<DesignTreeRoutingParamer>.Register(e => e.ProcessStDtlValueType);

        /// <summary>
        /// 参数值类型
        /// </summary>
        public ProcessStDtlValueType ProcessStDtlValueType
        {
            get { return this.GetProperty(ProcessStDtlValueTypeProperty); }
            set { this.SetProperty(ProcessStDtlValueTypeProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty =
            P<DesignTreeRoutingParamer>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

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
            P<DesignTreeRoutingParamer>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return this.GetRefEntity(ProcessProperty); }
            set { this.SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 单位 Unit
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitProperty = P<DesignTreeRoutingParamer>.Register(e => e.Unit);

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit
        {
            get { return this.GetProperty(UnitProperty); }
            set { this.SetProperty(UnitProperty, value); }
        }
        #endregion

        #region 标准值 SingleValue
        /// <summary>
        /// 标准值
        /// </summary>
        [Label("标准值")]
        public static readonly Property<string> SingleValueProperty = P<DesignTreeRoutingParamer>.Register(e => e.SingleValue);

        /// <summary>
        /// 标准值
        /// </summary>
        public string SingleValue
        {
            get { return this.GetProperty(SingleValueProperty); }
            set { this.SetProperty(SingleValueProperty, value); }
        }
        #endregion

        #region 上限值 RangeMaxValue
        /// <summary>
        /// 上限值
        /// </summary>
        [Label("上限值")]
        public static readonly Property<decimal?> RangeMaxValueProperty = P<DesignTreeRoutingParamer>.Register(e => e.RangeMaxValue);

        /// <summary>
        /// 上限值
        /// </summary>
        public decimal? RangeMaxValue
        {
            get { return this.GetProperty(RangeMaxValueProperty); }
            set { this.SetProperty(RangeMaxValueProperty, value); }
        }
        #endregion

        #region 下限值 RangeMinValue
        /// <summary>
        /// 下限值
        /// </summary>
        [Label("下限值")]
        public static readonly Property<decimal?> RangeMinValueProperty = P<DesignTreeRoutingParamer>.Register(e => e.RangeMinValue);

        /// <summary>
        /// 下限值
        /// </summary>
        public decimal? RangeMinValue
        {
            get { return this.GetProperty(RangeMinValueProperty); }
            set { this.SetProperty(RangeMinValueProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 项目参数编码 ProjectParamCode
        /// <summary>
        /// 项目参数编码
        /// </summary>
        [Label("项目参数编码")]
        public static readonly Property<string> ProjectParamCodeProperty = P<DesignTreeRoutingParamer>.RegisterView(e => e.ProjectParamCode, p => p.ProjectParam.Code);

        /// <summary>
        /// 项目参数编码
        /// </summary>
        public string ProjectParamCode
        {
            get { return this.GetProperty(ProjectParamCodeProperty); }
        }
        #endregion

        #region 项目参数名称 ProjectParamName
        /// <summary>
        /// 项目参数名称
        /// </summary>
        [Label("项目参数名称")]
        public static readonly Property<string> ProjectParamNameProperty = P<DesignTreeRoutingParamer>.RegisterView(e => e.ProjectParamName, p => p.ProjectParam.Name);

        /// <summary>
        /// 项目参数名称
        /// </summary>
        public string ProjectParamName
        {
            get { return this.GetProperty(ProjectParamNameProperty); }
        }
        #endregion

        #region 项目参数类型 ProjectParamType
        /// <summary>
        /// 项目参数类型
        /// </summary>
        [Label("项目参数类型")]
        public static readonly Property<string> ProjectParamTypeProperty = P<DesignTreeRoutingParamer>.RegisterView(e => e.ProjectParamType, p => p.ProjectParam.Type);

        /// <summary>
        /// 项目参数类型
        /// </summary>
        public string ProjectParamType
        {
            get { return this.GetProperty(ProjectParamTypeProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 实体配置
    /// </summary>
    public class DesignTreeRoutingParamerConfig : EntityConfig<DesignTreeRoutingParamer>
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("MES_PRODES_ROUPARAM").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
