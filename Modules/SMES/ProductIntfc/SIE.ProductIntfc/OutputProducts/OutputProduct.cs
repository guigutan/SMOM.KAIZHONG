using SIE.Common.Configs;
using SIE.Domain;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.ProductIntfc.Configs;
using System;

namespace SIE.ProductIntfc.OutputProducts   
{
    /// <summary>
    /// 联/副产品入库
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(OutputProductCriteria))]
    [Label("联/副产品入库")]
    public partial class OutputProduct : WorkOrder
    {
        //#region 联/副产品 EntityListType
        ///// <summary>
        ///// 联/副产品
        ///// </summary>
        //[Label("联/副产品")]
        //public static readonly ListProperty<EntityList<OutputProductDetail>> OutputProductDetailListTypeProperty = P<OutputProduct>.RegisterList(e => e.OutputProductDetailList);

        ///// <summary>
        ///// 联/副产品
        ///// </summary>
        //public EntityList<OutputProductDetail> OutputProductDetailList
        //{
        //    get { return this.GetLazyList(OutputProductDetailListTypeProperty); }
        //}
        //#endregion

        #region 副产品记录 OutputProductRecords
        /// <summary>
        /// 副产品记录
        /// </summary>
        [Label("副产品记录")]
        public static readonly ListProperty<EntityList<OutputProductRecord>> OutputProductRecordsProperty = P<OutputProduct>.RegisterList(e => e.OutputProductRecords);

        /// <summary>
        /// 副产品记录
        /// </summary>
        public EntityList<OutputProductRecord> OutputProductRecords
        {
            get { return this.GetLazyList(OutputProductRecordsProperty); }
        }
        #endregion

    }

    /// <summary>
    /// 入库工单 实体配置
    /// </summary>
    internal class OutputProductConfig : EntityConfig<OutputProduct>
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