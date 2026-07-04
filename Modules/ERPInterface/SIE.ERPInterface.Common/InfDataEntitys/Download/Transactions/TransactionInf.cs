using SIE.Domain;
using SIE.Inventory.Transactions;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.ERPInterface.Common.InfDataEntitys.Download
{
    /// <summary>
    /// 单据小类
    /// </summary>
    [RootEntity, Serializable]
    //[CriteriaQuery]
    [Label("单据小类")]
    public partial class TransactionInf : DownloadBaseEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<TransactionInf>.Register(e => e.Code);

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
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<TransactionInf>.Register(e => e.Name);

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
        [MaxLength(480)]
        [Label("描述")]
        public static readonly Property<string> DescriptionProperty = P<TransactionInf>.Register(e => e.Description);

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return GetProperty(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }
        #endregion

        #region 是否内部使用 IsInternalUse
        /// <summary>
        /// 是否内部使用
        /// </summary>
        [Label("是否内部使用")]
        public static readonly Property<bool> IsInternalUseProperty = P<TransactionInf>.Register(e => e.IsInternalUse);

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
        public static readonly Property<string> MesProcessNameProperty = P<TransactionInf>.Register(e => e.MesProcessName);

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
        public static readonly Property<string> RfcProcessNameProperty = P<TransactionInf>.Register(e => e.RfcProcessName);

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
        public static readonly Property<bool> IsUploadProperty = P<TransactionInf>.Register(e => e.IsUpload);

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
        [Label("归类")]
        public static readonly Property<SortOut> SortOutProperty = P<TransactionInf>.Register(e => e.SortOut);

        /// <summary>
        /// 归类
        /// </summary>
        public SortOut SortOut
        {
            get { return GetProperty(SortOutProperty); }
            set { SetProperty(SortOutProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 单据小类 实体配置
    /// </summary>
    internal class TransactionInfConfig : EntityConfig<TransactionInf>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("INF_TRANSACTION").MapAllProperties();
            Meta.Property(TransactionInf.DescriptionProperty).ColumnMeta.HasLength(960);
            Meta.EnablePhantoms();
        }
    }
}