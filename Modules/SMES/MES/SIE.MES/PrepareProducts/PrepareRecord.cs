using SIE.Domain;
using SIE.MES.PrepareProducts.Enums;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.PrepareProducts
{
    /// <summary>
    /// 产前准备记录
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(PrepareRecordCriteria))]
    [Label("产前准备记录")]
    public class PrepareRecord : WorkOrder
    {
        #region 产前准备记录明细 PrepareRecordDetail
        /// <summary>
        /// 产前准备记录子表
        /// </summary>
        [Label("产前准备记录明细")]
        public static readonly ListProperty<EntityList<PrepareRecordDetail>> PrepareRecordDetailProperty = P<PrepareRecord>.RegisterList(e => e.PrepareRecordDetail);

        /// <summary>
        /// 产前准备记录子表
        /// </summary>
        public EntityList<PrepareRecordDetail> PrepareRecordDetail
        {
            get { return this.GetLazyList(PrepareRecordDetailProperty); }
        }
        #endregion

    }

    /// <summary>
    /// 实体元配置
    /// </summary>
    public class PrepareRecordConfig : EntityConfig<PrepareRecord>
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WO").MapAllProperties();
            Meta.Property(WOLabelPrintDetailProperty.LabelPrintTemProperty).DontMapColumn();
            Meta.Property(WoWipBatchExt.AttacWoWipBatchProperty).DontMapColumn();
            Meta.Property(WorkOrder.IsReGenerateTaskProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}
