using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Logs
{
    /// <summary>
    /// 离线数据上传记录
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]    
    [Label("离线数据上传记录")]
    public partial class EdoOutlineUploadLog : DataEntity
    {
        #region 单号 BillNo
        /// <summary>
        /// 单号
        /// </summary>
        [Label("单号")]
        public static readonly Property<string> BillNoProperty = P<EdoOutlineUploadLog>.Register(e => e.BillNo);

        /// <summary>
        /// 单号
        /// </summary>
        public string BillNo
        {
            get { return this.GetProperty(BillNoProperty); }
            set { this.SetProperty(BillNoProperty, value); }
        }
        #endregion

        #region 设备编码 MachineCode
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> MachineCodeProperty = P<EdoOutlineUploadLog>.Register(e => e.MachineCode);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string MachineCode
        {
            get { return this.GetProperty(MachineCodeProperty); }
            set { this.SetProperty(MachineCodeProperty, value); }
        }
        #endregion

        #region 设备名称 MachineName
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> MachineNameProperty = P<EdoOutlineUploadLog>.Register(e => e.MachineName);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string MachineName
        {
            get { return this.GetProperty(MachineNameProperty); }
            set { this.SetProperty(MachineNameProperty, value); }
        }
        #endregion

        #region 工治具编码 EncodeCode
        /// <summary>
        /// 工治具编码
        /// </summary>
        [Label("工治具编码")]
        public static readonly Property<string> EncodeCodeProperty = P<EdoOutlineUploadLog>.Register(e => e.EncodeCode);

        /// <summary>
        /// 工治具编码
        /// </summary>
        public string EncodeCode
        {
            get { return this.GetProperty(EncodeCodeProperty); }
            set { this.SetProperty(EncodeCodeProperty, value); }
        }
        #endregion

        #region 工治具名称 EncodeName
        /// <summary>
        /// 工治具名称
        /// </summary>
        [Label("工治具名称")]
        public static readonly Property<string> EncodeNameProperty = P<EdoOutlineUploadLog>.Register(e => e.EncodeName);

        /// <summary>
        /// 工治具名称
        /// </summary>
        public string EncodeName
        {
            get { return this.GetProperty(EncodeNameProperty); }
            set { this.SetProperty(EncodeNameProperty, value); }
        }
        #endregion

        #region 序列号 Sn
        /// <summary>
        /// 序列号
        /// </summary>
        [Label("序列号")]
        public static readonly Property<string> SnProperty = P<EdoOutlineUploadLog>.Register(e => e.Sn);

        /// <summary>
        /// 序列号
        /// </summary>
        public string Sn
        {
            get { return this.GetProperty(SnProperty); }
            set { this.SetProperty(SnProperty, value); }
        }
        #endregion

        #region 处理状态 UploadState
        /// <summary>
        /// 处理状态
        /// </summary>
        [Label("处理状态")]
        public static readonly Property<UploadState> UploadStateProperty = P<EdoOutlineUploadLog>.Register(e => e.UploadState);

        /// <summary>
        /// 处理状态
        /// </summary>
        public UploadState UploadState
        {
            get { return this.GetProperty(UploadStateProperty); }
            set { this.SetProperty(UploadStateProperty, value); }
        }
        #endregion

        #region 类型 UploadType
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<UploadType> UploadTypeProperty = P<EdoOutlineUploadLog>.Register(e => e.UploadType);

        /// <summary>
        /// 类型
        /// </summary>
        public UploadType UploadType
        {
            get { return this.GetProperty(UploadTypeProperty); }
            set { this.SetProperty(UploadTypeProperty, value); }
        }
        #endregion

        #region 失败原因 FailReason
        /// <summary>
        /// 失败原因
        /// </summary>
        [Label("失败原因")]
        public static readonly Property<string> FailReasonProperty = P<EdoOutlineUploadLog>.Register(e => e.FailReason);

        /// <summary>
        /// 失败原因
        /// </summary>
        public string FailReason
        {
            get { return this.GetProperty(FailReasonProperty); }
            set { this.SetProperty(FailReasonProperty, value); }
        }
        #endregion

        #region 详细数据 DetailMsg
        /// <summary>
        /// 详细数据
        /// </summary>
        [Label("详细数据")]
        public static readonly Property<string> DetailMsgProperty = P<EdoOutlineUploadLog>.Register(e => e.DetailMsg);

        /// <summary>
        /// 详细数据
        /// </summary>
        public string DetailMsg
        {
            get { return this.GetProperty(DetailMsgProperty); }
            set { this.SetProperty(DetailMsgProperty, value); }
        }
        #endregion

        #region 盘点说明 Remark
        /// <summary>
        /// 盘点说明
        /// </summary>
        [Label("盘点说明")]
        public static readonly Property<string> RemarkProperty = P<EdoOutlineUploadLog>.Register(e => e.Remark);

        /// <summary>
        /// 盘点说明
        /// </summary>
        public string Remark
        {
            get { return this.GetProperty(RemarkProperty); }
            set { this.SetProperty(RemarkProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 离线数据上传记录 实体配置
    /// </summary>
    internal class EdoOutlineUploadLogConfig : EntityConfig<EdoOutlineUploadLog>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EDO_OUTLINE_UPLOADLOG").MapAllProperties();
            Meta.Property(EdoOutlineUploadLog.DetailMsgProperty).ColumnMeta.HasLength(4000);
            Meta.Property(EdoOutlineUploadLog.FailReasonProperty).ColumnMeta.HasLength(1000);
            Meta.EnablePhantoms();
        }
    }
}
