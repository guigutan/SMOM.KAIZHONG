using SIE.Domain;
using SIE.MES.Projects.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Projects
{
    /// <summary>
    /// 工序标准参数明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("工序标准参数明细")]
    public class ProcessStandardParamDtl : DataEntity
    {
        #region 工序标准参数 ProcessStandardParam
        /// <summary>
        /// 工序标准参数Id
        /// </summary>
        [Label("工序标准参数")]
        public static readonly IRefIdProperty ProcessStandardParamIdProperty =
            P<ProcessStandardParamDtl>.RegisterRefId(e => e.ProcessStandardParamId, ReferenceType.Parent);

        /// <summary>
        /// 工序标准参数Id
        /// </summary>
        public double ProcessStandardParamId
        {
            get { return (double)this.GetRefId(ProcessStandardParamIdProperty); }
            set { this.SetRefId(ProcessStandardParamIdProperty, value); }
        }

        /// <summary>
        /// 工序标准参数
        /// </summary>
        public static readonly RefEntityProperty<ProcessStandardParam> ProcessStandardParamProperty =
            P<ProcessStandardParamDtl>.RegisterRef(e => e.ProcessStandardParam, ProcessStandardParamIdProperty);

        /// <summary>
        /// 工序标准参数
        /// </summary>
        public ProcessStandardParam ProcessStandardParam
        {
            get { return this.GetRefEntity(ProcessStandardParamProperty); }
            set { this.SetRefEntity(ProcessStandardParamProperty, value); }
        }
        #endregion

        #region 项目参数 ProjectParam
        /// <summary>
        /// 项目参数Id
        /// </summary>
        [Label("项目参数")]
        public static readonly IRefIdProperty ProjectParamIdProperty =
            P<ProcessStandardParamDtl>.RegisterRefId(e => e.ProjectParamId, ReferenceType.Normal);

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
            P<ProcessStandardParamDtl>.RegisterRef(e => e.ProjectParam, ProjectParamIdProperty);

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
        public static readonly Property<ProcessStDtlValueType> ProcessStDtlValueTypeProperty = P<ProcessStandardParamDtl>.Register(e => e.ProcessStDtlValueType);

        /// <summary>
        /// 参数值类型
        /// </summary>
        public ProcessStDtlValueType ProcessStDtlValueType
        {
            get { return this.GetProperty(ProcessStDtlValueTypeProperty); }
            set { this.SetProperty(ProcessStDtlValueTypeProperty, value); }
        }
        #endregion

        #region 单位 Unit
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitProperty = P<ProcessStandardParamDtl>.Register(e => e.Unit);

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
        public static readonly Property<string> SingleValueProperty = P<ProcessStandardParamDtl>.Register(e => e.SingleValue);

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
        public static readonly Property<decimal?> RangeMaxValueProperty = P<ProcessStandardParamDtl>.Register(e => e.RangeMaxValue);

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
        public static readonly Property<decimal?> RangeMinValueProperty = P<ProcessStandardParamDtl>.Register(e => e.RangeMinValue);

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
        public static readonly Property<string> ProjectParamCodeProperty = P<ProcessStandardParamDtl>.RegisterView(e => e.ProjectParamCode, p => p.ProjectParam.Code);

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
        public static readonly Property<string> ProjectParamNameProperty = P<ProcessStandardParamDtl>.RegisterView(e => e.ProjectParamName, p => p.ProjectParam.Name);

        /// <summary>
        /// 项目参数名称
        /// </summary>
        public string ProjectParamName
        {
            get { return this.GetProperty(ProjectParamNameProperty); }
        }
        #endregion

        #region 项目参数类型ProjectParamType ProjectParamType
        /// <summary>
        /// 项目参数类型ProjectParamType
        /// </summary>
        [Label("项目参数类型")]
        public static readonly Property<string> ProjectParamTypeProperty = P<ProcessStandardParamDtl>.RegisterView(e => e.ProjectParamType, p => p.ProjectParam.Type);

        /// <summary>
        /// 项目参数类型ProjectParamType
        /// </summary>
        public string ProjectParamType
        {
            get { return this.GetProperty(ProjectParamTypeProperty); }
        }
        #endregion

        #region 标准参数状态 ProcessStStatus
        /// <summary>
        /// 标准参数状态
        /// </summary>
        [Label("标准参数状态")]
        public static readonly Property<ProcessStStatus> ProcessStStatusProperty = P<ProcessStandardParamDtl>.RegisterView(e => e.ProcessStStatus, p => p.ProcessStandardParam.ProcessStStatus);

        /// <summary>
        /// 标准参数状态
        /// </summary>
        public ProcessStStatus ProcessStStatus
        {
            get { return this.GetProperty(ProcessStStatusProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 实体配置
    /// </summary>
    public class ProcessStandardParamDtlConfig : EntityConfig<ProcessStandardParamDtl>
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("MES_PROCESS_STPADTL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
