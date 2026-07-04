using SIE.Domain;
using SIE.Items;
using SIE.Items.ViewModels;
using SIE.ManagedProperty;
using SIE.MES.WorkOrders;
using SIE.Tech.Processs;
using SIE.Wpf.MES.WorkOrders.ViewBehaviors;
using System.Linq;

namespace SIE.Wpf.MES.WorkOrders
{
    /// <summary>
    /// 工单工序BOM视图配置
    /// </summary>
    [CompiledPropertyDeclarer]
    internal class WorkOrderProcessBomViewConfig : WPFViewConfig<WorkOrderProcessBom>
    {
        /// <summary>
        /// 查看工单视图ViewGroup
        /// </summary>
        public const string ReadonlyView = "ReadonlyView";

        /// <summary>
        /// 工单工序BOM视图配置
        /// </summary>
        public const string WorkOrderProcessBomView = "WorkOrderProcessBomView";

        /// <summary>
        /// 叫料单工序BOM视图
        /// </summary>
        public const string CallMaterialBomView = "CallMaterialBomView";

        #region 工艺路线工序名称 RoutingProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        public static readonly Property<string> RoutingProcessNameProperty = P<WorkOrderProcessBom>.RegisterExtensionReadOnly("RoutingProcessName", typeof(WorkOrderProcessBomViewConfig),
            GetRoutingProcessName, WorkOrderProcessBom.RoutingProcessProperty);

        /// <summary>
        /// 获取工序名称
        /// </summary>
        /// <param name="me">工单工序BOM</param>
        /// <returns>工序名称</returns>
        public static string GetRoutingProcessName(WorkOrderProcessBom me)
        {
            if (me == null || me.RoutingProcess == null)
                return "NIL";
            return me.RoutingProcess.Name;
        }
        #endregion

        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(WorkOrderProcessBomView, CallMaterialBomView, ReadonlyView);
            if (ViewGroup == WorkOrderProcessBomView)
            {
                WorkOrderProcessBomConfigView();
            }
            else if (ViewGroup == ReadonlyView)
            {
                ReadOnlyCondfigView();
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
            View.GroupBy(RoutingProcessNameProperty);
            using (View.OrderProperties())
            {
                View.Property(p => p.ItemId).UsePagingLookUpEditor(p => p.DisplayMember = Item.CodeProperty.Name).HasLabel("物料编码");
                View.Property(p => p.ItemName).HasLabel("物料名称");
                View.Property(p => p.ItemSpecificationModel);
                View.Property(p => p.SingleQty);
                View.Property(p => p.ItemUnitName);
                View.Property(p => p.RoutingProcessId).UseDataSource((e, c, r) =>
                {
                    var bom = e as WorkOrderProcessBom;
                    if (bom == null || bom.WorkOrder == null)
                        return null;
                    var routingProcessList = new EntityList<WorkOrderRoutingProcess>();
                    routingProcessList.AddRange(bom.WorkOrder.RoutingProcessList.Where(p => p.Process.Type == ProcessType.Assembly));
                    return routingProcessList;
                }).UsePagingLookUpEditor().Show(ShowInWhere.Hide).HasLabel("工序名称");
                //View.ChildrenProperty(p => p.PropertyValueList).IsVisible = false;
                View.Property(p => p.ItemExtPropName).Readonly().HasOrderNo(105).Show(ShowInWhere.All);
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
            View.GroupBy(RoutingProcessNameProperty);
            using (View.OrderProperties())
            {
                View.Property(p => p.ItemId).UsePagingLookUpEditor(p => p.DisplayMember = Item.CodeProperty.Name).HasLabel("物料编码");
                View.Property(p => p.ItemName).HasLabel("物料名称");
                View.Property(p => p.ItemExtPropName).HasLabel("物料属性");
                View.Property(p => p.ItemSpecificationModel).HasLabel("基本型号");
                View.Property(p => p.SingleQty);
                View.Property(p => p.ItemUnitName);
                View.Property(p => p.RoutingProcess).UseDataSource((e, c, r) =>
                {
                    var bom = e as WorkOrderProcessBom;
                    if (bom == null || bom.WorkOrder == null)
                        return null;
                    var routingProcessList = new EntityList<WorkOrderRoutingProcess>();
                    routingProcessList.AddRange(bom.WorkOrder.RoutingProcessList.Where(p => p.Process.Type == ProcessType.Assembly || p.Process.Type == ProcessType.BatchAssembly || p.Process.Type == ProcessType.Rework));
                    return routingProcessList;
                }).UsePagingLookUpEditor().Show(ShowInWhere.Hide).HasLabel("工序名称");
              
            }
        }

        /// <summary>
        /// 工单工序Bom视图配置
        /// </summary>
        void WorkOrderProcessBomConfigView()
        {
            View.InlineEdit();
            View.UseCommands(WPFCommandNames.ListAdd, WPFCommandNames.ListEdit, WPFCommandNames.ListDelete);
            View.AddBehavior(typeof(WorkOrderProcessBomBehavior));
            View.UseChildrenAsHorizontal();
            View.UseLayoutSize(-6, -4);
            View.GroupBy(RoutingProcessNameProperty);
            using (View.OrderProperties())
            {
                View.Property(p => p.ItemId).UsePagingLookUpEditor(p =>
                {
                    p.QueryMembers = new System.Collections.Generic.List<IManagedProperty>() { Item.CodeProperty, Item.NameProperty };
                }).Show(ShowInWhere.All).HasLabel("物料编码");
                View.Property(p => p.ItemName).Show(ShowInWhere.All).HasLabel("物料名称");
                View.Property(p => p.ItemExtPropName).Show(ShowInWhere.All).HasLabel("物料属性");
                View.Property(p => p.ItemSpecificationModel).Show(ShowInWhere.All).HasLabel("基本型号");
                View.Property(p => p.SingleQty).Show(ShowInWhere.All);
                View.Property(p => p.ItemUnitName).Show(ShowInWhere.All);
                View.Property(p => p.RoutingProcess).UseDataSource((e, c, r) =>
                {
                    var bom = e as WorkOrderProcessBom;
                    if (bom == null || bom.WorkOrder == null)
                        return null;
                    var routingProcessList = new EntityList<WorkOrderRoutingProcess>();
                    routingProcessList.AddRange(bom.WorkOrder.RoutingProcessList.Where(p => p.Process.Type == ProcessType.Assembly || p.Process.Type == ProcessType.BatchAssembly || p.Process.Type == ProcessType.Rework));
                    return routingProcessList;
                }).UsePagingLookUpEditor().Show(ShowInWhere.All).HasLabel("工序名称");
                
            }
        }

        /// <summary>
        /// 只读视图配置
        /// </summary>
        void ReadOnlyCondfigView()
        {
            View.UseChildrenAsHorizontal();
            View.UseLayoutSize(-6, -4);
            View.GroupBy(RoutingProcessNameProperty);
            using (View.OrderProperties())
            {
                View.Property(p => p.Item).UsePagingLookUpEditor(p => p.DisplayMember = Item.CodeProperty.Name).Show(ShowInWhere.All).HasLabel("物料编码");
                View.Property(p => p.ItemName).Show(ShowInWhere.All).HasLabel("物料名称");
                View.Property(p => p.ItemExtPropName).Show(ShowInWhere.All).HasLabel("物料属性");
                View.Property(p => p.ItemSpecificationModel).Show(ShowInWhere.All).HasLabel("基本型号");
                View.Property(p => p.SingleQty).Show(ShowInWhere.All);
                View.Property(p => p.ItemUnitName).Show(ShowInWhere.All);
                View.Property(p => p.Process.Name).Show(ShowInWhere.All).HasLabel("工序名称");
            }
        }
    }
}
