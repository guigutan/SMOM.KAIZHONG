using SIE.Packages;

namespace SIE.Wpf.MES.WorkOrders
{
    /// <summary>
    /// 工单包装规则选择视图
    /// </summary>
    public class WorkOrderPackageRuleViewConfig : WPFViewConfig<ItemPackageRule>
    {
        /// <summary>
        /// 工单选择包装关系视图
        /// </summary>
        public const string WoSelectPackageRule = "ReadonlyView";

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(WoSelectPackageRule);
            if (ViewGroup == WoSelectPackageRule)
                ConfigWoSelectPackageRule();
        }

        /// <summary>
        /// 配置工单选择包装关系视图
        /// </summary>
        private void ConfigWoSelectPackageRule()
        {
            View.Property(p => p.Code).ShowInList().UseListSetting(w => w.ListGridWidth = 200);
            View.Property(p => p.Name).ShowInList();
            View.Property(p => p.Description).ShowInList();
            View.Property(p => p.CreateBy).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateBy).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 查询
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
        }
    }

    /// <summary>
    ///  工单包装规则选择视图明细列表
    /// </summary>
    public class ItemPackageRuleDetailViewConfig : WPFViewConfig<ItemPackageRuleDetail>
    {
        /// <summary>
        /// 工单包装规则选择视图明细列表
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.Property(p => p.PackageUnitName).HasLabel("包装单位");
            View.Property(p => p.Description);
            View.Property(p => p.LevelQty);
            View.Property(p => p.Qty).UseSpinEditor(p => p.Decimals = 0);
            View.Property(p => p.Weight).Show(ShowInWhere.All).UseListSetting(p => p.ListGridWidth = 80);
            View.Property(p => p.Height).Show(ShowInWhere.All).UseListSetting(p => p.ListGridWidth = 80);
            View.Property(p => p.Volume).Show(ShowInWhere.All).UseListSetting(p => p.ListGridWidth = 80);
            View.Property(p => p.Length).Show(ShowInWhere.All).UseListSetting(p => p.ListGridWidth = 80);
            View.Property(p => p.Width).Show(ShowInWhere.All).UseListSetting(p => p.ListGridWidth = 80);
            View.Property(p => p.CreateBy).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateBy).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
        }
    }
}