using SIE.Common;
using SIE.DIST;
using SIE.Domain;
using SIE.Items;
using SIE.MetaModel.View;
using SIE.ObjectModel;
using SIE.Web.Items.ViewModels;
using System.Linq;
using System.Text;

namespace SIE.Web.DIST
{
    /// <summary>
    /// 配送管理属性值视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class GoodsIssuePropertyValueExtViewConfig : WebViewConfig<PropertyValueViewModel>
    {
        /// <summary>
        /// 载具关联视图
        /// </summary>
        public static readonly string DistributionView = "DistributionView";

        #region 获取属性值 GoodIssueValueProperty
        /// <summary>
        /// 属性值
        /// </summary>
        [Label("值")]
        public static readonly Property<string> GoodIssueValueProperty = P<PropertyValueViewModel>.RegisterExtensionReadOnly("GoodIssueValue", typeof(GoodsIssuePropertyValueExtViewConfig),
            GetValue, PropertyValueViewModel.DefinitionIdProperty);

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="me">属性值视图模型</param>
        /// <returns>属性值</returns>
        public static string GetValue(PropertyValueViewModel me)
        { 
            StringBuilder sb = new StringBuilder();
            foreach (var value in me.Values)
            {
                sb.Append(value + ";");
            }

            return sb.ToString();
        }
        #endregion

        #region 获取属性 GoodIssueNameProperty
        /// <summary>
        /// 属性
        /// </summary>
        [Label("属性")]
        public static readonly Property<string> GoodIssueNameProperty = P<PropertyValueViewModel>.RegisterExtensionReadOnly("GoodIssueName", typeof(GoodsIssuePropertyValueExtViewConfig),
            GetName, PropertyValueViewModel.DefinitionIdProperty);

        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="me">属性视图模型</param>
        /// <returns>属性</returns>
        public static string GetName(PropertyValueViewModel me)
        {
            return me.Definition?.Name;
        }
        #endregion

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(DistributionView);
            if (ViewGroup == DistributionView)
            {
                DistributionListView();
            }
        }

        /// <summary>
        /// 配置列表
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete);
            using (View.OrderProperties())
            {
                View.Property(p => p.Definition).UsePagingLookUpOldEditor(p => { p.DataSourceProperty = "true"; p.XType = "GoodsIssuePropertyValue"; }).Show(ShowInWhere.All);
                View.Property(p => p.Value).UsePagingLookUpOldEditor(p =>
                {
                    p.Ischeckbox = true;
                    p.DisplayField = "Value";
                    p.BindDisplayField = "Value";
                    p.ValueField = "Value";
                    p.XType = "goodsissuepropertyvaluecb";//woprovaluecombopopup
                }).HasLabel("值").Show(ShowInWhere.All).Readonly(false);
            }
        }

        /// <summary>
        /// 载具关联视图
        /// </summary>
        void DistributionListView()
        {
            View.AssignAuthorize(typeof(GoodsIssue));
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.Property(GoodIssueNameProperty).Show(ShowInWhere.All).Readonly();
                View.Property(GoodIssueValueProperty).Show(ShowInWhere.All).Readonly();
            }
        }
    }

    /// <summary>
    /// 工单属性值扩展视图
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class GoodsIssuePropertyValueViewConfig : WebViewConfig<PropertyValueViewModel>
    {
        /// <summary>
        /// 配送管理属性值扩展视图配置
        /// </summary>
        public static readonly string goodsIssuePropertyValueExtendView = "goodsIssuePropertyValueExtendView";

        /// <summary>
        /// 配送管理物料属性子视图，ViewGroup
        /// </summary>
        public static readonly string GoodsIssuePropertyValueView = "GoodsIssuePropertyValueView";

        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(goodsIssuePropertyValueExtendView);
            if (ViewGroup == goodsIssuePropertyValueExtendView)
            {
                GoodsIssuePropertyValueExtendView();
            }
            if (ViewGroup == GoodsIssuePropertyValueView)
            {
                View.UseCommands("SIE.Web.DIST.GoodsIssue.Commands.GoodsIssuePropertyValueAddCommand", WebCommandNames.Edit, WebCommandNames.Delete);
                using (View.OrderProperties())
                {
                    View.Property(p => p.Definition).UseDataSource((source, pagingInfo, keyword) =>
                    {
                        var entity = source as PropertyValueViewModel;
                        if (entity != null)
                        {
                            var entitylist = RT.Service.Resolve<ItemController>().GetItemPropertys(entity.ItemId, "", null).
                            Select(m => m.Definition).Distinct((x, y) => x.Name == y.Name).AsEntityList();
                            return entitylist;
                        }
                        else
                        {
                            return null;
                        }
                    }).UsePagingLookUpOldEditor(p => p.XType = "propertyCombox").Show(ShowInWhere.All);
                    View.Property(BomPropertyValueViewModelViewConfig.BomValueProperty).Readonly(false).UseTextButtonFieldEditor(p => { p.ExtendJsObj = "SIE.Web.Items.ViewModels.PropertyValueEditor"; p.Editable = false; p.IsReadonly = false; }).Show(ShowInWhere.List);
                }
            }

        }

        /// <summary>
        /// 工单属性值扩展视图配置
        /// </summary>
        void GoodsIssuePropertyValueExtendView()
        {
            View.InlineEdit();
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete);
            using (View.OrderProperties())
            {
                View.Property(p => p.Definition).UsePagingLookUpOldEditor(p => { p.DataSourceProperty = "true"; p.XType = "WoProvalueDefinition"; }).Show(ShowInWhere.All).Readonly(false);
                View.Property(p => p.DefinitionValue).UsePagingLookUpOldEditor(p =>
                {
                    p.Ischeckbox = true;
                    p.DisplayField = "Value";
                    p.BindDisplayField = "Value";
                    p.ValueField = "Value";
                    p.XType = "goodsissuepropertyvaluecb";//woprovaluecombopopup
                }).HasLabel("值").Show(ShowInWhere.List).Readonly(false);

            }
        }
    }
}