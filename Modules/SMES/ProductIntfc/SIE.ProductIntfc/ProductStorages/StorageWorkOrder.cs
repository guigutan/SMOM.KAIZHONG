using SIE.Common.Configs;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.ProductIntfc.Configs;
using System;

namespace SIE.ProductIntfc.ProductStorages
{
    /// <summary>
    /// 入库工单
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(ProductStorageCriteria))]
    [EntityWithConfig(typeof(ProductStorageConfig))]
    [Label("成品入库")]
    public partial class StorageWorkOrder : WorkOrder
    {
    }

    /// <summary>
    /// 入库工单 实体配置
    /// </summary>
    internal class StorageWorkOrderConfig : EntityConfig<StorageWorkOrder>
    {
        /// <summary>
        /// 数据表Config,子类要包含有父类的Config内容
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WO");
            Meta.Property(WOLabelPrintDetailProperty.LabelPrintTemProperty).DontMapColumn();
            Meta.Property(WoWipBatchExt.AttacWoWipBatchProperty).DontMapColumn();
            Meta.Property(WorkOrder.IsReGenerateTaskProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}