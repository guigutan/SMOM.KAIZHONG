using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources.ProcessTechs.Enums;
using SIE.Resources.ProcessTechTypes;
using System;

namespace SIE.Resources.ProcessTechs
{
    /// <summary>
    /// 制程工艺查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("制程工艺查询")]
    public class ProcessTechCriteria : Criteria
    {
        #region 制程编号 Code
        /// <summary>
        /// 制程编号
        /// </summary>
        [Label("制程编号")]
        public static readonly Property<string> CodeProperty = P<ProcessTechCriteria>.Register(e => e.Code);

        /// <summary>
        /// 制程编号
        /// </summary>
        public string Code
        {
            get { return this.GetProperty(CodeProperty); }
            set { this.SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 制程名称 Name
        /// <summary>
        /// 制程名称
        /// </summary>
        [Label("制程名称")]
        public static readonly Property<string> NameProperty = P<ProcessTechCriteria>.Register(e => e.Name);

        /// <summary>
        /// 制程名称
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
            set { this.SetProperty(NameProperty, value); }
        }
        #endregion

        #region 制程工艺类型 ProcessTechType
        /// <summary>
        /// 制程工艺类型Id
        /// </summary>
        [Label("制程类型")]
        public static readonly IRefIdProperty ProcessTechTypeIdProperty = P<ProcessTechCriteria>.RegisterRefId(e => e.ProcessTechTypeId, ReferenceType.Normal);

        /// <summary>
        /// 制程工艺类型Id
        /// </summary>
        public double? ProcessTechTypeId
        {
            get { return (double?)GetRefNullableId(ProcessTechTypeIdProperty); }
            set { SetRefNullableId(ProcessTechTypeIdProperty, value); }
        }

        /// <summary>
        /// 制程工艺类型
        /// </summary>
        public static readonly RefEntityProperty<ProcessTechType> ProcessTechTypeProperty = P<ProcessTechCriteria>.RegisterRef(e => e.ProcessTechType, ProcessTechTypeIdProperty);

        /// <summary>
        /// 制程工艺类型
        /// </summary>
        public ProcessTechType ProcessTechType
        {
            get { return GetRefEntity(ProcessTechTypeProperty); }
            set { SetRefEntity(ProcessTechTypeProperty, value); }
        }
        #endregion

        #region 是否排产 ProcessTechState
        /// <summary>
        /// 是否排产
        /// </summary>
        [Label("是否排产")]
        public static readonly Property<ProcessTechState?> ProcessTechStateProperty = P<ProcessTechCriteria>.Register(e => e.ProcessTechState);

        /// <summary>
        /// 是否排产
        /// </summary>
        public ProcessTechState? ProcessTechState
        {
            get { return this.GetProperty(ProcessTechStateProperty); }
            set { this.SetProperty(ProcessTechStateProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <returns>制程工艺列表</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<ProcessTechBaseController>().GetProcessTechList(this);
        }
    }
}
