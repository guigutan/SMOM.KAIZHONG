using SIE.MES.WorkOrders;
using SIE.Packages;

namespace SIE.Web.MES.WorkOrders
{
    /// <summary>
    /// 工单包装规则选择视图
    /// </summary>
    public class WorkOrderPackageRuleViewConfig : WebViewConfig<ItemPackageRule>
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
            View.AssignAuthorize(typeof(WorkOrder));
            View.DeclareExtendViewGroup(WoSelectPackageRule);
            if (ViewGroup == WoSelectPackageRule)
            {
                ConfigWoSelectPackageRule();
            }
        }

        /// <summary>
        /// 配置工单选择包装关系视图
        /// </summary>
        private void ConfigWoSelectPackageRule()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).ShowInList(200).Readonly();
                View.Property(p => p.Name).ShowInList().Readonly();
                View.Property(p => p.Description).ShowInList().Readonly();
                View.ChildrenProperty(p => p.ItemPackageRuleDetailList).HasLabel("规则明细").Show(ChildShowInWhere.All);
                View.Property(p => p.CreateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            }
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
    public class ItemPackageRuleDetailViewConfig : WebViewConfig<ItemPackageRuleDetail>
    {
        /// <summary>
        ///  配置View
        /// </summary>
        protected override void ConfigView()
        {
            ////View.AssignAuthorize(typeof(ItemPackageRule));
        }

        /// <summary>
        /// 工单包装规则选择视图明细列表
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.Property(p => p.PackageUnitName).HasLabel("包装单位").Readonly();
            View.Property(p => p.Description).Readonly();
            View.Property(p => p.LevelQty).Readonly();
            View.Property(p => p.Qty).UseSpinEditor(p => p.AllowDecimals = false).Readonly();
            View.Property(p => p.Weight).ShowInList(80).Readonly();
            View.Property(p => p.Height).ShowInList(80).Readonly();
            View.Property(p => p.Volume).ShowInList(80).Readonly();
            View.Property(p => p.Length).ShowInList(80).Readonly();
            View.Property(p => p.Width).ShowInList(80).Readonly();
            View.Property(p => p.CreateBy).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateBy).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
        }
    }
}