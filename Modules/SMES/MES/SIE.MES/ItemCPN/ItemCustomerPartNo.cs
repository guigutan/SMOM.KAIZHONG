using SIE.Core.Items;
using SIE.Domain;
using SIE.KZ.Base.Interfaces;
using SIE.MES.ItemLine;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ItemCPN
{
    /// <summary>
    /// 物料客户料码查询
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(ItemCustomerPartNoCriteria))]
    [Label("物料客户料码查询")]
    public class ItemCustomerPartNo : DataEntity
    {
        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty = P<ItemCustomerPartNo>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId
        {
            get { return (double)this.GetRefId(ItemIdProperty); }
            set { this.SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty = P<ItemCustomerPartNo>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<ItemCustomerPartNo>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { this.SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<ItemCustomerPartNo>.RegisterView(e => e.ItemCode, p => p.Item.Code);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
            set { this.SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 客户 Customer
        /// <summary>
        /// 客户
        /// </summary>
        [Label("客户")]
        public static readonly Property<string> CustomerProperty = P<ItemCustomerPartNo>.Register(e => e.Customer);

        /// <summary>
        /// 客户
        /// </summary>
        public string Customer
        {
            get { return this.GetProperty(CustomerProperty); }
            set { this.SetProperty(CustomerProperty, value); }
        }
        #endregion

        #region 客户料号 CodeAlias
        /// <summary>
        /// 客户料号
        /// </summary>
        [Label("客户料号")]
        public static readonly Property<string> CodeAliasProperty = P<ItemCustomerPartNo>.Register(e => e.CodeAlias);

        /// <summary>
        /// 客户料号
        /// </summary>
        public string CodeAlias
        {
            get { return this.GetProperty(CodeAliasProperty); }
            set { this.SetProperty(CodeAliasProperty, value); }
        }
        #endregion

        #region 供应商代码 SupplierCode
        /// <summary>
        /// 供应商代码
        /// </summary>
        [Label("供应商代码")]
        public static readonly Property<string> SupplierCodeProperty = P<ItemCustomerPartNo>.Register(e => e.SupplierCode);

        /// <summary>
        /// 供应商代码
        /// </summary>
        public string SupplierCode
        {
            get { return this.GetProperty(SupplierCodeProperty); }
            set { this.SetProperty(SupplierCodeProperty, value); }
        }
        #endregion

        #region 版本号 VersionNo
        /// <summary>
        /// 版本号
        /// </summary>
        [Label("版本号")]
        public static readonly Property<string> VersionNoProperty = P<ItemCustomerPartNo>.Register(e => e.VersionNo);

        /// <summary>
        /// 版本号
        /// </summary>
        public string VersionNo
        {
            get { return this.GetProperty(VersionNoProperty); }
            set { this.SetProperty(VersionNoProperty, value); }
        }
        #endregion

        #region 项目号 ProjectName
        /// <summary>
        /// 项目号
        /// </summary>
        [Label("项目号")]
        public static readonly Property<string> ProjectNameProperty = P<ItemCustomerPartNo>.Register(e => e.ProjectName);

        /// <summary>
        /// 项目号
        /// </summary>
        public string ProjectName
        {
            get { return this.GetProperty(ProjectNameProperty); }
            set { this.SetProperty(ProjectNameProperty, value); }
        }
        #endregion

        #region 图号 Drawing
        /// <summary>
        /// 图号
        /// </summary>
        [Label("图号")]
        public static readonly Property<string> DrawingProperty = P<ItemCustomerPartNo>.Register(e => e.Drawing);

        /// <summary>
        /// 图号
        /// </summary>
        public string Drawing
        {
            get { return this.GetProperty(DrawingProperty); }
            set { this.SetProperty(DrawingProperty, value); }
        }
        #endregion

        #region 工序标签号 BatchNo
        /// <summary>
        /// 工序标签号
        /// </summary>
        [Label("工序标签号")]
        public static readonly Property<string> BatchNoProperty = P<ItemCustomerPartNo>.Register(e => e.BatchNo);

        /// <summary>
        /// 工序标签号
        /// </summary>
        public string BatchNo
        {
            get { return this.GetProperty(BatchNoProperty); }
            set { this.SetProperty(BatchNoProperty, value); }
        }
        #endregion

        #region 当前产线 LineCode
        /// <summary>
        /// 当前产线
        /// </summary>
        [Label("当前产线")]
        public static readonly Property<string> LineCodeProperty = P<ItemCustomerPartNo>.Register(e => e.LineCode);

        /// <summary>
        /// 当前产线
        /// </summary>
        public string LineCode
        {
            get { return this.GetProperty(LineCodeProperty); }
            set { this.SetProperty(LineCodeProperty, value); }
        }
        #endregion

        #region 工单数量 WorkOrderQty
        /// <summary>
        /// 工单数量
        /// </summary>
        [Label("工单数量")]
        public static readonly Property<decimal> WorkOrderQtyProperty = P<ItemCustomerPartNo>.Register(e => e.WorkOrderQty);

        /// <summary>
        /// 工单数量
        /// </summary>
        public decimal WorkOrderQty
        {
            get { return this.GetProperty(WorkOrderQtyProperty); }
            set { this.SetProperty(WorkOrderQtyProperty, value); }
        }
        #endregion

        #region 预留扩展属性

        #region 属性1 Attribute1
        /// <summary>
        /// 属性1
        /// </summary>
        [Label("属性1")]
        public static readonly Property<string> Attribute1Property = P<ItemCustomerPartNo>.Register(e => e.Attribute1);

        /// <summary>
        /// 属性1
        /// </summary>
        public string Attribute1
        {
            get { return this.GetProperty(Attribute1Property); }
            set { this.SetProperty(Attribute1Property, value); }
        }
        #endregion

        #region 属性2 Attribute2
        /// <summary>
        /// 属性2
        /// </summary>
        [Label("属性2")]
        public static readonly Property<string> Attribute2Property = P<ItemCustomerPartNo>.Register(e => e.Attribute2);

        /// <summary>
        /// 属性2
        /// </summary>
        public string Attribute2
        {
            get { return this.GetProperty(Attribute2Property); }
            set { this.SetProperty(Attribute2Property, value); }
        }
        #endregion

        #region 属性3 Attribute3
        /// <summary>
        /// 属性3
        /// </summary>
        [Label("属性3")]
        public static readonly Property<string> Attribute3Property = P<ItemCustomerPartNo>.Register(e => e.Attribute3);

        /// <summary>
        /// 属性3
        /// </summary>
        public string Attribute3
        {
            get { return this.GetProperty(Attribute3Property); }
            set { this.SetProperty(Attribute3Property, value); }
        }
        #endregion

        #region 属性4 Attribute4
        /// <summary>
        /// 属性4
        /// </summary>
        [Label("属性4")]
        public static readonly Property<string> Attribute4Property = P<ItemCustomerPartNo>.Register(e => e.Attribute4);

        /// <summary>
        /// 属性4
        /// </summary>
        public string Attribute4
        {
            get { return this.GetProperty(Attribute4Property); }
            set { this.SetProperty(Attribute4Property, value); }
        }
        #endregion

        #region 属性5 Attribute5
        /// <summary>
        /// 属性5
        /// </summary>
        [Label("属性5")]
        public static readonly Property<string> Attribute5Property = P<ItemCustomerPartNo>.Register(e => e.Attribute5);

        /// <summary>
        /// 属性5
        /// </summary>
        public string Attribute5
        {
            get { return this.GetProperty(Attribute5Property); }
            set { this.SetProperty(Attribute5Property, value); }
        }
        #endregion

        #region 属性6 Attribute6
        /// <summary>
        /// 属性6
        /// </summary>
        [Label("属性6")]
        public static readonly Property<string> Attribute6Property = P<ItemCustomerPartNo>.Register(e => e.Attribute6);

        /// <summary>
        /// 属性6
        /// </summary>
        public string Attribute6
        {
            get { return this.GetProperty(Attribute6Property); }
            set { this.SetProperty(Attribute6Property, value); }
        }
        #endregion

        #region 属性7 Attribute7
        /// <summary>
        /// 属性7
        /// </summary>
        [Label("属性7")]
        public static readonly Property<string> Attribute7Property = P<ItemCustomerPartNo>.Register(e => e.Attribute7);

        /// <summary>
        /// 属性7
        /// </summary>
        public string Attribute7
        {
            get { return this.GetProperty(Attribute7Property); }
            set { this.SetProperty(Attribute7Property, value); }
        }
        #endregion

        #region 属性8 Attribute8
        /// <summary>
        /// 属性8
        /// </summary>
        [Label("属性8")]
        public static readonly Property<string> Attribute8Property = P<ItemCustomerPartNo>.Register(e => e.Attribute8);

        /// <summary>
        /// 属性8
        /// </summary>
        public string Attribute8
        {
            get { return this.GetProperty(Attribute8Property); }
            set { this.SetProperty(Attribute8Property, value); }
        }
        #endregion

        #region 属性9 Attribute9
        /// <summary>
        /// 属性9
        /// </summary>
        [Label("属性9")]
        public static readonly Property<string> Attribute9Property = P<ItemCustomerPartNo>.Register(e => e.Attribute9);

        /// <summary>
        /// 属性9
        /// </summary>
        public string Attribute9
        {
            get { return this.GetProperty(Attribute9Property); }
            set { this.SetProperty(Attribute9Property, value); }
        }
        #endregion

        #region 属性10 Attribute10
        /// <summary>
        /// 属性10
        /// </summary>
        [Label("属性10")]
        public static readonly Property<string> Attribute10Property = P<ItemCustomerPartNo>.Register(e => e.Attribute10);

        /// <summary>
        /// 属性10
        /// </summary>
        public string Attribute10
        {
            get { return this.GetProperty(Attribute10Property); }
            set { this.SetProperty(Attribute10Property, value); }
        }
        #endregion

        #endregion
    }

    internal class ItemCustomerPartNoConfig : EntityConfig<ItemCustomerPartNo>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("ITEM_CUSOTMER_RELATION").MapAllProperties();
            Meta.Property(ItemCustomerPartNo.BatchNoProperty).DontMapColumn();
            Meta.Property(ItemCustomerPartNo.LineCodeProperty).DontMapColumn();
            Meta.Property(ItemCustomerPartNo.WorkOrderQtyProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}