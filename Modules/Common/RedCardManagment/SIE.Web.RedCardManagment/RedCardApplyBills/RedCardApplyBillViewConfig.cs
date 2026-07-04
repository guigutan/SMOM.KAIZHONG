using SIE.MetaModel.View;
using SIE.RedCardManagment.RedCardApplyBills;
using SIE.Web.RedCardManagment._Extentions_;
using SIE.Web.RedCardManagment.RedCardApplyBills.Commands;
using SIE.WorkFlow.Base.Common.Models;
using System.Collections.Generic;

namespace SIE.Web.RedCardManagment.RedCardApplyBills
{
    /// <summary>
    /// 红牌申请单视图配置
    /// </summary>
    internal class RedCardApplyBillViewConfig : WebViewConfig<RedCardApplyBill>
    {
        /// <summary>
        /// 只读视图
        /// </summary>
        const string ReadonlyView = "ReadonlyView";

        /// <summary>
        /// 默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.FormEdit();
            View.DeclareExtendViewGroup(new string[] { ReadonlyView });
            View.DeclareExtendViewGroup(WorkFlowViews.WorkFlowDetailsView, WorkFlowViews.WorkFlowReadonlyView);
            switch (ViewGroup)
            {
                case ReadonlyView:
                    ConfigReadonlyView();
                    break;
                case WorkFlowViews.WorkFlowDetailsView:
                case WorkFlowViews.WorkFlowReadonlyView:
                    ConfigWorkFlowReadonlyView();
                    break;
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.FormEdit();
            View.UseDefaultCommands();
            View.ReplaceCommands(WebCommandNames.Add, typeof(AddRedCardApplyBillCommand).FullName);
            View.ReplaceCommands(WebCommandNames.Edit, "SIE.Web.RedCardManagment.RedCardApplyBills.Commands.EditRedCardApplyBillCommand");
            View.RemoveCommands( WebCommandNames.Delete, WebCommandNames.Copy);
            View.UseCommand("SIE.Web.RedCardManagment.RedCardApplyBills.Commands.OpenStartWorkflowCommand");
            View.UseCommands(typeof(WithDrawReasonCommand).FullName, typeof(CancelReasonCommand).FullName, "SIE.Web.RedCardManagment.RedCardApplyBills.Commands.ViewApplyBillCommand");
            View.Property(p => p.No).ShowInList(width: 200);
            View.Property(p => p.ItemCode);
            View.Property(p => p.ItemName);
            View.Property(p => p.SupplierCode);
            View.Property(p => p.SupplierName);
            View.Property(p => p.ApplySource);
            View.Property(p => p.ApplySourceNo);
            View.Property(p => p.BillStatus);
            View.Property(p => p.WorkflowStarterId);
            View.Property(p => p.FlowInstanceCode);
            View.Property(p => p.WithDrawReason);
            View.Property(p => p.CancelReason);
            View.Property(p => p.RejectReason);
        }

        ///<summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AddBehavior("SIE.Web.RedCardManagment.RedCardApplyBills.Behaviors.EditRedCardApplyBillBehavior");
            View.ClearCommands();
            View.UseCommands(typeof(SaveRedCardApplyBillCommand).FullName);
            View.UseCommands(typeof(StartWorkflowCommand).FullName);
            using (View.OrderProperties())
            {
                using (View.DeclareGroup("基本信息", 4))
                {
                    View.Property(p => p.No).Readonly();
                    View.Property(p => p.ApplySource).Readonly();
                    View.Property(p => p.ApplyType).Readonly();                  
                    View.Property(p => p.SupplierId).UseSupplierLookupEidtor().UsePagingLookUpEditor((m, e) =>
                    {
                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        dic.Add(nameof(e.SupplierName), nameof(e.Supplier.Name));
                        m.DicLinkField = dic;
                        m.ReloadDataOnPopping = true;
                    }).Readonly(p => p.ApplyType == ApplyType.Auto);
                    View.Property(p => p.SupplierName).Readonly();
                    View.Property(p => p.ItemId).UseItemLookUpEditor().UsePagingLookUpEditor((m, e) =>
                    {
                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        dic.Add(nameof(e.ItemName), nameof(e.Item.Name));
                        m.DicLinkField = dic;
                        m.ReloadDataOnPopping = true;
                    }).Readonly(p => p.ApplyType == ApplyType.Auto);
                    View.Property(p => p.ItemName).Readonly();
                }
                using (View.DeclareGroup("问题描述", 1))
                {
                    View.Property(p => p.ProblemDescription).UseMemoEditor( ).ShowInDetail(columnSpan:4).Readonly(p=>p.ApplyType == ApplyType.Auto);
                }
                using (View.DeclareGroup("追溯条件", 2))
                {
                    View.Property(p => p.ProductDateStart).UseDateTimeEditor().HasLabel("生产周期");
                    View.Property(p => p.ProductDateEnd).UseDateTimeEditor().HasLabel("到");
                    View.Property(p => p.JoinProductBatchs).UseListSetting(e => { e.HelpInfo = "多个物料批次用分号;隔开"; }).ShowInDetail(columnSpan: 2);
                    View.Property(p => p.JoinBarcodes).UseListSetting(e => { e.HelpInfo = "多个SN用分号;隔开"; }).ShowInDetail(columnSpan: 2);
                }
            }
        }

        ///<summary>
        /// 只读视图
        /// </summary>
        protected void ConfigReadonlyView()
        {
            View.AddBehavior("SIE.Web.RedCardManagment.RedCardApplyBills.Behaviors.EditRedCardApplyBillBehavior");
            View.DisableEditing();
            using (View.OrderProperties())
            {
                using (View.DeclareGroup("基本信息", 4))
                {
                    View.Property(p => p.ApplySource);
                    View.Property(p => p.ApplySourceNo);
                    View.Property(p => p.FlowInstanceCode);
                    View.Property(p => p.SupplierId);
                    View.Property(p => p.SupplierName);
                    View.Property(p => p.ItemId);
                    View.Property(p => p.ItemName);
                }
                using (View.DeclareGroup("问题描述", 1))
                {
                    View.Property(p => p.ProblemDescription).UseMemoEditor().ShowInDetail(columnSpan: 4);
                }
                using (View.DeclareGroup("追溯条件", 2))
                {
                    View.Property(p => p.ProductDateStart).UseDateTimeEditor().HasLabel("生产周期");
                    View.Property(p => p.ProductDateEnd).UseDateTimeEditor().HasLabel("到");
                    View.Property(p => p.JoinProductBatchs).UseListSetting(e => { e.HelpInfo = "多个物料批次用分号;隔开"; }).ShowInDetail(columnSpan: 2);
                    View.Property(p => p.JoinBarcodes).UseListSetting(e => { e.HelpInfo = "多个SN用分号;隔开"; }).ShowInDetail(columnSpan: 2);
                }
            }
        }

        /// <summary>
        /// 流程申请信息视图
        /// </summary>
        protected void ConfigWorkFlowReadonlyView()
        {
            View.AddBehavior("SIE.Web.RedCardManagment.RedCardApplyBills.Behaviors.EditRedCardApplyBillBehavior");
            using (View.OrderProperties())
            {
                View.DisableEditing();
                using (View.DeclareGroup("基本信息", 4))
                {
                    View.Property(p => p.ApplySource);
                    View.Property(p => p.SupplierId);
                    View.Property(p => p.SupplierName);
                    View.Property(p => p.ItemId);
                    View.Property(p => p.ItemName);
                }
                using (View.DeclareGroup("问题描述", 1))
                {
                    View.Property(p => p.ProblemDescription).UseMemoEditor().ShowInDetail(columnSpan: 4);
                }
                using (View.DeclareGroup("追溯条件", 2))
                {
                    View.Property(p => p.ProductDateStart).UseDateTimeEditor().HasLabel("生产周期");
                    View.Property(p => p.ProductDateEnd).UseDateTimeEditor().HasLabel("到");
                    View.Property(p => p.JoinProductBatchs).UseListSetting(e => { e.HelpInfo = "多个物料批次用分号;隔开"; }).ShowInDetail(columnSpan: 2); 
                    View.Property(p => p.JoinBarcodes).UseListSetting(e => { e.HelpInfo = "多个SN用分号;隔开"; }).ShowInDetail(columnSpan: 2); 
                }
            }
        }
    }
}