using SIE.EMS.InventoryTasks;
using SIE.MetaModel.View;
using SIE.Resources.Employees;
using SIE.Web.Core.Common.Commands;
using System.Collections.Generic;

namespace SIE.Web.EMS.InventoryTasks
{
    /// <summary>
    /// 盘点任务备件盘点人视图配置
    /// </summary>
    public class InventoryTaskSparePartCounterViewConfig : WebViewConfig<InventoryTaskSparePartCounter>
    {
        /// <summary>
        /// 单个字符宽度
        /// </summary>
        private readonly int SingleCharWidth = 20;

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
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
                    m.BindDisplayField = InventoryTaskSparePartCounter.EmployeeCodeProperty.Name;
                })
                .ShowInList(width: SingleCharWidth * 4).HasLabel("工号");
            View.Property(p => p.Name).ShowInList(width: SingleCharWidth * 4);
            View.Property(p => p.First).ShowInList(width: SingleCharWidth * 3);
            View.Property(p => p.Second).ShowInList(width: SingleCharWidth * 3);

        }
    }
}
