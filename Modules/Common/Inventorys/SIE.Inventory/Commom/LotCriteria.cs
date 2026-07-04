using SIE.Domain;
using SIE.Items;
using SIE.ObjectModel;
using System;

namespace SIE.Inventory.Commom
{
    /// <summary>
    /// 批次信息
    /// </summary>
    [QueryEntity, Serializable]
    [Label("批次信息查询实体")]
    public partial class LotCriteria : Criteria
    {
        #region 批次 Code
        /// <summary>
        /// 批次
        /// </summary>
        [Label("批次")]
        [MaxLength(80)]
        public static readonly Property<string> CodeProperty = P<LotCriteria>.Register(e => e.Code);

        /// <summary>
        /// 批次
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty = P<LotCriteria>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double? ItemId
        {
            get { return (double?)GetRefNullableId(ItemIdProperty); }
            set { SetRefNullableId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty = P<LotCriteria>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary> 
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 批次属性01 LotAtt01
        /// <summary>
        /// 批次属性01
        /// </summary>
        [Label("批次属性01")]
        public static readonly Property<DateTime?> LotAtt01Property = P<LotCriteria>.Register(e => e.LotAtt01);

        /// <summary>
        /// 批次属性01
        /// </summary>
        public DateTime? LotAtt01
        {
            get { return GetProperty(LotAtt01Property); }
            set { SetProperty(LotAtt01Property, value); }
        }
        #endregion

        #region 批次属性02 LotAtt02
        /// <summary>
        /// 批次属性02
        /// </summary>
        [Label("批次属性02")]
        public static readonly Property<DateTime?> LotAtt02Property = P<LotCriteria>.Register(e => e.LotAtt02);

        /// <summary>
        /// 批次属性02
        /// </summary>
        public DateTime? LotAtt02
        {
            get { return GetProperty(LotAtt02Property); }
            set { SetProperty(LotAtt02Property, value); }
        }
        #endregion

        #region 批次属性03 LotAtt03
        /// <summary>
        /// 批次属性03
        /// </summary>
        [Label("批次属性03")]
        public static readonly Property<DateTime?> LotAtt03Property = P<LotCriteria>.Register(e => e.LotAtt03);

        /// <summary>
        /// 批次属性03
        /// </summary>
        public DateTime? LotAtt03
        {
            get { return GetProperty(LotAtt03Property); }
            set { SetProperty(LotAtt03Property, value); }
        }
        #endregion

        #region 批次属性04 LotAtt04
        /// <summary>
        /// 批次属性04
        /// </summary>
        [MaxLength(80)]
        [Label("批次属性04")]
        public static readonly Property<string> LotAtt04Property = P<LotCriteria>.Register(e => e.LotAtt04);

        /// <summary>
        /// 批次属性04
        /// </summary>
        public string LotAtt04
        {
            get { return GetProperty(LotAtt04Property); }
            set { SetProperty(LotAtt04Property, value); }
        }
        #endregion

        #region 批次属性05 LotAtt05
        /// <summary>
        /// 批次属性05
        /// </summary>
        [Label("批次属性05")]
        public static readonly Property<decimal?> LotAtt05Property = P<LotCriteria>.Register(e => e.LotAtt05);

        /// <summary>
        /// 批次属性05
        /// </summary>
        public decimal? LotAtt05
        {
            get { return GetProperty(LotAtt05Property); }
            set { SetProperty(LotAtt05Property, value); }
        }
        #endregion

        #region 批次属性06 LotAtt06
        /// <summary>
        /// 批次属性06
        /// </summary>
        [Label("批次属性06")]
        public static readonly Property<decimal?> LotAtt06Property = P<LotCriteria>.Register(e => e.LotAtt06);

        /// <summary>
        /// 批次属性06
        /// </summary>
        public decimal? LotAtt06
        {
            get { return GetProperty(LotAtt06Property); }
            set { SetProperty(LotAtt06Property, value); }
        }
        #endregion

        #region 批次属性07 LotAtt07
        /// <summary>
        /// 批次属性07
        /// </summary>
        [Label("批次属性07")]
        [MaxLength(80)]
        public static readonly Property<string> LotAtt07Property = P<LotCriteria>.Register(e => e.LotAtt07);

        /// <summary>
        /// 批次属性07
        /// </summary>
        public string LotAtt07
        {
            get { return GetProperty(LotAtt07Property); }
            set { SetProperty(LotAtt07Property, value); }
        }
        #endregion

        #region 批次属性08 LotAtt08
        /// <summary>
        /// 批次属性08
        /// </summary>
        [Label("批次属性08")]
        [MaxLength(80)]
        public static readonly Property<string> LotAtt08Property = P<LotCriteria>.Register(e => e.LotAtt08);

        /// <summary>
        /// 批次属性08
        /// </summary>
        public string LotAtt08
        {
            get { return GetProperty(LotAtt08Property); }
            set { SetProperty(LotAtt08Property, value); }
        }
        #endregion

        #region 批次属性09 LotAtt09
        /// <summary>
        /// 批次属性09
        /// </summary>
        [Label("批次属性09")]
        [MaxLength(80)]
        public static readonly Property<string> LotAtt09Property = P<LotCriteria>.Register(e => e.LotAtt09);

        /// <summary>
        /// 批次属性09
        /// </summary>
        public string LotAtt09
        {
            get { return GetProperty(LotAtt09Property); }
            set { SetProperty(LotAtt09Property, value); }
        }
        #endregion

        #region 批次属性10 LotAtt10
        /// <summary>
        /// 批次属性10
        /// </summary>
        [Label("批次属性10")]
        [MaxLength(80)]
        public static readonly Property<string> LotAtt10Property = P<LotCriteria>.Register(e => e.LotAtt10);

        /// <summary>
        /// 批次属性10
        /// </summary>
        public string LotAtt10
        {
            get { return GetProperty(LotAtt10Property); }
            set { SetProperty(LotAtt10Property, value); }
        }
        #endregion

        #region 批次属性11 LotAtt11
        /// <summary>
        /// 批次属性11
        /// </summary>
        [Label("批次属性11")]
        public static readonly Property<DateTime?> LotAtt11Property = P<LotCriteria>.Register(e => e.LotAtt11);

        /// <summary>
        /// 批次属性11
        /// </summary>
        public DateTime? LotAtt11
        {
            get { return GetProperty(LotAtt11Property); }
            set { SetProperty(LotAtt11Property, value); }
        }
        #endregion

        #region 批次属性12 LotAtt12
        /// <summary>
        /// 批次属性12
        /// </summary>
        [Label("批次属性12")]
        public static readonly Property<DateTime?> LotAtt12Property = P<LotCriteria>.Register(e => e.LotAtt12);

        /// <summary>
        /// 批次属性12
        /// </summary>
        public DateTime? LotAtt12
        {
            get { return GetProperty(LotAtt12Property); }
            set { SetProperty(LotAtt12Property, value); }
        }
        #endregion

        #region Asn单号 AsnNo
        /// <summary>
        /// Asn单号
        /// </summary>
        [Label("Asn单号")]
        public static readonly Property<string> AsnNoProperty = P<LotCriteria>.Register(e => e.AsnNo);

        /// <summary>
        /// Asn单号
        /// </summary>
        public string AsnNo
        {
            get { return this.GetProperty(AsnNoProperty); }
            set { this.SetProperty(AsnNoProperty, value); }
        }
        #endregion

        #region 是否排除默认 IsNotDefault
        /// <summary>
        /// 是否排除默认
        /// </summary>
        [Label("是否排除默认")]
        public static readonly Property<bool> IsNotDefaultProperty = P<LotCriteria>.Register(e => e.IsNotDefault);

        /// <summary>
        /// 是否排除默认
        /// </summary>
        public bool IsNotDefault
        {
            get { return this.GetProperty(IsNotDefaultProperty); }
            set { this.SetProperty(IsNotDefaultProperty, value); }
        }
        #endregion

        /// <summary>
        /// 重写此方法实现查询
        /// </summary>
        /// <returns>返回结果</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<LotController>().GetLotData(this);
        }
    }
}
