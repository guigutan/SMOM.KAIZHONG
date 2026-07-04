using SIE.Domain;
using SIE.MES.TeamManagement.RatedItems;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.TeamManagement.ScoreRecords
{
    /// <summary>
    /// 处理记录
    /// </summary>
    [ChildEntity, Serializable]
    ////[CriteriaQuery]
    [Label("处理记录")]
    public partial class ProcessRecord : DataEntity
    {
        #region 调整前分值 OldValue
        /// <summary>
        /// 调整前分值
        /// </summary>
        [Label("调整前分值")]
        public static readonly Property<decimal> OldValueProperty = P<ProcessRecord>.Register(e => e.OldValue);

        /// <summary>
        /// 调整前分值
        /// </summary>
        public decimal OldValue
        {
            get { return GetProperty(OldValueProperty); }
            set { SetProperty(OldValueProperty, value); }
        }
        #endregion

        #region 调整后分值 NewValue
        /// <summary>
        /// 调整后分值
        /// </summary>
        [Label("调整后分值")]
        public static readonly Property<decimal> NewValueProperty = P<ProcessRecord>.Register(e => e.NewValue);

        /// <summary>
        /// 调整后分值
        /// </summary>
        public decimal NewValue
        {
            get { return GetProperty(NewValueProperty); }
            set { SetProperty(NewValueProperty, value); }
        }
        #endregion

        #region 调整后评分项目 NewRatedItem
        /// <summary>
        /// 调整后评分项目Id
        /// </summary>
        public static readonly IRefIdProperty NewRatedItemIdProperty = P<ProcessRecord>.RegisterRefId(e => e.NewRatedItemId, ReferenceType.Normal);

        /// <summary>
        /// 调整后评分项目Id
        /// </summary>
        public double NewRatedItemId
        {
            get { return (double)GetRefId(NewRatedItemIdProperty); }
            set { SetRefId(NewRatedItemIdProperty, value); }
        }

        /// <summary>
        /// 调整后评分项目
        /// </summary>
        public static readonly RefEntityProperty<RatedItem> NewRatedItemProperty = P<ProcessRecord>.RegisterRef(e => e.NewRatedItem, NewRatedItemIdProperty);

        /// <summary>
        /// 调整后评分项目
        /// </summary>
        public RatedItem NewRatedItem
        {
            get { return GetRefEntity(NewRatedItemProperty); }
            set { SetRefEntity(NewRatedItemProperty, value); }
        }
        #endregion

        #region 调整前评分项目 OldRatedItem
        /// <summary>
        /// 调整前评分项目Id
        /// </summary>
        public static readonly IRefIdProperty OldRatedItemIdProperty = P<ProcessRecord>.RegisterRefId(e => e.OldRatedItemId, ReferenceType.Normal);

        /// <summary>
        /// 调整前评分项目Id
        /// </summary>
        public double OldRatedItemId
        {
            get { return (double)GetRefId(OldRatedItemIdProperty); }
            set { SetRefId(OldRatedItemIdProperty, value); }
        }

        /// <summary>
        /// 调整前评分项目
        /// </summary>
        public static readonly RefEntityProperty<RatedItem> OldRatedItemProperty = P<ProcessRecord>.RegisterRef(e => e.OldRatedItem, OldRatedItemIdProperty);

        /// <summary>
        /// 调整前评分项目
        /// </summary>
        public RatedItem OldRatedItem
        {
            get { return GetRefEntity(OldRatedItemProperty); }
            set { SetRefEntity(OldRatedItemProperty, value); }
        }
        #endregion

        #region 处理记录列表 PetitionRecord
        /// <summary>
        /// 申诉记录Id
        /// </summary>
        [Label("申诉记录")]
        public static readonly IRefIdProperty PetitionRecordIdProperty = P<ProcessRecord>.RegisterRefId(e => e.PetitionRecordId, ReferenceType.Parent);

        /// <summary>
        /// 申诉记录Id
        /// </summary>
        public double PetitionRecordId
        {
            get { return (double)GetRefId(PetitionRecordIdProperty); }
            set { SetRefId(PetitionRecordIdProperty, value); }
        }

        /// <summary>
        /// 申诉记录
        /// </summary>
        public static readonly RefEntityProperty<PetitionRecord> PetitionRecordProperty = P<ProcessRecord>.RegisterRef(e => e.PetitionRecord, PetitionRecordIdProperty);

        /// <summary>
        /// 申诉记录
        /// </summary>
        public PetitionRecord PetitionRecord
        {
            get { return GetRefEntity(PetitionRecordProperty); }
            set { SetRefEntity(PetitionRecordProperty, value); }
        }
        #endregion

        #region 注册视图属性(关联实体属性平铺显示，一般用于Web)
        #region 调整前项目评分编码 OldRatedItemCode
        /// <summary>
        /// 调整前项目评分编码
        /// </summary>
        [Label("调整前项目评分编码")]
        public static readonly Property<string> OldRatedItemCodeProperty = P<ProcessRecord>.RegisterView(e => e.OldRatedItemCode, p => p.OldRatedItem.Code);

        /// <summary>
        /// 调整前项目评分编码
        /// </summary>
        public string OldRatedItemCode
        {
            get { return this.GetProperty(OldRatedItemCodeProperty); }
        }
        #endregion

        #region 调整前项目评分名称 OldRatedItemName
        /// <summary>
        /// 调整前项目评分名称
        /// </summary>
        [Label("调整前项目评分名称")]
        public static readonly Property<string> OldRatedItemNameProperty = P<ProcessRecord>.RegisterView(e => e.OldRatedItemName, p => p.OldRatedItem.Name);

        /// <summary>
        /// 调整前项目评分名称
        /// </summary>
        public string OldRatedItemName
        {
            get { return this.GetProperty(OldRatedItemNameProperty); }
        }
        #endregion

        #region 调整后项目评分编码 NewRatedItemCode
        /// <summary>
        /// 调整后项目评分编码
        /// </summary>
        [Label("调整后项目评分编码")]
        public static readonly Property<string> NewRatedItemCodeProperty = P<ProcessRecord>.RegisterView(e => e.NewRatedItemCode, p => p.NewRatedItem.Code);

        /// <summary>
        /// 调整后项目评分编码
        /// </summary>
        public string NewRatedItemCode
        {
            get { return this.GetProperty(NewRatedItemCodeProperty); }
        }
        #endregion

        #region 调整后项目评分名称 NewRatedItemName
        /// <summary>
        /// 调整后项目评分名称
        /// </summary>
        [Label("调整后项目评分名称")]
        public static readonly Property<string> NewRatedItemNameProperty = P<ProcessRecord>.RegisterView(e => e.NewRatedItemName, p => p.NewRatedItem.Name);

        /// <summary>
        /// 调整后项目评分名称
        /// </summary>
        public string NewRatedItemName
        {
            get { return this.GetProperty(NewRatedItemNameProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 处理记录 实体配置
    /// </summary>
    internal class ProcessRecordConfig : EntityConfig<ProcessRecord>
    {
        /// <summary>
        /// 实体配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("WG_PRO_RECORD").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}