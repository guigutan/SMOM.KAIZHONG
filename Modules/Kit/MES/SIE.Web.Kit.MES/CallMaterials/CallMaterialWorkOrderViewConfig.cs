using SIE.Domain;
using SIE.Kit.MES.CallMaterials;
using SIE.ManagedProperty;
using SIE.MES.WorkOrders;
using SIE.Utils;
using SIE.Web.Kit.MES.CallMaterials.Commands;
using System;

namespace SIE.Web.Kit.MES.CallMaterials
{
    /// <summary>
    /// 叫料工单视图配置
    /// </summary>
    [CompiledPropertyDeclarer]
    public class CallMaterialWorkOrderViewConfig : WebViewConfig<CallMaterialWorkOrder>
    {
        /// <summary>
        /// 叫料工单视图
        /// </summary>
        public const string CallMaterialView = "CallMaterialView";

        #region 显示状态 DisplayState
        /// <summary>
        /// 显示状态  用一个字段显示工单状态和排产状态，工单状态有值时显示工单状态，排产状态有值时显示排产状态
        /// </summary>
        public static readonly Property<string> DisplayStateProperty = P<CallMaterialWorkOrder>.RegisterExtensionReadOnly("DisplayState",
            typeof(CallMaterialWorkOrderViewConfig), GetDisplayState, CallMaterialWorkOrder.WorkOrderProperty);

        /// <summary>
        /// 显示状态
        /// </summary>
        /// <param name="me">工单</param>
        /// <returns>工单状态/排产状态</returns>
        public static string GetDisplayState(CallMaterialWorkOrder me)
        {
            if (me.WoIsPause == YesNo.Yes)
                return EnumViewModel.EnumToLabel(me.WoState) + "暂停";
            else
                return EnumViewModel.EnumToLabel(me.WoState);
        }
        #endregion

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(CallMaterialView);
            if (ViewGroup == CallMaterialView)
                CallMaterialConfigListView();
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected void CallMaterialConfigListView()
        {
            View.RemoveCommands();
            View.UseCommands(typeof(CallMaterialCommand).FullName, typeof(CallMaterialWoUpCommand).FullName, typeof(CallMaterialWoDownCommand).FullName, typeof(SortSolutionsSettingCommand).FullName, typeof(ExcuteSolutionCommand).FullName, typeof(AutoCallMaterialCommand).FullName);

            using (View.OrderProperties())
            {
                View.Property(p => p.WorkOrderNo).Readonly().ShowInList();
                View.Property(p => p.ProductCode).Readonly().ShowInList().HasLabel("产品编码");
                View.Property(p => p.ProductName).Readonly().ShowInList().HasLabel("产品名称");
                View.Property(p => p.WoPlanQty).Readonly().ShowInList().HasLabel("计划数量");
                View.Property(p => p.WoFinishQty).Readonly().ShowInList().HasLabel("完工数量");
                View.Property(DisplayStateProperty).ShowInList().HasLabel("工单状态");
                View.Property(p => p.WoWorkShopName).Readonly().ShowInList().HasLabel("车间");
                View.Property(p => p.WoResourceName).Readonly().ShowInList().HasLabel("资源");
                View.Property(p => p.WoPlanBeginDate).Readonly().ShowInList().HasLabel("计划开始时间");
                View.Property(p => p.WoPlanEndDate).Readonly().ShowInList().HasLabel("计划完成时间");
                View.Property(p => p.WoActuStartDate).Readonly().ShowInList().HasLabel("实际开始时间");
                View.Property(p => p.WoActuFinishDate).Readonly().ShowInList().HasLabel("实际完成时间");
                View.Property(p => p.WoMakerName).Readonly().HasLabel("制单人").ShowInList();
                View.Property(p => p.WoMakeDate).Readonly().HasLabel("制单时间").ShowInList();
                View.Property(p => p.WoState).HasLabel("工单状态").Show(ShowInWhere.Hide);
                View.Property(p => p.WoIsPause).HasLabel("是否暂停").Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.FailReason).Readonly().ShowInList().HasLabel("备注").UseMemoEditor();
            }

            View.ChildrenProperty(p => p.MatchList).Show(ChildShowInWhere.Hide);
            View.ChildrenProperty(p => p.BillList).Show(ChildShowInWhere.List).HasLabel("叫料单").HasOrderNo(10);
            View.AttachChildrenProperty(typeof(WorkOrderProcessBom), (w) =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent.CastTo<CallMaterialWorkOrder>();
                if (parent == null)
                    return new EntityList<WorkOrderProcessBom>();
                var callMaterialWO = RF.GetById<CallMaterialWorkOrder>(parent.Id);
                return RT.Service.Resolve<WorkOrderController>().GetWoProcessBom(callMaterialWO.WorkOrderId, args.PagingInfo);
            }, viewGroup: WorkOrderProcessBomViewConfig.CallMaterialBomView).Show(ChildShowInWhere.List).HasLabel("工序BOM").HasOrderNo(30);

            View.AttachChildrenProperty(typeof(CallMatchWorkOrder), (w) =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent.CastTo<CallMaterialWorkOrder>();
                if (parent == null)
                    return new EntityList<CallMatchWorkOrder>();
                var callMaterialWO = RF.GetById<CallMaterialWorkOrder>(parent.Id);
                return RT.Service.Resolve<CallMaterialController>().GetCallMatchWorkOrder(callMaterialWO);
            }).Show(ChildShowInWhere.List).HasLabel("工单匹配").HasOrderNo(20);
        }
    }
}
