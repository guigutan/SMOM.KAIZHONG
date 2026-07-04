using SIE.Items;
using SIE.MES.ProjectDesigns;
using SIE.MES.ProjectDesigns.ChildInfos;
using SIE.MetaModel.View;
using SIE.Web.MES.ProjectDesigns.ChildCommands.BomCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ProjectDesigns.ChildInfos
{
    /// <summary>
    /// 工艺资料-产品Bom视图配置
    /// </summary>
    public class DesignTreeBomViewConfig : WebViewConfig<DesignTreeBom>
    {
        /// <summary>
        /// 查询视图
        /// </summary>
        public const string LookUpViewGroup = "LookUpViewGroup";

        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(ProjectDesign));
            View.DeclareExtendViewGroup(LookUpViewGroup);
            View.InlineEdit();
            if (ViewGroup == LookUpViewGroup)
            {
                ReadOnlyView();
            }
        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(TreeBomInitCommand).FullName, WebCommandNames.Edit, typeof(TreeBomSaveCommand).FullName, typeof(TreeBomUpdateCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.TreeLevel).Readonly().ShowInList();
                View.Property(p => p.BomCode).ShowInList(width: 120);
                View.Property(p => p.BomName).ShowInList(width: 120);
                View.Property(p => p.ProductCode).Readonly().ShowInList(width: 120);
                View.Property(p => p.ProductName).Readonly().ShowInList(width: 120);
                View.Property(p => p.Version).UseListSetting(e => { e.HelpInfo = "根据产品BOM版本生成规则(配置项--产品BOM版本生成规则)"; });
                View.Property(p => p.HasUp).Readonly().ShowInList();
                View.ChildrenProperty(p => p.DetailList).Show(ChildShowInWhere.All);
            }
        }

        /// <summary>
        /// 查看界面视图
        /// </summary>
        private void ReadOnlyView()
        {
            View.ClearCommands();
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.TreeLevel).Readonly().ShowInList();
                View.Property(p => p.BomCode).ShowInList(width: 120);
                View.Property(p => p.BomName).ShowInList(width: 120);
                View.Property(p => p.ProductCode).Readonly().ShowInList(width: 120);
                View.Property(p => p.ProductName).Readonly().ShowInList(width: 120);
                View.Property(p => p.Version).UseListSetting(e => { e.HelpInfo = "根据产品BOM版本生成规则(配置项--产品BOM版本生成规则)"; });
                View.Property(p => p.HasUp).Readonly().ShowInList();
                View.ChildrenProperty(p => p.DetailList).UseViewGroup(DesignTreeBomDetailViewConfig.LookUpViewGroup).Show(ChildShowInWhere.All);
            }
        }
    }
}
