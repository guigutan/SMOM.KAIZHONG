using SIE.Domain;
using SIE.MES.DashBoard.Reports.FpySettings;
using SIE.MES.DashBoard.Reports.LineFPY;
using SIE.MES.DashBoard.Reports.ShopFPY;
using SIE.Resources;
using SIE.Resources.Enterprises;
using SIE.Wpf.Resources;
using System.Linq;

namespace SIE.Wpf.MES.DashBoard.Reports.FpySettings
{
    /// <summary>
    /// 车间直通率设置视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class ShopFpySettingViewConfig : WPFViewConfig<ShopFpySetting>
    {
        #region 车间只读 ShopReadOnly
        /// <summary>
        /// 车间只读
        /// </summary> 
        public static readonly Property<bool> ShopReadOnlyProperty = P<ShopFpySetting>.RegisterExtensionReadOnly("ShopReadOnly", typeof(ShopFpySettingViewConfig),
            GetShopReadOnly, ShopFpySetting.LineFpyListProperty);

        /// <summary>
        /// 车间只读
        /// </summary>
        public static bool GetShopReadOnly(ShopFpySetting me)
        {
            return me.LineFpyList.Any(p => p.Resource != null);
        }
        #endregion

        #region 更新人 UpdateName
        /// <summary>
        /// 更新人
        /// </summary> 
        public static readonly Property<string> UpdateNameProperty = P<ShopFpySetting>.RegisterExtensionReadOnly("UpdateName", typeof(ShopFpySettingViewConfig),
            GetUpdateName, ShopFpySetting.UpdateByProperty);

        /// <summary>
        /// 更新人
        /// </summary>
        public static string GetUpdateName(ShopFpySetting me)
        {
            return RF.GetById<Employee>(me.UpdateBy)?.Name;
        }
        #endregion

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.HasDelegate(ShopFpySetting.ShopIdProperty);
            View.UseDefaultBehaviors();
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(ShopReportViewModel), typeof(LineReportViewModel));
            View.UseCommands(typeof(AddSettingCommand), WPFCommandNames.ListEdit, WPFCommandNames.ListDelete, WPFCommandNames.ListSave);
            View.Property(p => p.Shop).HasLabel("车间编码").UseResourceWorkShopEditor(e => { e.ReloadDataOnPopping = true; e.DisplayMember = Enterprise.CodeProperty.Name; }).Readonly(ShopReadOnlyProperty);
            View.Property(p => p.Shop.Name).HasLabel("车间名称");
            View.Property(p => p.Desired).UseSpinEditor(e => { e.Decimals = 2; e.MinValue = 0; e.MaxValue = 100; });
            View.Property(p => p.Alarm).UseSpinEditor(e => { e.Decimals = 2; e.MinValue = 0; e.MaxValue = 100; });
            View.ChildrenProperty(p => p.LineFpyList).HasLabel("资源");
        }

        ///<summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            // 配置明细视图
        }

        ///<summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            // 配置下拉视图
        }
    }
}