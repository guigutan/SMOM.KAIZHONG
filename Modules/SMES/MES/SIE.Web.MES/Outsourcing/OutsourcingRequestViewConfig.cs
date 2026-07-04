using Newtonsoft.Json;
using SIE.Domain;
using SIE.MES.Outsourcing;
using SIE.MES.Outsourcing.Model;
using SIE.MetaModel.View;
using SIE.Web.MES.Outsourcing.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.MES.Outsourcing
{
    /// <summary>
    /// 委外需求单视图
    /// </summary>
    public class OutsourcingRequestViewConfig : WebViewConfig<OutsourcingRequest>
    {
        /// <summary>
        /// 列表配置
        /// </summary>
        protected override void ConfigListView()
        {
            //View.UseDefaultCommands();
            //View.ReplaceCommands(WebCommandNames.Add, typeof(AddOutsourcingRequestCommand).FullName);
            //View.ReplaceCommands(WebCommandNames.Edit, "SIE.Web.MES.Outsourcing.Commands.EditOutsourcingCommand");
            //View.ReplaceCommands(WebCommandNames.Save, "SIE.Web.MES.Outsourcing.Commands.SaveOutsourcingCommand");
            //View.ReplaceCommands(WebCommandNames.Delete, "SIE.Web.MES.Outsourcing.Commands.DeleteRequestCommand");
            //View.RemoveCommands(WebCommandNames.Copy);
            //View.UseCommands(typeof(TemporaryAddCommand).FullName, typeof(ForceCompleteCommand).FullName);
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            View.Property(p => p.NO).Readonly();
            View.Property(p => p.RequestQty).Readonly(p => p.OutsourcingState != OutsourcingState.NotStarted);
            View.Property(p => p.OutboundQty).Readonly();
            View.Property(p => p.WarehousingQty).Readonly();
            View.Property(p => p.OutboundState).Readonly().Show();
            View.Property(p => p.OutsourcingState).Readonly();
            View.Property(p => p.ReportState).Show().Readonly();
            View.Property(p => p.WorkOrder).Readonly();
            View.Property(p => p.ProjectMaintainCode).Readonly();
            View.Property(p => p.BeginProcess).Readonly();
            View.Property(p => p.EndProcess).Readonly();
            View.Property(p => p.Supplier).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add(nameof(e.SupplierName), nameof(e.Supplier.Name));
                m.DicLinkField = dic;
            }).Show(ShowInWhere.All).Readonly(p => p.OutsourcingState != OutsourcingState.NotStarted);
            View.Property(p => p.SupplierName).Readonly();
            View.Property(p => p.InitiatorFactory).Readonly();
            View.Property(p => p.OutFactory).Readonly();
            View.Property(p => p.PlanBeginDate).Readonly();
            View.Property(p => p.ProduceCode).Readonly();
            View.Property(p => p.ProduceName).Readonly();
            View.Property(p => p.WipResource).Readonly();
            View.Property(p => p.WorkShop).Readonly();
            View.Property(p => p.ItemExtPropName).Readonly();

            View.ChildrenProperty(p => p.ProcessingOutsourcingOutboundList).HasOrderNo(20);
            View.ChildrenProperty(p => p.ProcessingOutsourcingInStockList).HasOrderNo(25);
            View.ChildrenProperty(p => p.OutsourcingReportLogList).HasOrderNo(30);
            View.AttachChildrenProperty(typeof(UnProcessingSNViewMidel), (e) =>
            {
                var args = e as ChildPagingDataWithParentEntityArgs;
                var item = JsonConvert.DeserializeObject<OutsourcingRequest>(args.ParentEntity);
                if (item == null)
                    return new EntityList<UnProcessingSNViewMidel>();
                var list = RT.Service.Resolve<OutsourcingRequestController>().GetUnProcessingSNViewMidels(item, args.PagingInfo);
                return list;
            }, ListView).Show(ChildShowInWhere.All).HasLabel("未发料明细").HasOrderNo(35);

        }
    }
}
