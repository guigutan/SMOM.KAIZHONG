using SIE.EMS.IdleArchives;
using SIE.Resources.Employees;
using SIE.Web.EMS.Extensions;
using SIE.Web.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.IdleArchives
{
   
   /// <summary>
   /// 查询
   /// </summary>
    public class IdleArchiveCriteriaViewConfig : WebViewConfig<IdleArchiveCriteria>
    {
        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.No).Show();
            View.Property(p => p.QureyFactoryId).UseFactoryEditor().Cascade(p=>p.ManageDept,null);
            View.Property(p => p.IdleArchiveType).Show();
            View.Property(p => p.ManageDeptId).UseUserBussinessDepartmentEditor( factoryIdPropertyName:"QureyFactoryId");
            View.Property(p => p.UseDeptId).UseUserBussinessDepartmentEditor(factoryIdPropertyName: "QureyFactoryId");
            View.Property(p => p.ApprovalStatus).Show();
            View.Property(p => p.ApplicantId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<EmployeeController>().GetEmployeeList(pagingInfo, keyword);
            }).Show();
            View.Property(p => p.CreateDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All);
        }
    }
}
