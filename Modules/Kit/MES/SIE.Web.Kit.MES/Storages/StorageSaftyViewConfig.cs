using SIE.Domain;
using SIE.Kit.MES.Storages;
using SIE.MetaModel.View;
using SIE.ObjectModel;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.Kit.MES.Storages
{
    /// <summary>
    /// 产线库存视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    internal class StorageSaftyViewConfig : WebViewConfig<StorageSafty>
    {
        #region 实际库存（剩余库存+配送中数量） ActualQty
        /// <summary>
        /// 实际库存（剩余库存+配送中数量）
        /// </summary>
        [Label("总数")]
        public static readonly Property<decimal> ActualQtyProperty = P<StorageSafty>.RegisterExtensionReadOnly("ActualQty", typeof(StorageSaftyViewConfig),
            GetActualQty, StorageSafty.SurplusQtyProperty, StorageSafty.DeliveryingQtyProperty);

        /// <summary>
        /// 实际库存（剩余库存+配送中数量）
        /// </summary>
        public static decimal GetActualQty(StorageSafty me)
        {
            return me.SurplusQty + me.DeliveryingQty;
        }
        #endregion

        #region 物料属性值 ItemPropertyValue
        /// <summary>
        /// 物料属性值
        /// </summary>
        [Label("物料属性值")]
        public static readonly Property<string> ItemPropertyValueProperty = P<StorageSafty>.RegisterExtensionReadOnly("ItemPropertyValueProperty", typeof(StorageSaftyViewConfig),
            GetItemPropertyValueProperty, StorageSafty.PropertyValueListProperty);

        /// <summary>
        /// 获取物料属性
        /// </summary>
        /// <param name="detail">产品BOM明细</param>
        /// <returns>string</returns>
        public static string GetItemPropertyValueProperty(StorageSafty detail)
        {
            var groups = detail.PropertyValueList.GroupBy(p => p.Definition.Name).ToList();
            string[] result = new string[groups.Count];
            for (int i = 0; i < groups.Count; i++)
            {
                var values = groups[i].Select(p => p.Value);
                result[i] = groups[i].Key + "：" + string.Join("、", values);
            }

            return string.Join("；", result);
        }
        #endregion

        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            //方法重写
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.ExportXls);
            View.Property(p => p.Item).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add(nameof(e.ItemName), nameof(e.Item.Name));
                m.DicLinkField = dic;
            }).HasLabel("物料编码");
            View.Property(p => p.ItemName).Readonly();
            View.Property(ItemPropertyValueProperty).Readonly(false).UseTextButtonFieldEditor(p =>
            {
                p.ExtendJsObj = "SIE.Web.Kit.MES.Storages.StorageSaftyPropertyEditor";
                p.Editable = false;
                p.IsReadonly = false;
            });
            View.Property(p => p.StorageAreaCode).Readonly();
            View.Property(p => p.StorageLocationId).HasLabel("货位编码");
            View.Property(p => p.SurplusQty).Readonly();
            View.Property(p => p.DeliveryingQty).Readonly();
            View.Property(ActualQtyProperty);
            View.Property(p => p.CreateByName);
            View.Property(p => p.CreateDate).ShowInList(150);
            View.Property(p => p.UpdateByName);
            View.Property(p => p.UpdateDate).ShowInList(150);
            View.ChildrenProperty(p => p.PropertyValueList).IsVisible = false;
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Item);
            View.Property(p => p.ItemName).HasLabel("物料名称");
            View.Property(p => p.MaxQty);
            View.Property(p => p.SafetyQty);
            View.Property(p => p.DeliveryQty);
        }

        /// <summary>
        /// 配置导入视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.PropertyRef(p => p.StorageArea.Code).HasLabel("工位货区编码");
            View.PropertyRef(p => p.Item.Code).HasLabel("物料编码");
            View.Property(p => p.MaxQty);
            View.Property(p => p.SafetyQty);
            View.Property(p => p.DeliveryQty);
        }
    }
}