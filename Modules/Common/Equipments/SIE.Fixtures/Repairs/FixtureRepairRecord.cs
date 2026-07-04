using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Fixtures.Repairs
{
    /// <summary>
	/// 维修记录
	/// </summary>
	[ChildEntity, Serializable]
    //[CriteriaQuery]
    [Label("维修记录")]
    public partial class FixtureRepairRecord : DataEntity
    {
        #region 备件编码 Code
        /// <summary>
        /// 备件编码
        /// </summary>
        [Label("备件编码")]
        public static readonly Property<string> CodeProperty = P<FixtureRepairRecord>.Register(e => e.Code);

        /// <summary>
        /// 备件编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 备件名称 Name
        /// <summary>
        /// 备件名称
        /// </summary>
        [Label("备件名称")]
        public static readonly Property<string> NameProperty = P<FixtureRepairRecord>.Register(e => e.Name);

        /// <summary>
        /// 备件名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 备件数量 Qty
        /// <summary>
        /// 备件数量
        /// </summary>
        [Label("备件数量")]
        public static readonly Property<int> QtyProperty = P<FixtureRepairRecord>.Register(e => e.Qty);

        /// <summary>
        /// 备件数量
        /// </summary>
        public int Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 更换说明 Description
        /// <summary>
        /// 更换说明
        /// </summary>
        [Label("更换说明")]
        public static readonly Property<string> DescriptionProperty = P<FixtureRepairRecord>.Register(e => e.Description);

        /// <summary>
        /// 更换说明
        /// </summary>
        public string Description
        {
            get { return GetProperty(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }
        #endregion

        #region 备件详情 FixtureRepairDetail
        /// <summary>
        /// 备件详情Id
        /// </summary>
        public static readonly IRefIdProperty FixtureRepairDetailIdProperty = P<FixtureRepairRecord>.RegisterRefId(e => e.FixtureRepairDetailId, ReferenceType.Parent);

        /// <summary>
        /// 备件详情Id
        /// </summary>
        public double FixtureRepairDetailId
        {
            get { return (double)GetRefId(FixtureRepairDetailIdProperty); }
            set { SetRefId(FixtureRepairDetailIdProperty, value); }
        }

        /// <summary>
        /// 备件详情
        /// </summary>
        public static readonly RefEntityProperty<FixtureRepairDetail> FixtureRepairDetailProperty = P<FixtureRepairRecord>.RegisterRef(e => e.FixtureRepairDetail, FixtureRepairDetailIdProperty);

        /// <summary>
        /// 备件详情
        /// </summary>
        public FixtureRepairDetail FixtureRepairDetail
        {
            get { return GetRefEntity(FixtureRepairDetailProperty); }
            set { SetRefEntity(FixtureRepairDetailProperty, value); }
        }
        #endregion

        #region 注册视图

        #region 工治具ID AccountCode
        /// <summary>
        /// 工治具ID
        /// </summary>
        [Label("工治具ID")]
        public static readonly Property<string> AccountCodeProperty = P<FixtureRepairRecord>.RegisterView(e => e.AccountCode, p => p.FixtureRepairDetail.FixtureAccount.Code);

        /// <summary>
        /// 工治具ID
        /// </summary>
        public string AccountCode
        {
            get { return this.GetProperty(AccountCodeProperty); }
        }

        #endregion

        #endregion
    }

    /// <summary>
    /// 维修记录 实体配置
    /// </summary>
    internal class FixtureRepairRecordConfig : EntityConfig<FixtureRepairRecord>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ELEC_FIX_REPAIR_REC").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
