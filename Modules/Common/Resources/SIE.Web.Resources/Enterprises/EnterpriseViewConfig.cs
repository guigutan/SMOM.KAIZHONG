using SIE.Domain;
using SIE.MetaModel.View;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;

namespace SIE.Web.Resources.Enterprises
{
    /// <summary>
    /// 组织架构视图配置
    /// </summary>
    [SIE.ManagedProperty.CompiledPropertyDeclarer]
    public class EnterpriseViewConfig : WebViewConfig<Enterprise>
    {
        #region 库存组织类型 InvOrgType
        /// <summary>
        /// 库存组织类型
        /// </summary>
        [Label("库存组织类型")]
        public static readonly Property<string> InvOrgTypeProperty = P<Enterprise>.RegisterExtensionReadOnly("InvOrgType", typeof(EnterpriseViewConfig),
            GetInvOrgType, Enterprise.InvOrgIdProperty);

        /// <summary>
        /// 库存组织类型
        /// </summary>
        /// <param name="me">企业模型</param>
        /// <returns>当前企业模型的库存组织类型</returns>
        public static string GetInvOrgType(Enterprise me)
        {
            return me.InvOrgId == 0 ? "通用" : "当前";
        }
        #endregion

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.DraggableForTree();
            View.InlineEdit();
            View.AddBehavior("SIE.Web.Resources.Enterprises.Behaviors.EnterpriseBehavior");
            View.UseCommands("SIE.Web.Resources.Enterprises.Commands.AddChildCommand", WebCommandNames.Edit, WebCommandNames.Save, WebCommandNames.Delete);
            View.UseCommands("SIE.Web.Resources.Commands.EnterpriseLevelTableCommand");
            View.Property(p => p.Code).ShowInList(width: 300);
            View.Property(p => p.Name);
            View.Property(p => p.Level).UsePagingLookUpEditor(p =>
            {
                p.XType = "enterpriseLevelComboList"; p.QueryMode = "remote"; p.DataSourceProperty = "enterpriseLevelComboList";
            });
            View.Property(p => p.IsResource).UseCheckEditor().Readonly(p => !p.LevelIsResource).HasLabel("资源").Show()
                .UseListSetting(e => { e.HelpInfo = "设为资源可编辑"; });
            View.Property(InvOrgTypeProperty).HasLabel("库存组织");
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 查询视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.LevelType).Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.DraggableForTree();
            View.Property(p => p.Code).ShowInList(width: 150);
            View.Property(p => p.Name).ShowInList(width: 150);
            View.Property(p => p.IsResource).Readonly().HasLabel("资源");
        }
    }
}
