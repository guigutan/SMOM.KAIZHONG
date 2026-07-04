using SIE.EMS.InventoryPlans;
using SIE.MetaModel.View;
using SIE.Resources.Employees;
using SIE.Web.EMS.InventoryPlans.Commands;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.InventoryPlans
{
    /// <summary>
    /// 盘点人(工治具)
    /// </summary>
    public class InventoryFixtureCounterViewConfig : WebViewConfig<InventoryFixtureCounter>
    {
        /// <summary>
        /// 单个字符宽度
        /// </summary>
        private readonly int SingleCharWidth = 20;

        /// <summary>
        /// 编辑视图
        /// </summary>
        public const string EditView = "EditView";

        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(EditView);
            if (ViewGroup == EditView)
            {
                ConfigEditView();
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Delete, typeof(SaveInventoryCounterCommand).FullName);
            View.Property(p => p.EmployeeId).UseDataSource((e, p, k) =>
            {

                return RT.Service.Resolve<EmployeeController>().GetEmployees(p, k);
            })
                .UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.Name), nameof(e.Employee.Name));
                    m.DicLinkField = keyValues;
                    m.DisplayField = Employee.CodeProperty.Name;
                    m.BindDisplayField = InventoryFixtureCounter.EmployeeCodeProperty.Name;
                })
                .ShowInList(width: SingleCharWidth * 4).HasLabel("工号");
            View.Property(p => p.Name).ShowInList(width: SingleCharWidth * 4);
            View.Property(p => p.First).ShowInList(60).Readonly(p => p.IsReadOnly);
            View.Property(p => p.Second).ShowInList(60).Readonly(p => p.IsReadOnly);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 编辑视图
        /// </summary>
        protected void ConfigEditView()
        {
            View.AssignAuthorize(typeof(InventoryPlan));
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Delete);
            using (View.OrderProperties())
            {

                View.Property(p => p.EmployeeId).ShowInList().UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.EmployeeCode), nameof(e.Employee.Code));
                    m.DicLinkField = keyValues;
                }).HasLabel("工号");
                View.Property(p => p.First).ShowInList(60);
                View.Property(p => p.Second).ShowInList(60);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}
