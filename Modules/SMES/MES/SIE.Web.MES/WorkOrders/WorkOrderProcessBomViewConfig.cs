using SIE.Domain;
using SIE.Items;
using SIE.ManagedProperty;
using SIE.MES.WorkOrders;
using SIE.MetaModel.View;
using SIE.Tech.Processs;
using SIE.Web.Items._Extentions_;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.MES.WorkOrders
{
    /// <summary>
    /// 工单工序BOM视图配置
    /// </summary>
    [CompiledPropertyDeclarer]
    internal class WorkOrderProcessBomViewConfig : WebViewConfig<WorkOrderProcessBom>
    {
        /// <summary>
        /// 工单工序BOM图配置
        /// </summary>
        public const string WorkOrderProcessBomView = "WorkOrderProcessBomView";
        private const string SourceEntityType = "WorkOrderProcessBom";
        private const string ItemIdField = "ItemId";
        private const string DbField = "ItemExtProp";

        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(WorkOrder));
            View.DeclareExtendViewGroup(WorkOrderProcessBomView, WorkOrderViewConfig.ReadonlyView);
            if (ViewGroup == WorkOrderProcessBomView)
            {
                WorkOrderProcessBomConfigView();
            }
            else
            {
                if (ViewGroup == WorkOrderViewConfig.ReadonlyView)
                {
                    ReadOnlyCondfigView();
                }
            }
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.UseDefaultCommands();
            View.UseChildrenAsHorizontal().UseLayoutSize(-7, -3);
            using (View.OrderProperties())
            {
                View.Property(p => p.Item).UsePagingLookUpEditor(p =>
                    {
                        p.DisplayField = Item.CodeProperty.Name;
                        p.XType = "ProcessBomItem";
                        p.DataSourceProperty = "true";
                    })
                    .HasLabel("物料编码").Cascade(p => p.Unit, null)
                    .UseListSetting(e => { e.HelpInfo = "更改物料清空单位"; });
                View.Property(p => p.ItemName).HasLabel("物料名称").HasOrderNo(30);

                View.Property(p => p.ItemExtPropName).UseItemExtPropRecordsFieldEditor(p =>
                {
                    p.IsAllRequired = true;
                    p.SourceEntityType = SourceEntityType;
                    p.ItemIdField = ItemIdField;
                    p.DbField = DbField;
                }).Readonly().HasOrderNo(31).Show(ShowInWhere.Detail);

                View.Property(p => p.IsAlternative).Show(ShowInWhere.All).HasOrderNo(26);

                View.Property(p => p.ItemSpecificationModel);
                View.Property(p => p.SingleQty).UseItemUnitEditor();
                View.Property(p => p.Weight).Show(ShowInWhere.All);
                View.Property(p => p.ItemUnitName).ShowInList(80);
                View.Property(p => p.RoutingProcess).UseDataSource((e, c, r) =>
                {
                    var bom = e as WorkOrderProcessBom;
                    if (bom == null || bom.WorkOrder == null)
                        return null;
                    var routingProcessList = new EntityList<WorkOrderRoutingProcess>();
                    routingProcessList.AddRange(bom.WorkOrder.RoutingProcessList.Where(p => p.ProcessType == ProcessType.Assembly || p.ProcessType == ProcessType.BatchAssembly || p.ProcessType == ProcessType.Rework));
                    return routingProcessList;
                }).UsePagingLookUpEditor().Show(ShowInWhere.Hide).HasLabel("工序名称");
                View.Property(p => p.WorkStep).UseDataSource((t, p, s) =>
                {
                    WorkOrderProcessBom bom = t as WorkOrderProcessBom;
                    if (bom == null || bom.ProcessId == null)
                    {
                        return new EntityList<WorkStep>();
                    }
                    return RT.Service.Resolve<SIE.Tech.Processs.ProcessController>().GetWorkSteps((double)bom.ProcessId);
                }
                );
                View.Property(p => p.AlterGroup);
                View.Property(p => p.Alter);
                View.Property(p => p.Priority).UseSpinEditor(p =>
                {
                    p.AllowBlank = true;
                    p.MinValue = 0;
                    p.Step = 1;
                });
                View.Property(p => p.IsFeedingClose).Show().Readonly();
                View.Property(p => p.Werks).Show().Readonly();
                View.Property(p => p.Meins).Show().Readonly();
            }
        }

        /// <summary>
        /// 明细视图配置
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.InlineEdit();
            View.UseDefaultCommands();
            View.UseChildrenAsHorizontal().UseLayoutSize(-7, -3);
            using (View.OrderProperties())
            {
                View.Property(p => p.Item).UsePagingLookUpEditor(p => p.DisplayField = Item.CodeProperty.Name).HasLabel("物料编码").ShowInList(40);
                View.Property(p => p.ItemName).HasLabel("物料名称").ShowInList(50);
                View.Property(p => p.ItemSpecificationModel).HasLabel("基本型号").ShowInList(60);
                View.Property(p => p.SingleQty).UseItemUnitEditor().ShowInList(70);
                View.Property(p => p.Weight).Show(ShowInWhere.All);
                View.Property(p => p.ItemUnitName).ShowInList(80);
                View.Property(p => p.RoutingProcess).UseDataSource((e, c, r) =>
                {
                    var bom = e as WorkOrderProcessBom;
                    if (bom == null || bom.WorkOrder == null)
                        return null;
                    var routingProcessList = new EntityList<WorkOrderRoutingProcess>();
                    routingProcessList.AddRange(bom.WorkOrder.RoutingProcessList.Where(p => p.ProcessType == ProcessType.Assembly || p.ProcessType == ProcessType.BatchAssembly || p.ProcessType == ProcessType.Rework));
                    return routingProcessList;
                }).UsePagingLookUpEditor().Show(ShowInWhere.Hide).HasLabel("工序名称");
                View.Property(p => p.WorkStep).UseDataSource((t, p, s) =>
                {
                    WorkOrderProcessBom bom = t as WorkOrderProcessBom;
                    if (bom == null || bom.ProcessId == null)
                    {
                        return new EntityList<WorkStep>();
                    }
                    return RT.Service.Resolve<SIE.Tech.Processs.ProcessController>().GetWorkSteps((double)bom.ProcessId);
                }
                );
                View.Property(p => p.AlterGroup);
                View.Property(p => p.Alter);
                View.Property(p => p.Priority).UseSpinEditor(p =>
                {
                    p.AllowBlank = true;
                    p.MinValue = 0;
                    p.Step = 1;
                });
                View.Property(p => p.ItemExtPropName).UseItemExtPropRecordsFieldEditor(p =>
                {
                    p.AllowBlank = true;
                    p.SourceEntityType = SourceEntityType;
                    p.ItemIdField = ItemIdField;
                    p.DbField = DbField;
                }).Readonly().HasOrderNo(55).Show(ShowInWhere.All);
                View.Property(p => p.IsAlternative).Show(ShowInWhere.All).HasOrderNo(56);
            }
        }

        /// <summary>
        /// 工单工序BOM视图配置
        /// </summary>
        void WorkOrderProcessBomConfigView()
        {
            View.InlineEdit();
            View.UseCommands("SIE.Web.MES.WorkOrders.Commands.ProcessBomAddCommand", WebCommandNames.Edit, "SIE.Web.MES.WorkOrders.Commands.WorkOrderDetailDelCommand");
            View.UseChildrenAsHorizontal();
            View.UseLayoutSize(-6, -4);
            using (View.OrderProperties())
            {
                View.Property(p => p.Item).UseDataSource((e, p, s) =>
                {
                    return RT.Service.Resolve<ItemController>().GetItems(s, p);
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add(nameof(e.ItemName), nameof(e.Item.Name));
                    dic.Add(nameof(e.ItemSpecificationModel), nameof(e.Item.SpecificationModel));
                    dic.Add(nameof(e.ItemUnitName), nameof(e.Item.UnitName));
                    m.DicLinkField = dic;
                }).Show(ShowInWhere.All).HasLabel("物料编码");
                View.Property(p => p.ItemName).Show(ShowInWhere.All).HasLabel("物料名称").Readonly().HasOrderNo(50);
                View.Property(p => p.ItemSpecificationModel).Show(ShowInWhere.All).HasLabel("基本型号").Readonly();
                View.Property(p => p.SingleQty).UseItemUnitEditor(p => p.MinValue = 0).Show(ShowInWhere.All);
                View.Property(p => p.Weight).Show(ShowInWhere.All);
                View.Property(p => p.ItemUnitName).Readonly().Show(ShowInWhere.All).ShowInList(80);
                View.Property(p => p.RoutingProcessId)
                    .UsePagingLookUpEditor(p => { p.DataSourceProperty = "true"; p.XType = "ProcessBomRoutingProcess"; })
                    .Show(ShowInWhere.All).HasLabel("工序名称");
                View.Property(p => p.WorkStep).UseDataSource((t, p, s) =>
                {
                    WorkOrderProcessBom bom = t as WorkOrderProcessBom;
                    if (bom == null || bom.ProcessId == null)
                    {
                        return new EntityList<WorkStep>();
                    }
                    return RT.Service.Resolve<SIE.Tech.Processs.ProcessController>().GetWorkSteps((double)bom.ProcessId);
                }
                ).Show(ShowInWhere.All);
                View.Property(p => p.AlterGroup).Show(ShowInWhere.All);
                View.Property(p => p.Alter).Show(ShowInWhere.All);
                View.Property(p => p.Priority).UseSpinEditor(p =>
                {
                    p.AllowBlank = true;
                    p.MinValue = 0;
                    p.Step = 1;
                }).Show(ShowInWhere.All);
                View.Property(p => p.ItemExtPropName).UseItemExtPropRecordsFieldEditor(p =>
                {
                    p.IsAllRequired = true;
                    p.SourceEntityType = SourceEntityType;
                    p.ItemIdField = ItemIdField;
                    p.DbField = DbField;
                }).HasOrderNo(55).Show(ShowInWhere.All);
                View.Property(p => p.IsAlternative).Show(ShowInWhere.All).HasOrderNo(56);
                View.Property(p => p.IsFeedingClose).Show().Readonly();
            }
        }

        /// <summary>
        /// 只读视图配置
        /// </summary>
        void ReadOnlyCondfigView()
        {
            View.UseChildrenAsHorizontal();
            View.UseLayoutSize(-6, -4);
            using (View.OrderProperties())
            {
                View.Property(p => p.Item).Readonly().Show(ShowInWhere.All).HasLabel("物料编码");
                View.Property(p => p.ItemName).Readonly().Show(ShowInWhere.All).HasLabel("物料名称").HasOrderNo(30);
                View.Property(p => p.ItemExtPropName).Readonly().HasOrderNo(31).Show(ShowInWhere.All);
                View.Property(p => p.IsAlternative).Show(ShowInWhere.All).HasOrderNo(32);
                View.Property(p => p.ItemSpecificationModel).Readonly().Show(ShowInWhere.All).HasLabel("基本型号");
                View.Property(p => p.SingleQty).UseItemUnitEditor().Readonly().Show(ShowInWhere.All);
                View.Property(p => p.Weight).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.ItemUnitName).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.ProcessNameView).Readonly().Show(ShowInWhere.All).HasLabel("工序名称");
                View.Property(p => p.WorkStep).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.AlterGroup).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.Alter).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.Priority).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.Werks).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.Meins).Readonly().Show(ShowInWhere.All);
            }
        }
    }
}
