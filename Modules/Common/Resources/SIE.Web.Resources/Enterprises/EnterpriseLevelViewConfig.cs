using SIE.Domain;
using SIE.MetaModel.View;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Web.Resources.Enterprises.Commands;

namespace SIE.Web.Resources.Enterprises
{
    /// <summary>
    /// 企业层级视图配置
    /// </summary>
    [SIE.ManagedProperty.CompiledPropertyDeclarer]
    public class EnterpriseLevelViewConfig : WebViewConfig<EnterpriseLevel>
    {
        #region 父层级 ParentLevel
        /// <summary>
        /// 父层级
        /// </summary>
        [Label("父层级")]
        public static readonly Property<string> ParentLevelProperty = P<EnterpriseLevel>.RegisterExtensionReadOnly("ParentLevel", typeof(EnterpriseLevelViewConfig),
            GetParentLevel, EnterpriseLevel.TreePIdProperty);

        /// <summary>
        /// 父层级
        /// </summary>
        /// <param name="me">企业层级</param>
        /// <returns>当前企业层级的父层级</returns>
        public static string GetParentLevel(EnterpriseLevel me)
        {
            var parent = RF.GetById<EnterpriseLevel>(me.TreePId);
            return parent?.Name;
        }
        #endregion

        #region 编码是否只读 IsCodeReadonly
        /// <summary>
        /// 编码是否只读
        /// </summary>
        public static readonly Property<bool> IsCodeReadonlyProperty = P<EnterpriseLevel>.RegisterExtensionReadOnly("IsCodeReadonly", typeof(EnterpriseLevelViewConfig),
            GetIsCodeReadonly, EnterpriseLevel.TypeProperty);

        /// <summary>
        /// 编码是否只读
        /// </summary>
        /// <param name="me">企业层级</param>
        /// <returns>企业类型不为空时编码只读</returns>
        public static bool GetIsCodeReadonly(EnterpriseLevel me)
        {
            if (me.Type.HasValue)
                return true;
            return false;
        }
        #endregion

        #region 库存组织类型 InvOrgType
        /// <summary>
        /// 库存组织类型
        /// </summary>
        [Label("库存组织类型")]
        public static readonly Property<string> InvOrgTypeProperty = P<EnterpriseLevel>.RegisterExtensionReadOnly("InvOrgType", typeof(EnterpriseLevelViewConfig),
            GetInvOrgType, EnterpriseLevel.InvOrgIdProperty);

        /// <summary>
        /// 库存组织类型
        /// </summary>
        /// <param name="me">企业层级</param>
        /// <returns>当前企业层级的库存组织类型</returns>
        public static string GetInvOrgType(EnterpriseLevel me)
        {
            return me.InvOrgId == 0 ? "通用" : "当前";
        }
        #endregion

        /// <summary>
        /// 视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(Enterprise));
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.DraggableForTree();
            View.InlineEdit();
            View.UseCommands("SIE.Web.Resources.Commands.AddChildCommand", WebCommandNames.Edit, WebCommandNames.Save, WebCommandNames.Delete);
            View.UseCommands(typeof(EnableResourceCommand).FullName, typeof(DisableResourceCommand).FullName);
            View.Property(p => p.Code).ShowInList(width: 300);
            View.Property(p => p.Name);
            View.Property(p => p.Type);
            View.Property(p => p.IsResource).Readonly();
            View.Property(InvOrgTypeProperty).HasLabel("库存组织");
        }

        /// <summary>
        /// 查询视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
        }

        /// <summary>
        /// 选择视图配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Type);
            View.Property(p => p.IsResource);
        }
    }
}
