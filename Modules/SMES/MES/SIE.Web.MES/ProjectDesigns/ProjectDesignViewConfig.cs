using SIE.Core.ProjectMaintains;
using SIE.CSM.Customers;
using SIE.Items;
using SIE.MES.ProjectDesigns;
using SIE.Web.MES.ProjectDesigns.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ProjectDesigns
{
    /// <summary>
    /// 项目号需求设计视图配置
    /// </summary>
    public class ProjectDesignViewConfig : WebViewConfig<ProjectDesign>
    {


        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {

        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.UseCommands(typeof(ProjectDesignAddCommand).FullName, "SIE.Web.MES.ProjectDesigns.Commands.ProjectDesignEditCommand", "SIE.Web.MES.ProjectDesigns.Commands.ProjectDesignDeleteCommand", typeof(ProjectDesignSaveCommand).FullName);
            View.UseCommands("SIE.Web.MES.ProjectDesigns.Commands.ProjectDesignDetailCommand", typeof(ProjectDesignCompleteCommand).FullName, typeof(ProjectDesignExamineCommand).FullName, typeof(ProjectDesignAgainstExamineCommand).FullName);
            View.UseCommands(typeof(ProjectDesignEnableCommand).FullName, typeof(ProjectDesignDisableCommand).FullName, "SIE.Web.MES.ProjectDesigns.Commands.ProjectDesignViewCommand");
            using (View.OrderProperties())
            {
                View.Property(p => p.ProjectMaintain).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<ProjectMaintainController>().GetProjectMaintains(p, k);
                }).Readonly(p => p.ExamineStatus == SIE.MES.ProjectDesigns.Enums.ExamineStatus.Examined).ShowInList(width: 120);
                View.Property(p => p.Product).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<ItemController>().GetItemList(k, p);
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.ProductName), nameof(e.Product.Name));
                    keyValues.Add(nameof(e.SpecificationModel), nameof(e.Product.SpecificationModel));
                    m.DicLinkField = keyValues;
                }).Readonly(p => p.ExamineStatus == SIE.MES.ProjectDesigns.Enums.ExamineStatus.Examined).HasLabel("产品编码").ShowInList(width: 120);
                View.Property(p => p.ProductName).Readonly().ShowInList(width: 120);
                View.Property(p => p.State).Readonly().ShowInList();
                View.Property(p => p.ExamineStatus).Readonly().ShowInList();
                View.Property(p => p.SpecificationModel).Readonly().ShowInList();
                View.Property(p => p.SaleOrderNo).Readonly(p => p.ExamineStatus == SIE.MES.ProjectDesigns.Enums.ExamineStatus.Examined).ShowInList(width: 150);
                View.Property(p => p.Customer).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<CustomerController>().GetCustomers(p, k);
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.CustomerName), nameof(e.Customer.Name));
                    m.DicLinkField = keyValues;
                }).Readonly(p => p.ExamineStatus == SIE.MES.ProjectDesigns.Enums.ExamineStatus.Examined).HasLabel("客户编码").ShowInList(width: 120);
                View.Property(p => p.CustomerName).Readonly().ShowInList(width: 120);
                View.Property(p => p.Qty).Readonly(p => p.ExamineStatus == SIE.MES.ProjectDesigns.Enums.ExamineStatus.Examined).ShowInList();
                View.Property(p => p.Unit).Readonly(p => p.ExamineStatus == SIE.MES.ProjectDesigns.Enums.ExamineStatus.Examined).ShowInList();
                View.Property(p => p.DeliveryDate).Readonly(p => p.ExamineStatus == SIE.MES.ProjectDesigns.Enums.ExamineStatus.Examined).ShowInList(width: 150);
                View.Property(p => p.Remark).Readonly(p => p.ExamineStatus == SIE.MES.ProjectDesigns.Enums.ExamineStatus.Examined).ShowInList(width: 150);
                View.Property(p => p.BaseInfo).Readonly().ShowInList();
                View.Property(p => p.RoutingInfo).Readonly().ShowInList();
                View.Property(p => p.BomInfo).Readonly().ShowInList();
                View.Property(p => p.AttachInfo).Readonly().ShowInList();
                View.Property(p => p.ExamineDate).Readonly().ShowInList(width: 150);
                View.Property(p => p.Examiner).Readonly().ShowInList(width: 150);
                View.ChildrenProperty(p => p.ProjectDesignLogList).HasLabel("操作跟踪日志");
            }
        }
    }
}
