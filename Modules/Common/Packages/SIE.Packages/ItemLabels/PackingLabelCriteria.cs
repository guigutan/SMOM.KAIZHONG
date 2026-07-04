using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.Packages.ItemLabels
{
    /// <summary>
    /// 包装查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("标签查询")]
    public partial class PackingLabelCriteria : Criteria
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public PackingLabelCriteria()
        {
            CreateDate = new DateRange();
            CreateDate.DateTimePart = DateTimePart.DateTime;  //选择日期格式为天
            CreateDate.DateRangeType = DateRangeType.LastMonth;  //默认日期为最近一月
        }
        #endregion

        #region 上级条码 PackageNo
        /// <summary>
        /// 上级条码
        /// </summary>
        [Label("上级条码")]
        public static readonly Property<string> PackageNoProperty = P<PackingLabelCriteria>.Register(e => e.PackageNo);

        /// <summary>
        /// 上级条码
        /// </summary>
        public string PackageNo
        {
            get { return GetProperty(PackageNoProperty); }
            set { SetProperty(PackageNoProperty, value); }
        }
        #endregion

        #region 条码 No
        /// <summary>
        /// 条码
        /// </summary>
        [Label("条码")]
        public static readonly Property<string> NoProperty = P<PackingLabelCriteria>.Register(e => e.No);

        /// <summary>
        /// 条码
        /// </summary>
        public string No
        {
            get { return GetProperty(NoProperty); }
            set { SetProperty(NoProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<PackingLabelCriteria>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return GetProperty(ItemCodeProperty); }
            set { SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<PackingLabelCriteria>.Register(e => e.ItemName);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return GetProperty(ItemNameProperty); }
            set { SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 批次 LotCode
        /// <summary>
        /// 批次
        /// </summary>
        [Label("批次")]
        public static readonly Property<string> LotCodeProperty = P<PackingLabelCriteria>.Register(e => e.LotCode);

        /// <summary>
        /// 批次
        /// </summary>
        public string LotCode
        {
            get { return GetProperty(LotCodeProperty); }
            set { SetProperty(LotCodeProperty, value); }
        }
        #endregion

        #region 创建时间 CreateDate
        /// <summary>
        /// 创建时间
        /// </summary>
        [Label("创建时间")]
        public static readonly Property<DateRange> CreateDateProperty = P<PackingLabelCriteria>.Register(e => e.CreateDate);

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateRange CreateDate
        {
            get { return GetProperty(CreateDateProperty); }
            set { SetProperty(CreateDateProperty, value); }
        }
        #endregion

        #region 创建人 CreateBy
        /// <summary>
        /// 创建人Id
        /// </summary>
        [Label("创建人")]
        public static readonly IRefIdProperty CreateByIdProperty = P<PackingLabelCriteria>.RegisterRefId(e => e.CreateById, ReferenceType.Normal);

        /// <summary>
        /// 创建人Id
        /// </summary>
        public double? CreateById
        {
            get { return (double?)GetRefNullableId(CreateByIdProperty); }
            set { SetRefNullableId(CreateByIdProperty, value); }
        }

        /// <summary>
        /// 创建人
        /// </summary>
        public static readonly RefEntityProperty<Employee> CreateByProperty = P<PackingLabelCriteria>.RegisterRef(e => e.CreateBy, CreateByIdProperty);

        /// <summary>
        /// 创建人
        /// </summary>
        public Employee CreateBy
        {
            get { return GetRefEntity(CreateByProperty); }
            set { SetRefEntity(CreateByProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询逻辑
        /// </summary>
        /// <returns>返回查询后的数据</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<PackingLabelController>().GetPackingLabels(this);
        }
    }
}
