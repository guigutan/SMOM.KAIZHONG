using SIE.Domain;
using SIE.Items;
using SIE.Kit.MES.Storages;
using SIE.MetaModel.View;
using SIE.ObjectModel;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.Kit.MES.Storages
{
    /// <summary>
    /// 产线物料货位视图配置
    /// </summary>   
    [ManagedProperty.CompiledPropertyDeclarer]
    internal class ItemStorageViewConfig : WebViewConfig<ItemStorage>
    {
        #region 物料属性值 ItemPropertyValue
        /// <summary>
        /// 物料属性值
        /// </summary>
        [Label("物料属性值")]
        public static readonly Property<string> ItemPropertyValueProperty = P<ItemStorage>.RegisterExtensionReadOnly("ItemPropertyValueProperty", typeof(ItemStorageViewConfig),
            GetItemPropertyValueProperty, ItemStorage.PropertyValueListProperty);

        /// <summary>
        /// 获取物料属性
        /// </summary>
        /// <param name="detail">产品BOM明细</param>
        /// <returns>string</returns>
        public static string GetItemPropertyValueProperty(ItemStorage detail)
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
            View.UseCommands("SIE.Web.Kit.MES.Storages.Commands.AddItemStorageCommand", WebCommandNames.Edit, "SIE.Web.Kit.MES.Storages.Commands.DeleteItemStorageCommand",
                WebCommandNames.ExportXls);
            View.UseImportCommands();
            View.Property(p => p.Item).ShowInList(150).UseDataSource((e, c, r) =>
            {
                List<int> type = new List<int>() { (int)ItemType.Material, (int)ItemType.SemiFinished };
                List<int> mode = new List<int>() { (int)ConsumeMode.Pull };
                var list = RT.Service.Resolve<ItemController>().GetItemsByTypeAndMode(type, mode, c, r);//原材料or半成品  &&拉式物料
                return list;
            }).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add(nameof(e.ItemName), nameof(e.Item.Name));
                m.DicLinkField = dic;
            }).HasLabel("物料编码");
            View.Property(p => p.ItemName).ShowInList(150).Readonly();
            View.Property(p => p.ItemConsumeMode).Readonly();
            View.Property(ItemPropertyValueProperty).Readonly(false).UseTextButtonFieldEditor(p =>
            {
                p.ExtendJsObj = "SIE.Web.Kit.MES.Storages.ItemStoragePropertyEditor";
                p.Editable = false;
                p.IsReadonly = false;
            });
            View.Property(p => p.SafetyQty).UseSpinEditor(p => { p.MinValue = 0; p.AllowNegative = false; p.Step = 1; p.AllowDecimals = false; });
            View.Property(p => p.CallMaterialBatch).UseSpinEditor(p => { p.MinValue = 0; p.AllowNegative = false; p.Step = 1; p.AllowDecimals = false; });
            View.Property(p => p.Qty).Readonly();
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
            View.Property(p => p.ItemName);
            View.Property(p => p.Qty).Readonly();
        }

        /// <summary>
        /// 配置导入视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.PropertyRef(p => p.Item.Code).HasLabel("物料编码");
            View.Property(p => p.SafetyQty);
            View.Property(p => p.CallMaterialBatch);
        }
    }
}