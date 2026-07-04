using SIE.EMS.InventoryTasks;
using SIE.MetaModel.View;
using SIE.Resources.Employees;
using SIE.Web.Core.Common.Commands;
using System.Collections.Generic;

namespace SIE.Web.EMS.InventoryTasks
{
    /// <summary>
    /// 盘点任务盘点人界面
    /// </summary>
    internal class InventoryTaskCounterViewConfig : WebViewConfig<InventoryTaskCounter>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, typeof(ImmediateDeleteCommand).FullName);

            View.Property(p => p.EmployeeId).UseDataSource((e, p, k) =>
            {

                return RT.Service.Resolve<EmployeeController>().GetEmployees(p, k);
            }).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.Name), nameof(e.Employee.Name));
                m.DicLinkField = keyValues;
                m.DisplayField = Employee.CodeProperty.Name;
                m.BindDisplayField = InventoryTaskCounter.EmployeeCodeProperty.Name;
            }).HasLabel("工号");
            View.Property(p => p.Name).Readonly();
            View.Property(p => p.First).ShowInList(60).Readonly(p => p.IsReadOnly);
            View.Property(p => p.Second).ShowInList(60).Readonly(p => p.IsReadOnly);
            View.Property(p => p.InventoryScope).ShowInList(80);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }
    }
}
