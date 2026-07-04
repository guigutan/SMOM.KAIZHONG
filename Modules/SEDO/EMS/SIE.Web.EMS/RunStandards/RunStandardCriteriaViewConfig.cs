using SIE.EMS.RunStandards;
using SIE.Equipments.EquipModels;
using SIE.Resources.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.RunStandards
{
   /// <summary>
   /// 查询视图
   /// </summary>
    public class RunStandardCriteriaViewConfig : WebViewConfig<RunStandardCriteria>
    {
       /// <summary>
       /// 配置查询视图
       /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.No).Show();
            View.Property(p => p.Name).Show();
            View.Property(p => p.EquipModelId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<EquipModelController>().GetEquipModels(pagingInfo, keyword);
            }).Show();
            View.Property(p => p.CreateId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<EmployeeController>().GetEmployeeList(pagingInfo, keyword);
            }).Show();
            View.Property(p => p.CreateDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All);
        }
    }
}
