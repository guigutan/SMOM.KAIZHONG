using SIE.Domain;
using SIE.MES.ProjectDesigns;
using SIE.MES.ProjectDesigns.ChildInfos;
using SIE.MES.Routings.RoutingBoms;
using SIE.MetaModel.View;
using SIE.Web.Items._Extentions_;
using SIE.Web.MES.ProjectDesigns.ChildCommands.RoutingCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ProjectDesigns.ChildInfos
{
    /// <summary>
    /// 工艺资料-产品工艺路线工序bom
    /// </summary>
    public class DesignTreeRoutingProBomViewConfig : WebViewConfig<DesignTreeRoutingProBom>
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
            View.UseCommands(typeof(TreeRoutingBomAddCommand).FullName, WebCommandNames.Edit, WebCommandNames.Delete, typeof(TreeRoutingBomImpCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.Material).UseDataSource((e, p, k) =>
                {
                    var bom = e as DesignTreeRoutingProBom;
                    return RT.Service.Resolve<RoutingBomController>().GetRoutingBomItemByProductId(p, bom.ProductId, k, null);
                }).UsePagingLookUpEditor((e, o) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(o.MaterialName), nameof(o.Material.Name));
                    keyValues.Add(nameof(o.SpecificationModel), nameof(o.Material.SpecificationModel));
                    keyValues.Add(nameof(o.UnitName), nameof(o.Material.UnitName));
                    e.DicLinkField = keyValues;
                }).HasLabel("物料编码").ShowInList(width: 150);
                View.Property(p => p.MaterialName).Readonly().ShowInList(width: 150);
                View.Property(p => p.SpecificationModel).Readonly().ShowInList(width: 150);
                View.Property(p => p.Amount).DefaultValue(1).UseItemUnitEditor(e => { e.MinValue = 0; e.ItemIdField = "MaterialId"; }).ShowInList();
                View.Property(p => p.UnitName).Readonly().ShowInList();
                View.Property(p => p.RoutingProcess).UseDataSource((e, p, k) =>
                {
                    var probom = e as DesignTreeRoutingProBom;
                    return RT.Service.Resolve<ProjectDesignController>().GetDesignTreeRoutingDetails(probom.DesignTreeRoutingId, p, k);
                }).HasLabel("工序").UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.Index), nameof(e.RoutingProcess.Index));
                    m.DicLinkField = keyValues;
                }).ShowInList(width: 120);
                View.Property(p => p.Index).Readonly().ShowInList();
                View.Property(p => p.Remark).ShowInList();
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
                View.Property(p => p.Material).HasLabel("物料编码").ShowInList(width: 150);
                View.Property(p => p.MaterialName).Readonly().ShowInList(width: 150);
                View.Property(p => p.SpecificationModel).Readonly().ShowInList(width: 150);
                View.Property(p => p.Amount).DefaultValue(1).UseItemUnitEditor(e => { e.MinValue = 0; e.ItemIdField = "MaterialId"; }).ShowInList();
                View.Property(p => p.UnitName).Readonly().ShowInList();
                View.Property(p => p.RoutingProcess).HasLabel("工序").ShowInList(width: 120);
                View.Property(p => p.Index).Readonly().ShowInList();
                View.Property(p => p.Remark).ShowInList();
            }
        }
    }
}
