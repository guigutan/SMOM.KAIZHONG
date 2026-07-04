using SIE.Inventory.Commom;
using SIE.Items;
using SIE.MetaModel.View;

namespace SIE.Web.Inventory.Common
{
    /// <summary>
    /// 批次 视图配置
    /// </summary>
    internal class LotViewConfig : WebViewConfig<Lot>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).HasLabel("批号").FixColumn().ShowInList(width: 150).Readonly();
                View.Property(p => p.ItemCode).FixColumn().ShowInList(width: 150).Readonly();
                View.Property(p => p.ItemName).Readonly().ShowInList(150);
                View.Property(p => p.ItemSpecificationModel).Readonly();
                View.Property(p => p.ItemExtPropName).ShowInList(width: 180).Readonly();
                View.Property(p => p.ItemUnitName).Readonly();
                View.Property(p => p.AsnNo).HasLabel("来源单号").Readonly();
                View.Property(p => p.LotAtt01).HasLabel("生产日期").Readonly().ShowInList(150);
                View.Property(p => p.LotAtt02).HasLabel("失效日期").Readonly().ShowInList(150);
                View.Property(p => p.LotAtt03).HasLabel("收货日期").Readonly().ShowInList(150);
                View.Property(p => p.LotAtt04).HasLabel("生产批次").Readonly();
                View.Property(p => p.LotAtt05).HasLabel("批次属性05").Readonly();
                View.Property(p => p.LotAtt06).HasLabel("批次属性06").Readonly();
                View.Property(p => p.LotAtt07).HasLabel("是否特采").Readonly();
                View.Property(p => p.LotAtt08).HasLabel("批次属性08").Readonly();
                View.Property(p => p.LotAtt09).HasLabel("批次属性09").Readonly();
                View.Property(p => p.LotAtt10).HasLabel("批次属性10").Readonly();
                View.Property(p => p.LotAtt11).HasLabel("批次属性11").Readonly();
                View.Property(p => p.LotAtt12).HasLabel("批次属性12").Readonly();
                View.Property(p => p.CreateDate).Readonly();
                View.Property(p => p.CreateByName).Readonly();
            }
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code).ShowInList(150);
            View.Property(p => p.ItemCode).Readonly().ShowInList(150);
            View.Property(p => p.LotAtt01).HasLabel("生产日期").Readonly().ShowInList(150);
            View.Property(p => p.LotAtt02).HasLabel("失效日期").Readonly().ShowInList(150);
            View.Property(p => p.LotAtt03).HasLabel("收货日期").Readonly().ShowInList(150);
            View.Property(p => p.LotAtt04).HasLabel("生产批次").Readonly();
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Item).UsePagingLookUpEditor(p =>
            {
                p.SearchFieldList.Add(Item.CodeProperty.Name);
                p.SearchFieldList.Add(Item.NameProperty.Name);
                p.SearchFieldList.Add(Item.SpecificationModelProperty.Name);
            }).Show(ShowInWhere.All);
            View.Property(p => p.Code);
            View.Property(p => p.LotAtt01).HasLabel("生产日期");
            View.Property(p => p.LotAtt02).HasLabel("最大有效期");
            View.Property(p => p.LotAtt03).HasLabel("收货日期");
            View.Property(p => p.LotAtt04).HasLabel("生产批次");
            View.Property(p => p.AsnNo).HasLabel("ASN单号");
        }
    }
}
