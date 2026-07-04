using SIE.Domain;
using SIE.Items;
using SIE.MES.ItemLine;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.BlueLable
{
    /// <summary>
    /// 蓝标
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(BlueLableCriterial))]
    [Label("蓝标")]
    public class BlueLable : DataEntity
    {
        #region 物料编码 Item
        /// <summary>
        /// 物料编码Id
        /// </summary>
        [Label("物料编码")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<BlueLable>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料编码Id
        /// </summary>
        public double ItemId
        {
            get { return (double)this.GetRefId(ItemIdProperty); }
            set { this.SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料编码
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty =
            P<BlueLable>.RegisterRef(e => e.Item, ItemIdProperty);

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
        public static readonly Property<string> ExternalIdentProperty = P<BlueLable>.Register(e => e.ExternalIdent);

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
        public static readonly Property<string> BatchNoProperty = P<BlueLable>.Register(e => e.BatchNo);

        /// <summary>
        /// 批号
        /// </summary>
        public string BatchNo
        {
            get { return this.GetProperty(BatchNoProperty); }
            set { this.SetProperty(BatchNoProperty, value); }
        }
        #endregion

        #region 包装数量 PackageNum
        /// <summary>
        /// 包装数量
        /// </summary>
        [Label("包装数量")]
        public static readonly Property<int> PackageNumProperty = P<BlueLable>.Register(e => e.PackageNum);

        /// <summary>
        /// 包装数量
        /// </summary>
        public int PackageNum
        {
            get { return this.GetProperty(PackageNumProperty); }
            set { this.SetProperty(PackageNumProperty, value); }
        }
        #endregion

        #region 库存地点 StorageLocation
        /// <summary>
        /// 库存地点
        /// </summary>
        [Label("库存地点")]
        public static readonly Property<string> StorageLocationProperty = P<BlueLable>.Register(e => e.StorageLocation);

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
        public static readonly Property<string> BlueLableBoxProperty = P<BlueLable>.Register(e => e.BlueLableBox);

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
        public static readonly Property<string> ProductionNoProperty = P<BlueLable>.Register(e => e.ProductionNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string ProductionNo
        {
            get { return this.GetProperty(ProductionNoProperty); }
            set { this.SetProperty(ProductionNoProperty, value); }
        }
        #endregion

        #region 创建删除标识 CreateDeleteident
        /// <summary>
        /// 创建删除标识
        /// </summary>
        [Label("创建删除标识")]
        public static readonly Property<string> CreateDeleteidentProperty = P<BlueLable>.Register(e => e.CreateDeleteident);

        /// <summary>
        /// 创建删除标识
        /// </summary>
        public string CreateDeleteident
        {
            get { return this.GetProperty(CreateDeleteidentProperty); }
            set { this.SetProperty(CreateDeleteidentProperty, value); }
        }
        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        [Required]
        public static readonly IRefIdProperty FactoryIdProperty =
            P<BlueLable>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double? FactoryId
        {
            get { return (double?)this.GetRefNullableId(FactoryIdProperty); }
            set { this.SetRefNullableId(FactoryIdProperty, value); }
        }

        /// <summary>
        /// 工厂
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> FactoryProperty =
            P<BlueLable>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise Factory
        {
            get { return this.GetRefEntity(FactoryProperty); }
            set { this.SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 是否装箱 IsPack
        /// <summary>
        /// 是否装箱
        /// </summary>
        [Label("是否装箱")]
        public static readonly Property<bool?> IsPackProperty = P<BlueLable>.Register(e => e.IsPack);

        /// <summary>
        /// 是否装箱
        /// </summary>
        public bool? IsPack
        {
            get { return this.GetProperty(IsPackProperty); }
            set { this.SetProperty(IsPackProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<BlueLable>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 蓝标 实体配置
    /// </summary>
    internal class BlueLableConfig : EntityConfig<BlueLable>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("BLUEL_LABLE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
