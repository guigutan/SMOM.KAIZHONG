using SIE.Domain;
using SIE.Items;
using SIE.MES.Routings.RoutingBoms;
using SIE.MetaModel.View;
using SIE.Web.Items._Extentions_;
using System.Collections.Generic;

namespace SIE.Web.MES.Routings.RoutingBoms
{
    /// <summary>
    /// 工序Bom视图配置
    /// </summary>
    internal class RoutingBomDetailViewConfig : WebViewConfig<RoutingBomDetail>
    {
        /// <summary>
        /// 工序Bom视图配置
        /// </summary>
        public const string RoutingBomDetailView = "RoutingBomDetailView";

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.InlineEdit();
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands("SIE.Web.MES.Routings.RoutingBoms.Commands.RoutingBomDetailAddCommand",
                "SIE.Web.MES.Routings.RoutingBoms.Commands.RoutingBomDetailEditCommand",
                WebCommandNames.Delete,
                WebCommandNames.Save,
                "SIE.Web.MES.Routings.RoutingBoms.Commands.RoutingBomDetailImportCommand",
                WebCommandNames.ExportXls);
            View.AddBehavior("SIE.Web.MES.Routings.RoutingBoms.Scripts.Behaviors.RoutingBomDetailBehavior");
            View.Property(p => p.Material).HasLabel("物料编码").UseDataSource((source, pagingInfo, keyword) =>
                {
                    RoutingBomDetail bom = source as RoutingBomDetail;
                    if (bom == null || bom.RoutingBom == null)
                    {
                        return new EntityList<Item>();
                    }
                    return RT.Service.Resolve<RoutingBomController>()
                             .GetRoutingBomItemByProductId(
                                pagingInfo,
                                bom.RoutingBom.ProductId,
                                keyword,
                                bom.RoutingBom.ProcessSegmentId);
                })
                .UsePagingLookUpEditor((e, o) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(o.MaterialName), nameof(o.Material.Name));
                    keyValues.Add(nameof(o.SpecificationModel), nameof(o.Material.SpecificationModel));
                    keyValues.Add(nameof(o.MaterialUnit), nameof(o.Material.UnitName));
                    keyValues.Add(nameof(o.IsAllowEdit), nameof(o.Material.EnableExtendProperty));
                    e.DicLinkField = keyValues;
                }).ShowInList(200);//.Readonly(p => p.PersistenceStatus != PersistenceStatus.New);
            View.Property(p => p.MaterialName).HasLabel("物料名称").HasOrderNo(10).Readonly();
            View.Property(p => p.ItemExtPropName).UseItemExtPropRecordsFieldEditor(p =>
            {
                p.IsAllRequired = true;
                p.ItemIdField = "MaterialId";
                p.DbField = "ItemExtProp";
            }).Readonly(p => !p.IsAllowEdit).HasLabel("物料扩展属性").HasOrderNo(11);


            View.Property(p => p.SpecificationModel).HasLabel("规格型号").HasOrderNo(20).Readonly();
            View.Property(p => p.MainMaterial).HasLabel("主料编码").HasOrderNo(30);
            View.Property(p => p.Amount).DefaultValue(1).UseItemUnitEditor(e => { e.MinValue = 0; e.ItemIdField = "MaterialId";}).HasOrderNo(40);
            View.Property(p => p.MaterialUnit).HasLabel("单位").HasOrderNo(50).Readonly();
            View.Property(p => p.RoutingProcessId).HasLabel("工序").Show(ShowInWhere.All).UsePagingLookUpEditor((m, e) =>
            {
                m.MethodClassName = "SIE.Web.MES.Routings.RoutingBoms.Scripts.RoutingProcessLookUp";
                m.QueryMode = "remote";
                m.DataSourceProperty = "RoutingProcessLookUp";
                m.ReloadDataOnPopping = true;

                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.ProcessIndex), nameof(e.RoutingProcess.Index));                
                m.DicLinkField = keyValues;

            }).HasOrderNo(60);
            View.Property(p => p.ProcessIndex).HasLabel("工序顺序").HasOrderNo(61).Readonly();
            View.Property(p => p.WorkStep).UseDataSource((t, p, s) =>
            {
                RoutingBomDetail bom = t as RoutingBomDetail;
                if (bom == null || bom.RoutingBom == null)
                {
                    return new EntityList<SIE.Tech.Processs.WorkStep>();
                }
                return RT.Service.Resolve<RoutingBomController>().GetWorkSteps(bom.RoutingProcessId);
            }).HasLabel("工步").HasOrderNo(70);
            View.Property(p => p.IsAttachment).HasOrderNo(80);
            View.Property(p => p.Description).HasLabel("备注").HasOrderNo(90);
        }

        protected override void ConfigImportView()
        {
            View.Property(p => p.ProductCode).HasLabel("产品编码").Show(ShowInWhere.All);
            View.Property(p => p.RoutingBom).HasLabel("工艺路线").Show(ShowInWhere.All);
            View.Property(p => p.RoutingVersion).HasLabel("工艺路线版本").Show(ShowInWhere.All);
            View.Property(p => p.Segment).HasLabel("工段").Show(ShowInWhere.All);
            View.Property(p => p.Material).HasLabel("物料编码").Show(ShowInWhere.All);
            View.Property(p => p.ItemExtProp).HasLabel("物料属性值").Show(ShowInWhere.All);
            View.Property(p => p.ItemExtPropName).HasLabel("物料属性值名称").Show(ShowInWhere.All);
            View.Property(p => p.ProcessNameForImport).HasLabel("工序名称").Show(ShowInWhere.All);            
            View.Property(p => p.WorkStep).HasLabel("工步名称").Show(ShowInWhere.All);
            View.Property(p => p.Amount).HasLabel("单位用量").Show(ShowInWhere.All);
            View.Property(p => p.IsAttachment).HasLabel("是否附件").Show(ShowInWhere.All);
            View.Property(p => p.Description).HasLabel("备注").Show(ShowInWhere.All);
        }
    }
}