using SIE.Domain;
using SIE.Items;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using System;

namespace SIE.AbnormalInfo.AbnormalInfos
{
    /// <summary>
    /// 异常信息查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("异常信息查询实体")]
    public partial class AbnormalInforCriteria : Criteria
    {
        #region 异常单号 No
        /// <summary>
        /// 异常单号
        /// </summary>
        [Label("异常单号")]
        public static readonly Property<string> NoProperty = P<AbnormalInforCriteria>.Register(e => e.No);

        /// <summary>
        /// 异常单号
        /// </summary>
        public string No
        {
            get { return GetProperty(NoProperty); }
            set { SetProperty(NoProperty, value); }
        }
        #endregion

        #region 产品 Item
        /// <summary>
        /// 产品Id
        /// </summary>
        [Label("产品编码")]
        public static readonly IRefIdProperty ItemIdProperty = P<AbnormalInforCriteria>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 产品Id
        /// </summary>
        public double? ItemId
        {
            get { return (double?)GetRefNullableId(ItemIdProperty); }
            set { SetRefNullableId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 产品
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty = P<AbnormalInforCriteria>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 产品
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 异常信息定义 AbnormalInfoDefinition
        /// <summary>
        /// 异常信息定义Id
        /// </summary>
        [Label("异常信息")]
        public static readonly IRefIdProperty AbnormalInfoDefinitionIdProperty = P<AbnormalInforCriteria>.RegisterRefId(e => e.AbnormalInfoDefinitionId, ReferenceType.Normal);

        /// <summary>
        /// 异常信息定义Id
        /// </summary>
        public double? AbnormalInfoDefinitionId
        {
            get { return (double?)GetRefNullableId(AbnormalInfoDefinitionIdProperty); }
            set { SetRefNullableId(AbnormalInfoDefinitionIdProperty, value); }
        }

        /// <summary>
        /// 异常信息定义
        /// </summary>
        public static readonly RefEntityProperty<AbnormalInfoDefinition> AbnormalInfoDefinitionProperty = P<AbnormalInforCriteria>.RegisterRef(e => e.AbnormalInfoDefinition, AbnormalInfoDefinitionIdProperty);

        /// <summary>
        /// 异常信息定义
        /// </summary>
        public AbnormalInfoDefinition AbnormalInfoDefinition
        {
            get { return GetRefEntity(AbnormalInfoDefinitionProperty); }
            set { SetRefEntity(AbnormalInfoDefinitionProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty = P<AbnormalInforCriteria>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double? ProcessId
        {
            get { return (double?)GetRefNullableId(ProcessIdProperty); }
            set { SetRefNullableId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty = P<AbnormalInforCriteria>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 产线 Line
        /// <summary>
        /// 产线Id
        /// </summary>
        [Label("产线")]
        public static readonly IRefIdProperty LineIdProperty = P<AbnormalInforCriteria>.RegisterRefId(e => e.LineId, ReferenceType.Normal);

        /// <summary>
        /// 产线Id
        /// </summary>
        public double? LineId
        {
            get { return (double?)GetRefNullableId(LineIdProperty); }
            set { SetRefNullableId(LineIdProperty, value); }
        }

        /// <summary>
        /// 产线
        /// </summary>
        public static readonly RefEntityProperty<WipResource> LineProperty = P<AbnormalInforCriteria>.RegisterRef(e => e.Line, LineIdProperty);

        /// <summary>
        /// 产线
        /// </summary>
        public WipResource Line
        {
            get { return GetRefEntity(LineProperty); }
            set { SetRefEntity(LineProperty, value); }
        }
        #endregion

        #region 车间 WorkShop
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty WorkShopIdProperty =
            P<AbnormalInforCriteria>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double? WorkShopId
        {
            get { return (double?)this.GetRefNullableId(WorkShopIdProperty); }
            set { this.SetRefNullableId(WorkShopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> WorkShopProperty =
            P<AbnormalInforCriteria>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise WorkShop
        {
            get { return this.GetRefEntity(WorkShopProperty); }
            set { this.SetRefEntity(WorkShopProperty, value); }
        }
        #endregion

        #region 异常状态 AbnormalStatus
        /// <summary>
        /// 异常状态
        /// </summary>
        [Label("异常状态")]
        public static readonly Property<AbnormalStatus?> AbnormalStatusProperty = P<AbnormalInforCriteria>.Register(e => e.AbnormalStatus);

        /// <summary>
        /// 异常状态
        /// </summary>
        public AbnormalStatus? AbnormalStatus
        {
            get { return GetProperty(AbnormalStatusProperty); }
            set { SetProperty(AbnormalStatusProperty, value); }
        }
        #endregion

        #region 创建时间 CreateDate
        /// <summary>
        /// 创建时间
        /// </summary>
        [Label("创建时间")]
        public static readonly Property<DateRange> CreateDateProperty = P<AbnormalInforCriteria>.Register(e => e.CreateDate);

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateRange CreateDate
        {
            get { return GetProperty(CreateDateProperty); }
            set { SetProperty(CreateDateProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询来料检验单
        /// </summary>
        /// <returns>来料检验单列表</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<AbnormalInfoController>().QueryAbnormalInfos(this);
        }
    }
}
