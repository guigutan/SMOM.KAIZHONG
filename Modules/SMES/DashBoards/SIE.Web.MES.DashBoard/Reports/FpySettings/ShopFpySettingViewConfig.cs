using SIE.Domain;
using SIE.MES.DashBoard.Reports.FpySettings;
using SIE.MES.DashBoard.Reports.LineFPY;
using SIE.MES.DashBoard.Reports.ShopFPY;
using SIE.MetaModel.View;
using SIE.Resources;
using SIE.Resources.Enterprises;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.MES.DashBoard.Reports.FpySettings
{
    /// <summary>
    /// 车间直通率设置视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class ShopFpySettingViewConfig : WebViewConfig<ShopFpySetting>
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
            View.AssignAuthorize(typeof(ShopReportViewModel), typeof(LineReportViewModel));
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete, WebCommandNames.Save);
            View.Property(p => p.Shop).HasLabel("车间编码")
                .UseResourceWorkShopEditor(p => p.DisplayField = Enterprise.CodeProperty.Name).Readonly(ShopReadOnlyProperty).UsePagingLookUpEditor(
                (m, e) =>
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add(nameof(e.ShopName), nameof(Enterprise.Name));
                    m.DicLinkField = dic;
                }).UseListSetting(e => { e.HelpInfo = "资源直通率存在资源不可编辑"; });
            View.Property(p => p.ShopName).HasLabel("车间名称");
            View.Property(p => p.Desired).UseSpinEditor(p =>
            {
                p.MinValue = 0;
                p.MaxValue = 100;
                p.DecimalPrecision = 2;
            });
            View.Property(p => p.Alarm).UseSpinEditor(p =>
            {
                p.MinValue = 0;
                p.MaxValue = 100;
                p.DecimalPrecision = 2;
            });
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
