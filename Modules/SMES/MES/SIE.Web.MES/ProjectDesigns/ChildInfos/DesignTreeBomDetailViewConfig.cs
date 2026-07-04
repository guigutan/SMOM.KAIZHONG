using SIE.Items;
using SIE.MES.ProjectDesigns;
using SIE.MES.ProjectDesigns.ChildInfos;
using SIE.MetaModel.View;
using SIE.Web.Items._Extentions_;
using SIE.Web.MES.ProjectDesigns.ChildCommands.BomCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ProjectDesigns.ChildInfos
{
    /// <summary>
    /// 工艺资料-产品Bom明细视图配置
    /// </summary>
    public class DesignTreeBomDetailViewConfig : WebViewConfig<DesignTreeBomDetail>
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
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete, typeof(TreeBomDetailImportCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.Item).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<ItemController>().GetItems(k, p);
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                    keyValuePairs.Add(nameof(e.ItemName), nameof(e.Item.Name));
                    keyValuePairs.Add(nameof(e.UnitName), nameof(e.Item.UnitName));
                    m.DicLinkField = keyValuePairs;
                }).HasLabel("物料编码").ShowInList(width: 120);
                View.Property(p => p.ItemName).Readonly().ShowInList(width: 120);
                View.Property(p => p.UnitQty).UseItemUnitEditor().ShowInList();
                View.Property(p => p.UnitName).Readonly().ShowInList();
                View.Property(p => p.LossRate).UseSpinEditor(e => e.MinValue = 0).ShowInList();
                View.Property(p => p.IsRecoilItem).ShowInList();
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
                View.Property(p => p.Item).HasLabel("物料编码").ShowInList(width: 120);
                View.Property(p => p.ItemName).Readonly().ShowInList(width: 120);
                View.Property(p => p.UnitQty).ShowInList();
                View.Property(p => p.UnitName).Readonly().ShowInList();
                View.Property(p => p.LossRate).ShowInList();
                View.Property(p => p.IsRecoilItem).ShowInList();
            }
        }
    }
}
