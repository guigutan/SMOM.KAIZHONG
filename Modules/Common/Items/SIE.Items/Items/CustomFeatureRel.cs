using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Items.Items
{
    /// <summary>
    /// 物料客户与特性关系
    /// </summary>
    [ChildEntity, Serializable]
    [Label("物料客户与特性关系")]
    public class CustomFeatureRel : DataEntity
    {
        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<CustomFeatureRel>.RegisterRefId(e => e.ItemId, ReferenceType.Parent);

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
        public static readonly RefEntityProperty<Item> ItemProperty =
            P<CustomFeatureRel>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 客户 Customer
        /// <summary>
        /// 客户
        /// </summary>
        [Label("客户")]
        public static readonly Property<string> CustomerProperty = P<CustomFeatureRel>.Register(e => e.Customer);

        /// <summary>
        /// 客户
        /// </summary>
        public string Customer
        {
            get { return this.GetProperty(CustomerProperty); }
            set { this.SetProperty(CustomerProperty, value); }
        }
        #endregion

        #region 其他特性 Zqttx
        /// <summary>
        /// 其他特性
        /// </summary>
        [Label("其他特性")]
        public static readonly Property<string> ZqttxProperty = P<CustomFeatureRel>.Register(e => e.Zqttx);

        /// <summary>
        /// 其他特性
        /// </summary>
        public string Zqttx
        {
            get { return this.GetProperty(ZqttxProperty); }
            set { this.SetProperty(ZqttxProperty, value); }
        }
        #endregion

    }

    internal class CustomFeatureRelConfig : EntityConfig<CustomFeatureRel>
    {

        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);
        }
        protected override void ConfigMeta()
        {
            Meta.MapTable("CUSTOM_FEATURE_REL").MapAllProperties();
            Meta.EnableInvOrg();
            Meta.EnablePhantoms();
        }
    }
}
