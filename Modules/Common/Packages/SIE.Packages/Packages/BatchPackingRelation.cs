using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Packages
{
    /// <summary>
    /// 批次包装关系
    /// </summary>
    [RootEntity, Serializable]
    [Label("批次包装关系")]
    [DisplayMember(nameof(PackageNo))]
    public class BatchPackingRelation : PackingRelation
    {
        #region 关联批次 PackingBatch
        /// <summary>
        /// 关联批次
        /// </summary>
        [Label("关联批次")]
        public static readonly Property<double?> PackingBatchProperty = P<BatchPackingRelation>.Register(e => e.PackingBatch);

        /// <summary>
        /// 关联批次
        /// </summary>
        public double? PackingBatch
        {
            get { return this.GetProperty(PackingBatchProperty); }
            set { this.SetProperty(PackingBatchProperty, value); }
        }
        #endregion

        #region 关联批次号 BatchNo
        /// <summary>
        /// 关联批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> BatchNoProperty = P<BatchPackingRelation>.Register(e => e.BatchNo);

        /// <summary>
        /// 关联批次号
        /// </summary>
        public string BatchNo
        {
            get { return this.GetProperty(BatchNoProperty); }
            set { this.SetProperty(BatchNoProperty, value); }
        }
        #endregion

        #region 生产子批次 Name
        /// <summary>
        /// 生产子批次
        /// </summary>
        [Label("生产子批次")]
        public static readonly Property<double?> ChildBatchProperty = P<BatchPackingRelation>.Register(e => e.ChildBatch);

        /// <summary>
        /// 生产子批次
        /// </summary>
        public double? ChildBatch
        {
            get { return this.GetProperty(ChildBatchProperty); }
            set { this.SetProperty(ChildBatchProperty, value); }
        }
        #endregion

        #region 生产子批次号 ChildBatchNo
        /// <summary>
        /// 生产子批次号
        /// </summary>
        [Label("生产子批次号")]
        public static readonly Property<string> ChildBatchNoProperty = P<BatchPackingRelation>.Register(e => e.ChildBatchNo);

        /// <summary>
        /// 生产子批次号
        /// </summary>
        public string ChildBatchNo
        {
            get { return this.GetProperty(ChildBatchNoProperty); }
            set { this.SetProperty(ChildBatchNoProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 批次包装关系 实体配置
    /// </summary>    
    internal class BatchPackingRelationConfig : EntityConfig<BatchPackingRelation>
    {
        /// <summary>
        /// 实体配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("PKG_RELATION").MapAllProperties();
            Meta.SupportTree();
            Meta.EnablePhantoms();
        }
    }
}
