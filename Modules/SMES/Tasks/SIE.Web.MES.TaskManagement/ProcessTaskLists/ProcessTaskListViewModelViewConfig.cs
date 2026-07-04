using SIE.Domain;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.ProcessTaskLists;
using SIE.MES.TaskManagement.Reports;
using SIE.MetaModel.View;
using SIE.Web.MES.TaskManagement.ProcessTaskLists.Commands;
using System;

namespace SIE.Web.MES.TaskManagement.ProcessTaskLists
{
    /// <summary>
    /// 
    /// </summary>
    internal class ProcessTaskListViewModelViewConfig : WebViewConfig<ProcessTaskListViewModel>
    {
        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            // 配置视图
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {

            View.UseDefaultCommands();
            View.UseClientOrder();
            View.RemoveCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete, WebCommandNames.Save, WebCommandNames.Copy);
            View.UseCommands(typeof(SplitTaskCommand).FullName);
            View.Property(p => p.No).Readonly();
            View.Property(p => p.ProductCode).Readonly();
            View.Property(p => p.ProductName).Readonly();
            View.Property(p => p.ProcessName).Readonly();
            View.Property(p => p.WoState).Readonly();
            View.Property(p => p.WorkOrderPlanQty).Readonly();
            View.Property(p => p.TasksGeneratedQty).ShowInList(150).Readonly();
            View.Property(p => p.SendQty).ShowInList(150).Readonly();

            View.Property(p => p.PlanBeginDate).ShowInList(150).Readonly();
            View.Property(p => p.PlanEndDate).ShowInList(150).Readonly();
            View.Property(p => p.FactoryName).Readonly();
            View.Property(p => p.WorkShopName).Readonly();
            View.Property(p => p.ResourcepName).Readonly();

            View.AttachChildrenProperty(typeof(ResourcesTasksViewModel), (e) =>
            {
                var args = e as ChildPagingDataArgs;
                var entity = args.Parent.CastTo<ProcessTaskListViewModel>();
                if (entity == null)
                {
                    return new EntityList<ResourcesTasksViewModel>();
                }
                return RT.Service.Resolve<ProcessTaskListController>().GetResourcesTasksViewModels(args.SortInfo, args.PagingInfo, entity);
            }).Show(ChildShowInWhere.List).HasLabel("已排任务单").OrderNo = 40;
        }

        ///<summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            // 配置明细视图
        }

        ///<summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            // 配置下拉视图
        }
    }
}