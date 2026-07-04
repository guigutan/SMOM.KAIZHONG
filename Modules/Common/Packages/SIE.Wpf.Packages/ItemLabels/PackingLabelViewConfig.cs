using SIE.Domain;
using SIE.Packages.ItemLabels;
using SIE.Wpf.Packages.ItemLabels.ViewBehaviors;
using System;

namespace SIE.Wpf.Packages.ItemLabels
{
    /// <summary>
    /// 视图配置
    /// </summary>
    internal class PackingLabelViewConfig : WPFViewConfig<PackingLabel>
    {
        /// <summary>
        /// 分类子视图
        /// </summary>
        public const string LevelDetailView = "LevelDetailView";

        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(new string[] { LevelDetailView });
            if (ViewGroup == LevelDetailView)
            {
                ConfigLabelLevelDetail();
            }
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands(WPFCommandNames.Export);
            using (View.OrderProperties())
            {
                View.Property(p => p.PackageNo);
                View.Property(p => p.No);
                View.Property(p => p.PackingType.Name).HasLabel("类型");
                View.Property(p => p.ItemPackageRule.Code).HasLabel("物料包装规则");
                View.Property(p => p.ItemPackageRule.Name).HasLabel("物料包装规则名称");
                View.Property(p => p.Item).HasLabel("物料编码");
                View.Property(p => p.Item.Name).HasLabel("物料名称");
                View.Property(p => p.Item.SpecificationModel).HasLabel("物料规格");
                View.Property(p => p.Item.Unit.Name).HasLabel("主单位");
                View.Property(p => p.Qty);
                View.Property(p => p.IsScrapped);
                View.Property(p => p.Lot).HasLabel("物料批次号");
                View.Property(p => p.ProductionDate);
                View.Property(p => p.InvalidDate);
                View.Property(p => p.CollectTime);
                View.Property(p => p.ProductBatch);
                View.Property(p => p.IsPrinted);
                View.Property(p => p.PrintTimes);
                View.Property(p => p.IsSequence);
            }
        }

        /// <summary>
        /// 配置层级明细视图
        /// </summary>
        private void ConfigLabelLevelDetail()
        {
            View.ClearCommands();
            View.AddBehavior(typeof(PackingLabelBehavior));
            View.DisableEditing();
            View.UseCommands(WPFCommandNames.Export);
            using (View.OrderProperties())
            {
                View.Property(p => p.No).Readonly().Show();
                View.Property(p => p.PackingType.Name).HasLabel("类型").Show();
                View.Property(p => p.ItemPackageRule.Code).HasLabel("物料包装规则").Show();
                View.Property(p => p.ItemPackageRule.Name).HasLabel("物料包装规则名称").Show();
                View.Property(p => p.Item).Readonly().HasLabel("物料编码").Show();
                View.Property(p => p.Item.Name).HasLabel("物料名称").Show();
                View.Property(p => p.Item.SpecificationModel).HasLabel("物料规格").Show();
                View.Property(p => p.Item.Unit.Name).HasLabel("主单位").Show();
                View.Property(p => p.Qty).Readonly().Show();
                View.Property(p => p.IsScrapped).Readonly().Show();
                View.Property(p => p.Lot).Readonly().HasLabel("物料批次号").Show();
                View.Property(p => p.ProductionDate).Readonly().Show();
                View.Property(p => p.InvalidDate).Readonly().Show();
                View.Property(p => p.CollectTime).Readonly().Show();
                View.Property(p => p.ProductBatch).Readonly().Show();
                View.Property(p => p.IsPrinted).Readonly().Show();
                View.Property(p => p.PrintTimes).Readonly().Show();
                View.Property(p => p.IsSequence).Readonly().Show();
                View.Property(p => p.AsnNo).Readonly().Show();
                View.Property(p => p.SourceData).Readonly().HasLabel("明细行号").Show();
            }
        }
    }
}
