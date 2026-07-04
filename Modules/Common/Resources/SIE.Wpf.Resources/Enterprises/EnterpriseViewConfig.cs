using SIE.Domain;
using SIE.Resources.Enterprises;
using SIE.Wpf.Resources.Enterprises.Commands;
using SIE.Wpf.Resources.Enterprises.ViewBehaviors;

namespace SIE.Wpf.Resources.Enterprises
{
    /// <summary>
    /// 组织架构视图配置
    /// </summary>
    [SIE.ManagedProperty.CompiledPropertyDeclarer]
    public class EnterpriseViewConfig : WPFViewConfig<Enterprise>
    {
        #region 父级 ParentName
        /// <summary>
        /// 父级
        /// </summary>
        public static readonly Property<string> ParentNameProperty = P<Enterprise>.RegisterExtensionReadOnly("ParentName", typeof(EnterpriseViewConfig),
            GetParentName, Enterprise.TreePIdProperty);

        /// <summary>
        /// 父级
        /// </summary>
        /// <param name="me">企业模型</param>
        /// <returns>当前企业模型的父级</returns>
        public static string GetParentName(Enterprise me)
        {
            var parent = RF.GetById<Enterprise>(me.TreePId);
            return parent?.Name;
        }
        #endregion

        #region 是否有子级 HasChild
        /// <summary>
        /// 是否有子级
        /// </summary>
        public static readonly Property<bool> HasChildProperty = P<Enterprise>.RegisterExtensionReadOnly("HasChild", typeof(EnterpriseViewConfig),
            GetHasChild, Enterprise.IdProperty);

        /// <summary>
        /// 是否有子级
        /// </summary>
        /// <param name="me">企业模型</param>
        /// <returns>true/false</returns>
        public static bool GetHasChild(Enterprise me)
        {
            return RT.Service.Resolve<EnterpriseController>().EnterpriseHasChild(me.Id);
        }
        #endregion

        #region 库存组织类型 InvOrgType
        /// <summary>
        /// 库存组织类型
        /// </summary>
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
            //return "dangqian";
        }
        #endregion

        #region 资源是否可修改 IsResourceModifyProperty
        /// <summary>
        /// 资源是否可修改
        /// </summary>
        public static readonly Property<bool> IsResourceModifyProperty = P<Enterprise>.RegisterExtensionReadOnly("IsResourceModify", typeof(EnterpriseViewConfig),
            GetIsResourceModify, Enterprise.LevelIdProperty);

        /// <summary>
        /// 资源是否可修改
        /// </summary>
        /// <param name="me">企业模型</param>
        /// <returns>true/false</returns>
        public static bool GetIsResourceModify(Enterprise me)
        {
            return !me.Level.IsResource;
        }
        #endregion

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior(typeof(EnterpriseBehavior));
            View.InlineEdit();
            View.AddBehavior(typeof(EnterpriseChangeBehavior));
            View.UseCommands(typeof(AddChildCommand), WPFCommandNames.ListEdit, WPFCommandNames.ListSave, typeof(DeleteEnterpriseCommand));
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Level).UsePagingLookUpEditor(p =>
            {
                p.ReloadDataOnPopping = true;
            }).UseDataSource((e, p, r) =>
            {
                var enterprise = e as Enterprise;
                var result = new EntityList<EnterpriseLevel>();
                var treeParent = RF.GetById<Enterprise>(enterprise?.TreePId);
                Enterprise parent = treeParent;
                if (parent == null)
                {
                    parent = (Enterprise)e.ParentList.Find(enterprise?.TreePId, true);
                }

                if (parent != null)
                    result.AddRange(RT.Service.Resolve<EnterpriseController>().GetEnterpriseLevelsByParentId(p, r, parent.LevelId));
                return result;
            }).Readonly(HasChildProperty);
            //View.Property(p => p.Level.IsResource).UseEditor(WPFEditorNames.Check).HasLabel("资源");
            View.Property(p => p.IsResource).UseEditor(WPFEditorNames.Check).Readonly(IsResourceModifyProperty).HasLabel("资源");
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
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.IsResource).UseEditor(WPFEditorNames.Check).Readonly().HasLabel("资源");
            //View.Property(p => p.Level.IsResource).UseEditor(WPFEditorNames.Check).HasLabel("资源");
            // View.Property(InvOrgTypeProperty).HasLabel("库存组织");
        }
    }
}
