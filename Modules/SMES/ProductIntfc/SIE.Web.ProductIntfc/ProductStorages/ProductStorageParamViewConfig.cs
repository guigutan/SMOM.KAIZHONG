using SIE.Items;
using SIE.MetaModel.View;
using SIE.ProductIntfc.ProductStorages;
using SIE.Web.ProductIntfc._Extentions_;
using System.Collections.Generic;

namespace SIE.Web.ProductIntfc.ProductStorages
{
    /// <summary>
    /// 成品入库参数视图配置
    /// </summary>
    public class ProductStorageParamViewConfig : WebViewConfig<ProductStorageParam>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            //方法重写
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.UseDefaultCommands().UseImportCommands();
            View.ReplaceCommands(WebCommandNames.Add, "SIE.Web.ProductIntfc.ProductStorages.Commands.ParamAddCommand");
            using (View.OrderProperties())
            {
                View.Property(p => p.ItemId).HasLabel("产品编码").UseStorageParamLookUpEditor().UsePagingLookUpEditor(
                (m, e) =>
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add(nameof(e.ItemName), nameof(Item.Name));
                    m.DicLinkField = dic;
                }).ShowInList(150);
                View.Property(p => p.ItemName).ShowInList(150).Readonly().HasLabel("产品名称");
                View.Property(p => p.ItemType).UseEnumEditor().Readonly().HasLabel("入库类型");
                View.Property(p => p.InspDimension).HasLabel("入库维度");
                View.Property(p => p.Qty).UseSpinEditor(p => p.DecimalPrecision = 0).HasLabel("入库参数");
            }
        }

        /// <summary>
        /// 弹出窗体配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            base.ConfigSelectionView();
            View.Property(p => p.ItemCode).HasLabel("产品编码");
            View.Property(p => p.ItemName).HasLabel("产品名称");
            View.Property(p => p.ItemType).UseEnumEditor().HasLabel("入库类型");
        }

        /// <summary>
        /// 查询视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            base.ConfigQueryView();
            View.Property(p => p.Item).HasLabel("产品编码");
            View.Property(p => p.ItemType).UseEnumEditor().HasLabel("入库类型");
        }

        /// <summary>
        /// 配置导入视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.PropertyRef(p => p.Item.Code).HasLabel("产品编码");
            View.Property(p => p.InspDimension).HasLabel("入库维度");
            View.Property(p => p.Qty).HasLabel("入库参数");
        }
    }
}
