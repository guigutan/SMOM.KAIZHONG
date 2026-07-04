using SIE.Common;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Inventory.Transactions
{
    /// <summary>
    /// 单据小类
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("单据小类")]
    [DisplayMember(nameof(Name))]
    public partial class Transaction : DataEntity, IStateEntity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Transaction()
        {
            State = State.Enable;
        }

        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<Transaction>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<Transaction>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 描述 Description
        /// <summary>
        /// 描述
        /// </summary>
        [Label("描述")]
        public static readonly Property<string> DescriptionProperty = P<Transaction>.Register(e => e.Description);

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return this.GetProperty(DescriptionProperty); }
            set { this.SetProperty(DescriptionProperty, value); }
        }
        #endregion

        #region 是否内部使用 IsInternalUse
        /// <summary>
        /// 是否内部使用
        /// </summary>
        [Label("是否内部使用")]
        public static readonly Property<bool> IsInternalUseProperty = P<Transaction>.Register(e => e.IsInternalUse);

        /// <summary>
        /// 是否内部使用
        /// </summary>
        public bool IsInternalUse
        {
            get { return GetProperty(IsInternalUseProperty); }
            set { SetProperty(IsInternalUseProperty, value); }
        }
        #endregion

        #region MES流程名 MesProcessName
        /// <summary>
        /// MES流程名
        /// </summary>
        [Label("MES流程名")]
        public static readonly Property<string> MesProcessNameProperty = P<Transaction>.Register(e => e.MesProcessName);

        /// <summary>
        /// MES流程名
        /// </summary>
        public string MesProcessName
        {
            get { return GetProperty(MesProcessNameProperty); }
            set { SetProperty(MesProcessNameProperty, value); }
        }
        #endregion

        #region RFC流程名 RfcProcessName
        /// <summary>
        /// RFC流程名
        /// </summary>
        [Label("RFC流程名")]
        public static readonly Property<string> RfcProcessNameProperty = P<Transaction>.Register(e => e.RfcProcessName);

        /// <summary>
        /// RFC流程名
        /// </summary>
        public string RfcProcessName
        {
            get { return GetProperty(RfcProcessNameProperty); }
            set { SetProperty(RfcProcessNameProperty, value); }
        }
        #endregion

        #region 是否整单上传 IsUpload
        /// <summary>
        /// 是否整单上传
        /// </summary>
        [Label("是否整单上传")]
        public static readonly Property<bool> IsUploadProperty = P<Transaction>.Register(e => e.IsUpload);

        /// <summary>
        /// 是否整单上传
        /// </summary>
        public bool IsUpload
        {
            get { return GetProperty(IsUploadProperty); }
            set { SetProperty(IsUploadProperty, value); }
        }
        #endregion

        #region 归类 SortOut
        /// <summary>
        /// 归类
        /// </summary>
        [Label("分类")]
        public static readonly Property<SortOut> SortOutProperty = P<Transaction>.Register(e => e.SortOut);

        /// <summary>
        /// 归类
        /// </summary>
        public SortOut SortOut
        {
            get { return GetProperty(SortOutProperty); }
            set { SetProperty(SortOutProperty, value); }
        }
        #endregion

        #region 数据来源类型 SourceType
        /// <summary>
        /// 数据来源类型
        /// </summary>
        [Label("数据来源类型")]
        public static readonly Property<SourceType> SourceTypeProperty = P<Transaction>.Register(e => e.SourceType);

        /// <summary>
        /// 数据来源类型
        /// </summary>
        public SourceType SourceType
        {
            get { return GetProperty(SourceTypeProperty); }
            set { SetProperty(SourceTypeProperty, value); }
        }
        #endregion

        #region 禁止修改 IsEdit
        /// <summary>
        /// 禁止修改
        /// </summary>
        [Label("禁止修改")]
        public static readonly Property<bool> IsEditProperty = P<Transaction>.Register(e => e.IsEdit);

        /// <summary>
        /// 禁止修改
        /// </summary>
        public bool IsEdit
        {
            get { return GetProperty(IsEditProperty); }
            set { SetProperty(IsEditProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<State> StateProperty = P<Transaction>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public State State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 单据小类 实体配置
    /// </summary>
    internal class TransactionConfig : EntityConfig<Transaction>
    {
        /// <summary>
        /// 属性元数据配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("TRANS_TRANSACTION").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}