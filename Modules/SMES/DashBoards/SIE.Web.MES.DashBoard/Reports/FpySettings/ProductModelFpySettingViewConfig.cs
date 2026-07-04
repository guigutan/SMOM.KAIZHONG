using SIE.Domain;
using SIE.MES.DashBoard.Reports.FpySettings;
using SIE.MES.DashBoard.Reports.ProductFPY;
using SIE.MetaModel.View;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.MES.DashBoard.Reports.FpySettings
{
    /// <summary>
    /// 产品机型直通率设置视图配置
    /// </summary> 
    [ManagedProperty.CompiledPropertyDeclarer]
    public class ProductModelFpySettingViewConfig : WebViewConfig<ProductModelFpySetting>
    {
        #region 机型只读 ModelReadOnly
        /// <summary>
        /// 机型只读
        /// </summary> 
        public static readonly Property<bool> ModelReadOnlyProperty = P<ProductModelFpySetting>.RegisterExtensionReadOnly("ModelReadOnly", typeof(ProductModelFpySettingViewConfig),
            GetModelReadOnly, ProductModelFpySetting.ProductFpyListProperty);

        /// <summary>
        /// 机型只读
        /// </summary>
        /// <param name="me">实体</param>
        /// <returns>是否空</returns>
        public static bool GetModelReadOnly(ProductModelFpySetting me)
        {
            return me.ProductFpyList.Any(p => p.Product != null);
        }
        #endregion

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.HasDelegate(ProductModelFpySetting.ModelIdProperty);
            View.AssignAuthorize(typeof(ProductReportViewModel));
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands("SIE.Web.MES.DashBoard.Reports.FpySettings.Commands.AddSettingCommand", WebCommandNames.Edit, WebCommandNames.Delete, WebCommandNames.Save);
            View.Property(p => p.Model).HasLabel("产品机型编码").UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.ProductModelName), nameof(e.Model.Name));
                m.DicLinkField = keyValues;
            }).Readonly(ModelReadOnlyProperty).UseListSetting(e => { e.HelpInfo = "产品直通率设置存在产品不可编辑"; });
            View.Property(p => p.ProductModelName).HasLabel("产品机型名称").Readonly(true);
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
            View.ChildrenProperty(p => p.ProductFpyList).HasLabel("产品");
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            // 配置明细视图
        }

        /// <summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            // 配置下拉视图
        }
    }
}