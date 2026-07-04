using SIE.Domain;
using SIE.Items;
using SIE.MES.ItemLine;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.BlueLable
{
    /// <summary>
    /// 蓝标数据查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("蓝标数据查询实体")]
    public class BlueLableCriterial : Criteria
    {
        #region 物料编码 Item
        /// <summary>
        /// 物料编码Id
        /// </summary>
        [Label("物料编码")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<BlueLableCriterial>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料编码Id
        /// </summary>
        public double? ItemId
        {
            get { return (double?)this.GetRefId(ItemIdProperty); }
            set { this.SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料编码
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty =
            P<BlueLableCriterial>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料编码
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 箱号 ExternalIdent
        /// <summary>
        /// 箱号
        /// </summary>
        [Label("箱号")]
        public static readonly Property<string> ExternalIdentProperty = P<BlueLableCriterial>.Register(e => e.ExternalIdent);

        /// <summary>
        /// 箱号
        /// </summary>
        public string ExternalIdent
        {
            get { return this.GetProperty(ExternalIdentProperty); }
            set { this.SetProperty(ExternalIdentProperty, value); }
        }
        #endregion

        #region 批号 BatchNo
        /// <summary>
        /// 批号
        /// </summary>
        [Label("批号")]
        public static readonly Property<string> BatchNoProperty = P<BlueLableCriterial>.Register(e => e.BatchNo);

        /// <summary>
        /// 批号
        /// </summary>
        public string BatchNo
        {
            get { return this.GetProperty(BatchNoProperty); }
            set { this.SetProperty(BatchNoProperty, value); }
        }
        #endregion

        #region 库存地点 StorageLocation
        /// <summary>
        /// 库存地点
        /// </summary>
        [Label("库存地点")]
        public static readonly Property<string> StorageLocationProperty = P<BlueLableCriterial>.Register(e => e.StorageLocation);

        /// <summary>
        /// 库存地点
        /// </summary>
        public string StorageLocation
        {
            get { return this.GetProperty(StorageLocationProperty); }
            set { this.SetProperty(StorageLocationProperty, value); }
        }
        #endregion

        #region HU外箱蓝标 BlueLableBox
        /// <summary>
        /// HU外箱蓝标
        /// </summary>
        [Label("HU外箱蓝标")]
        public static readonly Property<string> BlueLableBoxProperty = P<BlueLableCriterial>.Register(e => e.BlueLableBox);

        /// <summary>
        /// HU外箱蓝标
        /// </summary>
        public string BlueLableBox
        {
            get { return this.GetProperty(BlueLableBoxProperty); }
            set { this.SetProperty(BlueLableBoxProperty, value); }
        }
        #endregion

        #region 工单号 ProductionNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> ProductionNoProperty = P<BlueLableCriterial>.Register(e => e.ProductionNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string ProductionNo
        {
            get { return this.GetProperty(ProductionNoProperty); }
            set { this.SetProperty(ProductionNoProperty, value); }
        }
        #endregion

        #region 创建时间 CreateDate
        /// <summary>
        /// 创建时间
        /// </summary>
        [Label("创建时间")]
        public static readonly Property<DateRange> CreateDateProperty = P<BlueLableCriterial>.Register(e => e.CreateDate);

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateRange CreateDate
        {
            get { return this.GetProperty(CreateDateProperty); }
            set { this.SetProperty(CreateDateProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<BlueLableController>().CriterialBlueLable(this);
        }
    }

}
