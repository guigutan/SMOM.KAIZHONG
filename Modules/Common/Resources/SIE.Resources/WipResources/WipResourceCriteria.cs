using SIE.Diagnostics;
using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources.CalendarSchemes;
using SIE.Resources.Enterprises;
using SIE.Resources.ProcessTechTypes;
using System;
using System.Collections.Generic;

namespace SIE.Resources.WipResources
{
    /// <summary>
    /// 生产资源查询实体
    /// </summary>
    ////[RootEntity, Serializable]
    [QueryEntity, Serializable]
    [Label("生产资源查询")]
    public class WipResourceCriteria : Criteria
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WipResourceCriteria()
        {
            IsInvalid = false;
        }

        #region 资源编码 No
        /// <summary>
        /// 资源编码
        /// </summary>
        [Label("资源编号")]
        public static readonly Property<string> NoProperty = P<WipResourceCriteria>.Register(e => e.No);

        /// <summary>
        /// 资源编码
        /// </summary>
        public string No
        {
            get { return this.GetProperty(NoProperty); }
            set { this.SetProperty(NoProperty, value); }
        }
        #endregion

        #region 资源名称 Name
        /// <summary>
        /// 资源名称
        /// </summary>
        [Label("资源名称")]
        public static readonly Property<string> NameProperty = P<WipResourceCriteria>.Register(e => e.Name);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
            set { this.SetProperty(NameProperty, value); }
        }
        #endregion

        #region 来源类型 SourceType
        /// <summary>
        /// 来源类型
        /// </summary>
        [Label("来源类型")]
        public static readonly Property<SyncSourceType?> SourceTypeProperty = P<WipResourceCriteria>.Register(e => e.SourceType);

        /// <summary>
        /// 来源类型
        /// </summary>
        public SyncSourceType? SourceType
        {
            get { return this.GetProperty(SourceTypeProperty); }
            set { this.SetProperty(SourceTypeProperty, value); }
        }
        #endregion

        #region 启用状态 State
        /// <summary>
        /// 启用状态
        /// </summary>
        [Label("启用状态")]
        public static readonly Property<ResourceState?> StateProperty = P<WipResourceCriteria>.Register(e => e.State);

        /// <summary>
        /// 启用状态
        /// </summary>
        public ResourceState? State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        #region 制程工艺类型 ProcessTechType
        /// <summary>
        /// 制程工艺类型Id
        /// </summary>
        [Label("制程类型")]
        public static readonly IRefIdProperty ProcessTechTypeIdProperty = P<WipResourceCriteria>.RegisterRefId(e => e.ProcessTechTypeId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<ProcessTechType> ProcessTechTypeProperty = P<WipResourceCriteria>.RegisterRef(e => e.ProcessTechType, ProcessTechTypeIdProperty);

        /// <summary>
        /// 制程工艺类型
        /// </summary>
        public ProcessTechType ProcessTechType
        {
            get { return GetRefEntity(ProcessTechTypeProperty); }
            set { SetRefEntity(ProcessTechTypeProperty, value); }
        }
        #endregion

        #region 日历方案 CalendarScheme
        /// <summary>
        /// 日历方案Id
        /// </summary>
        [Label("日历方案")]
        public static readonly IRefIdProperty CalendarSchemeIdProperty =
            P<WipResourceCriteria>.RegisterRefId(e => e.CalendarSchemeId, ReferenceType.Normal);

        /// <summary>
        /// 日历方案Id
        /// </summary>
        public double? CalendarSchemeId
        {
            get { return (double?)this.GetRefNullableId(CalendarSchemeIdProperty); }
            set { this.SetRefNullableId(CalendarSchemeIdProperty, value); }
        }

        /// <summary>
        /// 日历方案
        /// </summary>
        public static readonly RefEntityProperty<CalendarScheme> CalendarProperty =
            P<WipResourceCriteria>.RegisterRef(e => e.CalendarScheme, CalendarSchemeIdProperty);

        /// <summary>
        /// 日历方案
        /// </summary>
        public CalendarScheme CalendarScheme
        {
            get { return this.GetRefEntity(CalendarProperty); }
            set { this.SetRefEntity(CalendarProperty, value); }
        }
        #endregion 

        #region 所属车间 WorkShop
        /// <summary>
        /// 所属车间Id
        /// </summary>
        [Label("所属车间")]
        public static readonly IRefIdProperty WorkShopIdProperty = P<WipResourceCriteria>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

        /// <summary>
        /// 所属车间Id
        /// </summary>
        public double? WorkShopId
        {
            get { return (double?)GetRefNullableId(WorkShopIdProperty); }
            set { SetRefNullableId(WorkShopIdProperty, value); }
        }

        /// <summary>
        /// 所属车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> WorkShopProperty = P<WipResourceCriteria>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

        /// <summary>
        /// 所属车间
        /// </summary>
        public Enterprise WorkShop
        {
            get { return GetRefEntity(WorkShopProperty); }
            set { SetRefEntity(WorkShopProperty, value); }
        }
        #endregion

        #region 所属工厂 Factory
        /// <summary>
        /// 所属工厂Id
        /// </summary>
        [Label("所属工厂")]
        public static readonly IRefIdProperty FactoryIdProperty = P<WipResourceCriteria>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

        /// <summary>
        /// 所属工厂Id
        /// </summary>
        public double? FactoryId
        {
            get { return (double?)GetRefNullableId(FactoryIdProperty); }
            set { SetRefNullableId(FactoryIdProperty, value); }
        }

        /// <summary>
        /// 所属工厂
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> FactoryProperty = P<WipResourceCriteria>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 所属工厂
        /// </summary>
        public Enterprise Factory
        {
            get { return GetRefEntity(FactoryProperty); }
            set { SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 是否过滤无效资源 IsInvalid
        /// <summary>
        /// 是否过滤无效资源
        /// </summary>
        [Label("是否过滤无效资源")]
        public static readonly Property<bool> IsInvalidProperty = P<WipResourceCriteria>.Register(e => e.IsInvalid);

        /// <summary>
        /// 是否过滤无效资源
        /// </summary>
        public bool IsInvalid
        {
            get { return GetProperty(IsInvalidProperty); }
            set { SetProperty(IsInvalidProperty, value); }
        }
        #endregion

        #region 排除Id FilterId
        /// <summary>
        /// 排除Id
        /// </summary>
        public static readonly Property<List<double>> FilterIdProperty = P<WipResourceCriteria>.Register(e => e.FilterId);

        /// <summary>
        /// 排除Id
        /// </summary>
        public List<double> FilterId
        {
            get { return this.GetProperty(FilterIdProperty); }
            set { this.SetProperty(FilterIdProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <returns>计划资源列表</returns>
        protected override EntityList Fetch()
        {
            using (DebugTrace.Start("获取生产资源".L10N()))
            {
                return RT.Service.Resolve<WipResourceController>().GetSchedResources(this);
            }
        }
    }
}