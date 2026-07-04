using SIE.Domain;
using SIE.MES.Projects.Enums;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ProjectDesigns.ViewModels
{
    /// <summary>
    /// 项目号需求设计-工艺卡参数
    /// </summary>
    [RootEntity, Serializable]
    [Label("项目号需求设计-工艺卡参数")]
    public class ProjectDesignCardParamter : ViewModel
    {
        #region 参数编码 ParameterCode
        /// <summary>
        /// 参数编码
        /// </summary>
        [Label("参数编码")]
        public static readonly Property<string> ParameterCodeProperty = P<ProjectDesignCardParamter>.Register(e => e.ParameterCode);

        /// <summary>
        /// 参数编码
        /// </summary>
        public string ParameterCode
        {
            get { return this.GetProperty(ParameterCodeProperty); }
            set { this.SetProperty(ParameterCodeProperty, value); }
        }
        #endregion

        #region 参数名称 ParameterName
        /// <summary>
        /// 参数名称
        /// </summary>
        [Label("参数名称")]
        public static readonly Property<string> ParameterNameProperty = P<ProjectDesignCardParamter>.Register(e => e.ParameterName);

        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParameterName
        {
            get { return this.GetProperty(ParameterNameProperty); }
            set { this.SetProperty(ParameterNameProperty, value); }
        }
        #endregion

        #region 参数类型 ParameterType
        /// <summary>
        /// 参数类型
        /// </summary>
        [Label("参数类型")]
        public static readonly Property<string> ParameterTypeProperty = P<ProjectDesignCardParamter>.Register(e => e.ParameterType);

        /// <summary>
        /// 参数类型
        /// </summary>
        public string ParameterType
        {
            get { return this.GetProperty(ParameterTypeProperty); }
            set { this.SetProperty(ParameterTypeProperty, value); }
        }
        #endregion

        #region 参数值类型 ProcessStDtlValueType
        /// <summary>
        /// 参数值类型
        /// </summary>
        [Label("参数值类型")]
        public static readonly Property<ProcessStDtlValueType> ProcessStDtlValueTypeProperty = P<ProjectDesignCardParamter>.Register(e => e.ProcessStDtlValueType);

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
        public static readonly Property<string> UnitProperty = P<ProjectDesignCardParamter>.Register(e => e.Unit);

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
        public static readonly Property<string> SingleValueProperty = P<ProjectDesignCardParamter>.Register(e => e.SingleValue);

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
        public static readonly Property<string> RangeMaxValueProperty = P<ProjectDesignCardParamter>.Register(e => e.RangeMaxValue);

        /// <summary>
        /// 上限值
        /// </summary>
        public string RangeMaxValue
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
        public static readonly Property<string> RangeMinValueProperty = P<ProjectDesignCardParamter>.Register(e => e.RangeMinValue);

        /// <summary>
        /// 下限值
        /// </summary>
        public string RangeMinValue
        {
            get { return this.GetProperty(RangeMinValueProperty); }
            set { this.SetProperty(RangeMinValueProperty, value); }
        }
        #endregion

    }
}
