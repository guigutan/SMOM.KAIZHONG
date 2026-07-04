using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Fixtures.Fixtures.Accounts
{
    /// <summary>
	/// feeder详情
	/// </summary>
	[ChildEntity, Serializable]
    [Label("feeder详情")]
    public partial class FixtureAccountTool : DataEntity
    {
        #region 标签编码 LabelCode
        /// <summary>
        /// 标签编码
        /// </summary>
        [Label("标签编码")]
        public static readonly Property<string> LabelCodeProperty = P<FixtureAccountTool>.Register(e => e.LabelCode);

        /// <summary>
        /// 标签编码
        /// </summary>
        public string LabelCode
        {
            get { return GetProperty(LabelCodeProperty); }
            set { SetProperty(LabelCodeProperty, value); }
        }
        #endregion

        #region 使用次数 UseNum
        /// <summary>
        /// 使用次数
        /// </summary>
        [Label("使用次数")]
        public static readonly Property<int> UseNumProperty = P<FixtureAccountTool>.Register(e => e.UseNum);

        /// <summary>
        /// 使用次数
        /// </summary>
        public int UseNum
        {
            get { return GetProperty(UseNumProperty); }
            set { SetProperty(UseNumProperty, value); }
        }
        #endregion

        #region 保养后使用次数 MaintainedUseNum
        /// <summary>
        /// 保养后使用次数
        /// </summary>
        [Label("保养后使用次数")]
        public static readonly Property<int> MaintainedUseNumProperty = P<FixtureAccountTool>.Register(e => e.MaintainedUseNum);

        /// <summary>
        /// 保养后使用次数
        /// </summary>
        public int MaintainedUseNum
        {
            get { return GetProperty(MaintainedUseNumProperty); }
            set { SetProperty(MaintainedUseNumProperty, value); }
        }
        #endregion

        #region 累计抛料数 TotalThrowQty
        /// <summary>
        /// 累计抛料数
        /// </summary>
        [Label("累计抛料数")]
        public static readonly Property<int> TotalThrowQtyProperty = P<FixtureAccountTool>.Register(e => e.TotalThrowQty);

        /// <summary>
        /// 累计抛料数
        /// </summary>
        public int TotalThrowQty
        {
            get { return GetProperty(TotalThrowQtyProperty); }
            set { SetProperty(TotalThrowQtyProperty, value); }
        }
        #endregion

        #region 保养后抛料数 MaintainedThrowQty
        /// <summary>
        /// 保养后抛料数
        /// </summary>
        [Label("保养后抛料数")]
        public static readonly Property<int> MaintainedThrowQtyProperty = P<FixtureAccountTool>.Register(e => e.MaintainedThrowQty);

        /// <summary>
        /// 保养后抛料数
        /// </summary>
        public int MaintainedThrowQty
        {
            get { return GetProperty(MaintainedThrowQtyProperty); }
            set { SetProperty(MaintainedThrowQtyProperty, value); }
        }
        #endregion

        #region 工治具台账 FixtureAccount
        /// <summary>
        /// 工治具台账Id
        /// </summary>
        public static readonly IRefIdProperty FixtureAccountIdProperty = P<FixtureAccountTool>.RegisterRefId(e => e.FixtureAccountId, ReferenceType.Parent);

        /// <summary>
        /// 工治具台账Id
        /// </summary>
        public double FixtureAccountId
        {
            get { return (double)GetRefId(FixtureAccountIdProperty); }
            set { SetRefId(FixtureAccountIdProperty, value); }
        }

        /// <summary>
        /// 工治具台账
        /// </summary>
        public static readonly RefEntityProperty<FixtureAccount> FixtureAccountProperty = P<FixtureAccountTool>.RegisterRef(e => e.FixtureAccount, FixtureAccountIdProperty);

        /// <summary>
        /// 工治具台账
        /// </summary>
        public FixtureAccount FixtureAccount
        {
            get { return GetRefEntity(FixtureAccountProperty); }
            set { SetRefEntity(FixtureAccountProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// feeder详情 实体配置
    /// </summary>
    internal class FixtureAccountToolConfig : EntityConfig<FixtureAccountTool>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_ACC_TOOL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
