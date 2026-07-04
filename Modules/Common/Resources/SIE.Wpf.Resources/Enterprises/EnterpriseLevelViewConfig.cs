using SIE.Domain;
using SIE.Resources.Enterprises;
using SIE.Wpf.Resources.Enterprises.Commands;

namespace SIE.Wpf.Resources.Enterprises
{
    /// <summary>
    /// 企业层级视图配置
    /// </summary>
    [SIE.ManagedProperty.CompiledPropertyDeclarer]
    public class EnterpriseLevelViewConfig : WPFViewConfig<EnterpriseLevel>
    {
        #region 父层级 ParentLevel
        /// <summary>
        /// 父层级
        /// </summary>
        internal static readonly Property<string> ParentLevelProperty = P<EnterpriseLevel>.RegisterExtensionReadOnly("ParentLevel", typeof(EnterpriseLevelViewConfig),
            GetParentLevel, EnterpriseLevel.TreePIdProperty);

        /// <summary>
        /// 父层级
        /// </summary>
        /// <param name="me">企业层级</param>
        /// <returns>当前企业层级的父层级</returns>
        internal static string GetParentLevel(EnterpriseLevel me)
        {
            var parent = RF.GetById<EnterpriseLevel>(me.TreePId);
            return parent?.Name;
        }
        #endregion

        #region 编码是否只读 IsCodeReadonly
        /// <summary>
        /// 编码是否只读
        /// </summary>
        internal static readonly Property<bool> IsCodeReadonlyProperty = P<EnterpriseLevel>.RegisterExtensionReadOnly("IsCodeReadonly", typeof(EnterpriseLevelViewConfig),
            GetIsCodeReadonly, EnterpriseLevel.TypeProperty);

        /// <summary>
        /// 编码是否只读
        /// </summary>
        /// <param name="me">企业层级</param>
        /// <returns>企业类型不为空时编码只读</returns>
        internal static bool GetIsCodeReadonly(EnterpriseLevel me)
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
        internal static readonly Property<string> InvOrgTypeProperty = P<EnterpriseLevel>.RegisterExtensionReadOnly("InvOrgType", typeof(EnterpriseLevelViewConfig),
            GetInvOrgType, EnterpriseLevel.InvOrgIdProperty);

        /// <summary>
        /// 库存组织类型
        /// </summary>
        /// <param name="me">企业层级</param>
        /// <returns>当前企业层级的库存组织类型</returns>
        internal static string GetInvOrgType(EnterpriseLevel me)
        {
            return me.InvOrgId == 0 ? "通用" : "当前";
        }
        #endregion

        #region 资源是否只读 IsResourceReadonly
        /// <summary>
        /// 资源是否只读
        /// </summary>
        internal static readonly Property<bool> IsResourceReadonlyProperty = P<EnterpriseLevel>.RegisterExtensionReadOnly("IsResourceReadonly", typeof(EnterpriseLevelViewConfig),
            GetIsResourceReadonly, EnterpriseLevel.CodeProperty);

        /// <summary>
        /// 资源是否只读
        /// </summary>
        internal static bool GetIsResourceReadonly(EnterpriseLevel me)
        {
            return me.PersistenceStatus != PersistenceStatus.New;
        }
        #endregion

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior(typeof(EnterpriseLevelBehavior));
            View.InlineEdit();
            View.UseCommands(typeof(AddLevelChildCommand), WPFCommandNames.ListEdit, WPFCommandNames.ListSave, typeof(DeleteLevelCommand), typeof(EnableResourceCommand), typeof(DisableResourceCommand));
            View.Property(p => p.Code);//.Readonly(IsCodeReadonlyProperty);--类型不能为空时，允许修改Code
            View.Property(p => p.Name);
            View.Property(p => p.Type);
            View.Property(p => p.IsResource).Readonly(IsResourceReadonlyProperty);
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
