using SIE.Domain;
using SIE.MES.WIP.Products;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.WIP.Repairs
{
    /// <summary>
    /// 维修缺陷保存记录
    /// </summary>
    [RootEntity, Serializable]
    [Label("维修主记录")]
    public class RepairMainRecord : DataEntity
    {
        #region 维修类型 RepairType
        /// <summary>
        /// 维修类型
        /// </summary>
        [Label("维修类型")]
        public static readonly Property<RepairType?> RepairTypeProperty = P<RepairMainRecord>.Register(e => e.RepairType);

        /// <summary>
        /// 维修类型
        /// </summary>
        public RepairType? RepairType
        {
            get { return this.GetProperty(RepairTypeProperty); }
            set { this.SetProperty(RepairTypeProperty, value); }
        }
        #endregion

        #region 维修开始时间 RepairStart
        /// <summary>
        /// 维修开始时间
        /// </summary>
        [Label("维修开始时间")]
        public static readonly Property<DateTime> RepairStartProperty = P<RepairMainRecord>.Register(e => e.RepairStart);

        /// <summary>
        /// 维修开始时间
        /// </summary>
        public DateTime RepairStart
        {
            get { return this.GetProperty(RepairStartProperty); }
            set { this.SetProperty(RepairStartProperty, value); }
        }
        #endregion

        #region 工单Id WorkOrderId
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单ID")]
        public static readonly Property<double> WorkOrderIdProperty = P<RepairMainRecord>.Register(e => e.WorkOrderId);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId
        {
            get { return this.GetProperty(WorkOrderIdProperty); }
            set { this.SetProperty(WorkOrderIdProperty, value); }
        }
        #endregion

        #region 产品条码 Sn
        /// <summary>
        /// 产品条码
        /// </summary>
        [Label("产品条码")]
        public static readonly Property<string> SnProperty = P<RepairMainRecord>.Register(e => e.Sn);

        /// <summary>
        /// 产品条码
        /// </summary>
        public string Sn
        {
            get { return this.GetProperty(SnProperty); }
            set { this.SetProperty(SnProperty, value); }
        }
        #endregion

        #region 维修缺陷保存记录列表 RepairDefectRecordList
        /// <summary>
        /// 维修缺陷保存记录列表
        /// </summary>
        [Label("维修缺陷保存记录列表")]
        public static readonly ListProperty<EntityList<RepairDefectRecord>> RepairDefectRecordListProperty = P<RepairMainRecord>.RegisterList(e => e.RepairDefectRecordList);

        /// <summary>
        /// 维修缺陷保存记录列表列表
        /// </summary>
        public EntityList<RepairDefectRecord> RepairDefectRecordList
        {
            get { return this.GetLazyList(RepairDefectRecordListProperty); }
        }
        #endregion
        #region 图片 Attachments
        /// <summary>
        /// 图片
        /// </summary>
        public static readonly ListProperty<EntityList<RepairAttachment>> AttachmentsProperty = P<RepairMainRecord>.RegisterList(e => e.Attachments);
        /// <summary>
        /// 图片
        /// </summary>
        public EntityList<RepairAttachment> Attachments
        {
            get { return this.GetLazyList(AttachmentsProperty); }
        }
        #endregion
    }


    /// <summary>
    /// 维修缺陷保存记录 实体配置
    /// </summary>
    internal class RepairRecordConfig : EntityConfig<RepairMainRecord>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_REP_MAIN_RECORD").MapAllProperties();            
            Meta.DisablePhantoms();
        }
    }
}
